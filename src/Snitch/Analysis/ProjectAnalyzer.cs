using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Depends.Core.Graph;
using Microsoft.Extensions.Logging;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;

namespace Snitch.Analysis
{
    internal sealed class ProjectAnalyzer
    {
        public ProjectAnalyzerResult Analyze(Project project,  SourceCacheContext sourceCacheContext, ConcurrentDictionary<PackageIdentity, SourcePackageDependencyInfo> resolvedPackages)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            // Analyze the project.
            var result = new List<PackageToRemove>();
            AnalyzeProject(project, project, result, sourceCacheContext, resolvedPackages);

            if (project.LockFilePath != null)
            {
                // Now prune stuff that we're not interested in removing
                // such as private package references and analyzers.
                result = PruneResults(project, result);
            }

            return new ProjectAnalyzerResult(project, result);
        }

        private List<ProjectPackage> AnalyzeProject(Project root, Project project, List<PackageToRemove> result, SourceCacheContext sourceCacheContext, ConcurrentDictionary<PackageIdentity, SourcePackageDependencyInfo> resolvedPackages)
        {
            var accumulated = new List<ProjectPackage>();
            result ??= new List<PackageToRemove>();

            if (project.ProjectReferences.Count > 0)
            {
                // Iterate through all project references.
                foreach (var child in project.ProjectReferences)
                {
                    // Analyze the project recursively.
                    foreach (var item in AnalyzeProject(root, child, result, sourceCacheContext, resolvedPackages))
                    {
                        // Didn't exist previously in the list of accumulated packages?
                        if (!accumulated.ContainsPackage(item.Package))
                        {
                            accumulated.Add(new ProjectPackage(item.Project, item.Package, sourceCacheContext, resolvedPackages));
                        }
                    }
                }

                // Was any package in the current project references
                // by one of the projects referenced by the project?
                foreach (var package in project.Packages)
                {
                    var found = accumulated.FindProjectPackage(package);
                    if (found != null)
                    {
                        if (!result.ContainsPackage(found.Package))
                        {
                            if (project.Name.Equals(root.Name, StringComparison.OrdinalIgnoreCase))
                            {
                                result.Add(new PackageToRemove(project, package, found));
                            }
                        }
                    }
                    else
                    {
                        AddToAccumulated(package);
                    }
                }
            }
            else
            {
                foreach (var item in project.Packages)
                {
                    if (!accumulated.ContainsPackage(item))
                    {
                        AddToAccumulated(item);
                    }
                }
            }

            void AddToAccumulated(Package package)
            {
                if (package.PrivateAssets?.Contains("compile") == true)
                {
                    return;
                }

                // Add the package to the list of accumulated packages.
                accumulated.Add(new ProjectPackage(project, package, sourceCacheContext, resolvedPackages));
            }

            // analyze accumulated packages for redundant transitive dependencies
            foreach (var package in project.Packages)
            {
                var otherPackages = accumulated.Where(p => p.Package.Name != package.Name);
                var transitiveParent = otherPackages.FirstOrDefault(op => op.PackageDependencies.TryGetValue(package.Name, out Package pd) && pd.IsSameVersion(package));
                if (transitiveParent != null)
                {
                    result.Add(new PackageToRemove(project, package, transitiveParent));
                }
            }

            return accumulated;
        }

        private static List<PackageToRemove> PruneResults(Project project, List<PackageToRemove> packages)
        {
            // Read the lockfile.
            var lockfile = new LockFileFormat().Read(project.LockFilePath);

            // Find the expected target.
            var framework = NuGetFramework.Parse(project.TargetFramework);
            var target = lockfile.PackageSpec.TargetFrameworks.FirstOrDefault(
                x => x.FrameworkName.Framework.Equals(framework.Framework, StringComparison.OrdinalIgnoreCase));

            // Could we not find the target?
            if (target == null)
            {
                throw new InvalidOperationException("Could not determine target framework");
            }

            var result = new List<PackageToRemove>();
            foreach (var package in packages)
            {
                // Try to find the dependency.
                var dependency = target.Dependencies.FirstOrDefault(
                    x => x.Name.Equals(package.Package.Name, StringComparison.OrdinalIgnoreCase));

                if (dependency != null)
                {
                    // Auto referenced or private package?
                    if (dependency.AutoReferenced ||
                        dependency.SuppressParent == LibraryIncludeFlags.All)
                    {
                        continue;
                    }
                }

                result.Add(package);
            }

            return result;
        }
    }
}

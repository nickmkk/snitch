using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Depends.Core;
using Depends.Core.Graph;
using Microsoft.Extensions.Logging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;

namespace Snitch.Analysis
{
    [DebuggerDisplay("{PackageDescription(),nq}")]
    internal sealed class ProjectPackage
    {
        private static readonly DependencyAnalyzer _dependencyAnalyzer = new(new LoggerFactory());       

        public Project Project { get; }

        public Package Package { get; }

        public IReadOnlyDictionary<string, Package> PackageDependencies { get; }

        public ProjectPackage(Project project, Package package, SourceCacheContext sourceCacheContext, ConcurrentDictionary<PackageIdentity, SourcePackageDependencyInfo> resolvedPackages)
        {
            Project = project ?? throw new ArgumentNullException(nameof(project));
            Package = package ?? throw new ArgumentNullException(nameof(package));

            if (package.Range == null)
            {
                var dependencyGraph = _dependencyAnalyzer.Analyze(package.Name, package.Version, project.TargetFramework, sourceCacheContext, resolvedPackages);
                PackageDependencies = dependencyGraph.Nodes.Where(n => n.Id != "NETStandard.Library")
                                                           .OfType<PackageReferenceNode>()
                                                           .Select(n => new Package(n.PackageId, n.Version, null))
                                                           .OrderBy(p => p.Name)
                                                           .ToImmutableDictionary(k => k.Name);
            }
            else
            {
                PackageDependencies = Array.Empty<Package>().ToImmutableDictionary(k => k.Name);
            }
        }

        private string PackageDescription()
        {
            return $"{Project.Name}: {Package.Name}";
        }
    }
}

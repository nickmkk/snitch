using System;
using System.Diagnostics;
using System.Linq;

namespace Snitch.Analysis
{
    [DebuggerDisplay("{PackageDescription(),nq}")]
    internal sealed class PackageToRemove
    {
        public Project Project { get; }
        public Package Package { get; }
        public ProjectPackage Original { get; }

        public bool CanBeRemoved => Package.IsSameVersion(Original.Package) || (Original.PackageDependencies.TryGetValue(Package.Name, out Package pd) && Package.IsSameVersion(pd));
        public bool VersionMismatch => !Package.IsSameVersion(Original.Package) && !(Original.PackageDependencies.TryGetValue(Package.Name, out Package pd) && Package.IsSameVersion(pd));

        public PackageToRemove(Project project, Package package, ProjectPackage original)
        {
            Project = project ?? throw new ArgumentNullException(nameof(project));
            Package = package ?? throw new ArgumentNullException(nameof(package));
            Original = original ?? throw new ArgumentNullException(nameof(original));
        }

        private string PackageDescription()
        {
            return $"{Project.Name}: {Package.Name} ({Original.Project?.Name ?? Original.Package.Name})";
        }
    }
}
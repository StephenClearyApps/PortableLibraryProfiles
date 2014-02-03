using System.Runtime.Versioning;

namespace FrameworkProfiles
{
    public class FrameworkProfile
    {
        public FrameworkName Name { get; set; }
        public string DisplayName { get; set; }
        public virtual bool SupportedByVisualStudio2013 { get; set; }

        public virtual string NugetTarget
        {
            get { return NugetTargets.GetKnownNugetTarget(this); }
        }
    }
}
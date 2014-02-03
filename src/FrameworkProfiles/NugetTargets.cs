using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using Comparers;
using EqualityComparers;

namespace FrameworkProfiles
{
    public static class NugetTargets
    {
        private static readonly Dictionary<FrameworkName, string> KnownNugetTargets = new Dictionary<FrameworkName, string>
        {
            { new FrameworkName(".NETFramework,Version=v4.0,Profile=*"), "net4" },
            { new FrameworkName(".NETFramework,Version=v4.0.3,Profile=*"), "net403" },
            { new FrameworkName(".NETFramework,Version=v4.5,Profile=*"), "net45" },
            { new FrameworkName(".NETFramework,Version=v4.5.1,Profile=*"), "net451" },
            { new FrameworkName("Silverlight,Version=v4.0"), "sl4" },
            { new FrameworkName("Silverlight,Version=v5.0"), "sl5" },
            { new FrameworkName("Silverlight,Version=v4.0,Profile=WindowsPhone*"), "wp7" },
            { new FrameworkName("Silverlight,Version=v4.0,Profile=WindowsPhone7*"), "wp71" },
            { new FrameworkName("WindowsPhone,Version=v8.0"), "wp8" },
            { new FrameworkName(".NETCore,Version=v4.5,Profile=*"), "win8" },
            { new FrameworkName(".NETCore,Version=v4.5.1,Profile=*"), "win81" },
        };

        public static string GetKnownNugetTarget(FrameworkProfile profile)
        {
            string ret;
            return KnownNugetTargets.TryGetValue(profile.Name, out ret) ? ret : string.Empty;
        }
    }
}

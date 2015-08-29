using System;
using System.Runtime.Versioning;

namespace FrameworkProfiles
{
    public class FrameworkProfile
    {
        public FrameworkName Name { get; set; }
        public string DisplayName { get; set; }
        public Version MaximumVisualStudioVersion { get; set; }
        public virtual bool SupportedByVisualStudio2013 { get { return MaximumVisualStudioVersion == null || MaximumVisualStudioVersion >= new Version(12, 0); } }
        public virtual bool SupportedByVisualStudio2015 { get { return MaximumVisualStudioVersion == null || MaximumVisualStudioVersion >= new Version(14, 0); } }

        /// <summary>
        /// Whether this profile supports async/await. This includes profiles supporting async/await via Microsoft.Bcl.Async.
        /// </summary>
        public virtual bool SupportsAsync
        {
            get
            {
                // .NET 4.0 and newer.
                if (Name.Identifier == ".NETFramework" && Name.Version >= new Version(4, 0))
                    return true;

                // Windows 8 and newer.
                if (Name.Identifier == ".NETCore")
                    return true;

                // Silverlight 4 and newer.
                if (Name.Identifier == "Silverlight" && string.IsNullOrEmpty(Name.Profile) && Name.Version >= new Version(4, 0))
                    return true;

                // Windows Phone 7.1.
                if (Name.FullName == "Silverlight,Version=v4.0,Profile=WindowsPhone7*")
                    return true;

                // Windows Phone 8 and newer.
                if (Name.Identifier == "WindowsPhone")
                    return true;

                // Windows Phone Apps.
                if (Name.Identifier == "WindowsPhoneApp")
                    return true;

                // Mono
                if (Name.Identifier == "MonoAndroid" || Name.Identifier == "MonoTouch" || Name.Identifier.StartsWith("Xamarin"))
                    return true;

                return false;
            }
        }

        public virtual bool SupportsGenericVariance
        {
            get
            {
                // .NET 4.0 and newer.
                if (Name.Identifier == ".NETFramework" && Name.Version >= new Version(4, 0))
                    return true;

                // Windows 8 and newer.
                if (Name.Identifier == ".NETCore")
                    return true;

                // Silverlight 5 and newer.
                if (Name.Identifier == "Silverlight" && string.IsNullOrEmpty(Name.Profile) && Name.Version >= new Version(5, 0))
                    return true;

                // Windows Phone 8 and newer.
                if (Name.Identifier == "WindowsPhone")
                    return true;

                // Windows Phone Apps.
                if (Name.Identifier == "WindowsPhoneApp")
                    return true;

                // Mono
                if (Name.Identifier == "MonoAndroid" || Name.Identifier == "MonoTouch" || Name.Identifier.StartsWith("Xamarin"))
                    return true;

                return false;
            }
        }

        public virtual bool IsXamarin
        {
            get { return (Name.Identifier == "MonoAndroid" || Name.Identifier == "MonoTouch" || Name.Identifier.StartsWith("Xamarin")); }
        }

        public virtual string NugetTarget
        {
            get { return NugetTargets.GetNugetTarget(this); }
        }
    }
}
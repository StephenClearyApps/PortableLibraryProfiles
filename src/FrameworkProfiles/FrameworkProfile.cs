using System;
using System.Collections.Generic;
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

        public virtual bool IsPrivate
        {
            get { return IsXamarin || (Name.Identifier == "DNXcore"); }
        }

        public virtual string NugetTarget
        {
            get { return NugetTargets.GetNugetTarget(this); }
        }

        public virtual Version NetStandardGeneration
        {
            get
            {
                // https://github.com/dotnet/corefx/blob/master/Documentation/project-docs/standard-platform.md

                // Note: This only includes platforms that can be targeted in the PCL.

                // > .NET 4.6 are 5.5
                if (Name.Identifier == ".NETFramework" && Name.Version > new Version(4, 6))
                    return new Version(1, 4);

                // > .NET 4.6 is 5.4
                if (Name.Identifier == ".NETFramework" && Name.Version == new Version(4, 6))
                    return new Version(1, 3);

                // > .NET 4.5.1 and 4.5.2 are 5.3
                if (Name.Identifier == ".NETFramework" && Name.Version > new Version(4, 5))
                    return new Version(1, 2);

                // .NET 4.5 is 5.2
                if (Name.Identifier == ".NETFramework" && Name.Version == new Version(4, 5))
                    return new Version(1, 1);

                // Windows 8.1 is 5.3
                if (Name.Identifier == ".NETCore" && Name.Version == new Version(4, 5, 1))
                    return new Version(1, 2);

                // Windows 8.0 is 5.2
                if (Name.Identifier == ".NETCore" && Name.Version == new Version(4, 5))
                    return new Version(1, 1);

                // DNXcore 5.0 is 5.5
                if (Name.Identifier == "DNXcore" && Name.Version == new Version(5, 0))
                    return new Version(1, 4);

                // Windows Phone App 8.1 is 5.3
                if (Name.Identifier == "WindowsPhoneApp" && Name.Version == new Version(8, 1))
                    return new Version(1, 2);

                // Windows Phone Silverlight 8 and 8.1 are 5.1
                if (Name.Identifier == "WindowsPhone")
                    return new Version(1, 0);

                // No other platforms support generations.
                return null;
            }
        }

        public virtual Version DotnetGeneration
        {
            get
            {
                // https://github.com/davidfowl/aspnetvnextwebapiapp/blob/master/Generations.md
                
                // Note: This only includes platforms that can be targeted in the PCL.

                // > .NET 4.6 are 5.5
                if (Name.Identifier == ".NETFramework" && Name.Version > new Version(4, 6))
                    return new Version(5, 5);

                // > .NET 4.6 is 5.4
                if (Name.Identifier == ".NETFramework" && Name.Version == new Version(4, 6))
                    return new Version(5, 4);

                // > .NET 4.5.1 and 4.5.2 are 5.3
                if (Name.Identifier == ".NETFramework" && Name.Version > new Version(4, 5))
                    return new Version(5, 3);

                // .NET 4.5 is 5.2
                if (Name.Identifier == ".NETFramework" && Name.Version == new Version(4, 5))
                    return new Version(5, 2);

                // Windows 8.1 is 5.3
                if (Name.Identifier == ".NETCore" && Name.Version == new Version(4, 5, 1))
                    return new Version(5, 3);

                // Windows 8.0 is 5.2
                if (Name.Identifier == ".NETCore" && Name.Version == new Version(4, 5))
                    return new Version(5, 2);

                // DNXcore 5.0 is 5.5
                if (Name.Identifier == "DNXcore" && Name.Version == new Version(5, 0))
                    return new Version(5, 5);

                // Windows Phone App 8.1 is 5.3
                if (Name.Identifier == "WindowsPhoneApp" && Name.Version == new Version(8, 1))
                    return new Version(5, 3);

                // Windows Phone Silverlight 8 and 8.1 are 5.1
                if (Name.Identifier == "WindowsPhone")
                    return new Version(5, 1);

                // No other platforms support generations.
                return null;
            }
        }

        private static readonly Dictionary<FrameworkName, string> SpecialFriendlyNames = new Dictionary<FrameworkName, string>
        {
            { new FrameworkName("Xbox,Version=v4.0,Profile=*"), "XBox 360" },
            { new FrameworkName("Silverlight,Version=v4.0,Profile=WindowsPhone*"), "Windows Phone Silverlight 7.0" },
            { new FrameworkName("Silverlight,Version=v4.0,Profile=WindowsPhone7*"), "Windows Phone Silverlight 7.5" },
            { new FrameworkName(".NETCore,Version=v4.5,Profile=*"), "Windows 8.0" },
            { new FrameworkName(".NETCore,Version=v4.5.1,Profile=*"), "Windows 8.1" },
        };

        public virtual string FriendlyName
        {
            get
            {
                if (SpecialFriendlyNames.ContainsKey(Name))
                    return SpecialFriendlyNames[Name];
                return DisplayName + " " + Name.Version.FriendlyName();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Xml.Linq;

namespace FrameworkProfiles
{
    public static class PortableFrameworkProfileEnumerator
    {
        public static IEnumerable<PortableProfile> EnumeratePortableProfiles(string path)
        {
            path = path ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Reference Assemblies", "Microsoft", "Framework", ".NETPortable");
            var versions = Directory.EnumerateDirectories(path);
            foreach (var version in versions)
            {
                var profilePath = Path.Combine(version, "Profile");
                var profiles = Directory.EnumerateDirectories(profilePath);
                foreach (var profile in profiles)
                {
                    var versionFileName = Path.GetFileName(version);
                    var profileFileName = Path.GetFileName(profile);
                    if (versionFileName == null || profileFileName == null)
                        continue;
                    var ret = new PortableProfile
                    {
                        Name = new FrameworkName(".NETPortable", new Version(versionFileName.Substring(1)), Path.GetFileName(profile))
                    };

                    var frameworkListFileName = Path.Combine(profile, "RedistList", "FrameworkList.xml");
                    var frameworkList = XElement.Load(frameworkListFileName);
                    ret.DisplayName = frameworkList.Attribute("Name").Value;

                    var supportedFrameworkPath = Path.Combine(profile, "SupportedFrameworks");
                    var supportedFrameworks = Directory.EnumerateFiles(supportedFrameworkPath);
                    foreach (var supportedFramework in supportedFrameworks)
                    {
                        var xml = XElement.Load(supportedFramework);
                        ret.SupportedFrameworks.Add(new FrameworkProfile
                        {
                            DisplayName = xml.Attribute("DisplayName").Value,
                            Name = new FrameworkName(xml.Attribute("Identifier").Value, new Version(xml.Attribute("MinimumVersion").Value), xml.Attribute("Profile").Value),
                            SupportedByVisualStudio2013 = new Version((xml.Attribute("MaximumVisualStudioVersion") ?? new XAttribute("MaximumVisualStudioVersion", "12.0")).Value) >= new Version(12, 0),
                        });
                    }

                    yield return ret;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Xml.Linq;
using FrameworkProfiles.FileSystem;

namespace FrameworkProfiles
{
    public static class PortableFrameworkProfileEnumerator
    {
        public static IEnumerable<PortableProfile> EnumeratePortableProfiles(IFolder folder)
        {
            folder = folder ?? DiskFileSystem.Folder(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Reference Assemblies", "Microsoft", "Framework", ".NETPortable"));
            var versions = folder.EnumerateFolders();
            foreach (var version in versions)
            {
                var profiles = version.Folder("Profile").EnumerateFolders();
                foreach (var profile in profiles)
                {
                    var versionFileName = Path.GetFileName(version.FullPath);
                    var profileFileName = Path.GetFileName(profile.FullPath);
                    if (versionFileName == null || profileFileName == null)
                        continue;
                    var ret = new PortableProfile
                    {
                        Name = new FrameworkName(".NETPortable", new Version(versionFileName.Substring(1)), profileFileName)
                    };

                    var frameworkListFile = profile.Folder("RedistList").File("FrameworkList.xml");
                    var frameworkList = XElement.Load(frameworkListFile.FullPath);
                    ret.DisplayName = frameworkList.Attribute("Name").Value;

                    var supportedFrameworkFolder = profile.Folder("SupportedFrameworks");
                    var supportedFrameworks = supportedFrameworkFolder.EnumerateFiles();
                    foreach (var supportedFramework in supportedFrameworks)
                    {
                        var xml = XElement.Load(supportedFramework.FullPath);
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

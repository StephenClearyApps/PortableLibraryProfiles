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
        public static readonly string MachineProfilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Reference Assemblies", "Microsoft", "Framework", ".NETPortable");

        public static IEnumerable<PortableProfile> EnumeratePortableProfiles(IFolder folder)
        {
            foreach (var version in folder.EnumerateFolders())
            {
                var profilesFolder = version.Folder("Profile");
                if (profilesFolder == null)
                    continue;
                foreach (var profile in profilesFolder.EnumerateFolders())
                {
                    var versionFileName = Path.GetFileName(version.FullPath.TrimEnd('/'));
                    var profileFileName = Path.GetFileName(profile.FullPath.TrimEnd('/'));
                    var ret = new PortableProfile
                    {
                        Name = new FrameworkName(".NETPortable", new Version(versionFileName.Substring(1)), profileFileName)
                    };

                    var frameworkListFile = profile.Folder("RedistList").File("FrameworkList.xml");
                    var frameworkList = XElement.Load(frameworkListFile.Open());
                    ret.DisplayName = frameworkList.Attribute("Name").Value;

                    var supportedFrameworkFolder = profile.Folder("SupportedFrameworks");
                    var supportedFrameworks = supportedFrameworkFolder.EnumerateFiles();
                    foreach (var supportedFramework in supportedFrameworks)
                    {
                        var xml = XElement.Load(supportedFramework.Open());
                        var maximumVisualStudioVersionAttribute = xml.Attribute("MaximumVisualStudioVersion");
                        var childFramework = new FrameworkProfile
                        {
                            DisplayName = xml.Attribute("DisplayName").Value,
                            Name = new FrameworkName(xml.Attribute("Identifier").Value, new Version(xml.Attribute("MinimumVersion").Value), xml.Attribute("Profile").Value),
                            MaximumVisualStudioVersion = maximumVisualStudioVersionAttribute == null ? null : new Version(maximumVisualStudioVersionAttribute.Value),
                        };
                        if (!childFramework.IsXamarin)
                            ret.SupportedFrameworks.Add(childFramework);
                    }

                    yield return ret;
                }
            }
        }
    }
}

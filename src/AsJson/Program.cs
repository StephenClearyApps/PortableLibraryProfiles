using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FrameworkProfiles;
using FrameworkProfiles.FileSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AsJson
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                foreach (var path in Directory.EnumerateDirectories("."))
                {
                    CleanDirectory(path);
                    ProcessProfiles(DiskFileSystem.Folder(path), Path.Combine(path, "profiles.json"));
                }

                foreach (var zip in Directory.EnumerateFiles(".", "*.zip"))
                    ProcessProfiles(ZipFileSystem.Open(zip), Path.ChangeExtension(zip, "json"));
                ProcessProfiles(DiskFileSystem.Folder(PortableFrameworkProfileEnumerator.MachineProfilePath), "profiles.json");

                Console.WriteLine("Done.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
            Console.ReadKey();
        }

        static void ProcessProfiles(IFolder path, string jsonFile)
        {
            var profiles = PortableFrameworkProfileEnumerator.EnumeratePortableProfiles(path)
                .OrderBy(x => int.Parse(x.Name.Profile.Substring(7)))
                .Select(p => new
                {
                    p.Name.FullName,
                    p.DisplayName,
                    ProfileName = p.Name.Profile,
                    p.SupportedByVisualStudio2013,
                    p.SupportsAsync,
                    p.SupportsGenericVariance,
                    p.NugetTarget,
                    Frameworks = p.SupportedFrameworks.Select(f => new
                    {
                        f.Name.FullName,
                        f.DisplayName,
                    }).ToArray(),
                }).ToArray();

            File.WriteAllText(jsonFile, JsonConvert.SerializeObject(profiles, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
        }

        static void CleanDirectory(string path)
        {
            var versions = Directory.EnumerateDirectories(path);
            foreach (var version in versions)
            {
                foreach (var file in Directory.EnumerateFiles(version).ToArray())
                    File.Delete(file);

                foreach (var subdir in Directory.EnumerateDirectories(version).Where(x => !x.Contains("Profile") && !x.Contains("RedistList")))
                    Directory.Delete(subdir, true);

                var profiles = Directory.EnumerateDirectories(Path.Combine(version, "Profile"));
                foreach (var profile in profiles)
                {
                    foreach (var file in Directory.EnumerateFiles(profile).ToArray())
                        File.Delete(file);
                    foreach (var subdir in Directory.EnumerateDirectories(profile).Where(x => !x.Contains("SupportedFrameworks") && !x.Contains("RedistList")))
                        Directory.Delete(subdir, true);
                }
            }
        }
    }
}

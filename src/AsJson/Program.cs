using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FrameworkProfiles;
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
                    ProcessProfiles(path);
                }

                ProcessProfiles(null);

                Console.WriteLine("Done.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
            Console.ReadKey();
        }

        static void ProcessProfiles(string path)
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

            var filename = path == null ? "profiles.json" : Path.Combine(path, "profiles.json");
            File.WriteAllText(filename, JsonConvert.SerializeObject(profiles, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
        }
    }
}

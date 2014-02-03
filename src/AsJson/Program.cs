using System;
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
                // Sort and project to reduce amount of cruft in the output.
                var profiles = PortableFrameworkProfileEnumerator.EnumeratePortableProfiles()
                    .OrderBy(x => int.Parse(x.Name.Profile.Substring(7)))
                    .Select(p => new
                    {
                        p.Name.FullName,
                        p.DisplayName,
                        ProfileName = p.Name.Profile,
                        p.SupportedByVisualStudio2013,
                        p.NugetTarget,
                        Frameworks = p.SupportedFrameworks.Select(f => new
                        {
                            f.Name.FullName,
                            f.DisplayName,
                        }).ToArray(),
                    }).ToArray();

                Clipboard.SetText(JsonConvert.SerializeObject(profiles, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() }));

                Console.WriteLine("The portable framework library profiles have been copied to the clipboard.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
            Console.ReadKey();
        }
    }
}

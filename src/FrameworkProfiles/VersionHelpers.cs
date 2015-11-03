using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkProfiles
{
    public static class VersionHelpers
    {
        public static string FriendlyName(this Version version)
        {
            var result = version.Major + "." + version.Minor;
            if (version.Build > 0 || version.Revision > 0)
                result += "." + version.Build;
            if (version.Revision > 0)
                result += "." + version.Revision;
            return result;
        }
    }
}

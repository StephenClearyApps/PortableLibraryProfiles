using System.Collections.Generic;

namespace FrameworkProfiles
{
    public sealed class PortableProfile : FrameworkProfile
    {
        public PortableProfile()
        {
            SupportedFrameworks = new List<FrameworkProfile>();
        }

        public List<FrameworkProfile> SupportedFrameworks { get; private set; }
    }
}
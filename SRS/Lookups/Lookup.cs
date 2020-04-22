using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRS.Lookups
{
    public class Lookup
    {
        public List<RegionLookup> regionLookup { get; set; }

        public List<CountryLookup> countryLookup { get; set; }

        public List<StateLookup> stateLookup { get; set; }

        public List<BuildingLookup> BuildingLookup { get; set; }
    }
}

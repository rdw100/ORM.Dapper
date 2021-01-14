using System.Collections.Generic;

namespace ORM.Dapper.Core.Models
{
    public class Region
    {
        public int RegionID { get; set; }
        public string RegionDescription { get; set; }

        public List<Territory> Territories { get; } = new List<Territory>();
    }
}

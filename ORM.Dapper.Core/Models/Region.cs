using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace ORM.Dapper.Core.Models
{
    public class Region
    {
        public int RegionID { get; set; }
        public string RegionDescription { get; set; }

        [Computed]
        public bool IsNew => this.RegionID == default;

        [Write(false)]
        public List<Territory> Territories { get; } = new List<Territory>();
    }
}
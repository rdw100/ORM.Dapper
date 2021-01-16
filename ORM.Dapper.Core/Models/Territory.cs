using Dapper.Contrib.Extensions;

namespace ORM.Dapper.Core.Models
{
    [Table("Territories")]
    public class Territory
    {
        [ExplicitKey]
        public int TerritoryID { get; set; }
        public string TerritoryDescription { get; set; }
        public int RegionID { get; set; }
        [Computed]
        public bool IsNew => (this.TerritoryID == default);
        public bool IsDeleted { get; set; }
    }
}
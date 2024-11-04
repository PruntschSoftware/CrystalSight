namespace CrystalSight.Web.DataContract
{
    public class StoredValue
    {
        public Guid Id { get; set; }
        public string Symbol { get; set; }
        public decimal Value { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}

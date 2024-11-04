using CrystalSight.Web.Authentication.DataContract;

namespace CrystalSight.Web.DataContract
{
    public class Device
    {
        public Guid Id { get; set; }
        public string MacAdress { get; set; }
        public ApplicationUser User { get; set; }
        public string Symbol { get; set; }

    }
}

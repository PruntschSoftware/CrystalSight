using System.Collections.Generic;
using System.Xml;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CrystalSight.Web.Authentication.DataContract;
using CrystalSight.Web.DataContract;

namespace CrystalSight.Web
{
    public class SqLiteContext : IdentityDbContext<ApplicationUser>
    {
        public SqLiteContext() : base()
        {
        }
        public SqLiteContext(DbContextOptions<SqLiteContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=tickerDatabase.db");

        public virtual DbSet<StoredValue> StoredValues { get; set; } = null!;
        public virtual DbSet<Device> Devices { get; set; } = null!;
    }
}

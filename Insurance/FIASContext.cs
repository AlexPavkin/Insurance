namespace Insurance
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class FIASContext : DbContext
    {
        public FIASContext()
            : base("name=FIASContext")
        {
        }

        public virtual DbSet<AddressObjects> AddressObjects { get; set; }
        public virtual DbSet<Houses> Houses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddressObjects>()
                .Property(e => e.AOLEVEL)
                .HasPrecision(10, 0);

            modelBuilder.Entity<AddressObjects>()
                .Property(e => e.ACTSTATUS)
                .HasPrecision(10, 0);

            modelBuilder.Entity<AddressObjects>()
                .Property(e => e.CENTSTATUS)
                .HasPrecision(10, 0);

            modelBuilder.Entity<AddressObjects>()
                .Property(e => e.OPERSTATUS)
                .HasPrecision(10, 0);

            modelBuilder.Entity<AddressObjects>()
                .Property(e => e.CURRSTATUS)
                .HasPrecision(10, 0);

            modelBuilder.Entity<AddressObjects>()
                .Property(e => e.LIVESTATUS)
                .HasPrecision(1, 0);

            modelBuilder.Entity<Houses>()
                .Property(e => e.ESTSTATUS)
                .HasPrecision(1, 0);

            modelBuilder.Entity<Houses>()
                .Property(e => e.STRSTATUS)
                .HasPrecision(10, 0);

            modelBuilder.Entity<Houses>()
                .Property(e => e.STATSTATUS)
                .HasPrecision(10, 0);

            modelBuilder.Entity<Houses>()
                .Property(e => e.COUNTER)
                .HasPrecision(10, 0);
        }
    }
}

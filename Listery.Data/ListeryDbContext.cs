using Listery.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listery.Data
{
    public class ListeryDbContext : DbContext
    {
        public DbSet<GroceryItem> Items { get; set; }
        public DbSet<GroceryList> Lists { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<Household> Households { get; set; }

        public ListeryDbContext() : base("DefaultConnection")
        {
            // Migrates database do latest version automatically
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ListeryDbContext, Migrations.Configuration>());

            // Workaround to avoid DLL error
            var ensureDLLIsCopied =
                System.Data.Entity.SqlServer.SqlProviderServices.Instance;

            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Definition of 'Subject' as a primary key on User entity
            modelBuilder.Entity<User>().HasKey(t => t.Subject);
            modelBuilder.Entity<User>().Property(t => t.Subject).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Definition of 'Subject' as a foreign key on UserClaim entity
            modelBuilder.Entity<UserClaim>()
                .HasRequired(c => c.User)
                .WithMany(c => c.Claims)
                .HasForeignKey(c => c.Subject)
                .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Linq;
using Identity.Management.Models;
using Identity.Management.DataAccess.ModelsMapping;

namespace Identity.Management.DataAccess
{
    /// <summary>
    /// the ApplicationDbContext
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IDbContext//, IIdentityUserContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        //public DbSet<AccountGroup> AccountGroups { get; set; }
        //public DbSet<AccountUser> AccountUsers { get; set; }
        //public DbSet<Vendor> Vendors { get; set; }
        //public DbSet<Account> Accounts { get; set; }

        ////UserRef is readonly entity that refs to User
        //public DbSet<UserReference> UserRef { get; protected set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("Application");
            //modelBuilder.Entity<Account>().ToTable("Account");
            //modelBuilder.Entity<Vendor>().ToTable("Vendor");
            //modelBuilder.Entity<AccountGroup>().ToTable("AccountGroup");

            OnIdentityModelsCreating(modelBuilder);
            OnApplicationModelsCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            //if (ChangeTracker.Entries()
            //     .Any(x => x.Metadata.ClrType == typeof(UserReference)))
            //{
            //    //  throw new InvalidOperationException(DbContextHelper.ReadOnlyEntityMsg);
            //}
            return base.SaveChanges();
        }

        private void OnApplicationModelsCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new EventConfig());
            //modelBuilder.ApplyConfiguration(new EssenceEventConfig());
            //modelBuilder.ApplyConfiguration(new AccountUserConfig());

            ////set value objects
            //modelBuilder.Entity<Event<UnexpectedActivityDetails>>().OwnsOne(s => s.Details);
            //modelBuilder.Entity<Event<UnexpectedEntryExitDetails>>().OwnsOne(s => s.Details).OwnsOne(p => p.Period);
            //modelBuilder.Entity<Event<StayHomeDetails>>().OwnsOne(s => s.Details);
            //modelBuilder.Entity<Event<PowerDetails>>().OwnsOne(s => s.Details);
            //modelBuilder.Entity<Event<BatteryDetails>>().OwnsOne(s => s.Details);
            //modelBuilder.Entity<Event<PanelStatusDetails>>().OwnsOne(s => s.Details);
            //modelBuilder.Entity<Event<FallAlertDetails>>().OwnsOne(s => s.Details);
            //modelBuilder.Entity<Event<EmergencyPanicDetails>>().OwnsOne(s => s.Details);

            ////seeding 
            //modelBuilder.Entity<Vendor>().HasData(new Vendor(EventVendors.ESSENCE));

            ////test seeding data
            //modelBuilder.Entity<AccountGroup>().HasData(new AccountGroup() { Name = "TestGroup" });
        }

        private void OnIdentityModelsCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ApplicationUserConfig());
            //modelBuilder.ApplyConfiguration(new UserReferenceConfig());

            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable("Role", "Identity");
            });

            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRole", "Identity");
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaim", "Identity");
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogin", "Identity");
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaim", "Identity");

            });

            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserToken", "Identity");
            });

        }
    }
}

using Identity.Management.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Management.DataAccess.ModelsMapping
{
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("User", "Identity");
            builder.HasKey(a => a.Id);
            builder.Property(x => x.UserName).HasColumnName("UserName");
            builder.Property(x => x.Email).HasColumnName("Email");
            builder.Property(x => x.PhoneNumber).HasColumnName("PhoneNumber");
        }

    }
}

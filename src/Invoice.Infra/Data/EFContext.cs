﻿using Invoice.Domain.IdDocumentTypes;
using Invoice.Domain.Users;
using Invoice.Infra.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Invoice.Infra.Data
{
    public class EFContext : DbContext
    {
        public EFContext(DbContextOptions<EFContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<IdDocumentType> IdDocumentType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new IdDocumentTypeMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
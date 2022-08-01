﻿using Microsoft.EntityFrameworkCore;
using Invoice.Domain.Entities;
using Invoice.Infra.Mappings;

namespace Invoice.Infra.Data
{
    public class EFContext : DbContext
    {
        public EFContext(DbContextOptions<EFContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<TipoDocumentoId> TipoDocumentoId { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new TipoDocumentoIdMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
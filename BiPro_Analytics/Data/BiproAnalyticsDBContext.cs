using BiPro_Analytics.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiPro_Analytics.Data
{
    public class BiproAnalyticsDBContext : IdentityDbContext
    {
        public BiproAnalyticsDBContext(DbContextOptions<BiproAnalyticsDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Empresa> Empresas { get; set; }
        public virtual DbSet<Trabajador> Trabajadores { get; set; }
        public virtual DbSet<RiesgosTrabajador> RiesgosTrabajadores { get; set; }
        public virtual DbSet<RegistroPrueba> RegistroPruebas { get; set; }
        public virtual DbSet<Incapacidad> Incapacidades { get; set; }
        public virtual DbSet<UsuarioTrabajador> UsuariosTrabajadores { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Unidad> Unidades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Trabajador>()
            //    .HasOne(p => p.Empresa)
            //    .WithMany(b => b.Trabajadores);

            modelBuilder.Entity<Trabajador>()
                .HasOne(p => p.Empresa).WithMany(b => b.Trabajadores).HasForeignKey("FK_EmpresasId").IsRequired();


            base.OnModelCreating(modelBuilder);
        }

    }
}

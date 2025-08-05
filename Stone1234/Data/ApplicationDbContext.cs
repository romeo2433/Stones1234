using Microsoft.EntityFrameworkCore;
using Stones.Models;

namespace Stones.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Utilisateur> Utilisateur { get; set; }
        public DbSet<Qualite> Qualite { get; set; }
        public DbSet<Carat_> Carat_ { get; set; }
        public DbSet<Forme> Forme { get; set; }
        public DbSet<Pierre> Pierres { get; set; }
        public DbSet<MouvementStock> MouvementStock { get; set; }
        public DbSet<MouvementStockDetail> MouvementStockDetail { get; set; }
        public DbSet<Pavillon> Pavillons { get; set; }
        public DbSet<Ville> Villes { get; set; }
        public DbSet<PierreStockViewModel> PierreStockViewModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PierreStockViewModel>().HasNoKey();

            modelBuilder.Entity<Utilisateur>().ToTable("utilisateur");
            modelBuilder.Entity<Qualite>().ToTable("qualite");
            modelBuilder.Entity<Carat_>().ToTable("carat_");
            modelBuilder.Entity<Forme>().ToTable("forme");
            modelBuilder.Entity<Pierre>().ToTable("pierres");
            modelBuilder.Entity<MouvementStock>().ToTable("mouvement_stock");
            modelBuilder.Entity<MouvementStockDetail>().ToTable("mouvement_stock_detail");
            modelBuilder.Entity<Pavillon>().ToTable("pavillons");
            modelBuilder.Entity<Ville>().ToTable("villes");

            // Configurations des relations...
            // (Gardez vos configurations existantes ici)
        }
    }
}
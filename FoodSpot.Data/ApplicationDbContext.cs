using Microsoft.EntityFrameworkCore;
using FoodSpot.Model;

namespace FoodSpot.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<RestaurantModel> Restaurants { get; set; }
        public DbSet<ReviewModel> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração para Category
            modelBuilder.Entity<CategoryModel>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).HasMaxLength(200).IsRequired();
                entity.Property(c => c.Description).HasMaxLength(500);
            });

            // Configuração para User
            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).HasMaxLength(200).IsRequired();
                entity.Property(u => u.Email).HasMaxLength(200).IsRequired();
                entity.Property(u => u.PasswordHash).HasMaxLength(500).IsRequired();

                // Mapear boolean para NUMBER(1) no Oracle
                entity.Property(u => u.IsActive)
                    .HasConversion<int>()
                    .HasDefaultValue(1);
            });

            // Configuração para Restaurant
            modelBuilder.Entity<RestaurantModel>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Name).HasMaxLength(200).IsRequired();
                entity.Property(r => r.Description).HasMaxLength(1000);
                entity.Property(r => r.Address).HasMaxLength(300);
                entity.Property(r => r.Phone).HasMaxLength(20);
                entity.Property(r => r.ImageUrl).HasMaxLength(500);
                entity.Property(r => r.AverageRating).HasPrecision(5, 2);

                entity.HasOne(r => r.Category)
                    .WithMany(c => c.Restaurants)
                    .HasForeignKey(r => r.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuração para Review
            modelBuilder.Entity<ReviewModel>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Rating).IsRequired();
                entity.Property(r => r.Comment).HasMaxLength(2000);

                entity.HasOne(r => r.User)
                    .WithMany(u => u.Reviews)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Restaurant)
                    .WithMany(rest => rest.Reviews)
                    .HasForeignKey(r => r.RestaurantId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ============================================
            // SEED DATA - DADOS INICIAIS PARA TESTE
            // ============================================

            // Seed de Categorias
            modelBuilder.Entity<CategoryModel>().HasData(
                new CategoryModel
                {
                    Id = 1,
                    Name = "Italiana",
                    Description = "Comida Italiana",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new CategoryModel
                {
                    Id = 2,
                    Name = "Japonesa",
                    Description = "Comida Japonesa",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new CategoryModel
                {
                    Id = 3,
                    Name = "Brasileira",
                    Description = "Comida Brasileira",
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            // Seed de Restaurantes
            modelBuilder.Entity<RestaurantModel>().HasData(
                new RestaurantModel
                {
                    Id = 1,
                    Name = "La Pasta",
                    Description = "Autêntica comida italiana com receitas tradicionais da Toscana",
                    Address = "Rua das Flores, 123 - São Paulo/SP",
                    Phone = "(11) 3000-0000",
                    ImageUrl = "https://via.placeholder.com/400x200?text=La+Pasta",
                    CategoryId = 1,
                    AverageRating = 4.5,
                    ReviewCount = 10,
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new RestaurantModel
                {
                    Id = 2,
                    Name = "Sakura Sushi",
                    Description = "O melhor sushi e sashimi da cidade, com peixes frescos diariamente",
                    Address = "Av. Paulista, 456 - São Paulo/SP",
                    Phone = "(11) 3001-0000",
                    ImageUrl = "https://via.placeholder.com/400x200?text=Sakura",
                    CategoryId = 2,
                    AverageRating = 4.8,
                    ReviewCount = 25,
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new RestaurantModel
                {
                    Id = 3,
                    Name = "Churrascaria Gaúcha",
                    Description = "Carne de primeira qualidade no sistema rodízio",
                    Address = "Rua dos Pampas, 789 - São Paulo/SP",
                    Phone = "(11) 3002-0000",
                    ImageUrl = "https://via.placeholder.com/400x200?text=Churrascaria",
                    CategoryId = 3,
                    AverageRating = 4.7,
                    ReviewCount = 15,
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            // Configuração global para mapear boolean para NUMBER(1) no Oracle
            configurationBuilder.Properties<bool>()
                .HaveConversion<int>();
        }
    }
}

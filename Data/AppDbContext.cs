using ProjectManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ProjectManager.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Employee> Employees => Set<Employee>();

        public DbSet<Project> Projects => Set<Project>();

        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        public DbSet<TaskCategory> Categories => Set<TaskCategory>();

        public DbSet<TaskLog> TaskLogs => Set<TaskLog>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employees");

                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);

                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);

                entity.Property(e => e.Department).HasMaxLength(50);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Projects");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);

                entity.Property(p => p.Description).HasMaxLength(500);

                entity.Ignore(p => p.IsOverdue);

                entity.Ignore(p => p.DurationDays);
            });

            modelBuilder.Entity<TaskCategory>(entity =>
            {
                entity.ToTable("TaskCategories");

                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.ToTable("Tasks");

                entity.HasKey(t => t.Id);

                entity.Property(t => t.Title).IsRequired().HasMaxLength(200);

                entity.Property(t => t.Description).HasMaxLength(2000);

                entity.HasOne(t => t.AssignedEmployee)
                      .WithMany(e => e.Tasks)
                      .HasForeignKey(t => t.AssignedEmployeeId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(t => t.Project)
                      .WithMany(p => p.Tasks)
                      .HasForeignKey(t => t.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(t => t.Category)
                      .WithMany(c => c.Tasks)
                      .HasForeignKey(t => t.CategoryId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Ignore(t => t.IsOverdue);

                entity.Ignore(t => t.PriorityDisplay);

                entity.Ignore(t => t.StatusDisplay);
            });

            modelBuilder.Entity<TaskLog>(entity =>
            {
                entity.ToTable("TaskLogs");

                entity.HasKey(l => l.Id);

                entity.Property(l => l.Description).IsRequired().HasMaxLength(1000);

                entity.HasOne(l => l.TaskItem)
                      .WithMany(t => t.Logs)
                      .HasForeignKey(l => l.TaskItemId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public async Task ExecuteInTransactionAsync(Func<Task> operation)
        {
            using var transaction = await Database.BeginTransactionAsync();

            try
            {
                await operation();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();

                throw;
            }
        }
    }
}

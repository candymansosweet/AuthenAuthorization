using Domain;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using System.Linq.Expressions;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AssignGroup> AssignGroups { get; set; }
        public DbSet<GroupPermission> GroupPermissions { get; set; }
        public DbSet<AssignPermission> AssignPermissions { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Áp dụng Global Query Filter cho tất cả các entity kế thừa từ BaseModel
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()) // lấy tất cả các entities được định nghĩa hoặc liên quan trong DbSet
            {
                if (typeof(BaseModel).IsAssignableFrom(entityType.ClrType)) // chỉ áp dụng với các class kế thừa từ BaseModel
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e"); // 
                    var propertyMethod = typeof(EF).GetMethod("Property").MakeGenericMethod(typeof(bool));
                    var isDeletedProperty = Expression.Call(propertyMethod, parameter, Expression.Constant("IsDeleted"));
                    var compareExpression = Expression.Equal(isDeletedProperty, Expression.Constant(false));
                    var lambda = Expression.Lambda(compareExpression, parameter);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                    //IgnoreQueryFilters
                }
            }
        }
        // ghi đè để trước khi lưu data, cập nhật ngày tháng, xóa cứng chuyển sang xóa mềm
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }
        private void OnBeforeSaving() 
        {
            try
            {
                IEnumerable<EntityEntry> entries = ChangeTracker.Entries();
                DateTime utcNow = DateTime.UtcNow;

                foreach (EntityEntry entry in entries)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            // Set UpdatedDate to current date/time for updated entities
                            entry.Property("UpdatedDate").CurrentValue = utcNow;
                            break;
                        case EntityState.Added:
                            // Set CreatedDate and UpdatedDate to current date/time for new entities
                            entry.Property("CreatedDate").CurrentValue = utcNow;
                            entry.Property("UpdatedDate").CurrentValue = utcNow;
                            break;
                        case EntityState.Detached:
                            break;
                        case EntityState.Unchanged:
                            break;
                        case EntityState.Deleted:
                            // Thực hiện xóa mềm
                            entry.State = EntityState.Modified;
                            entry.Property("IsDeleted").CurrentValue = true;
                            entry.Property("UpdatedDate").CurrentValue = utcNow;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }

        }
    }
}
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder
            .UseSqlServer("Data Source=.;Initial Catalog=AuthenAuthorization;Integrated Security=True;TrustServerCertificate=True");
        return new AppDbContext(optionsBuilder.Options);
    }
}
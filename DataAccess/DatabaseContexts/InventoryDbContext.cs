using CoreLogic.Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DatabaseContexts;
public class InventoryDbContext(DbContextOptions<InventoryDbContext> options) : DbContext(options)
{
    public DbSet<Device> Devices { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<DeviceStatus> DeviceStatuses { get; set; }
    public DbSet<MovementReason> MovementReasons { get; set; }
    public DbSet<DeviceMovement> DeviceMovements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Индексы для фильтрации и сортировки
        modelBuilder.Entity<Device>()
            .HasIndex(d => d.CurrentDepartmentId);
        
        modelBuilder.Entity<Device>()
            .HasIndex(d => d.CurrentStatusId);
        
        modelBuilder.Entity<Device>()
            .HasIndex(d => d.InventoryNumber);

        modelBuilder.Entity<Device>()
            .HasIndex(d => d.Sticker);
        
        modelBuilder.Entity<DeviceMovement>()
            .HasIndex(m => new { m.DeviceId, m.MovedAt })
            .IsDescending(false, true);
        
        modelBuilder.Entity<DeviceMovement>()
            .HasIndex(m => new { m.ToDepartmentId, m.MovedAt });
        
        // Отношения
        modelBuilder.Entity<Device>()
            .HasOne(d => d.CurrentDepartment)
            .WithMany()
            .HasForeignKey(d => d.CurrentDepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Device>()
            .HasOne(d => d.CurrentStatus)
            .WithMany()
            .HasForeignKey(d => d.CurrentStatusId);
        
        modelBuilder.Entity<DeviceMovement>()
            .HasOne(m => m.Device)
            .WithMany(d => d.Movements)
            .HasForeignKey(m => m.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DeviceMovement>()
            .HasOne(m => m.FromDepartment)
            .WithMany()
            .HasForeignKey(m => m.FromDepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<DeviceMovement>()
            .HasOne(m => m.ToDepartment)
            .WithMany()
            .HasForeignKey(m => m.ToDepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<DeviceMovement>()
            .HasOne(m => m.Reason)
            .WithMany()
            .HasForeignKey(m => m.ReasonId);
    }
}
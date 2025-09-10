using Microsoft.EntityFrameworkCore;
using MagicSignal.Modules.Accounts.Domain.Entities;
using MagicSignal.Modules.Accounts.Infrastructure.Persistence.Configurations;

namespace MagicSignal.Modules.Accounts.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    // همه رو به یک شکل تغییر دادم
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<VipMembershipRequests> VipMembershipRequests { get; set; }
    public DbSet<VipRequest> VipRequests { get; set; }
    
    // داخل کلاس AccountsDbContext این DbSet رو اضافه کن:
    public DbSet<AdminApproval> AdminApprovals { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<VipMembershipRequests>().ToTable("VipMembershipRequests");
        // User to Role relationship
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Article Configuration
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.Content)
                .IsRequired();
            
            entity.Property(e => e.Summary)
                .HasMaxLength(500);
            
            // Relationship with User (Author)
            entity.HasOne(e => e.Author)
                .WithMany()
                .HasForeignKey(e => e.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Relationship with Category
            entity.HasOne(e => e.Category)
                .WithMany(c => c.Articles)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Category Configuration
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.Description)
                .HasMaxLength(500);
            
            entity.HasIndex(e => e.Name)
                .IsUnique();
        });
        //  این رو اضافه کن:

        modelBuilder.Entity<AdminApproval>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AdminComment).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.DecisionDate).IsRequired();
            entity.HasIndex(e => e.VipRequestId);
            entity.HasIndex(e => e.AdminUserId);
        });
        
        // VipRequest configuration
        modelBuilder.Entity<VipRequest>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RequestType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
            entity.Property(e => e.CreatedAt).IsRequired();
        
            // Foreign key به User
            entity.HasIndex(e => e.UserId);
        });

        

        // Apply configurations
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
    }
}
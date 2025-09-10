namespace MagicSignal.Modules.Accounts.Domain.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!; // Admin, VipUser, User
    public string Description { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation property برای Many-to-Many
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    
    // Navigation property برای One-to-Many  
    public ICollection<User> Users { get; set; } = new List<User>();
}
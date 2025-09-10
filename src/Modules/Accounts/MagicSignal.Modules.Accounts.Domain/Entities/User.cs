using System.ComponentModel.DataAnnotations.Schema;

namespace MagicSignal.Modules.Accounts.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    
    // نگهداری پسورد به‌صورت هش
    public string PasswordHash { get; set; } = default!;

    // اختیاری: اگر بخوای Password خام فقط در زمان ساخت استفاده بشه، اینطوری باشه
    [NotMapped]
    public string? Password { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    
    // اضافه کنید - برای One-to-Many
    public Guid RoleId { get; set; }
    public virtual Role Role { get; set; } = null!;
    
    // نگه دارید - برای Many-to-Many
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    
    // Propertyهای اصلی که در دیتابیس هستند
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Phone { get; set; } = "";

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";

    // اضافه کردن PhoneNumber
    [NotMapped]
    public string PhoneNumber => Phone;
     
    public int status { get; set; }

}
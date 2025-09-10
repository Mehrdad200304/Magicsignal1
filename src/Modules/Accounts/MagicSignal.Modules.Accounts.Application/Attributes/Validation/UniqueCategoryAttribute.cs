using System.ComponentModel.DataAnnotations;
using MagicSignal.Modules.Accounts.Application.Interfaces.Services;

namespace MagicSignal.Modules.Accounts.Application.Attributes.Validation
{
    public class UniqueCategoryAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success; // اگه خالیه، Required خودش چک می‌کنه
            }

            var categoryService = validationContext.GetService(typeof(ICategoryService)) as ICategoryService;
            if (categoryService == null)
            {
                return new ValidationResult("سرویس دسته‌بندی در دسترس نیست");
            }

            var categories = categoryService.GetAllAsync().Result;
            var existingCategory = categories.FirstOrDefault(c => 
                c.Name.Equals(value.ToString(), StringComparison.OrdinalIgnoreCase));
            
            if (existingCategory != null)
            {
                return new ValidationResult("دسته‌بندی با این نام قبلاً وجود دارد");
            }

            return ValidationResult.Success;
        }
    }
}
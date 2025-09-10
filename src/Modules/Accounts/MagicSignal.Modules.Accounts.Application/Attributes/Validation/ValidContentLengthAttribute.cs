using System.ComponentModel.DataAnnotations;

namespace MagicSignal.Modules.Accounts.Application.Attributes.Validation
{
    public class ValidContentLengthAttribute : ValidationAttribute
    {
        private readonly int _minLength;
        private readonly int _maxLength;

        public ValidContentLengthAttribute(int minLength = 10, int maxLength = 50000)
        {
            _minLength = minLength;
            _maxLength = maxLength;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("محتوا نمی‌تواند خالی باشد");
            }

            var content = value.ToString();
            if (string.IsNullOrWhiteSpace(content))
            {
                return new ValidationResult("محتوا نمی‌تواند خالی باشد");
            }

            if (content.Length < _minLength)
            {
                return new ValidationResult($"محتوا باید حداقل {_minLength} کاراکتر باشد");
            }

            if (content.Length > _maxLength)
            {
                return new ValidationResult($"محتوا نمی‌تواند بیش از {_maxLength} کاراکتر باشد");
            }

            // چک کردن کلمات نامناسب (اختیاری)
            var inappropriateWords = new[] { "spam", "fake" };
            if (inappropriateWords.Any(word => content.ToLower().Contains(word)))
            {
                return new ValidationResult("محتوا شامل کلمات نامناسب است");
            }

            return ValidationResult.Success;
        }
    }
}
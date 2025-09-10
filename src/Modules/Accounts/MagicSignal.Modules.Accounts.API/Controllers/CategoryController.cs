using MagicSignal.Modules.Accounts.Application.Interfaces.Services;
using MagicSignal.Modules.Accounts.Domain.Entities;
using MagicSignal.Modules.Accounts.Application.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MagicSignal.Modules.Accounts.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/category
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        // GET: api/category/{id}
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        // POST: api/category
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            // مرحله ۱: بررسی ModelState
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "داده‌های ورودی نامعتبر است",
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            // مرحله ۲: Validation های سفارشی
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                return BadRequest(ApiResponse<Category>.ErrorResponse("نام دسته‌بندی نمی‌تواند خالی باشد"));
            }

            if (category.Name.Length > 100)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "نام دسته‌بندی نمی‌تواند بیش از ۱۰۰ کاراکتر باشد"
                });
            }
            //Business Logic Validation (اضافه کن بعد از validation های قبلی)
        
            // چک کردن کلمات ممنوع در نام دسته‌بندی
            var forbiddenWords = new[] { "test", "fake", "spam", "admin" };
            if (forbiddenWords.Any(word => category.Name.ToLower().Contains(word)))
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "نام دسته‌بندی شامل کلمات غیرمجاز است"
                });
            }

            // حداقل ۳ کاراکتر برای نام
            if (category.Name.Length < 3)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "نام دسته‌بندی باید حداقل ۳ کاراکتر باشد"
                });
            }

            // اگه توضیحات خالی نیست، باید حداقل ۱۰ کاراکتر باشه
            if (!string.IsNullOrEmpty(category.Description) && category.Description.Length < 10)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "توضیحات باید حداقل ۱۰ کاراکتر باشد یا خالی باشد"
                });
            }

            // مرحله ۳: بررسی تکراری نبودن
            var categories = await _categoryService.GetAllAsync();
            var existingCategory = categories.FirstOrDefault(c => 
                c.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase));
            if (existingCategory != null)
            {
                return Conflict(new
                {
                    Success = false,
                    Message = "دسته‌بندی با این نام قبلاً وجود دارد"
                });
            }
            
            try
            {
                // مرحله ۴: ایجاد دسته‌بندی
                var result = await _categoryService.AddAsync(category);
                
                return CreatedAtAction(
                    nameof(GetById), 
                    new { id = result.Id }, 
                    new
                    {
                        Success = true,
                        Message = "دسته‌بندی با موفقیت ایجاد شد",
                        Data = result
                    });
            }
            catch (Exception ex)
            {
                // مرحله ۵: Error Handling
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "خطای داخلی سرور",
                    Error = ex.Message
                });
            }
        }

        // PUT: api/category/{id}
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Category category)
        {
            if (id != category.Id)
                return BadRequest();

            await _categoryService.UpdateAsync(category);
            return NoContent();
        }

        // DELETE: api/category/{id}
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }

        // GET: api/category/exists/{id}
        [HttpGet("exists/{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> Exists(Guid id)
        {
            var exists = await _categoryService.ExistsAsync(id);
            return Ok(exists);
        }
    }
}
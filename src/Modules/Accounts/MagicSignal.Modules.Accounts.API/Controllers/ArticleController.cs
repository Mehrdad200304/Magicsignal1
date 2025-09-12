using MagicSignal.Modules.Accounts.Application.Interfaces.Services;
using MagicSignal.Modules.Accounts.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MagicSignal.Modules.Accounts.Application.DTOs.Article;


namespace MagicSignal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService; // اضافه کردم

        public ArticleController(IArticleService articleService, ICategoryService categoryService) // تغییر کردم
        {
            _articleService = articleService;
            _categoryService = categoryService; // اضافه کردم
        }

        // GET: api/article
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var articles = await _articleService.GetAllAsync();
            return Ok(articles);
        }

        // GET: api/article/{id}
        [HttpGet("{id:guid}")] // تغییر کردم از guid به int
        public async Task<IActionResult> GetById(Guid id)
        {
            var article = await _articleService.GetByIdAsync(id);
            if (article == null)
                return NotFound();
            return Ok(article);
        }

        // GET: api/article/category/{categoryId}
        [HttpGet("category/{categoryId:guid}")] // تغییر کردم از guid به int
        public async Task<IActionResult> GetByCategory(Guid categoryId)
        {
            var articles = await _articleService.GetByCategoryIdAsync(categoryId);
            return Ok(articles);
        }

        // GET: api/article/published
        [HttpGet("published")]
        public async Task<IActionResult> GetPublished()
        {
            var articles = await _articleService.GetPublishedArticlesAsync();
            return Ok(articles);
        }

        // POST: api/article
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ArticleDto articleDto)
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
            if (string.IsNullOrWhiteSpace(articleDto.Title))
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "عنوان مقاله نمی‌تواند خالی باشد"
                });
            }
            Article article = new Article
            {
                Title = articleDto.Title,
                Content = articleDto.Content,
                Summary = articleDto.Summary,
                AuthorId = articleDto.AuthorId,
                CategoryId = articleDto.CategoryId,
                IsPublished = articleDto.IsPublished
            };

            if (articleDto.Title.Length < 5)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "عنوان مقاله باید حداقل ۵ کاراکتر باشد"
                });
            }

            if (articleDto.Title.Length > 200)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "عنوان مقاله نمی‌تواند بیش از ۲۰۰ کاراکتر باشد"
                });
            }
            if (string.IsNullOrWhiteSpace(articleDto.Content))
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "محتوای مقاله نمی‌تواند خالی باشد"
                });
            }

            // مرحله ۳: Business Logic Validation
            var forbiddenWords = new[] { "fake", "spam", "scam" };
            if (forbiddenWords.Any(word => articleDto.Title.ToLower().Contains(word)))
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "عنوان مقاله شامل کلمات غیرمجاز است"
                });
            }
            
           if (!string.IsNullOrEmpty(articleDto.Summary) && articleDto.Summary.Length < 10)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "خلاصه مقاله باید حداقل ۱۰ کاراکتر باشد یا خالی باشد"
                });
            }

            if (articleDto.Content.Trim().Length < 50)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "محتوای مقاله باید حداقل ۵۰ کاراکتر معنی‌دار باشد"
                });
            }

            // مرحله ۴: بررسی وجود Author و Category
            if (articleDto.AuthorId == Guid.Empty)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "شناسه نویسنده معتبر نیست"
                });
            }

            if (articleDto.CategoryId == Guid.Empty)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "شناسه دسته‌بندی معتبر نیست"
                });
            }

            // مرحله ۵: بررسی وجود Category
            var categoryExists = await _categoryService.ExistsAsync(articleDto.CategoryId);
            if (!categoryExists)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "دسته‌بندی انتخاب شده وجود ندارد"
                });
            }

            try
            {
                var result = await _articleService.AddAsync(article);

                return Ok(new
                {
                    Success = true,
                    Message = "مقاله با موفقیت ایجاد شد",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "خطای داخلی سرور",
                    Error = ex.Message
                });
            }
        }

        // PUT: api/article/{id}
        [HttpPut("{id:guid}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Article article)
        {
            if (id != article.Id)
                return BadRequest();

            await _articleService.UpdateAsync(article);
            return NoContent();
        }

        // DELETE: api/article/{id}
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _articleService.DeleteAsync(id);
            return NoContent();
        }

        // PATCH: api/article/{id}/increment-view
        [HttpPatch("{id:guid}/increment-view")]
        public async Task<IActionResult> IncrementView(Guid id)
        {
            await _articleService.IncrementViewCountAsync(id);
            return NoContent();
        }
    }
}
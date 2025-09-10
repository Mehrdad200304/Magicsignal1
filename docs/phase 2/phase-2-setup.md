✅ مرحله ۲: ماژول‌بندی اولیه (مدت: ۱ تا ۲ هفته)

اقدامات روز ۱ تا ۲: طراحی موجودیت‌ها (Entities)
ساخت موجودیت User با فیلدهای:
Id, Username, Email, Password, Role
ساخت موجودیت Role مثل:
Admin, VipUser, RegularUser
ساخت رابطه‌ی UserRole (برای ارتباط چند-به‌چند اگر نیاز بود)
افزودن Seed Data برای تست اولیه
- پیکربندی DbContext و اتصال به دیتابیس
- ایجاد Migration و اعمال آن روی دیتابیس


〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰



اقدامات روز ۵تا ۷:

ساخت PasswordHashingService

ویژگی‌های کلیدی:
- HashPassword: تولید hash امن از password ورودی
- VerifyPassword: مقایسه password با hash ذخیره شده
- Automatic Salt: BCrypt خودکار salt تصادفی تولید می‌کند
- Configurable Cost: امکان تنظیم سختی محاسبه


ساخت اینترفیس IPasswordHashingService.cs
مزایای استفاده از Interface:
- Dependency Injection: قابلیت تزریق وابستگی
- Testability: امکان Mock کردن برای تست
- Flexibility: قابلیت تعویض implementation
- Clean Architecture: جداسازی business logic از infrastructure

ثبت در Dependency Injection
تغییر AuthService Construct
تغییر SeedData برای Hash کردن

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

اقدامات روز ۸ تا ۹:

طراحی و پیاده‌سازی کلاس‌های موجودیت Article و Category برای ماژول محتوا در پروژه

#ساختار موجودیت‌ها

ساخت  Category Entity

مسئولیت: مدیریت دسته‌بندی مقالات

فیلدها:
- Id (int): شناسه یکتا
- Name (string): نام دسته‌بندی (حداکثر ۱۰۰ کاراکتر)
- Description (string?): توضیحات اختیاری (حداکثر ۵۰۰ کاراکتر)
- CreatedDate (DateTime): تاریخ ایجاد (پیش‌فرض: زمان فعلی)
- IsActive (bool): وضعیت فعال/غیرفعال (پیش‌فرض: true)

ساخت Article Entity

مسئولیت: مدیریت محتوای مقالات

فیلدها:
- Id (int): شناسه یکتا
- Title (string): عنوان مقاله (حداکثر ۲۰۰ کاراکتر)
- Content (string): محتوای کامل مقاله
- Summary (string?): خلاصه مقاله (حداکثر ۵۰۰ کاراکتر)
- AuthorId (int): شناسه نویسنده (FK to User)
- CategoryId (int): شناسه دسته‌بندی (FK to Category)
- CreatedDate (DateTime): تاریخ ایجاد
- UpdatedDate (DateTime?): تاریخ آخرین بروزرسانی
- IsPublished (bool): وضعیت انتشار (پیش‌فرض: false)
- ViewCount (int): تعداد بازدید (پیش‌فرض: 0)

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

اقدامات روز ۱۰ تا ۱۲:

پیاده‌سازی کنترلرهای RESTful API برای مدیریت مقالات (Articles) و دسته‌بندی‌ها (Categories) در لایه Web

ساخت ArticleController
مسئولیت‌ها:
مدیریت CRUD عملیات مقالات -
- پیاده‌سازی RESTful endpoints
- استفاده از ArticleService برای business logic

ساخت CategoryController
مسئولیت‌ها:
- مدیریت CRUD عملیات دسته‌بندی‌ها
- پیاده‌سازی RESTful endpoints
- استفاده از CategoryService برای business logic


تنظیم Route ها و HTTP Methods
Route Patterns پیاده‌سازی شده:
- /api/[controller] - Base route pattern
- RESTful conventions (GET, POST, PUT, DELETE)
- Parameter binding برای ID ها

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

اقدامات روز ۱۳ تا ۱۴:

پیاده‌سازی سیستم کنترل دسترسی مبتنی بر نقش (Role-based Access Control) و JWT Authentication برای API های محتوایی

وضعیت فعلی Authentication
موارد بررسی شده:
- ✅ AuthService موجود و کامل
- ✅ JWT Token generation آماده
- ✅ Login/Register methods پیاده‌سازی شده
- ✅ User/Role entities موجود
تنظیم JWT در Program.cs
تغییرات اعمال شده:

// JWT Authentication Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
      ValidAudience = builder.Configuration["JwtSettings:Audience"],
      IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!))
    };
  });

// Swagger JWT Support
c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme { ... });
c.AddSecurityRequirement(new OpenApiSecurityRequirement { ... });

AuthController بررسی

اقدامات روز  ۱۵:ساخت پروژه Unit Test برای AuthService با استفاده از xUnit، Moq، و In-Memory Database

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

اقدامات روز  ۱۵: ساخت پروژه Unit Test برای AuthService با استفاده از xUnit، Moq، و In-Memory Database

ساخت Test Class
اضافه کردن Project References
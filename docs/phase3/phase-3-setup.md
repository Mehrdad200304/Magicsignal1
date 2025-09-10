✅ مرحله ۳: پیاده‌سازی Workflow ها

اقدامات گام 1 : نصب WorkflowCore package

بررسی و تأیید نصب پکیج
فایل پروژه (.csproj) را باز کنید و وجود خط زیر را بررسی میکنیم:

//<PackageReference Include="WorkflowCore" Version="X.Y.Z" />

این خط نشان‌دهنده نصب موفقیت‌آمیز پکیج است

تنظیمات اولیه WorkflowCore در پروژه

### اضافه کردن فضای نام
در فایل Program.cs، فضای نام WorkflowCore را اضافه میکنیم:

using WorkflowCore.Interface;

ثبت سرویس در DI Container
سرویس WorkflowCore را در DI کانتینر ثبت میکنیم

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

اقدامات گام 2:  تنظیم Program.cs و Dependency  Injection
اضافه کردن WorkflowCore Service

تنظیم SQL Server Persistenc
جایگزین کردن کد WorkflowCore Service

تنظیم Workflow Host

نتیجه گام ۲
- ✅ WorkflowCore به پروژه اضافه شده
- ✅ EntityFramework persistence تنظیم شده
- ✅ Hosted Service برای اجرای workflow فعال شده
- ✅ WorkflowHost شروع به کار کرده

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

اقدامات گام 3 : ساخت VIP Request Entity

# توضیح Properties VipRequest:
- Id: کلید اصلی (Primary Key)
- UserId: شناسه کاربر درخواست‌دهنده
- RequestType: نوع درخواست VIP
- Status: وضعیت درخواست (Pending, Approved, Rejected)
- Description: توضیحات اختیاری
- CreatedAt: زمان ایجاد درخواست
- UpdatedAt: زمان آخرین بروزرسانی
- ProcessedAt: زمان پردازش درخواست
- ProcessedBy: شناسه کاربری که درخواست را پردازش کرده


تعریف Relationships

### Navigation Properties:
`csharp
// Navigation Properties - Relationships
[ForeignKey("UserId")]
public virtual User? User { get; set; }

[ForeignKey("ProcessedBy")]
public

توضیح Relationships:
- User: کاربری که درخواست VIP داده است
- ProcessedByUser: ادمین/کاربری که درخواست را بررسی و پردازش کرده
- virtual: برای Lazy Loading در Entity Framework
- ?: Nullable چون ممکن اس


اضافه کردن DbSet VipRequest به DbContext

تنظیم VipRequest Entitiy در OnModelCre
توضیح:
- DbSet<VipRequest>: برای دسترسی به جدول در Entity Framework
- VipMembershipRequests: نام DbSet و نام جدول در دیتابیس
- .ToTable(): مشخص کردن نام دقیق جدول

ایجاد و اعمال Migration

✅ چک لیست تکمیل گام ۳

-  قدم ۱: Entity Class ایجاد شد
-  قدم ۲: Properties تعریف شدند
-  قدم ۳: Relationships برقرار شدند
-  قدم ۴: DbContext پیکربندی شد
-  قدم ۵: Migration اجرا شد

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

اقدامات گام 4: طراحی DTOs مورد نیاز

ساخت  فایل‌های مورد نیاز DTOs

ساخت VipRegistrationRequestDto
ساخت VipRegistrationResponseDto
ساخت AdminApprovalDto
ساخت VipStatusDto

توضیحات  DTOs

### VipRegistrationRequestDto
هدف: دریافت اطلاعات درخواست VIP از کاربر
- UserId: شناسه کاربر درخواست‌کننده
- PackageType: نوع پکیج (ماهانه/سالانه)
- Amount: مبلغ پرداختی
- PaymentMethod: روش پرداخت

### VipRegistrationResponseDto
هدف: ارسال پاسخ درخواست VIP به کاربر
- Success: وضعیت موفقیت عملیات
- Message: پیام توضیحی
- RequestId: شناسه درخواست برای پیگیری
- ExpirationDate: تاریخ انقضای VIP
- TransactionId: کد پیگیری تراکنش

### AdminApprovalDto
هدف: تأیید یا رد درخواست توسط ادمین
- RequestId: شناسه درخواست مورد بررسی
- IsApproved: تصمیم ادمین (تأیید/رد)
- AdminComment: نظر اختیاری ادمین

### VipStatusDto
هدف: نمایش وضعیت VIP کاربر
- UserId: شناسه کاربر
- IsVip: آیا کاربر VIP است
- ExpirationDate: تاریخ انقضای VIP
- PackageType: نوع پکیج فعال

# ✅ نتیجه گام ۴
- ✅ ۴ DTO اساسی برای workflow VIP ساخته شد
- ✅ ساختار مناسب برای انتقال داده‌ها آماده شد
- ✅ DTOs برای کاربر و ادمین جداگانه طراحی شد
- ✅ فیلدهای مورد نیاز برای پیگیری و مدیریت تعریف شد

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

اقدامات گام 5: ساخت Step های مورد نیاز برای workflow ثبت ‌نام VIP با استفاده از WorkflowCore

ساخت پوشه Steps

ساخت فایل BaseWorkflowStep
ساخت فایل VipWorkflowData
ساخت فایل CheckPaymentStep
ساخت فایل ProcessPaymentStep
ساخت فایل SendToAdminStep
ساخت فایل ActivateVipStep
ساخت فایل SendNotificationStep


🎯 #توضیحات Workflow Steps

1. CheckPaymentStep      → چک اعتبار پرداخت
2. ProcessPaymentStep    → پردازش پرداخت
3. SendToAdminStep       → ارسال درخواست به ادمین
4. WaitForApprovalStep   → انتظار تأیید ادمین
5. ActivateVipStep       → فعال‌سازی VIP
6. SendNotificationStep  → اطلاع‌رسانی موفقیت


- ✅ نتیجه گام ۵
- ✅ ۸ کلاس Step مختلف ساخته شد
- ✅ هر Step وظیفه مشخصی در workflow دارد
- ✅ Error handling و exception management اضافه شد
- ✅ جریان کامل از پرداخت تا فعال‌سازی VIP طراحی شد
- ✅ قابلیت tracking وضعیت در هر مرحله

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

اقدامات گام 6 : ترکیب همه Step های ساخته شده در یک workflow کامل و ایجاد API Controller برای مدیریت درخواست‌های VIP

ساخت فایل VipRegistrationWorkflow


تنظیم Workflow در Program.cs
اضافه کردن Using ها در Program.cs
using MagicSignal.Application.Workflows;
using MagicSignal.Application.Workflows.Steps;


ثبت Workflow و Steps در Services

ساخت Controller برای شروع Workflow
ساخت فایل VipWorkflowController

اضافه کردن API Endpoints به فایل VipWorkflowController

تنظیم Dependency Injection

✅ نتیجه گام ۶
- ✅ Workflow کامل با ۶ Step پیاده‌سازی شد
- ✅ Controller با ۳ endpoint مختلف ساخته شد
- ✅ Integration با DTOs انجام شد
- ✅ Dependency Injection تنظیم شد
- ✅ جریان کامل از درخواست تا فعال‌سازی VIP آماده شد

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

اقدامات گام 7 : تکمیل و بهبود Controller های مورد نیاز برای مدیریت workflow های VIP و ساخت Admin Panel

تکمیل فایل VipWorkflowControlle

#بهبودهای اعمال شده:
- ✅ اضافه کردن try-catch blocks برای error handling
- ✅ اضافه کردن validation برای input data
- ✅ بهبود response format و messages
- ✅ اضافه کردن method های جدید

ساخت AdminController


تنظیم Authorization در فایل AdminController
using Microsoft.AspNetCore.Authorization;
[Authorize(Roles = "Admin")]  // فقط Admin ها دسترسی دارن
public class AdminController : ControllerBase


تنظیم Authorizationدر فایل VipWorkflowController
using Microsoft.AspNetCore.Authorization;
[Authorize]  // کاربران login شده دسترسی دارن
public class VipWorkflowController : ControllerBase

✅ نتیجه گام ۷
- ✅ VipWorkflowController کامل و بهبود یافت
- ✅ AdminController با 4 endpoint ساخته شد
- ✅ Authorization و Role-based access اضافه شد
- ✅ Error handling و validation تقویت شد
- ✅ 9 endpoint مختلف آماده استفاده
- ✅ Response format استاندارد شد

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

اقدامات گام 8: تست اولیه workflow

تست VIPRegistrationWorkflow

تست کامل جریان ثبت‌نام VIP

بررسی Response
- ✅ Success: true برگردانده شد
- ✅ Message فارسی و مناسب بود
- ✅ RequestId تولید شد
- ✅ TransactionId (WorkflowId) صحیح بود
- ✅ WorkflowCore به درستی

تست Admin Panel APIs
تست endpoint های AdminController و Authorization
تست Authorization Scenarios

تست Error Handling
##هدف: بررسی مدیریت خطاها در سیستم
تست VipWorkflow Error

- ✅ نتیجه گام ۸
- ✅ ۱۵+ تست مختلف انجام شد
- ✅ همه API endpoints تست و تأیید شدند
- ✅ Error handling در سطح مناسبی قرار دارد
- ✅ Integration testing موفق بود
- ✅ سیستم آماده استفاده است
- ✅ مشکلات امنیتی یافت نشد
- ✅ Performance تحت فشار قابل قبول است

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

اقدامات گام 9 : ساخت Admin Approval Entity

طراحی و پیاده‌سازی سیستم تأیید/رد درخواست‌های VIP توسط ادمین با ایجاد Entity مخصوص و سرویس‌های مربوطه

طراحی Relations** ✅
- AdminApproval ←→ VipRequest (One-to-Many)
- AdminApproval ←→ **User

ساخت فایل Entity

ساخت Repository و Service
ساخت IAdminApprovalRepository Interface✅

پیاده‌سازی Repository در Infrastructure✅
ساخت AdminApprovalRepository

اضافه کردن DbSet به ApplicationDbContext✅

ساخت AdminApprovalService

عملیات Service:
- ApproveRequestAsync: تأیید درخواست VIP
- RejectRequestAsync: رد درخواست VIP
- GetApprovalStatisticsAsync: آمار تأیید/رد
- IsRequestAlreadyProcessedAsync: جلوگیری از پردازش مجدد

اعمال Migration و Configuration

نتیجه گام ۹

- ✅ AdminApproval Entity کامل طراحی و پیاده‌سازی شد
- ✅ Repository Pattern با interface و implementation
- ✅ Service Layer برای business logic
- ✅ Database Migration موفق انجام شد
- ✅ CRUD Operations کامل با عملیات تخصصی
- ✅ Error Handling و Validation مناسب
- ✅ Performance Optimization با indexing
- ✅ Soft Delete برای data integrity

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

گام 10: پیاده‌سازی Approval Service
پیاده‌سازی کامل سرویس تأیید/رد درخواست‌های VIP توسط ادمین و ادغام آن با کنترلرها و WorkflowCore

تنظیم Dependency Injection
Program.cs (در پروژه API)
- DependencyInjection.cs (در پروژه Application یا Infrastructure)
- ServiceCollectionExtensions.cs

ثبت Repositor

ثبت Service
AdminApprovalServicevرو ثبت میکنیم

تست DI Configurati

اتصال Service به Controller ها

آپدیت AdminController Constructor
✅اضافه کردن IAdminApprovalService و تنظیم Dependency Injection

جایگزینی Mock Data با Service

آپدیت Method های Controller
بهبود endpoint ها و اضافه کردن exception handling


نتیجه
✅ Pagination اضافه شد
✅ Filtering برای کاربران VIP
✅ Validation بهتر
✅ Exception Handling کامل‌تر
✅ Response Format یکسان

ایجاد WorkflowStep برای Admin Approval
فایل:Application/Workflows/Steps/AdminApprovalStep

تریگر Workflow از Controller
آپدیت AdminController برای تریگر Events

آپدیت VIPRequestStatus

Notification به کاربر
ساخت فایل NotificationStep.cs

ساخت Workflow Definiti
ساخت فایل VipApprovalWorkflow.cs


ثبت Workflow و Steps در Program.cs
// WorkflowCore
builder.Services.AddWorkflow();
// Steps
builder.Services.AddTransient<AdminApprovalStep>();
builder.Services.AddTransient<UpdateVipStatusStep>();
builder.Services.AddTransient<NotificationStep>();


# 📋 فایل‌های ایجاد/تغییر یافته:
1. ✅ AdminApprovalStep.cs - جدید
2. ✅ UpdateVipStatusStep.cs - جدید
3. ✅ NotificationStep.cs - جدید
4. ✅ VipApprovalWorkflow.cs - جدید
5. ✅ AdminController.cs - آپدیت شده
6. ✅ VipWorkflowController.cs - تصحیح شده
7. ✅ Program.cs - آپدیت شده

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰
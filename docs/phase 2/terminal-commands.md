✅Command و ترمینال های مورد نیاز روز ۱ تا ۲

🟢## Migration و Database Setup

✅### دستورات Migration

✅# رفتن به مسیر اصلی پروژه
cd C:\Users\Mehrdad\MagicSignal

✅# ایجاد Migration
dotnet ef migrations add Init_AccountsSchema --project src/Modules/Accounts/MagicSignal.Modules.Accounts.Infrastructure --startup-project src/MagicSignal.API

✅# اعمال Migration
dotnet ef database update --project src/Modules/Accounts/MagicSignal.Modules.Accounts.Infrastructure --startup-project src/MagicSignal.API

### نصب و راه‌اندازی

#### پیش‌نیازها


✅- SQL Server Express / LocalDB



✅نصب پکیج‌های EF Core


`bash
dotnet add src/MagicSignal.API package Microsoft.EntityFrameworkCore.Design
dotnet add src/Modules/Accounts/MagicSignal.Modules.Accounts.Infrastructure package Microsoft.EntityFrameworkCore
dotnet add src/Modules/Accounts/MagicSignal.Modules.Accounts.Infrastructure package Microsoft.EntityFrameworkCore.SqlServer

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰
✅Command و ترمینال های مورد نیاز روز  ۱۵

پکیج‌های نصب شده

# Mock کردن Dependencies
dotnet add tests/MagicSignal.Modules.Accounts.Tests package Moq

# In-Memory Database برای تست
dotnet add tests/MagicSignal.Modules.Accounts.Tests package Microsoft.EntityFrameworkCore.InMemory


اضافه کردن Project References
# Reference به Application Layer
dotnet add tests/MagicSignal.Modules.Accounts.Tests reference src/Modules/Accounts/MagicSignal.Modules.Accounts.Application

# Reference به Domain Layer
dotnet add tests/MagicSignal.Modules.Accounts.Tests reference src/Modules/Accounts/MagicSignal.Modules.Accounts.Domain
🛠️ مراحل ایجاد پروژه و دستورات اجرا شده

✅ ایجاد راه‌حل (Solution) و پروژه‌ها👇‌

dotnet new sln -n MagicSignal
dotnet new classlib -n MagicSignal.Domain -o src/MagicSignal.Domain
dotnet new classlib -n MagicSignal.Application -o src/MagicSignal.Application
dotnet new classlib -n MagicSignal.Infrastructure -o src/MagicSignal.Infrastructure
dotnet new webapi -n MagicSignal.API -o src/MagicSignal.API

# افزودن پروژه‌ها به فایل Solution
dotnet sln MagicSignal.sln add src/MagicSignal.Domain
dotnet sln MagicSignal.sln add src/MagicSignal.Application
dotnet sln MagicSignal.sln add src/MagicSignal.Infrastructure
dotnet sln MagicSignal.sln add src/MagicSignal.API


✅ تنظیم وابستگی بین پروژه‌ها👇‌

dotnet add src/MagicSignal.Application/MagicSignal.Application.csproj reference src/MagicSignal.Domain/MagicSignal.Domain.csproj
dotnet add src/MagicSignal.Infrastructure/MagicSignal.Infrastructure.csproj reference src/MagicSignal.Domain/MagicSignal.Domain.csproj
dotnet add src/MagicSignal.API/MagicSignal.API.csproj reference src/MagicSignal.Application/MagicSignal.Application.csproj
dotnet add src/MagicSignal.API/MagicSignal.API.csproj reference src/MagicSignal.Infrastructure/MagicSignal.Infrastructure.csproj

✅ ایجاد ساختار ماژول Accounts👇‌

dotnet new classlib -n MagicSignal.Modules.Accounts.API -o src/Modules/Accounts/MagicSignal.Modules.Accounts.API
dotnet new classlib -n MagicSignal.Modules.Accounts.Application -o src/Modules/Accounts/MagicSignal.Modules.Accounts.Application
dotnet new classlib -n MagicSignal.Modules.Accounts.Domain -o src/Modules/Accounts/MagicSignal.Modules.Accounts.Domain
dotnet new classlib -n MagicSignal.Modules.Accounts.Infrastructure -o src/Modules/Accounts/MagicSignal.Modules.Accounts.Infrastructure

✅ افزودن وابستگی بین پروژه‌های ماژول Accounts👇‌

# اضافه کردن رفرنس‌ها بین لایه‌های ماژول Accounts
dotnet add src/Modules/Accounts/MagicSignal.Modules.Accounts.Application/MagicSignal.Modules.Accounts.Application.csproj reference src/Modules/Accounts/MagicSignal.Modules.Accounts.Domain/MagicSignal.Modules.Accounts.Domain.csproj
dotnet add src/Modules/Accounts/MagicSignal.Modules.Accounts.Infrastructure/MagicSignal.Modules.Accounts.Infrastructure.csproj reference src/Modules/Accounts/MagicSignal.Modules.Accounts.Application/MagicSignal.Modules.Accounts.Application.csproj
dotnet add src/Modules/Accounts/MagicSignal.Modules.Accounts.Infrastructure/MagicSignal.Modules.Accounts.Infrastructure.csproj reference src/Modules/Accounts/MagicSignal.Modules.Accounts.Domain/MagicSignal.Modules.Accounts.Domain.csproj

dotnet add src/Modules/Accounts/MagicSignal.Modules.Accounts.API/MagicSignal.Modules.Accounts.API.csproj reference src/Modules/Accounts/MagicSignal.Modules.Accounts.Application/MagicSignal.Modules.Accounts.Application.csproj
dotnet add src/Modules/Accounts/MagicSignal.Modules.Accounts.API/MagicSignal.Modules.Accounts.API.csproj reference src/Modules/Accounts/MagicSignal.Modules.Accounts.Domain/MagicSignal.Modules.Accounts.Domain.csproj
dotnet add src/Modules/Accounts/MagicSignal.Modules.Accounts.API/MagicSignal.Modules.Accounts.API.csproj reference src/Modules/Accounts/MagicSignal.Modules.Accounts.Infrastructure/MagicSignal.Modules.Accounts.Infrastructure.csproj

# رفرنس API ماژول به API اصلی
dotnet add src/MagicSignal.API/MagicSignal.API.csproj reference src/Modules/Accounts/MagicSignal.Modules.Accounts.API/MagicSignal.Modules.Accounts.API.csproj

✅ افزودن پروژه‌های ماژول به فایل Solution👇‌

dotnet sln MagicSignal.sln add src/Modules/Accounts/MagicSignal.Modules.Accounts.API
dotnet sln MagicSignal.sln add src/Modules/Accounts/MagicSignal.Modules.Accounts.Application
dotnet sln MagicSignal.sln add src/Modules/Accounts/MagicSignal.Modules.Accounts.Domain
dotnet sln MagicSignal.sln add src/Modules/Accounts/MagicSignal.Modules.Accounts.Infrastructure

✅ افزودن Swagger برای مستندسازی API👇‌

cd src/MagicSignal.API
dotnet add package Swashbuckle.AspNetCore

و در Program.cs:
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (app.Environment.IsDevelopment())
{
app.UseSwagger();
app.UseSwaggerUI();
}
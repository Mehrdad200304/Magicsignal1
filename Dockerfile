# Build stage - مرحله ساخت
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution file
COPY ["MagicSignal.sln", "./"]

# Copy main project files
COPY ["src/MagicSignal.API/MagicSignal.API.csproj", "src/MagicSignal.API/"]
COPY ["src/MagicSignal.Application/MagicSignal.Application.csproj", "src/MagicSignal.Application/"]
COPY ["src/MagicSignal.Domain/MagicSignal.Domain.csproj", "src/MagicSignal.Domain/"]
COPY ["src/MagicSignal.Infrastructure/MagicSignal.Infrastructure.csproj", "src/MagicSignal.Infrastructure/"]

# Copy Modules project files
COPY ["src/Modules/Accounts/MagicSignal.Modules.Accounts.API/MagicSignal.Modules.Accounts.API.csproj", "src/Modules/Accounts/MagicSignal.Modules.Accounts.API/"]
COPY ["src/Modules/Accounts/MagicSignal.Modules.Accounts.Application/MagicSignal.Modules.Accounts.Application.csproj", "src/Modules/Accounts/MagicSignal.Modules.Accounts.Application/"]
COPY ["src/Modules/Accounts/MagicSignal.Modules.Accounts.Domain/MagicSignal.Modules.Accounts.Domain.csproj", "src/Modules/Accounts/MagicSignal.Modules.Accounts.Domain/"]
COPY ["src/Modules/Accounts/MagicSignal.Modules.Accounts.Infrastructure/MagicSignal.Modules.Accounts.Infrastructure.csproj", "src/Modules/Accounts/MagicSignal.Modules.Accounts.Infrastructure/"]

# Restore dependencies - بازیابی وابستگی‌ها
RUN dotnet restore "MagicSignal.sln"

# Copy everything else - کپی بقیه فایل‌ها
COPY . .

# Build the application - ساخت اپلیکیشن
WORKDIR "/src/src/MagicSignal.API"
RUN dotnet build "MagicSignal.API.csproj" -c Release -o /app/build

# Publish stage - مرحله انتشار
FROM build AS publish
RUN dotnet publish "MagicSignal.API.csproj" -c Release -o /app/publish

# Runtime stage - مرحله اجرا
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copy published app - کپی اپلیکیشن منتشر شده
COPY --from=publish /app/publish .

# Expose port - باز کردن پورت
EXPOSE 80
EXPOSE 443

# Set entry point - تعیین نقطه ورود
ENTRYPOINT ["dotnet", "MagicSignal.API.dll"]
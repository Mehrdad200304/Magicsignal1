using MagicSignal.Modules.Accounts.DependencyInjection;
using MagicSignal.Modules.Accounts.Application.DependencyInjection;
using MagicSignal.Modules.Accounts.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MagicSignal.Modules.Accounts.Infrastructure.Persistence;
using MagicSignal.Modules.Accounts.Infrastructure.Services;
using Microsoft.OpenApi.Models;
using MagicSignal.API.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Services;
using WorkflowCore.Persistence.EntityFramework;
using WorkflowCore.Persistence.SqlServer;
using MagicSignal.Modules.Accounts.Application.Workflows;
using MagicSignal.Modules.Accounts.Application.Workflows.Steps;
using MagicSignal.Modules.Accounts.Application.Interfaces.Repositories;
using MagicSignal.Modules.Accounts.Application.Interfaces.Services;
using MagicSignal.Modules.Accounts.Infrastructure.Repositories;
using MagicSignal.Modules.Accounts.Application.Services;
//using MagicSignal.Modules.Accounts.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// اضافه کردن سرویس‌ها
builder.Services.AddAccountsModule();

// 👇 اضافه کردن ApplicationDbContext به DI
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔐 JWT Authentication Configuration
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

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer();
//builder.Services.AddSignalR();


// اضافه کردن workflow
builder.Services.AddTransient<VipRegistrationWorkflow>();

// ثبت تمام Step ها
builder.Services.AddTransient<CheckPaymentStep>();
builder.Services.AddTransient<ProcessPaymentStep>();
builder.Services.AddTransient<SendToAdminStep>();
builder.Services.AddTransient<WaitForApprovalStep>();
builder.Services.AddTransient<ActivateVipStep>();
builder.Services.AddTransient<SendNotificationStep>();
builder.Services.AddTransient<AdminApprovalStep>();

builder.Services.AddScoped<IAdminApprovalRepository, AdminApprovalRepository>();
builder.Services.AddScoped<IAdminApprovalService, AdminApprovalService>();
builder.Services.AddWorkflow(cfg =>
{
    cfg.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), true, true);
});

builder.Services.AddAutoMapper(typeof(Program)); // یا Assembly که mapping profiles دارید


// builder.Services.AddTransient<IWorkflowHost, WorkflowHost>();
// 🔐 Swagger با JWT Support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Accounts API",
        Version = "v1"
    });
    
    // JWT Authorization برای Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();


// فعال‌سازی Swagger فقط در محیط توسعه
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});


app.MapControllers();

// 👇 اجرای SeedData
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.InitializeAsync(services).GetAwaiter().GetResult();
}
// Force SeedData
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    
    // پاک کردن و دوباره ساختن
    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();
    
    await SeedData.InitializeAsync(services);
}
var workflowHost = app.Services.GetRequiredService<IWorkflowHost>();
workflowHost.RegisterWorkflow<VipRegistrationWorkflow, MagicSignal.Modules.Accounts.Application.Workflows.VipWorkflowData>();
workflowHost.RegisterWorkflow<VipApprovalWorkflow, MagicSignal.Modules.Accounts.Application.Workflows.VipWorkflowData>();
workflowHost.Start();
app.Run();
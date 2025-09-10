✅ساختار های مورد نیاز روز گام 3

🟢ساختار VipRequest Entitiy
src/Modules/Accounts/MagicSignal.Modules.Accounts.Domain/Entitie

🟢ساختار ApplicationDbContext
src/Modules/Accounts/MagicSignal.Modules.Accounts.Infrastructure/Persistence/ApplicationDbContext.cs

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

✅ساختار های مورد نیاز روز گام 4

MagicSignal.Modules.Accountd.Application/
└── DTOs/
└── Workflow/
├── VipRegistrationRequestDto.cs
├── VipRegistrationResponseDto.cs
├── AdminApprovalDto.cs
└── VipStatusDto.cs

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

✅ساختار های مورد نیاز روز گام 5

# 📋 ساختار نهایی فایل‌ها
src/Modules/Accounts/MagicSignal.Modules.Accounts.Application/Workflow/Steps

Workflows/
└── Steps/
             ├── BaseWorkflowStep.cs (کلاس پایه)
             ├── VipWorkflowData.cs (داده‌های workflow)
             ├── CheckPaymentStep.cs (چک پرداخت)
             ├── ProcessPaymentStep.cs (پردازش پرداخت)
             ├── SendToAdminStep.cs (ارسال به ادمین)
             ├── WaitForApprovalStep.cs (انتظار تأیید ادمین)
             ├── ActivateVipStep.cs (فعال‌سازی VIP)
             └── SendNotificationStep.cs (اطلاع‌رسانی)

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

✅ساختار های مورد نیاز روز گام 6

ساختار پوشه:

MagicSignal.Application/
└── Workflows/
├── Steps/ (قبلی) ✅
└── VipRegistrationWorkflow.cs ←

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

✅ساختار های مورد نیاز روز گام 7

ساختار پوشه AdminController:
src/Modules/Accounts/MagicSignal.Modules.Accounts.API/Controllers/AdminController.cs

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

✅ساختار های مورد نیاز روز گام 9

Domain/Entities/
└── AdminApproval.cs✅

Application/Interfaces/
├── Repositories/
│   └── IAdminApprovalRepository.cs✅
└── Services/
    └── IAdminApprovalService.cs✅

Application/Services/
└── AdminApprovalService.cs✅

Infrastructure/Repositories/
└── AdminApprovalRepository.cs✅

Infrastructure/Data/
└── AccountsDbContext.cs✅ (آپدیت شده)

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

✅ساختار های مورد نیاز روز گام 10

Application/Workflows/
├── Steps/
│  └──AdminApprovalSteps.cs✅
   └──UpdateVipStatusStep.cs✅ 
   └──NotificationStep.cs✅
└── Vip approval workflow.cs✅
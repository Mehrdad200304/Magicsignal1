MagicSignal/
│
├── MagicSignal.sln
├── src/
│   ├── MagicSignal.API/             → پروژه اصلی Web API
│   ├── MagicSignal.Application/     → لایه منطق برنامه (UseCases)
│   ├── MagicSignal.Domain/          → لایه دامنه (Entities, Interfaces)
│   ├── MagicSignal.Infrastructure/  → لایه زیرساخت (EF Core, Services)
│   └── MagicSignal.Modules/
│       └── Accounts/                → ماژول حساب‌ها (با ساختار جداگانه)
│           ├── Accounts.API/
│           ├── Accounts.Application/
│           ├── Accounts.Domain/
│           └── Accounts.Infrastructure/
│
└── tests/                           → محل قرارگیری تست‌ها (فعلاً خالی)
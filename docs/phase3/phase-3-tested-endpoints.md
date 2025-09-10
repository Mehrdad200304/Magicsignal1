✅ تست گام ۶
## 📋 API Endpoints ایجاد شده

| Method | Endpoint | توضیح |
|--------|----------|-------|
| POST | /api/vip-workflow/register | شروع workflow ثبت‌نام VIP |
| GET | /api/vip-workflow/status/{requestId} | چک وضعیت درخواست VIP |
| POST | /api/vip-workflow/admin-approval | تأیید/رد درخواست توسط ادمین |


〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

✅ تست گام ۷

## 📋 API Endpoints ایجاد شده

### VipWorkflowController:
| Method | Endpoint | توضیح |
|--------|----------|-------|
| POST | /api/vip-workflow/register | شروع workflow ثبت‌نام VIP |
| GET | /api/vip-workflow/status/{requestId} | چک وضعیت درخواست VIP |
| POST | /api/vip-workflow/admin-approval | تأیید/رد درخواست توسط ادمین |
| DELETE | /api/vip-workflow/cancel/{requestId} | لغو درخواست VIP |
| GET | /api/vip-workflow/admin/requests | لیست همه درخواست‌ها برای ادمین |

### AdminController:
| Method | Endpoint | توضیح |
|--------|----------|-------|
| GET | /api/admin/pending-requests | درخواست‌های در انتظار تأیید |
| POST | /api/admin/approve/{requestId} | تأیید درخواست VIP |
| POST | /api/admin/reject/{requestId} | رد درخواست VIP |
| GET | /api/admin/users | لیست کاربران |

〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰〰

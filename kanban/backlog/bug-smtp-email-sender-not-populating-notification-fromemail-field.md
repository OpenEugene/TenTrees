---
priority: medium
tags: []
---

# Bug: SMTP Email Sender not populating Notification FromEmail field

## 🐛 Bug Description

SMTP is configured correctly with an Email Sender, but when notifications are created, the `FromEmail` field in the Notification table remains **NULL/empty**, causing "Invalid Sender: <>" errors when attempting to send emails.

## 📋 Environment

- **Platform**: Oqtane (.NET 10)
- **Deployment**: MonsterASP production server
- **Database**: SQL Server (db38494.databaseasp.net)
- **Configuration**: `Server/appsettings.Production.json`

## 🔍 Current Configuration

### SMTP Settings (Admin Panel)
```
Enabled: Yes
Host: mail1001.mailasp.net
Port: 587
SSL Options: Automatic
Authentication: Basic
Username: postmaster@tentrees.org
Password: (configured)
Email Sender: postmaster@tentrees.org ✅
```

### appsettings.Production.json
```json
{
  "RenderMode": "Interactive",
  "Runtime": "WebAssembly",
  "Database": {
    "DefaultDBType": "Oqtane.Database.SqlServer.SqlServerDatabase, Oqtane.Server"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=db38494.databaseasp.net; ..."
  },
  "Localization": {
    "DefaultCulture": "en-ZA"
  }
}
```

**Missing**: `Installation.HostEmail` configuration

## ❌ Actual Behavior

When notifications are created (e.g., via "Test SMTP Configuration"):

**Database Record:**
```
NotificationId: 7
FromEmail: [EMPTY/NULL] ❌
ToEmail: postmaster@tentrees.org
Subject: 10Trees SMTP Configuration Test
Body: SMTP Server Is Configured Correctly
```

**Result:**
```
Notification Job: Succeeded
Processing Notifications For Site: Default Site
Notification Id: 3 Has An Invalid Sender: <> And Has Been Deleted
Notifications Delivered: 0
```

## ✅ Expected Behavior

Notifications should use the configured **SMTP Email Sender** as the default FROM address:

```
NotificationId: 7
FromEmail: postmaster@tentrees.org ✅
ToEmail: postmaster@tentrees.org
Subject: 10Trees SMTP Configuration Test
```

## 🔍 Root Cause Analysis

Oqtane checks these sources for notification `FromEmail` in order:
1. Notification.FromEmail (explicitly set in code)
2. Site.Email (site default email)
3. **Host User Email** (from Installation.HostEmail) ← Missing!
4. NULL (causes error)

Without `Installation.HostEmail` in appsettings:
- No Host user email is set during installation
- Notifications default to NULL FromEmail
- SMTP sends fail with "Invalid Sender" error

## 🔧 Proposed Solution

Add Installation section to `Server/appsettings.Production.json`:

```json
{
  "RenderMode": "Interactive",
  "Runtime": "WebAssembly",
  "Database": {
    "DefaultDBType": "Oqtane.Database.SqlServer.SqlServerDatabase, Oqtane.Server"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=db38494.databaseasp.net; ..."
  },
  "Installation": {
    "HostEmail": "postmaster@tentrees.org",
    "HostPassword": "TempPassword2026!",
    "DefaultAlias": "tentrees.org"
  },
  "Localization": {
    "DefaultCulture": "en-ZA"
  }
}
```

This will:
1. Create Host user with email `postmaster@tentrees.org`
2. Use this as default FROM for all notifications
3. Fix "Invalid Sender" errors

## 📝 Workaround

**Option 1: SQL Update**
```sql
UPDATE [Notification] 
SET FromEmail = 'postmaster@tentrees.org'
WHERE FromEmail IS NULL OR FromEmail = '';
```

**Option 2: Manual Configuration**
1. Admin Dashboard → Site Settings
2. Set Host Email to `postmaster@tentrees.org`
3. Restart application

## ✅ Acceptance Criteria

- [ ] Add Installation section to appsettings.Production.json
- [ ] Verify Host user is created with correct email
- [ ] Test SMTP configuration successfully sends email
- [ ] Verify Notification.FromEmail is populated with postmaster@tentrees.org
- [ ] Document requirement in DEPLOYMENT.md
- [ ] Update .github/copilot-instructions.md with Oqtane configuration best practices

## 🔗 Related Files

- `Server/appsettings.Production.json`
- `DEPLOYMENT.md`
- `.github/copilot-instructions.md`

## 📊 Priority

**Medium** - Blocks email notifications in production but has workarounds

## 🏷️ Labels

- bug
- configuration
- production
- email
- oqtane

---

**Environment:** Production MonsterASP  
**Branch:** feature/5-village-data-management  
**Discovered:** 2026-01-19

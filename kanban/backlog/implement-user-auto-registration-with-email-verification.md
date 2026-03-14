---
priority: medium
tags: []
---

# Implement User Auto-Registration with Email Verification

## Feature: User Auto-Registration and Email Infrastructure

Implement user self-registration functionality with email verification to allow mentors and staff to create accounts independently, with appropriate email infrastructure for notifications.

### Priority: Medium
**Tags:** `security`, `infrastructure`, `user-management`

### Module Requirements
- Configure email service (SMTP)
- Extend Oqtane user registration
- Create email templates (bilingual: en-ZA, ts-ZA)
- Configure email verification workflow
- Create registration approval process (optional)
- Create bilingual resource files

### Email Infrastructure Setup Checklist

#### ✅ SMTP Configuration
- [ ] Configure email service provider (e.g., SendGrid, SMTP2GO, Gmail SMTP, Office365)
- [ ] Add SMTP credentials to configuration
  - Host
  - Port
  - Username
  - Password
  - Enable SSL/TLS
- [ ] Store credentials securely (Azure Key Vault, user secrets, environment variables)
- [ ] Configure email sender address (e.g., noreply@10trees.org)
- [ ] Test SMTP connectivity
- [ ] Set up fallback email provider (optional)

#### ✅ Email Service Implementation
- [ ] Create `IEmailService` interface
- [ ] Implement email service in Server/Services
- [ ] Configure dependency injection
- [ ] Implement email queue for reliability
- [ ] Add email logging
- [ ] Handle email sending failures gracefully
- [ ] Implement retry logic for failed emails

#### ✅ Email Templates
- [ ] Create welcome email template (English)
- [ ] Create welcome email template (Xitsonga)
- [ ] Create email verification template (English)
- [ ] Create email verification template (Xitsonga)
- [ ] Create password reset template (English)
- [ ] Create password reset template (Xitsonga)
- [ ] Create account approval notification (English)
- [ ] Create account approval notification (Xitsonga)
- [ ] Support HTML and plain text versions
- [ ] Include 10 Trees branding/logo

### User Registration Checklist

#### ✅ Registration Form
- [ ] Create user registration page
- [ ] Collect required fields:
  - Full name
  - Email address
  - Password (with strength requirements)
  - Confirm password
  - Preferred language (en-ZA or ts-ZA)
  - Optional: Phone number
- [ ] Display role selection (or default to Mentor)
- [ ] Implement client-side validation
- [ ] Implement server-side validation
- [ ] Check for duplicate email addresses
- [ ] Bilingual form labels and validation messages

#### ✅ Email Verification Workflow
- [ ] Generate email verification token
- [ ] Send verification email with link
- [ ] Create email verification page
- [ ] Verify token and activate account
- [ ] Display success message
- [ ] Redirect to login page
- [ ] Set expiration on verification tokens (e.g., 24 hours)
- [ ] Allow resend verification email
- [ ] Prevent login until email verified

#### ✅ Registration Approval (Optional)
- [ ] Admin approval workflow:
  - User registers
  - Admin receives notification email
  - Admin reviews and approves/rejects
  - User receives approval/rejection email
- [ ] Admin approval UI in User Administration module
- [ ] Pending approval status indicator
- [ ] Auto-approval for specific domains (optional)

#### ✅ Role and Village Assignment
- [ ] Default role: Mentor (or "Pending")
- [ ] Admin can assign proper role after registration
- [ ] Admin can assign village after registration
- [ ] Optional: Self-select village during registration (dropdown)
- [ ] Enforce role-based restrictions until approved
- [ ] Send notification email when role/village assigned

#### ✅ Integration with Oqtane
- [x] Leverage Oqtane's built-in registration (if compatible)
- [ ] Extend Oqtane UserController
- [ ] Configure Oqtane email settings
- [ ] Test with Oqtane authentication flow
- [ ] Ensure compatibility with existing user roles
- [ ] Preserve Oqtane audit logging

### Security Checklist

#### ✅ Password Requirements
- [ ] Minimum 8 characters
- [ ] Require uppercase and lowercase
- [ ] Require number
- [ ] Require special character
- [ ] Display password strength indicator
- [ ] Prevent common passwords

#### ✅ Account Security
- [ ] Rate limiting on registration attempts
- [ ] CAPTCHA to prevent bot registrations
- [ ] Email verification required before login
- [ ] Account lockout after failed login attempts
- [ ] Secure token generation for email verification
- [ ] Prevent email enumeration attacks
- [ ] Log registration attempts

### Email Configuration

#### Example SMTP Settings (appsettings.json)
```json
{
  "EmailSettings": {
    "SmtpHost": "smtp.sendgrid.net",
    "SmtpPort": 587,
    "EnableSsl": true,
    "FromEmail": "noreply@10trees.org",
    "FromName": "10 Trees Program",
    "Username": "[encrypted]",
    "Password": "[encrypted]"
  }
}
```

### Email Template Examples

#### Welcome Email (English)
```
Subject: Welcome to 10 Trees Digital Platform

Dear [Name],

Welcome to the 10 Trees Digital Tracking Platform!

Please verify your email address by clicking the link below:
[Verification Link]

This link will expire in 24 hours.

If you did not create this account, please ignore this email.

Best regards,
10 Trees Program Team
```

#### Welcome Email (Xitsonga)
```
Subject: Ku amukeriwa eka 10 Trees Digital Platform

[Xitsonga translation - requires bilingual tester input]
```

### User Experience

#### ✅ Registration Flow
1. User navigates to registration page
2. User fills out registration form
3. User submits form
4. System sends verification email
5. User clicks verification link in email
6. Account activated
7. Admin assigns role and village (if required)
8. User receives confirmation email
9. User can log in

#### ✅ UI/UX Considerations
- [ ] Mobile-friendly registration form
- [ ] Clear instructions in both languages
- [ ] Progress indicators
- [ ] Friendly error messages
- [ ] Success confirmation page
- [ ] Link to login page after verification

### Testing Checklist

#### ✅ Functional Testing
- [ ] Test registration form validation
- [ ] Test email sending
- [ ] Test email verification link
- [ ] Test expired verification token
- [ ] Test duplicate email registration
- [ ] Test password strength requirements
- [ ] Test in both languages (en-ZA, ts-ZA)
- [ ] Test on mobile devices

#### ✅ Email Testing
- [ ] Test email delivery
- [ ] Test email formatting (HTML and plain text)
- [ ] Test links in emails
- [ ] Test email in both languages
- [ ] Test spam filter compliance
- [ ] Test with different email providers (Gmail, Outlook, etc.)

### Configuration Steps for Production

1. **Choose Email Provider**
   - SendGrid (recommended for transactional emails)
   - SMTP2GO
   - Amazon SES
   - Office 365 SMTP
   - Gmail SMTP (for testing only)

2. **Configure DNS Records** (if using custom domain)
   - SPF record
   - DKIM record
   - DMARC record
   - MX records (if receiving replies)

3. **Set Up Email Monitoring**
   - Track delivery rates
   - Monitor bounce rates
   - Monitor spam complaints
   - Set up alerts for failures

### Dependencies

- Related to Issue #4 (User Administration)
- Requires completion for Issue #5 (Village Data Management) for village assignment

### Success Criteria

- [ ] Users can register without admin intervention
- [ ] Email verification works reliably
- [ ] Emails delivered in user's preferred language
- [ ] Admins can review and assign roles/villages
- [ ] Registration process is mobile-friendly
- [ ] Email delivery monitored and reliable
- [ ] Secure password requirements enforced

### Future Enhancements

- Social login (Google, Microsoft)
- SMS verification (alternative to email)
- Two-factor authentication
- Password recovery via SMS
- Biometric login for mobile

### Technical Notes

- Use ASP.NET Core Identity email features where possible
- Leverage Oqtane's existing user management infrastructure
- Queue emails for async processing
- Consider using Hangfire or similar for email queue
- Bilingual email templates use ResourceKeys
- Store email templates in database for easy editing (optional)

### Documentation

- [ ] Document SMTP setup process
- [ ] Document email template customization
- [ ] Create admin guide for managing registrations
- [ ] Document troubleshooting for email issues
- [ ] Create user guide for registration process

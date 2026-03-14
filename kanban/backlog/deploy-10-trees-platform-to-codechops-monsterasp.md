---
priority: medium
tags: []
---

# Deploy 10 Trees Platform to CodeChops MonsterASP

## Deployment: CodeChops MonsterASP Hosting

Deploy the 10 Trees Digital Tracking Platform to the CodeChops MonsterASP hosting environment for production use.

### Priority: High
**Tags:** `deployment`, `infrastructure`, `devops`

### Hosting Platform
- **Provider**: CodeChops MonsterASP
- **Application Type**: Blazor WebAssembly (Oqtane Framework)
- **Target Framework**: .NET 10
- **Database**: SQL Server

### Deployment Checklist

#### ✅ Pre-Deployment Preparation
- [ ] Verify .NET 10 SDK installed on server
- [ ] Confirm SQL Server availability and connection
- [ ] Obtain database credentials
- [ ] Verify port configuration (HTTP/HTTPS)
- [ ] Review hosting requirements with CodeChops
- [ ] Ensure server meets Oqtane framework requirements

#### ✅ Application Configuration
- [ ] Update `appsettings.json` for production environment
- [ ] Configure connection strings for production database
- [ ] Set production URLs (no dynamic port binding `:0`)
- [ ] Configure CORS policies if needed
- [ ] Set up environment variables
- [ ] Configure logging levels for production
- [ ] Disable debug mode

#### ✅ Database Setup
- [ ] Create production database on SQL Server
- [ ] Run Entity Framework migrations
- [ ] Seed initial data:
  - Villages (e.g., Orpen Gate Village, Londelozzi)
  - User roles (Mentor, Educator, Project Manager, Admin, Executive Director)
  - Initial admin user account
- [ ] Verify database connectivity from server
- [ ] Configure backup strategy

#### ✅ Build and Publish
- [ ] Build solution in Release mode
- [ ] Publish Blazor WebAssembly application
- [ ] Verify published files include:
  - Server files
  - Client WebAssembly files
  - wwwroot static assets
  - Oqtane framework dependencies
- [ ] Test published build locally before deployment
- [ ] Verify all modules compile correctly
- [ ] Check resource files (.resx) are included

#### ✅ Deployment to MonsterASP
- [ ] Upload published files to server
- [ ] Configure IIS or hosting service
- [ ] Set application pool (.NET Core)
- [ ] Configure web.config
- [ ] Set file permissions
- [ ] Configure HTTPS/SSL certificate
- [ ] Set up custom domain (if applicable)
- [ ] Configure URL rewriting for Blazor routing

#### ✅ Post-Deployment Verification
- [ ] Verify application loads in browser
- [ ] Test Blazor WebAssembly loads correctly
- [ ] Verify Oqtane installation wizard runs (first-time)
- [ ] Complete Oqtane setup wizard
- [ ] Test database connectivity
- [ ] Verify localization resources load (en-ZA, ts-ZA)
- [ ] Test Enrollment module functionality
- [ ] Test mobile responsiveness
- [ ] Verify signature capture works
- [ ] Test language switching

#### ✅ User Acceptance Testing
- [ ] Create test user accounts (Mentor, Educator, Admin)
- [ ] Test complete enrollment workflow
- [ ] Test village data isolation
- [ ] Test role-based permissions
- [ ] Test bilingual functionality (English and Xitsonga)
- [ ] Test on target devices (Nokia, Samsung smartphones)
- [ ] Verify mobile performance
- [ ] Test form validation

#### ✅ Security Configuration
- [ ] Configure firewall rules
- [ ] Set up SSL/TLS (HTTPS only)
- [ ] Configure authentication settings
- [ ] Review and restrict database access
- [ ] Configure CORS if needed
- [ ] Enable security headers
- [ ] Review Oqtane security settings

#### ✅ Monitoring and Maintenance
- [ ] Set up application logging
- [ ] Configure error tracking
- [ ] Set up database monitoring
- [ ] Configure backup schedule
- [ ] Document server access credentials (secure storage)
- [ ] Create deployment runbook
- [ ] Plan for updates and maintenance windows

#### ✅ Documentation
- [ ] Document production URL
- [ ] Document deployment process
- [ ] Document database connection details (secure)
- [ ] Create admin guide for managing users
- [ ] Create troubleshooting guide
- [ ] Document rollback procedure

### Deployment Considerations

#### Blazor WebAssembly Specifics
- Static file hosting for WebAssembly files
- Correct MIME types for `.wasm`, `.dll`, `.json` files
- Enable Brotli or gzip compression
- Configure routing for SPA (rewrite to index.html)

#### Oqtane Framework Specifics
- First-time installation wizard
- Module installation process
- Theme configuration
- Tenant configuration (single-tenant for this project)

#### Mobile Optimization
- Test on low-bandwidth connections
- Verify touch-friendly UI works correctly
- Test on small screen devices (Nokia, Samsung)
- Verify fonts are readable on small screens

### Known Issues to Address
- ⚠️ Never use dynamic port binding (`:0`) - causes Blazor WebAssembly errors
- Ensure correct culture configuration for bilingual support
- Verify signature capture works on mobile browsers

### Contact Information
- **Hosting Provider**: CodeChops MonsterASP
- **Technical Contact**: [To be added]
- **Deployment Support**: [To be added]

### Success Criteria
- [ ] Application accessible via production URL
- [ ] All modules load without errors
- [ ] Mobile devices can access and use application
- [ ] Database queries execute successfully
- [ ] Bilingual localization works correctly
- [ ] User authentication and authorization functional
- [ ] Form submissions save to database
- [ ] Signature capture works on mobile devices

### Rollback Plan
- Maintain backup of previous deployment
- Document database rollback procedure
- Keep copy of configuration files
- Test rollback procedure before deployment

### Future Deployment Considerations
- CI/CD pipeline setup (GitHub Actions)
- Automated deployment process
- Staging environment for testing
- Blue-green deployment strategy
- Database migration automation

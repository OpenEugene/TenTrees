# Deployment Guide for 10 Trees Platform

## Production Environment

### Database Configuration
- **Host**: db38494.databaseasp.net
- **Database**: db38494
- **User**: db38494
- **Connection String**: Configured in `Server/appsettings.Production.json`

### Prerequisites
- .NET 10 SDK
- Access to production server (MonsterASP hosting)
- Database already provisioned and accessible

## Deployment Steps

### 1. Build for Production

```powershell
# Navigate to Server project
cd Server

# Build in Release mode
dotnet build --configuration Release

# Publish the application
dotnet publish --configuration Release --output ./publish
```

### 2. Database Migration

Before first deployment, run migrations:

```powershell
# Update database with all migrations
dotnet ef database update --configuration Release
```

**Note**: Migrations will create:
- Village table (OpenEug.TenTreesVillage)
- Enrollment table (OpenEug.TenTreesEnrollment)
- Participant table (OpenEug.TenTreesParticipant)
- Seed data: Orpen Gate Village, Londelozzi

### 3. Deploy to MonsterASP

#### Option A: FTP Deployment
1. Connect to FTP server provided by MonsterASP
2. Upload contents of `Server/publish` folder to web root
3. Ensure `appsettings.Production.json` is included

#### Option B: Git Deploy (if supported)
```powershell
# Push to production branch
git push production main
```

### 4. Post-Deployment Verification

1. **Test Database Connection**
   - Navigate to site URL
   - Verify Oqtane installer completes
   - Check database tables created

2. **Test Village Module**
   - Login as admin
   - Navigate to Village management
   - Verify Orpen Gate Village and Londelozzi are present

3. **Test Enrollment Module**
   - Navigate to Enrollment
   - Test multi-step form workflow
   - Verify bilingual support (English/Xitsonga)

4. **Test Mobile Access**
   - Access from mobile device
   - Verify touch-friendly interface
   - Test offline capabilities

## Configuration Files

### appsettings.Production.json
Located at: `Server/appsettings.Production.json`
- Production database connection string
- Logging configured for production (Warning level)
- Default culture set to en-ZA (South African English)

### Environment Variables (Optional)
For added security, consider using environment variables:

```bash
# Set connection string via environment variable
export ConnectionStrings__DefaultConnection="Server=db38494.databaseasp.net;..."
```

## Security Checklist

- [x] Connection string in appsettings.Production.json
- [ ] Ensure appsettings.Production.json is NOT committed to public repo (add to .gitignore if needed)
- [ ] Change database password after initial deployment
- [ ] Enable HTTPS in production
- [ ] Configure CORS if needed
- [ ] Set up backup strategy for production database
- [ ] Configure error logging (Application Insights, etc.)

## Rollback Plan

If deployment fails:

1. **Restore Previous Version**
   ```powershell
   # Deploy previous publish folder
   ```

2. **Database Rollback**
   ```powershell
   # Rollback migration
   dotnet ef database update <PreviousMigrationName> --configuration Release
   ```

## Monitoring

### Health Checks
- Site URL: Check responds with 200 OK
- Database: Verify connection via admin panel
- Logs: Check for errors in Oqtane event log

### Performance Metrics
- Page load time < 3 seconds
- Database query time < 500ms
- Mobile responsive on low-end devices

## Support

**Database Provider**: MonsterASP  
**Hosting Provider**: MonsterASP  
**Project Repository**: https://github.com/OpenEugene/10Trees

## Troubleshooting

### Database Connection Fails
1. Verify connection string in appsettings.Production.json
2. Check database server allows remote connections
3. Verify firewall rules on server
4. Test connection with SQL Server Management Studio

### Migrations Fail
1. Check database user has sufficient permissions (CREATE TABLE, ALTER, etc.)
2. Verify database is empty or in correct state
3. Check migration files are included in publish output

### Module Not Appearing
1. Verify DLL files are deployed
2. Check module registration in database
3. Clear browser cache
4. Restart application pool

---

**Last Updated**: January 2026  
**Version**: 1.0.0

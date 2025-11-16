# Azure Deployment Guide for CodeBidder

## ? Deployment Configuration Created

Your GitHub Actions workflow has been configured to automatically deploy your ASP.NET Core Razor Pages application to Azure.

---

## ?? IMPORTANT: Set Up GitHub Secret

Before the deployment can work, you need to add the publish profile as a GitHub Secret.

### Steps to Add the Secret:

1. **Go to your GitHub repository**: https://github.com/PawanLambole/codebidder

2. **Navigate to Settings**:
   - Click on **Settings** tab
   - In the left sidebar, click **Secrets and variables** > **Actions**

3. **Create New Secret**:
   - Click **New repository secret**
   - Name: `AZURE_WEBAPP_PUBLISH_PROFILE`
   - Value: **Paste the ENTIRE contents of your publish profile file**
     (The XML content from `<publishData>` to `</publishData>`)

4. **Click Add secret**

---

## ?? Your Azure Configuration

### Web App Details:
- **App Name**: codebidder
- **URL**: https://codebidder-hrdec0bsaxf9hggj.centralindia-01.azurewebsites.net
- **Region**: Central India
- **Runtime**: .NET 8.0

### Database Details:
- **SQL Server**: codebidder-sql-server.database.windows.net
- **Database**: CodeBidder_db
- **Connection String**: Automatically configured via publish profile

---

## ?? How Deployment Works

### Automatic Deployment:
Every time you push code to the `master` branch, GitHub Actions will:

1. ? Checkout your code
2. ? Set up .NET 8
3. ? Restore NuGet packages
4. ? Build the project (SQMS.csproj)
5. ? Publish the application
6. ? Deploy to Azure Web App

### Manual Deployment:
You can also trigger deployment manually:
1. Go to **Actions** tab in GitHub
2. Select **Build and Deploy ASP.NET Core to Azure Web App**
3. Click **Run workflow**

---

## ?? Connection String Configuration

Your production connection string is automatically managed by Azure through the publish profile.

### Local Development:
Your `appsettings.json` uses LocalDB:
```
Server=(localdb)\\MSSQLLocalDB;Database=CodeBidder_db
```

### Production (Azure):
Automatically uses:
```
Server=tcp:codebidder-sql-server.database.windows.net,1433
Database=CodeBidder_db
```

---

## ?? Next Steps

1. ? **Add the GitHub Secret** (see above)
2. ? **Commit and push** the workflow file
3. ? **Monitor deployment** in the Actions tab
4. ? **Test your app** at the Azure URL

---

## ?? Troubleshooting

### If deployment fails:

1. **Check GitHub Actions logs**:
   - Go to Actions tab
   - Click on the failed workflow
   - Review the error messages

2. **Common issues**:
   - ? Missing GitHub secret ? Add `AZURE_WEBAPP_PUBLISH_PROFILE`
   - ? Build errors ? Check your code compiles locally
   - ? Database connection ? Verify Azure SQL firewall rules

3. **Database migrations**:
   If you need to run EF Core migrations on Azure:
   ```bash
   # Add this to your workflow after the deploy step if needed
   - name: Run database migrations
     run: |
       cd ${{ env.AZURE_WEBAPP_PACKAGE_PATH }}
       dotnet ef database update
   ```

---

## ?? Security Notes

- ? Publish profile credentials are stored securely in GitHub Secrets
- ? Database passwords are not exposed in code
- ? Connection strings are managed by Azure App Service

**IMPORTANT**: Never commit the publish profile file (`.PublishSettings`) to your repository!

---

## ?? Support

If you encounter issues:
1. Check the GitHub Actions logs
2. Review Azure App Service logs in Azure Portal
3. Verify all secrets are correctly configured

---

**Your deployment is ready!** Just add the GitHub secret and push your code. ??

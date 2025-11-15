# CodeBidder - Deployment Guide

## Azure Deployment Instructions

### Prerequisites
- Azure Account (Free tier available at https://azure.microsoft.com/free/)
- GitHub Account (already set up)

### Step-by-Step Deployment

#### 1. Create Azure Resources

**A. Create Azure App Service:**
1. Go to https://portal.azure.com
2. Click "Create a resource"
3. Search for "Web App" and click Create
4. Fill in the details:
   - **Subscription**: Choose your subscription
   - **Resource Group**: Create new (e.g., "rg-codebidder")
   - **Name**: `codebidder` (or your preferred unique name)
   - **Publish**: Code
   - **Runtime stack**: .NET 8 (LTS)
   - **Operating System**: Windows or Linux
   - **Region**: Choose closest to your users
   - **Pricing Plan**: Free F1 (for testing) or Basic B1 (for production)
5. Click "Review + Create" then "Create"

**B. Create Azure SQL Database:**
1. In Azure Portal, click "Create a resource"
2. Search for "SQL Database" and click Create
3. Fill in the details:
   - **Resource Group**: Use same as above (rg-codebidder)
   - **Database name**: `CodeBidder_db`
   - **Server**: Click "Create new"
     - Server name: `codebidder-sql-server` (must be globally unique)
     - Location: Same as your Web App
     - Authentication: SQL Authentication
     - Admin login: Choose a username
     - Password: Choose a strong password (save this!)
   - **Compute + storage**: Basic or Standard (choose based on needs)
4. Click "Review + Create" then "Create"

#### 2. Configure Connection String in Azure

1. Go to your App Service in Azure Portal
2. Click "Configuration" under Settings
3. Click "New connection string"
4. Add:
   - **Name**: `DefaultConnection`
   - **Value**: Get from your SQL Database:
     - Go to SQL Database ? Click "Show database connection strings"
     - Copy the ADO.NET connection string
     - Replace `{your_password}` with your actual password
   - **Type**: SQLAzure
5. Click "OK" then "Save"

#### 3. Set Up GitHub Actions Deployment

1. In Azure Portal, go to your App Service
2. Click "Deployment Center" under Deployment
3. Choose "GitHub" as the source
4. Authorize GitHub if needed
5. Select:
   - **Organization**: PawanLambole
   - **Repository**: codebidder
   - **Branch**: main
6. Click "Save"

**OR manually set up:**

1. In Azure Portal, go to your App Service
2. Click "Download publish profile" (in Overview or Deployment Center)
3. Go to your GitHub repository: https://github.com/PawanLambole/codebidder
4. Click "Settings" ? "Secrets and variables" ? "Actions"
5. Click "New repository secret"
6. Add:
   - **Name**: `AZURE_WEBAPP_PUBLISH_PROFILE`
   - **Value**: Paste entire contents of the publish profile file
7. Click "Add secret"

#### 4. Update GitHub Actions Workflow

The workflow file has been created at `.github/workflows/azure-deploy.yml`

Edit line 11 to match your Azure Web App name:
```yaml
AZURE_WEBAPP_NAME: your-actual-webapp-name    # Change this!
```

#### 5. Initialize Database

After deployment:
1. In Azure Portal, go to App Service ? "Console" or "SSH"
2. Run migration commands:
```bash
dotnet ef database update
```

Or use the Azure SQL Query Editor:
1. Go to your SQL Database in Azure Portal
2. Click "Query editor"
3. Run your database schema scripts

#### 6. Configure Firewall

1. Go to your SQL Server in Azure Portal
2. Click "Networking" under Security
3. Add your Azure App Service:
   - Enable "Allow Azure services and resources to access this server"
4. Click "Save"

### Alternative: Quick Deploy from Visual Studio

If you have Visual Studio installed:

1. Right-click on the project in Solution Explorer
2. Select "Publish"
3. Choose "Azure"
4. Select "Azure App Service (Windows)" or "Azure App Service (Linux)"
5. Sign in with your Azure account
6. Select your subscription and App Service
7. Click "Publish"

### Verify Deployment

1. Visit your app URL: `https://your-app-name.azurewebsites.net`
2. Check the logs in Azure Portal ? App Service ? "Log stream"

### Troubleshooting

**Database Connection Issues:**
- Verify connection string in App Service Configuration
- Check SQL Server firewall rules
- Ensure "Allow Azure services" is enabled

**Application Not Starting:**
- Check Application Insights or Log stream in Azure Portal
- Verify .NET 8 runtime is selected
- Check web.config is correct

**Build Failures:**
- Check GitHub Actions logs
- Ensure all NuGet packages restore correctly
- Verify .NET SDK version matches

### Cost Estimation

- **Free Tier**: $0/month (limited resources, good for testing)
- **Basic Tier**: ~$13-55/month (recommended for small production apps)
- **SQL Database**: ~$5-15/month (Basic tier)

### Support

For issues, check:
- Azure Portal ? App Service ? "Diagnose and solve problems"
- GitHub Actions tab for build logs
- Azure Application Insights for runtime errors

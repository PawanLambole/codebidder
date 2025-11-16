# ?? Deployment Troubleshooting Guide

## ?? CURRENT ERROR FIX

### Error: "Publish profile does not contain kudu URL"

**This is what you're seeing!** Here's the fix:

#### **Solution 1: Re-download Fresh Publish Profile**

Your publish profile might be corrupted or incomplete.

1. Go to Azure Portal: https://portal.azure.com
2. Navigate to your App Service: **codebidder**
3. Click **"Download publish profile"** (top menu bar)
4. A file will download: `codebidder.PublishSettings`
5. **Open it in Notepad** and copy THE ENTIRE content
6. Go to GitHub: https://github.com/PawanLambole/codebidder/settings/secrets/actions
7. Click on `AZURE_WEBAPP_PUBLISH_PROFILE`
8. Click **"Update"**
9. **Paste the ENTIRE XML** (from `<publishData>` to `</publishData>`)
10. Click **"Update secret"**

#### **Solution 2: I've Updated the Workflow**

I've changed the workflow to use a more compatible deployment method:
- Changed from `ubuntu-latest` to `windows-latest` runner
- Using `azure/webapps-deploy@v2` which handles publish profiles better
- Simplified to single job to avoid artifact issues

**After updating the secret, the deployment should work!**

---

## Quick Checklist - Fix Your Deployment Now!

### ? **Step 1: Verify GitHub Secret is Added**

**This is the #1 reason for deployment failures!**

1. Go to: https://github.com/PawanLambole/codebidder/settings/secrets/actions

2. **Check if you see**: `AZURE_WEBAPP_PUBLISH_PROFILE` in the list

3. **If NOT there**, you need to add it:
   - Click "New repository secret"
   - Name: `AZURE_WEBAPP_PUBLISH_PROFILE`
   - Value: Your entire publish profile XML content
   - Click "Add secret"

---

### ? **Step 2: Check the Error in GitHub Actions**

1. Go to: https://github.com/PawanLambole/codebidder/actions

2. Click on the failed workflow run

3. Look for the error message - Common errors:

#### **Error: "Secret AZURE_WEBAPP_PUBLISH_PROFILE not found"**
**Solution**: Add the GitHub secret (see Step 1)

#### **Error: "Publish profile does not contain kudu URL"**
**Solution**: 
- Download a FRESH publish profile from Azure Portal
- Make sure you copy the ENTIRE XML content
- Update the GitHub secret with the complete content
- The publish profile should be around 3000-4000 characters long
- It must include multiple `<publishProfile>` sections

#### **Error: "Build failed" or "Compilation error"**
**Solution**: 
```bash
# Run locally to check
dotnet build SQMS.csproj --configuration Release
```

#### **Error: "Publish failed"**
**Solution**: Check if all required files are committed
```bash
git status
git add .
git commit -m "Add missing files"
git push
```

#### **Error: "Deployment failed" or "401 Unauthorized"**
**Solution**: 
- Your publish profile might be expired
- Download a fresh publish profile from Azure Portal
- Update the GitHub secret with the new content

---

### ? **Step 3: Verify Azure App Service is Running**

1. Go to Azure Portal: https://portal.azure.com

2. Navigate to your App Service: **codebidder**

3. Check Status: Should show **"Running"**

4. If stopped, click **"Start"**

---

### ? **Step 4: Re-download Publish Profile (If Needed)**

If deployment keeps failing with authentication errors:

1. In Azure Portal, go to your App Service (codebidder)

2. Click **"Download publish profile"** (top menu bar - looks like a download icon)

3. Open the downloaded `.PublishSettings` file in Notepad

4. **Select ALL** the content (Ctrl+A) and **Copy** (Ctrl+C)

5. Update GitHub Secret:
   - Go to: https://github.com/PawanLambole/codebidder/settings/secrets/actions
   - Click on `AZURE_WEBAPP_PUBLISH_PROFILE`
   - Click **"Update"** button
   - Delete old content and paste the new content
   - Click **"Update secret"**

**Important**: Make sure the content includes:
```xml
<publishData>
  <publishProfile profileName="codebidder - Web Deploy" ...>
  </publishProfile>
  <publishProfile profileName="codebidder - FTP" ...>
  </publishProfile>
  <publishProfile profileName="codebidder - Zip Deploy" ...>
  </publishProfile>
</publishData>
```

All three profile sections must be present!

---

## ?? Detailed Error Diagnosis

### Check Build Logs:

1. Go to Actions ? Your failed workflow

2. Click on "build" job

3. Look for red ? marks

4. Common build issues:
   - Missing NuGet packages
   - Syntax errors in code
   - Missing files

### Check Deploy Logs:

1. In the same workflow, click "deploy" job

2. Look for authentication or connection errors

3. Common deploy issues:
   - Wrong app name
   - Invalid credentials
   - Azure firewall blocking

---

## ?? Still Failing? Try This:

### Option 1: Clean Build
```bash
cd "D:\backup\Desktop\CodeBidder (2)\CodeBidder"
dotnet clean
dotnet restore
dotnet build --configuration Release
dotnet publish --configuration Release
```

### Option 2: Check All Files are Committed
```bash
git status
# If you see untracked files:
git add .
git commit -m "Add all project files"
git push origin master
```

### Option 3: Verify Publish Profile Format

Your publish profile should look like this:
```xml
<publishData>
  <publishProfile profileName="codebidder - Web Deploy" ...>
    ...credentials...
  </publishProfile>
  ...
</publishData>
```

Make sure:
- ? It starts with `<publishData>`
- ? It ends with `</publishData>`
- ? No extra text before or after
- ? No missing characters

---

## ?? Expected Workflow Steps:

When working correctly, you should see:

1. ? **Checkout code** - Gets your code from GitHub
2. ? **Set up .NET Core** - Installs .NET 8
3. ? **Restore dependencies** - Downloads NuGet packages
4. ? **Build project** - Compiles your code
5. ? **Publish project** - Creates deployment package
6. ? **Upload artifact** - Saves build for deployment
7. ? **Download artifact** - Gets build package
8. ? **Deploy to Azure** - Uploads to Azure

If ANY step fails, click on it to see the detailed error.

---

## ?? Quick Links:

- **GitHub Actions**: https://github.com/PawanLambole/codebidder/actions
- **GitHub Secrets**: https://github.com/PawanLambole/codebidder/settings/secrets/actions
- **Azure Portal**: https://portal.azure.com
- **Your App URL**: https://codebidder-hrdec0bsaxf9hggj.centralindia-01.azurewebsites.net

---

## ?? Next Steps:

1. ? Verify GitHub secret exists
2. ? Check the error in Actions tab
3. ? Follow the specific solution for your error
4. ? Retry the deployment

**Copy the exact error message from GitHub Actions and I can help you fix it!**

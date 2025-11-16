# ?? GitHub Secret Setup - Step by Step

## What You Need to Do RIGHT NOW

You need to add your Azure publish profile as a GitHub Secret so the deployment can authenticate with Azure.

---

## ?? Step-by-Step Instructions

### Step 1: Prepare Your Publish Profile Content

You've already downloaded the publish profile. It looks like this:

```xml
<publishData>
  <publishProfile profileName="codebidder - Web Deploy" ...>
    ...
  </publishProfile>
  ...
</publishData>
```

**You need the ENTIRE XML content** - from `<publishData>` to `</publishData>`

---

### Step 2: Go to GitHub Repository Settings

1. Open your browser and go to:
   ```
   https://github.com/PawanLambole/codebidder/settings/secrets/actions
   ```

2. Or manually navigate:
   - Go to: https://github.com/PawanLambole/codebidder
   - Click **Settings** tab
   - Click **Secrets and variables** in left sidebar
   - Click **Actions**

---

### Step 3: Create New Secret

1. Click the **"New repository secret"** button (green button on the right)

2. Fill in the form:
   - **Name**: `AZURE_WEBAPP_PUBLISH_PROFILE`
     ?? **IMPORTANT**: Use this EXACT name (case-sensitive)
   
   - **Secret**: Paste the entire publish profile XML content
     (Everything from `<publishData>` to `</publishData>`)

3. Click **"Add secret"** button

---

### Step 4: Verify Secret Was Added

After adding, you should see:
- Secret name: `AZURE_WEBAPP_PUBLISH_PROFILE`
- Status: Created/Updated timestamp
- ? Green checkmark

**Note**: You won't be able to view the secret value after saving (this is normal for security)

---

## ? What Happens Next

Once you've added the secret:

1. The GitHub Actions workflow will be able to authenticate with Azure
2. Every push to `master` branch will trigger automatic deployment
3. You can monitor deployment progress in the Actions tab

---

## ?? Test the Deployment

After adding the secret, let's test it:

### Option 1: Push Code (Automatic)
```bash
git add .
git commit -m "Setup Azure deployment"
git push origin master
```

### Option 2: Manual Trigger
1. Go to: https://github.com/PawanLambole/codebidder/actions
2. Click "Build and Deploy ASP.NET Core to Azure Web App"
3. Click "Run workflow" dropdown
4. Click green "Run workflow" button

---

## ? Common Mistakes to Avoid

1. ? **Wrong secret name**: Must be exactly `AZURE_WEBAPP_PUBLISH_PROFILE`
2. ? **Incomplete content**: Must include entire `<publishData>...</publishData>` XML
3. ? **Extra spaces**: Don't add extra spaces before or after the XML
4. ? **Wrong repository**: Make sure you're in the codebidder repository

---

## ?? Need Help?

If you get stuck:
1. Double-check the secret name is exact: `AZURE_WEBAPP_PUBLISH_PROFILE`
2. Make sure you copied the ENTIRE publish profile content
3. Try deleting and re-adding the secret if needed

---

## ?? Visual Guide

Your GitHub Secrets page should look like this after adding:

```
Repository secrets

AZURE_WEBAPP_PUBLISH_PROFILE    Updated 1 minute ago    [Update] [Remove]
```

---

**Once the secret is added, you're all set for automatic deployment! ??**

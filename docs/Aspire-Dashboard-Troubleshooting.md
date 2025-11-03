# Aspire Dashboard Not Showing - Troubleshooting Guide

## What Should Happen

When you press F5 on AppHost:

1. **Console window opens** showing Aspire startup messages
2. **Browser opens automatically** with the Aspire Dashboard
3. **Dashboard shows:**
   - Resources tab: All services, SQL Server, databases
   - Console tab: Live logs from all services
   - Traces, Metrics tabs: Observability data

## 🔍 Quick Checks

### 1. Check Docker Desktop
**Aspire needs Docker to run SQL Server!**

✅ Open Docker Desktop  
✅ Make sure it's running (green icon in system tray)  
✅ Make sure it's not stuck on "Starting..."

If Docker isn't running, that's your issue!

### 2. Look at the Console Window
The AppHost console should show:
```
info: Aspire.Hosting.DistributedApplication[0]
      Dashboard running at: http://localhost:15888
```

**If you see this:** Open that URL manually in your browser

**If you DON'T see this:** There's an error. Look for red error messages.

### 3. Check the Browser
Sometimes the browser doesn't open automatically:

**Manually open:** `http://localhost:15888`  
**Or try:** `http://localhost:15000`, `http://localhost:17000`, `http://localhost:18888`

### 4. Check Visual Studio Output Window
In Visual Studio:
- View → Output
- Select "Aspire App Host" from dropdown
- Look for the dashboard URL or error messages

## 🛠️ Common Issues

### Issue 1: Docker Not Running
**Symptom:** Console window closes immediately or shows errors

**Fix:**
1. Start Docker Desktop
2. Wait for it to fully start (green icon)
3. Try again

### Issue 2: Port Already in Use
**Symptom:** Error message about port conflicts

**Fix:**
1. Close the console window
2. Stop any other Aspire apps
3. In Task Manager, kill any lingering AppHost or dashboard processes
4. Try again

### Issue 3: Aspire Dashboard Not Installed
**Symptom:** Error about missing dashboard

**Fix:**
```bash
dotnet workload update
dotnet workload install aspire
```

Then restart Visual Studio.

### Issue 4: Browser Doesn't Open Automatically
**Symptom:** Console shows URL but browser doesn't open

**Fix:**
1. Copy the URL from console (e.g., `http://localhost:15888`)
2. Open it manually in your browser
3. Bookmark it for future use

### Issue 5: .NET 9 SDK Not Found
**Symptom:** Build errors about .NET 9

**Fix:**
Download and install .NET 9 SDK from:
https://dotnet.microsoft.com/download/dotnet/9.0

## 📋 Verification Steps

### Step 1: Verify AppHost Starts
```bash
cd src/AppHost
dotnet run
```

You should see:
```
info: Aspire.Hosting.DistributedApplication[0]
      Dashboard running at: http://localhost:15888
```

### Step 2: Verify Dashboard Access
Open: `http://localhost:15888`

You should see the Aspire Dashboard with:
- Resources tab showing services
- Console logs
- Navigation menu on the left

### Step 3: Verify SQL Server Starts
In the Dashboard:
- Click "Resources" tab
- Look for "sql" resource
- Status should be "Running"

### Step 4: Verify Services Start
In the Dashboard:
- Look for "auth", "app1", "notifications"
- All should show "Running" status
- Click each to see logs

## 🎯 Expected Behavior

### On First Run:
1. **AppHost console:** Shows startup messages
2. **Docker:** Pulls SQL Server image (takes a few minutes first time)
3. **Browser:** Opens to Aspire Dashboard
4. **Dashboard:** Shows resources starting one by one:
   - sql (SQL Server container)
   - authdb (database)
   - app1db (database)
   - auth (Auth service)
   - app1 (App1 service)
   - notifications (Notifications service)

### After Services Start:
- **Resources tab:** All green "Running" status
- **Console tab:** Live logs from all services
- **Click auth:** See Auth service logs
- **Click app1:** See App1 service logs

## 🖼️ What the Dashboard Looks Like

```
┌──────────────────────────────────────────────────────────────┐
│  Aspire Dashboard                                  🔄 Search  │
├──────────────────────────────────────────────────────────────┤
│                                                               │
│  📊 Resources  📝 Console  📈 Traces  📊 Metrics  ⚙️ Structured │
│                                                               │
├──────────────────────────────────────────────────────────────┤
│  Resource               Type        State      Endpoints      │
├──────────────────────────────────────────────────────────────┤
│  🗄️  sql               Container    Running    :1433          │
│  🗄️  authdb            Database     -          -              │
│  🗄️  app1db            Database     -          -              │
│  🌐 auth               Project      Running    https://...    │
│  🌐 app1               Project      Running    https://...    │
│  🌐 notifications      Project      Running    https://...    │
└──────────────────────────────────────────────────────────────┘
```

**Click any resource** to see:
- Logs
- Environment variables
- Endpoints
- Metrics

## 🚨 If Nothing Works

### Try Clean Start:

1. **Close everything:**
   - Close Visual Studio
   - Stop Docker Desktop
   - Kill any AppHost processes in Task Manager

2. **Clean solution:**
   ```bash
   cd DyvenixSolution
   dotnet clean
   ```

3. **Start fresh:**
   - Start Docker Desktop
   - Wait for it to be ready
   - Open Visual Studio
   - Open Dyvenix.sln
   - Set AppHost as startup
   - Press F5

4. **Watch the console:**
   - Don't click away from the console window
   - Look for the dashboard URL
   - Wait for "Dashboard running at: http://..."

5. **Open browser manually:**
   - Copy the URL from console
   - Paste in browser
   - Bookmark it

## 📞 Still Not Working?

**Check these:**
1. Is Docker Desktop running? (Must be green icon)
2. Is .NET 9 SDK installed? (`dotnet --version` should show 9.x)
3. Is Aspire workload installed? (`dotnet workload list` should show aspire)
4. Are ports available? (Nothing else on 15000, 15888, 1433)

**Get the logs:**
In AppHost console, you should see detailed startup logs. Any errors in red? Share those.

**Common error messages:**
- "Docker is not running" → Start Docker Desktop
- "Port already in use" → Kill other processes using that port
- "Unable to find workload" → Run `dotnet workload install aspire`

## ✅ Success!

When it works, you'll see:
- ✅ Console with "Dashboard running at: http://localhost:15888"
- ✅ Browser opens to Aspire Dashboard
- ✅ All resources showing as "Running"
- ✅ Can click each service to see logs
- ✅ Can access Swagger at each service URL

The dashboard is your control center for the entire solution!

# Quick Start - What to Expect

## 🚀 When You Press F5

### 1. Console Window Opens
You'll see a **black console window** with messages like:
```
info: Aspire.Hosting.DistributedApplication[0]
      Aspire version: 9.0.0
info: Aspire.Hosting.DistributedApplication[0]
      Dashboard running at: http://localhost:15888    ← LOOK FOR THIS!
info: Aspire.Hosting.DistributedApplication[0]
      Now listening on: http://localhost:15000
```

**👀 IMPORTANT:** Look for "Dashboard running at:" - that's the URL!

### 2. Browser Opens Automatically
A browser window/tab should open to:
```
http://localhost:15888
```

Showing the **Aspire Dashboard**.

### 3. You See the Dashboard
The Aspire Dashboard is a web UI showing:
- **Resources tab** (default): All your services
- **Console tab**: Live logs from all services
- **Traces/Metrics tabs**: Performance data

## 🖥️ No Separate Console Windows!

**You do NOT see:**
- ❌ Separate console for Auth service
- ❌ Separate console for App1 service  
- ❌ Separate console for Notifications service

**You DO see:**
- ✅ One console for AppHost
- ✅ Browser with Aspire Dashboard
- ✅ All service logs in the dashboard

## 📊 What the Dashboard Shows

### Resources Tab
```
Resource          Type        State       Endpoints
─────────────────────────────────────────────────────
sql              Container    Running     :1433
authdb           Database     -           -
app1db           Database     -           -
auth             Project      Running     https://localhost:7001
app1             Project      Running     https://localhost:7002
notifications    Project      Running     https://localhost:7003
```

**Click any resource** to see its logs, environment, endpoints.

### Console Tab
Shows live logs from **all services** combined:
```
[auth] info: Auth service starting...
[app1] info: App1 service starting...
[sql] Starting SQL Server...
```

## 🎯 How to Access Services

### From the Dashboard:
1. Click **Resources** tab
2. Find your service (auth, app1, notifications)
3. Click the **Endpoints** column
4. Click the URL to open Swagger

### Direct URLs:
The exact ports are **randomly assigned** each run.

**Find them in the dashboard** or console output:
```
Auth:          https://localhost:7001/swagger
App1:          https://localhost:7002/swagger
Notifications: https://localhost:7003/swagger
```

## ⚠️ If Browser Doesn't Open

**Manually open:** The URL from the console

Example:
1. Look at console: "Dashboard running at: http://localhost:15888"
2. Open browser
3. Go to: `http://localhost:15888`

## ⚠️ If Console is Blank

**Possible causes:**

1. **Docker not running**
   - Solution: Start Docker Desktop, wait for green icon, try again

2. **Aspire dashboard not installed**
   - Solution: Run `dotnet workload install aspire` in terminal

3. **Error during startup**
   - Solution: Look at Visual Studio Output window (View → Output, select "Aspire App Host")

## ✅ What Success Looks Like

### Console Window:
```
info: Aspire.Hosting.DistributedApplication[0]
      Dashboard running at: http://localhost:15888
info: Aspire.Hosting.DistributedApplication[0]
      Login to the dashboard at http://localhost:15888
```

### Browser:
- Shows Aspire Dashboard UI
- Resources tab shows 6 items
- All services show "Running" (green)

### Click "auth" resource:
- See Auth service logs
- See environment variables
- See endpoint URL
- Click endpoint → Swagger opens

## 🧪 Quick Test

1. **Open dashboard** (http://localhost:15888)
2. **Click auth** resource
3. **Click endpoint** URL
4. **Add `/swagger`** to URL
5. **Try `/api/health`** endpoint in Swagger

## 📋 Checklist

Before pressing F5:
- [ ] Docker Desktop is running (green icon)
- [ ] .NET 9 SDK installed (`dotnet --version`)
- [ ] Aspire workload installed (`dotnet workload list`)
- [ ] AppHost set as startup project in Visual Studio

After pressing F5:
- [ ] Console window opens
- [ ] See "Dashboard running at:" message
- [ ] Browser opens to dashboard
- [ ] All resources show "Running"

## 🔧 Pro Tips

### Bookmark the Dashboard URL
The dashboard URL usually stays the same, so bookmark it!

### Keep Console Window Visible
The console shows important startup messages and errors.

### Use the Console Tab
In dashboard, click **Console** to see all service logs in one place.

### Check Resource Endpoints
Click any service to see its Swagger URL.

### Use Dashboard for Debugging
See logs, traces, and metrics all in one place - no hunting through multiple windows!

## 🚨 Emergency: Nothing Works

**Try this:**
1. Close everything (VS, Docker, browsers)
2. Start Docker Desktop
3. Open terminal:
   ```bash
   cd src/AppHost
   dotnet run
   ```
4. Watch for dashboard URL
5. Open it manually

If `dotnet run` fails, you'll see the error directly!

## 📚 Need More Help?

See: `docs/Aspire-Dashboard-Troubleshooting.md` for detailed troubleshooting.

---

**Remember:** 
- ✅ One console (AppHost)
- ✅ Dashboard in browser
- ✅ All services managed by Aspire
- ✅ No separate console windows needed!

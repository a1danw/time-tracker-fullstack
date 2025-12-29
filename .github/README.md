# ⏱️ Time Tracker Application

A full-stack **Time Tracking application** built with **ASP.NET Web API**, **Blazor WebAssembly**, **Entity Framework Core**, and deployed to **Azure App Service** with an **Azure SQL Database** backend.

---

## 🏗️ Architecture Overview

- **Backend**: ASP.NET Web API (.NET)
- **Frontend**: Blazor WebAssembly
- **Database**: Azure SQL Database
- **ORM**: Entity Framework Core
- **Hosting**: Azure App Service
- **CI/CD**: GitHub Actions
- **Charts**: Radzen Blazor Components

---

## ☁️ Azure Resources

### Azure App Service
Hosts the ASP.NET Web API and Blazor client.

### Azure SQL Database
- **Server**: timetracker-dbserver  
- **Database**: timetracker-db  

**Credentials (Demo / Dev only):**
- Username: timetrackeradmin  
- Password: Secretpass123  

> ⚠️ Do NOT use these credentials in production.

---

## 🔐 Configuring the Connection String in Azure

### Step 1: Get the Connection String from Azure SQL Database

1. Navigate to **Azure Portal** → **SQL databases** → **timetracker-db**
2. Select **Settings** → **Connection strings**
3. Copy the **ADO.NET** connection string
4. Replace `{your_password}` in the copied string with your actual password: `Secretpass123`

### Step 2: Add Connection String to Azure App Service

1. Navigate to **Azure Portal** → **App Services** → **Your App Service**
2. Select **Settings** → **Configuration**
3. Under **Connection strings**, click **+ New connection string**
4. Configure the connection string:
   - **Name**: `DefaultConnection`
   - **Value**: Paste the connection string from Step 1
   - **Type**: `SQLAzure`
5. Click **OK**, then **Save** at the top of the Configuration page

### Example Connection String

```
Server=tcp:timetracker-dbserver.database.windows.net,1433;
Initial Catalog=timetracker-db;
Persist Security Info=False;
User ID=timetrackeradmin;
Password=Secretpass123;
MultipleActiveResultSets=False;
Encrypt=True;
TrustServerCertificate=False;
Connection Timeout=30;
```

> 💡 **Tip**: The connection string will be automatically available to your application via `Configuration.GetConnectionString("DefaultConnection")` in your ASP.NET Core app.

---

## 🔓 Configuring Firewall Rules for Azure SQL Database

To use the **Query Editor** or connect from your local machine, you need to allow IP addresses through the firewall.

### Allow All IP Addresses (Dev Mode Only)

1. Navigate to **Azure Portal** → **SQL servers** → **timetracker-dbserver**
2. Select **Security** → **Networking**
3. Under **Firewall rules**, click **+ Add a firewall rule**
4. Configure the firewall rule:
   - **Rule name**: `AllAccess`
   - **Start IP**: `0.0.0.0`
   - **End IP**: `255.255.255.255`
5. Click **Save**

> ⚠️ **Security Warning**: This allows access from any IP address. Only use this configuration for development/testing. In production, restrict access to specific IP addresses or use Azure services integration.

### Allow Your Current IP (Recommended)

Alternatively, you can allow only your current IP address:
1. In the **Networking** page, click **+ Add your client IPv4 address**
2. Click **Save**

### Enable Azure Services Access (Required for App Service)

To allow your Azure App Service to connect to the database:
1. In the **Networking** page, ensure **Allow Azure services and resources to access this server** is **checked**
2. Click **Save**

> 💡 **Important**: This setting is required for your deployed Web API to communicate with the database.

---

## ⚠️ Important Notice: Preventing 500 Internal Server Errors in Azure

When deploying your Web API to Azure, you may encounter a **500 Internal Server Error** when making API calls. This issue is often related to the database server configuration.

### Troubleshooting Steps

1. **Navigate to the Networking Settings of Your Database Server:**
   - Go to the **Azure Portal**
   - Select your database server: **timetracker-dbserver**
   - In the left-hand menu, click on **Networking**

2. **Enable Access for Azure Services:**
   - Find the checkbox labeled **Allow Azure services and resources to access this server**
   - Make sure this checkbox is **checked**
   - Click **Save**

> 💡 **Why This Matters**: Enabling this setting allows your Azure-hosted web application to connect to your database server without running into firewall issues. This simple step can resolve the 500 Internal Server Error that may occur during API calls.

---

## 🗄️ Entity Framework Core & Migrations

### Install EF Core Tools
```bash
dotnet tool install --global dotnet-ef
```

### Initial Migration
```bash
cd ./TimeTracker.Api
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

### Generate SQL Scripts	

#### Output to terminal
```bash
dotnet ef migrations script
```

#### Save to file
```bash
dotnet ef migrations script -o Scripts/InitialCreate.sql
```

---

### Applying Migration Scripts to Azure SQL Database

After generating the SQL script, you have two options to create the tables in Azure:

#### Option 1: Using Azure Portal Query Editor

1. Navigate to **Azure Portal** → **SQL databases** → **timetracker-db**
2. Select **Query editor** from the left menu
3. Login with:
   - **SQL server authentication**
   - Username: `timetrackeradmin`
   - Password: `Secretpass123`
4. Open the generated SQL script file (e.g., `Scripts/InitialCreate.sql`)
5. Copy the entire script content
6. Paste it into the Query Editor
7. Click **Run** to execute the script

#### Option 2: Using Azure Data Studio

1. Navigate to **Azure Portal** → **SQL databases** → **timetracker-db**
2. Click **Overview** → **Connect** → **Azure Data Studio**
3. This will open Azure Data Studio with a connection dialog
4. Enter the credentials:
   - **Server**: `timetracker-dbserver.database.windows.net`
   - **Authentication type**: `SQL Login`
   - **User name**: `timetrackeradmin`
   - **Password**: `Secretpass123`
   - **Database**: `timetracker-db`
5. Click **Connect**
6. Open a new query window
7. Open or paste the generated SQL script (e.g., `Scripts/InitialCreate.sql`)
8. Execute the query to create the tables

> 💡 **Tip**: Azure Data Studio provides better IntelliSense, syntax highlighting, and script management capabilities for complex migrations.

---

### Adding New Migrations (Example: Projects)

```bash
cd ./TimeTracker.Api
dotnet ef migrations add Projects
dotnet ef database update
```

---

### Multiple / Idempotent Scripts

```bash
dotnet ef migrations script -i -o Scripts/Complete.sql
```

This prevents errors when tables already exist.

---

## 📊 Charts & Reporting

The Blazor client uses **Radzen Blazor Components** to display:
- Hours per day
- Time per project
- Weekly / monthly summaries
- Bar, line, and pie charts

---

## 🚀 Deployment

- GitHub Actions CI/CD
- Azure App Service deployment
- Environment-based configuration

---

## 📌 Notes

- Credentials are demo-only
- Use Azure Key Vault for production secrets
- Commit migration scripts for traceability

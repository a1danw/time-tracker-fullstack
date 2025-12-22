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

## 🔐 Connection String (Azure App Service)

Add this connection string to **Azure App Service → Configuration → Connection strings**.

**Name**: `DefaultConnection`

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

var builder = DistributedApplication.CreateBuilder(args);

// Add SQL Server
var sqlPassword = builder.AddParameter("sql-password", secret: true);
//var sql = builder.AddSqlServer("sql", sqlPassword, port: 1433)
//    .WithDataVolume();

// Add databases
//var authDb = sql.AddDatabase("authdb");
//var app1Db = sql.AddDatabase("app1db");

var authDb = builder.AddConnectionString("authdb");
var app1Db = builder.AddConnectionString("app1db");

// Auth Service
var auth = builder.AddProject<Projects.Auth_Host>("auth")
    .WithReference(authDb);

// App1 Service
var app1 = builder.AddProject<Projects.App1_Host>("app1")
    .WithReference(app1Db)
    .WithReference(auth);

// Notifications Service
var notifications = builder.AddProject<Projects.Notifications_Host>("notifications")
    .WithReference(auth);

builder.Build().Run();

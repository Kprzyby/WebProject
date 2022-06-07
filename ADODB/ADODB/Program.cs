using ADODB.DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("CookieAuthentication")
        .AddCookie("CookieAuthentication", options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.None;
            options.Cookie.Name = "UserLoginCookie";
            options.LoginPath = "/Login/UserLogin";
            options.AccessDeniedPath = "/Login/AccessDenied";
            options.Cookie.SameSite = SameSiteMode.Strict;
        });

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizePage("/Index", "LoggedPrivileges");
    options.Conventions.AuthorizePage("/Products/CreateProduct", "Admin&ExecutivePrivileges");
    options.Conventions.AuthorizePage("/Products/EditProduct", "Admin&ExecutivePrivileges");
    options.Conventions.AuthorizePage("/Products/DeleteProduct", "Admin&ExecutivePrivileges");
    options.Conventions.AuthorizePage("/Categories/Index", "LoggedPrivileges");
    options.Conventions.AuthorizePage("/Categories/CreateCategory", "Admin&ExecutivePrivileges");
    options.Conventions.AuthorizePage("/Categories/EditCategory", "Admin&ExecutivePrivileges");
    options.Conventions.AuthorizePage("/Categories/DeleteCategory", "Admin&ExecutivePrivileges");
    options.Conventions.AuthorizePage("/AddUser", "AdminPrivileges");
    options.Conventions.AuthorizePage("/AboutUs", "LoggedPrivileges");
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPrivileges", policy =>
          policy.RequireRole("admin"));
    options.AddPolicy("Admin&ExecutivePrivileges", policy =>
          policy.RequireRole("admin", "executive"));
    options.AddPolicy("LoggedPrivileges", policy =>
          policy.RequireRole("admin", "executive", "employee"));
});

builder.Services.AddDbContext<ContextClass>(e => e.UseSqlServer(builder.Configuration.GetConnectionString("MyCompanyDB")));

builder.Services.AddTransient<ILogs, LogCreator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

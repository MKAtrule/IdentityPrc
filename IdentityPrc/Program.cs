using IdentityPrc.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DeleteRolePolicy",
        policy => policy.RequireClaim("Delete Role")
                        .RequireClaim("Edit Role")
        );
}); builder.Services.AddIdentity<User,Role>(
    options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireLowercase = true;
    }
    )
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddAuthentication()
.AddGoogle(options =>
{
    options.ClientId = "730611870487-eultpr59ltvp98991agfv3idohbk94aa.apps.googleusercontent.com";
    options.ClientSecret = "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";
   
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using MobileTrolleyToursV2.Areas.Identity;
using MobileTrolleyToursV2.Data;
using MobileTrolleyTours.Models.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using MobileTrolleyTours.Models.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlite(connectionString));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddDbContext<UserDbContext>();
//builder.Services.AddIdentity<User, IdentityRole>()
//                .AddEntityFrameworkStores<UserDbContext>()
//                .AddDefaultTokenProviders();



//builder.Services.AddTransient<IUserStore<User>, UserStore<User>>();
builder.Services.AddTransient<IUser, User>();
builder.Services.AddDbContext<AppDbContext>();
//builder.Services.AddDbContext<UserDbContext>();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)  //IdentityUser
    .AddEntityFrameworkStores<AppDbContext>();

//builder.Services.AddDefaultIdentity<User>();
//    .AddEntityFrameworkStores<ApplicationDbContext>();




//builder.Services.AddIdentity<IUser, IdentityRole>();
//.AddDefaultTokenProviders();

// Add application services.
//builder.Services.AddTransient<IEmailSender, IEmailSender>();

//builder.Services.AddMvc();



builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
//builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

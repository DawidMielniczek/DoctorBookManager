using DoctorBookManager.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DoctorBookManager.Areas.Identity.Data;
using DoctorBookManager.Data.Repositories;
using DoctorBookManager.Data.Entities;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IRepository<DoctorModel>, BaseRepository<DoctorModel>>();
builder.Services.AddScoped<IRepository<DoctorReviewModel>, BaseRepository<DoctorReviewModel>>();
builder.Services.AddScoped<IRepository<DoctorAppointmentModel>, BaseRepository<DoctorAppointmentModel>>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//builder.Services.AddScoped<RoleManager<IdentityRole>>();
//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//        .AddEntityFrameworkStores<ApplicationDbContext>()
//        .AddDefaultTokenProviders();
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultUI().AddDefaultTokenProviders();
var app = builder.Build();
Seeder.SeedData(app).GetAwaiter().GetResult();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
    pattern: "{controller=Review}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

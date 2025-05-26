using IonosLedWebMvc.Ver2.Data;
using IonosLedWebMvc.Ver2.Repos;
using IonosLedWebMvc.Ver2.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");


// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationContext>(options => options.UseMySql(connection, ServerVersion.Parse("8.4.4-mysql")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddScoped<ILampRepo, LampRepo>();
builder.Services.AddScoped<LampService>();
builder.Services.AddScoped<SalaryService>();
builder.Services.AddScoped<UserEventsService>();
builder.Services.AddScoped<StatisticsService>();
builder.Services.AddScoped<LampModelService>();

builder.Services.AddSession();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

/*app.UseHttpsRedirection();*/
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
/*var cookiePolicyOptions = new CookiePolicyOptions
{
	MinimumSameSitePolicy = SameSiteMode.Strict,
};
app.UseCookiePolicy(cookiePolicyOptions);*/
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using FluentValidation.AspNetCore;
using GeoQuest.Middlewares;
using GeoQuest.Services.Implementation;
using GeoQuest.Services;
using GeoQuest.Models;
using Task = System.Threading.Tasks.Task;
using GeoQuest.Middlewares.UserContext;
using GeoQuest.Repositories.Implementation;
using GeoQuest.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "GeoQuestCookie";
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.HttpOnly = false;
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                // User is not authenticated, return 401 Unauthorized
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = context =>
            {
                // User is authenticated but lacks necessary permissions, return 403 Forbidden
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddCors(c =>
{
    c.AddPolicy("CorsPolicy", options =>
        options.AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("http://localhost:4200", "https://geoquestapp.azurewebsites.net")
    //.AllowAnyOrigin()
    );
});

builder.Services.AddSingleton<UserContext>(); // Register UserContext as a singleton

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<GeoQuestContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GeoQuest")),
    ServiceLifetime.Transient);



// Services
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<ISubjectService, SubjectService>();
builder.Services.AddTransient<ITestService, TestService>();
builder.Services.AddTransient<ITaskService, TaskService>();
builder.Services.AddTransient<ITestInstanceService, TestInstanceService>();
builder.Services.AddTransient<ITestTaskInstanceService, TestTaskInstanceService>();

// Repositories
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<IAuthRepository, AuthRepository>();
builder.Services.AddTransient<ISubjectRepository, SubjectRepository>();
builder.Services.AddTransient<ITestRepository, TestRepository>();
builder.Services.AddTransient<ITaskRepository, TaskRepository>();
builder.Services.AddTransient<ITestInstanceRepository, TestInstanceRepository>();
builder.Services.AddTransient<ITestTaskInstanceRepository, TestTaskInstanceRepository>();


var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCookiePolicy();

app.UseMiddleware<UserContextMiddleware>();

app.MapControllers();

app.Run();

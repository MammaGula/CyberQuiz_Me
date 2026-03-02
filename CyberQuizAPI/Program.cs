using CyberQuiz.BLL.Interfaces;
using CyberQuiz.BLL.Services;
using CyberQuiz.DAL.Data;
using CyberQuiz.DAL.Entities;
using CyberQuiz.DAL.Repositories;
using CyberQuiz.DAL.Repositories.Interfaces;
using CyberQuiz.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CyberQuiz.BLL.Interfaces;
using CyberQuiz.BLL.Services;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// 1. Add DbContext (SQL Server)
// -----------------------------

builder.Services.AddDbContext<CyberQuizDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---------------------------------------------
// 2. Add Identity (Users + Roles + EF Storage)
// ---------------------------------------------
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;              // Must contain a digit
    options.Password.RequiredLength = 6;               // Minimum length 6 characters
    options.Password.RequireNonAlphanumeric = false;   // Non-alphanumeric characters not required
    options.Password.RequireUppercase = false;         // Uppercase letters not required
    options.Password.RequireLowercase = false;         // Lowercase letters not required
    
    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);  // Lockout duration 5 minutes
    options.Lockout.MaxFailedAccessAttempts = 5;                       // Lockout after 5 failed attempts
    options.Lockout.AllowedForNewUsers = true;                         // Enable lockout for new users
    
    // User settings
    options.User.RequireUniqueEmail = true;            // Email must be unique
    
    // SignIn settings
    options.SignIn.RequireConfirmedEmail = false;      // Email confirmation not required (adjust as needed)
})
    .AddEntityFrameworkStores<CyberQuizDbContext>()
    .AddDefaultTokenProviders();

// Configure cookie behavior for API (make cookies usable from Blazor UI and return 401/403 for API calls)
//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.Cookie.Name = "CyberQuiz.Auth";
//    options.Cookie.HttpOnly = true;
//    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//    options.Cookie.SameSite = SameSiteMode.None; // allow cross-site cookie for UI on different origin

//    // Prevent automatic redirects for API calls — return proper status codes instead
//    options.Events.OnRedirectToLogin = context =>
//    {
//        if (context.Request.Path.StartsWithSegments("/api"))
//        {
//            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//            return Task.CompletedTask;
//        }
//        context.Response.Redirect(context.RedirectUri);
//        return Task.CompletedTask;
//    };

//    options.Events.OnRedirectToAccessDenied = context =>
//    {
//        if (context.Request.Path.StartsWithSegments("/api"))
//        {
//            context.Response.StatusCode = StatusCodes.Status403Forbidden;
//            return Task.CompletedTask;
//        }
//        context.Response.Redirect(context.RedirectUri);
//        return Task.CompletedTask;
//    };
//});

// -----------------------------
// 3. Add Authentication & Authorization system
// -----------------------------
builder.Services.AddAuthorization();

// -----------------------------
// 4. Add Controllers
// -----------------------------
builder.Services.AddControllers();

// -----------------------------
// 5. Add Swagger for API documentation
// -----------------------------
builder.Services.AddSwaggerGen();

// -----------------------------
// 6. Add CORS (för Blazor UI): API contacts Blazor UI via CORS, not by referencing it directly
// -----------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUI", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .WithOrigins(
                  "https://localhost:7255",   // Blazor UI HTTPS
                  "http://localhost:5063",    // Blazor UI HTTP
                  "http://localhost:5275/swagger",     // API HTTP (for testing)
                  "http://localhost:7050"     // API HTTP (for testing)
              );
    });
});


// -----------------------------
// 6.1 Register HttpClients
// -----------------------------
//builder.Services.AddHttpClient<AiService>(client =>
//{
//    var baseUrl = builder.Configuration["Ai:BaseUrl"];
//    if (!string.IsNullOrWhiteSpace(baseUrl))
//    {
//        client.BaseAddress = new Uri(baseUrl);
//    }

//    if (int.TryParse(builder.Configuration["Ai:TimeoutSeconds"], out var timeoutSeconds) && timeoutSeconds > 0)
//    {
//        client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
//    }
//});



// -----------------------------
// 7. Register Repositories + UnitOfWork
// -----------------------------
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IAnswerOptionRepository, AnswerOptionRepository>();
builder.Services.AddScoped<IUserResultRepository, UserResultRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IQuizService, QuizService>();

var app = builder.Build();


// -----------------------------
// 8. Seed database on startup
// -----------------------------
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<CyberQuizDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await db.Database.MigrateAsync();

        if (app.Environment.IsDevelopment())
        {
            await DbSeeder.SeedAsync(db, userManager, roleManager);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database error: {ex.Message}");
        throw;
    }
}




// -----------------------------
// 9. Middleware pipeline
// -----------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CyberQuiz API V1");
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowUI");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


// UI <--> API (Refererar inte till varandra, peka på varandra via CORS och baseAdress)
// API --> BLL & Shared (Class Library)
// BLL -> DAL & Shared
// DAL -> Shared

//SCENARIO
// Användare som svara på en fråga
// 1. UI > skaickar svaret till API
// 2. API > skickar till BLL 
// 3. BLL > Räkna rätt / fel
// 4. BLL -> Säg till DAL att spara
// 5. DAL -> Sparar till databasen
// 6. BLL > Räkna progression
// 7. API > Retunerar ett resultat
// 8. UI > Visar feedback


// UI > Pages
// API > Endpoints: POST, GET, PUT, DELETE, api/ai/feedback
// BLL: logik, rätt, fel, progression, services
// DAL : migration, dbContext, modeller(endast för DATABASE)
// Shared: DTO objekt som används mellan lager
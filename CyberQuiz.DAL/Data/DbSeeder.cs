using CyberQuiz.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CyberQuiz.DAL.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(
        CyberQuizDbContext db,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        ArgumentNullException.ThrowIfNull(db);
        ArgumentNullException.ThrowIfNull(userManager);
        ArgumentNullException.ThrowIfNull(roleManager);

		// Ensure the host applies migrations before calling the seeder.
		// (Avoid doing db.Database.MigrateAsync() here to keep seeding and schema management separate.)

		// 1. Seed Identity Roles (User, Admin)
        await SeedRolesAsync(roleManager);

        // 3. Seed Default Users (user, admin) using transaction logic if needed
        //await SeedUsersAsync(userManager);

		// 3. Seed Quiz Content (Categories, SubCategories, Questions, Answers)
        await SeedQuizDataAsync(db);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = { "User", "Admin" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role));
                EnsureSucceeded(result, $"Failed to create role '{role}'.");
            }
        }
    }



    //========================
    // SEED USER & ADMIN
    //========================
    //private static async Task SeedUsersAsync(UserManager<AppUser> userManager)
    //{
    //    // Seed regular user
    //    if (await userManager.FindByNameAsync("user") == null)
    //    {
    //        var user = new AppUser
    //        {
    //            UserName = "user",
    //            Email = "user@example.com",
    //            EmailConfirmed = true,
    //            FullName = "Regular User"
    //        };
    //        var result = await userManager.CreateAsync(user, "Password1234!");
    //        if (result.Succeeded)
    //        {
    //            await userManager.AddToRoleAsync(user, "User");
    //        }
    //    }

    //    // Seed admin user
    //    if (await userManager.FindByNameAsync("admin") == null)
    //    {
    //        var admin = new AppUser
    //        {
    //            UserName = "admin",
    //            Email = "admin@cyberquiz.com",
    //            EmailConfirmed = true,
    //            FullName = "System Administrator"
    //        };
    //        var result = await userManager.CreateAsync(admin, "Admin1234!");
    //        if (result.Succeeded)
    //        {
    //            await userManager.AddToRoleAsync(admin, "Admin");
    //        }
    //    }
    //}





    //======================================================================
    // SEED QUIZ DATA (Categories, SubCategories, Questions, AnswerOptions)
    //======================================================================
    private static async Task SeedQuizDataAsync(CyberQuizDbContext db)
    {
        // Check if data already exists to avoid duplication
        if (await db.Categories.AnyAsync())
        {
            return; // Data already seeded
        }

        // Use transaction for data integrity (Atomicity)
        await using var transaction = await db.Database.BeginTransactionAsync();
        try
        {
            // ====================================================================================
            // CATEGORY 1: .NET Web Development
            // ====================================================================================
            var cat1 = new Category { Name = ".NET Web Development" };

            // --- SubCategory 1.1: Web API & MVC ---
            var sub1_1 = new SubCategory { Name = "Web API & MVC", Category = cat1, SortOrder = 1 };
            sub1_1.Questions = new List<Question>
            {
                new Question {
                    Text = "What does MVC stand for?",
                    SubCategory = sub1_1,
                    AnswerOptions = {
                        new AnswerOption { Text = "Model View Controller", IsCorrect = true, DisplayOrder = 1 },
                        new AnswerOption { Text = "Main Virtual Code", IsCorrect = false, DisplayOrder = 2 },
                        new AnswerOption { Text = "Model Visual Core", IsCorrect = false, DisplayOrder = 3 }
                    }
                },
                new Question {
                    Text = "Which HTTP method is used to retrieve data?",
                    SubCategory = sub1_1,
                    AnswerOptions = {
                        new AnswerOption { Text = "POST", IsCorrect = false, DisplayOrder = 1 },
                        new AnswerOption { Text = "GET", IsCorrect = true, DisplayOrder = 2 },
                        new AnswerOption { Text = "PUT", IsCorrect = false, DisplayOrder = 3 }
                    }
                },
                new Question {
                    Text = "What attribute is used to define a POST endpoint?",
                    SubCategory = sub1_1,
                    AnswerOptions = {
                        new AnswerOption { Text = "[HttpGet]", IsCorrect = false, DisplayOrder = 1 },
                        new AnswerOption { Text = "[HttpPost]", IsCorrect = true, DisplayOrder = 2 },
                        new AnswerOption { Text = "[FromBody]", IsCorrect = false, DisplayOrder = 3 }
                    }
                },
                new Question {
                    Text = "What is Middleware in ASP.NET Core?",
                    SubCategory = sub1_1,
                    AnswerOptions = {
                        new AnswerOption { Text = "Hardware component", IsCorrect = false, DisplayOrder = 1 },
                        new AnswerOption { Text = "Software pipeline to handle requests", IsCorrect = true, DisplayOrder = 2 },
                        new AnswerOption { Text = "Database engine", IsCorrect = false, DisplayOrder = 3 }
                    }
                },
                new Question {
                    Text = "What is the return type for an API action that returns JSON?",
                    SubCategory = sub1_1,
                    AnswerOptions = {
                        new AnswerOption { Text = "string", IsCorrect = false, DisplayOrder = 1 },
                        new AnswerOption { Text = "IActionResult", IsCorrect = true, DisplayOrder = 2 },
                        new AnswerOption { Text = "void", IsCorrect = false, DisplayOrder = 3 }
                    }
                }
            };

            // --- SubCategory 1.2: Blazor Framework ---
            var sub1_2 = new SubCategory { Name = "Blazor Framework", Category = cat1, SortOrder = 2 };
            sub1_2.Questions = new List<Question>
            {
                new Question {
                    Text = "What language does Blazor use for client-side logic?",
                    SubCategory = sub1_2,
                    AnswerOptions = {
                        new AnswerOption { Text = "JavaScript only", IsCorrect = false, DisplayOrder = 1 },
                        new AnswerOption { Text = "C#", IsCorrect = true, DisplayOrder = 2 },
                        new AnswerOption { Text = "Python", IsCorrect = false, DisplayOrder = 3 }
                    }
                },
                new Question {
                    Text = "What symbol is used for Razor syntax?",
                    SubCategory = sub1_2,
                    AnswerOptions = {
                        new AnswerOption { Text = "$", IsCorrect = false, DisplayOrder = 1 },
                        new AnswerOption { Text = "@", IsCorrect = true, DisplayOrder = 2 },
                        new AnswerOption { Text = "#", IsCorrect = false, DisplayOrder = 3 }
                    }
                },
                new Question {
                    Text = "Which hosting model runs Blazor on the server via SignalR?",
                    SubCategory = sub1_2,
                    AnswerOptions = {
                        new AnswerOption { Text = "Blazor WebAssembly", IsCorrect = false, DisplayOrder = 1 },
                        new AnswerOption { Text = "Blazor Server", IsCorrect = true, DisplayOrder = 2 },
                        new AnswerOption { Text = "Blazor Static", IsCorrect = false, DisplayOrder = 3 }
                    }
                },
                new Question {
                    Text = "Which lifecycle method is called when a component initializes?",
                    SubCategory = sub1_2,
                    AnswerOptions = {
                        new AnswerOption { Text = "OnStart", IsCorrect = false, DisplayOrder = 1 },
                        new AnswerOption { Text = "OnInitializedAsync", IsCorrect = true, DisplayOrder = 2 },
                        new AnswerOption { Text = "OnLoad", IsCorrect = false, DisplayOrder = 3 }
                    }
                },
                new Question {
                    Text = "How do you bind an input value to a variable?",
                    SubCategory = sub1_2,
                    AnswerOptions = {
                        new AnswerOption { Text = "@bind", IsCorrect = true, DisplayOrder = 1 },
                        new AnswerOption { Text = "v-model", IsCorrect = false, DisplayOrder = 2 },
                        new AnswerOption { Text = "ng-model", IsCorrect = false, DisplayOrder = 3 }
                    }
                }
            };


            // ====================================================================================
            // CATEGORY 2: C# Programming
            // ====================================================================================
            var cat2 = new Category { Name = "C# Programming" };

            // --- SubCategory 2.1: C# Fundamentals ---
            var sub2_1 = new SubCategory { Name = "C# Fundamentals", Category = cat2, SortOrder = 1 };
            sub2_1.Questions = new List<Question>
            {
                new Question {
                    Text = "What is the entry point of a C# console application?",
                    SubCategory = sub2_1,
                    AnswerOptions = {
                        new AnswerOption { Text = "Start()", IsCorrect = false, DisplayOrder = 1 },
                        new AnswerOption { Text = "Main()", IsCorrect = true, DisplayOrder = 2 },
                        new AnswerOption { Text = "Run()", IsCorrect = false, DisplayOrder = 3 }
                    }
                },
                new Question {
                    Text = "Which type is a Value Type?",
                    SubCategory = sub2_1,
                    AnswerOptions = {
                        new AnswerOption { Text = "string", IsCorrect = false, DisplayOrder = 1 },
                        new AnswerOption { Text = "int", IsCorrect = true, DisplayOrder = 2 },
                        new AnswerOption { Text = "class", IsCorrect = false, DisplayOrder = 3 }
                    }
                },
                new Question {
                    Text = "Which keyword makes a class inaccessible outside its assembly?",
                    SubCategory = sub2_1,
                    AnswerOptions = {
                        new AnswerOption { Text = "private", IsCorrect = false, DisplayOrder = 1 },
                        new AnswerOption { Text = "internal", IsCorrect = true, DisplayOrder = 2 },
                        new AnswerOption { Text = "public", IsCorrect = false, DisplayOrder = 3 }
                    }
                },
                new Question {
                    Text = "What is an Interface?",
                    SubCategory = sub2_1,
                    AnswerOptions = {
                        new AnswerOption { Text = "A class with full implementation", IsCorrect = false, DisplayOrder = 1 },
                        new AnswerOption { Text = "A contract defines method signatures", IsCorrect = true, DisplayOrder = 2 },
                        new AnswerOption { Text = "A variable type", IsCorrect = false, DisplayOrder = 3 }
                    }
                },
                new Question {
                    Text = "Which symbol is used for inheritance?",
                    SubCategory = sub2_1,
                    AnswerOptions = {
                        new AnswerOption { Text = ":", IsCorrect = true, DisplayOrder = 1 },
                        new AnswerOption { Text = "=>", IsCorrect = false, DisplayOrder = 2 },
                        new AnswerOption { Text = "::", IsCorrect = false, DisplayOrder = 3 }
                    }
                }
            };

            // --- SubCategory 2.2: Advanced C# ---
            var sub2_2 = new SubCategory { Name = "Advanced C#", Category = cat2, SortOrder = 2 };
            sub2_2.Questions = new List<Question>
            {
                new Question {
                    Text = "What keyword is paired with 'await'?",
                    SubCategory = sub2_2,
                    AnswerOptions = {
                        new AnswerOption { Text = "sync", IsCorrect = false, DisplayOrder = 1 },
                        new AnswerOption { Text = "async", IsCorrect = true, DisplayOrder = 2 },
                        new AnswerOption { Text = "task", IsCorrect = false, DisplayOrder = 3 }
                    }
                },
                new Question {
                    Text = "What is LINQ used for?",
                    SubCategory = sub2_2,
                    AnswerOptions = {
                        new AnswerOption { Text = "Querying data", IsCorrect = true, DisplayOrder = 1 },
                        new AnswerOption { Text = "Styling web pages", IsCorrect = false, DisplayOrder = 2 },
                        new AnswerOption { Text = "Compiling code", IsCorrect = false, DisplayOrder = 3 }
                    }
                },
                new Question {
                    Text = "What does 'Dependency Injection' help with?",
                    SubCategory = sub2_2,
                    AnswerOptions = {
                        new AnswerOption { Text = "Assuming hard dependencies", IsCorrect = false, DisplayOrder = 1 },
                        new AnswerOption { Text = "Decoupling classes and testing", IsCorrect = true, DisplayOrder = 2 },
                        new AnswerOption { Text = "Increasing code size", IsCorrect = false, DisplayOrder = 3 }
                    }
                },
                new Question {
                    Text = "What is a 'Delegate'?",
                    SubCategory = sub2_2,
                    AnswerOptions = {
                        new AnswerOption { Text = "A UI component", IsCorrect = false, DisplayOrder = 1 },
                        new AnswerOption { Text = "A type-safe function pointer", IsCorrect = true, DisplayOrder = 2 },
                        new AnswerOption { Text = "A database table", IsCorrect = false, DisplayOrder = 3 }
                    }
                },
                new Question {
                    Text = "What lifetime is created once per request?",
                    SubCategory = sub2_2,
                    AnswerOptions = {
                        new AnswerOption { Text = "Singleton", IsCorrect = false, DisplayOrder = 1 },
                        new AnswerOption { Text = "Scoped", IsCorrect = true, DisplayOrder = 2 },
                        new AnswerOption { Text = "Transient", IsCorrect = false, DisplayOrder = 3 }
                    }
                }
            };


            // ====================================================================================
            // CATEGORY 3: Architecture & Data
            // ====================================================================================
            var cat3 = new Category { Name = "Architecture & Data" };

            // --- SubCategory 3.1: 3-Layer Architecture ---
            var sub3_1 = new SubCategory { Name = "3-Layer Architecture", Category = cat3, SortOrder = 1 };
            sub3_1.Questions = new List<Question>
            {
                new Question {
                    Text = "Which is NOT one of the 3 layers?",
                    SubCategory = sub3_1,
                    AnswerOptions = {
                        new AnswerOption { Text = "Presentation Layer", IsCorrect = false, DisplayOrder = 1 },
                        new AnswerOption { Text = "Business Logic Layer", IsCorrect = false, DisplayOrder = 2 },
                        new AnswerOption { Text = "Cloud Layer", IsCorrect = true, DisplayOrder = 3 }
                    }
                },
                new Question {
                    Text = "What is the responsibility of DAL?",
                    SubCategory = sub3_1,
                    AnswerOptions = {
                        new AnswerOption { Text = "Handle UI events", IsCorrect = false, DisplayOrder = 1 },
                        new AnswerOption { Text = "Data Access / Database interaction", IsCorrect = true, DisplayOrder = 2 },
                        new AnswerOption { Text = "Validate business rules", IsCorrect = false, DisplayOrder = 3 }
                    }
                },
                new Question {
                    Text = "Where should complex business rules reside?",
                    SubCategory = sub3_1,
                    AnswerOptions = {
                        new AnswerOption { Text = "Presentation Layer", IsCorrect = false, DisplayOrder = 1 },
                        new AnswerOption { Text = "Business Logic Layer (BLL)", IsCorrect = true, DisplayOrder = 2 },
                        new AnswerOption { Text = "Data Access Layer", IsCorrect = false, DisplayOrder = 3 }
                    }
                },
                new Question {
                    Text = "Why use multi-layer architecture?",
                    SubCategory = sub3_1,
                    AnswerOptions = {
                        new AnswerOption { Text = "To make code header to read", IsCorrect = false, DisplayOrder = 1 },
                        new AnswerOption { Text = "Separation of concerns (SoC)", IsCorrect = true, DisplayOrder = 2 },
                        new AnswerOption { Text = "To use more memory", IsCorrect = false, DisplayOrder = 3 }
                    }
                },
                new Question {
                    Text = "Are DTOs (Data Transfer Objects) usually used in DAL?",
                    SubCategory = sub3_1,
                    AnswerOptions = {
                        new AnswerOption { Text = "Yes, strictly", IsCorrect = false, DisplayOrder = 1 },
                        new AnswerOption { Text = "No, mostly between BLL and UI", IsCorrect = true, DisplayOrder = 2 },
                        new AnswerOption { Text = "No, they are for database tables", IsCorrect = false, DisplayOrder = 3 }
                    }
                }
            };

            // --- SubCategory 3.2: Entity Framework Core ---
            var sub3_2 = new SubCategory { Name = "Entity Framework Core", Category = cat3, SortOrder = 2 };
            sub3_2.Questions = new List<Question>
            {
                new Question {
                    Text = "What is Entity Framework Core?",
                    SubCategory = sub3_2,
                    AnswerOptions = {
                        new AnswerOption { Text = "A Database", IsCorrect = false, DisplayOrder = 1 },
                        new AnswerOption { Text = "An ORM (Object-Relational Mapper)", IsCorrect = true, DisplayOrder = 2 },
                        new AnswerOption { Text = "A UI Framework", IsCorrect = false, DisplayOrder = 3 }
                    }
                },
                new Question {
                    Text = "What represents a table in EF Core?",
                    SubCategory = sub3_2,
                    AnswerOptions = {
                        new AnswerOption { Text = "DbSet<T>", IsCorrect = true, DisplayOrder = 1 },
                        new AnswerOption { Text = "DbContext", IsCorrect = false, DisplayOrder = 2 },
                        new AnswerOption { Text = "Migration", IsCorrect = false, DisplayOrder = 3 }
                    }
                },
                new Question {
                    Text = "Which command adds a new migration?",
                    SubCategory = sub3_2,
                    AnswerOptions = {
                        new AnswerOption { Text = "dotnet run", IsCorrect = false, DisplayOrder = 1 },
                        new AnswerOption { Text = "dotnet ef migrations add", IsCorrect = true, DisplayOrder = 2 },
                        new AnswerOption { Text = "dotnet build", IsCorrect = false, DisplayOrder = 3 }
                    }
                },
                new Question {
                    Text = "What is the Code-First approach?",
                    SubCategory = sub3_2,
                    AnswerOptions = {
                        new AnswerOption { Text = "Database created from C# classes", IsCorrect = true, DisplayOrder = 1 },
                        new AnswerOption { Text = "C# classes created from Database", IsCorrect = false, DisplayOrder = 2 },
                        new AnswerOption { Text = "Writing SQL manually", IsCorrect = false, DisplayOrder = 3 }
                    }
                },
                new Question {
                    Text = "What is a Navigation Property?",
                    SubCategory = sub3_2,
                    AnswerOptions = {
                        new AnswerOption { Text = "A URL link", IsCorrect = false, DisplayOrder = 1 },
                        new AnswerOption { Text = "Property to access related entities", IsCorrect = true, DisplayOrder = 2 },
                        new AnswerOption { Text = "A connection string", IsCorrect = false, DisplayOrder = 3 }
                    }
                }
            };




            //============================================================================================
            // TRANSITION: Save Categories with SubCategories, Questions, and Answers(save all or nothing)
            //============================================================================================
            await db.Categories.AddRangeAsync(cat1, cat2, cat3);
            await db.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private static void EnsureSucceeded(IdentityResult result, string message)
    {
        if (result.Succeeded)
        {
            return;
        }

        var errors = string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
        throw new InvalidOperationException($"{message} Errors: {errors}");
    }
}
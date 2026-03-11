using CyberQuiz.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CyberQuiz.DAL.Data;

public class CyberQuizDbContext : DbContext
{


    public CyberQuizDbContext(DbContextOptions<CyberQuizDbContext> options)
       : base(options)
    {
		ArgumentNullException.ThrowIfNull(options);
    }

    // DbSets for our entities
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<SubCategory> SubCategories => Set<SubCategory>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<AnswerOption> AnswerOptions => Set<AnswerOption>();
    public DbSet<UserResult> UserResults => Set<UserResult>();

    // No necessary to declare DbSet<AppUser> for Identity tables (AspNetUsers, AspNetRoles, etc.) because it's already included via IdentityDbContext<AppUser>


    // ========================================================================================================================================
    //  OnModelCreating: Tell database to create columns, relationships, constraints, and cascade delete behaviors based on our entity classes 
    // ========================================================================================================================================
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Call the base method to ensure Identity configurations are applied( for creating Identitytables)
        base.OnModelCreating(builder);


        // Start defining configulation of our entities > relationships, constraints, and cascade delete behaviors

        // Category → SubCategory
        builder.Entity<Category>()
            .HasMany(c => c.SubCategories)
            .WithOne(sc => sc.Category)
            .HasForeignKey(sc => sc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // SubCategory → Question
        builder.Entity<SubCategory>()
            .HasMany(sc => sc.Questions)
            .WithOne(q => q.SubCategory)
            .HasForeignKey(q => q.SubCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Question → AnswerOption
        builder.Entity<Question>()
            .HasMany(q => q.AnswerOptions)
            .WithOne(a => a.Question)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        // AppUser → UserResult
        //builder.Entity<AppUser>()
        //    .HasMany(u => u.Results)
        //    .WithOne(r => r.User)
        //    .HasForeignKey(r => r.UserId)
        //    .OnDelete(DeleteBehavior.Cascade); // cascade: if user is deleted (user choice)

        // Question → UserResult
        builder.Entity<Question>()
            .HasMany(q => q.Results)
            .WithOne(r => r.Question)
            .HasForeignKey(r => r.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);

        // AnswerOption → UserResult
        builder.Entity<AnswerOption>()
            .HasMany(a => a.UserResults)
            .WithOne(r => r.AnswerOption)
            .HasForeignKey(r => r.AnswerOptionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Default values for domain consistency in model/database
        builder.Entity<Question>()
            .Property(q => q.Points)
            .HasDefaultValue(1);

        builder.Entity<UserResult>()
            .Property(r => r.AnsweredAt)
            .HasDefaultValueSql("SYSUTCDATETIME()");



        // ==========================================
        //  INDEXES FOR PERFORMANCE: Tell SQL Server to prepare / order data in advance
        //  for faster queries (especially for sorting and lookups)
        // ==========================================

        // Index for sorting SubCategories by Order inside a Category
        builder.Entity<SubCategory>()
            .HasIndex(s => new { s.CategoryId, s.SortOrder })
            .IsUnique();

        // Index for AnswerOptions display order
        builder.Entity<AnswerOption>()
            .HasIndex(a => new { a.QuestionId, a.DisplayOrder })
            .IsUnique();

        // Unique filtered index to allow only one correct answer per question
        builder.Entity<AnswerOption>()
            .HasIndex(a => a.QuestionId)
            .HasFilter("[IsCorrect] = 1")
            .IsUnique();


        //// Index for User History lookups (faster profile/progression queries)
        //builder.Entity<UserResult>()
        //    .HasIndex(u => new { u.UserId, u.AnsweredAt });
    }
}






// Casecade: When a Category is deleted, all related SubCategories will also be deleted. 

// Restrict: When an AnswerOption is deleted, related UserResults will not be deleted, and the delete operation will be blocked if there are related UserResults.
// This is to preserve the integrity of quiz attempt history.
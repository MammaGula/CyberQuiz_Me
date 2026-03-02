using CyberQuiz.DAL.Data;
using CyberQuiz.DAL.Repositories.Interfaces;

namespace CyberQuiz.DAL.Repositories;

/// <summary>
/// UnitOfWork pattern implementation: coordinates multiple repositories and ensures they share the same DbContext.
/// All changes are committed together in a single transaction when SaveAsync() is called.
/// This maintains data consistency across multiple repository operations.
/// </summary>
/// 
public class UnitOfWork : IUnitOfWork
{
    private readonly CyberQuizDbContext _db;

    // Repository properties - all share the same DbContext instance
    public ICategoryRepository Categories { get; }
    public ISubCategoryRepository SubCategories { get; }
    public IQuestionRepository Questions { get; }
    public IAnswerOptionRepository AnswerOptions { get; }
    public IUserResultRepository UserResults { get; }



    // Constructor injection of DbContext and repositories > They will use the same DbContext instance,
    // They will be in the same transaction scope when we call SaveAsync() on the UnitOfWork,
    // ensuring that all changes across repositories are committed together.
    // If any repository operation fails, the entire transaction will be rolled back(cancelled together).
    public UnitOfWork(
        CyberQuizDbContext db,
        ICategoryRepository categories,
        ISubCategoryRepository subCategories,
        IQuestionRepository questions,
        IAnswerOptionRepository answerOptions,
        IUserResultRepository userResults)
    {
		ArgumentNullException.ThrowIfNull(db);
		ArgumentNullException.ThrowIfNull(categories);
		ArgumentNullException.ThrowIfNull(subCategories);
		ArgumentNullException.ThrowIfNull(questions);
		ArgumentNullException.ThrowIfNull(answerOptions);
		ArgumentNullException.ThrowIfNull(userResults);

		_db = db;
		Categories = categories;
		SubCategories = subCategories;
		Questions = questions;
		AnswerOptions = answerOptions;
		UserResults = userResults;
    }


    /// <summary>
    /// Commits all changes made through repositories in a single database transaction.
    /// Task<int>: Returns the number of affected rows.
    /// </summary>
    public async Task<int> SaveAsync() 
        => await _db.SaveChangesAsync();
}


// UnitOfWork!!!!:
// 1. Data Consistency : If one repository operation fails, the entire transaction will be rolled back, ensuring that the database remains in a consistent state.
// 2. Transaction Management : Handle transactions automatically across multiple repositories. 
// 3. Clean Code: No need to inject DbContext into each repository separately.
// 4. Testability 

using CyberQuiz.DAL.Data;
using CyberQuiz.DAL.Entities;
using CyberQuiz.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Linq;

namespace CyberQuiz.DAL.Repositories;

public class UserResultRepository : IUserResultRepository
{
    private readonly CyberQuizDbContext _db;

    public UserResultRepository(CyberQuizDbContext db)
    {
        ArgumentNullException.ThrowIfNull(db);
        _db = db;
    }


    // Returns all UserResults from the database (AsNoTracking = ReadOnly)
    public async Task<List<UserResult>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _db.UserResults
            .AsNoTracking()
            .OrderBy(r => r.Id)
            .ToListAsync(cancellationToken);


    // Returns a UserResult by its Id, or null if not found
    public async Task<UserResult?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _db.UserResults.FindAsync([id], cancellationToken);


    // Returns the latest UserResult per QuestionId for a specific user
    public async Task<List<UserResult>> GetLatestResultsForUserAndQuestionIdsAsync(string userId, IEnumerable<int> questionIds, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        ArgumentNullException.ThrowIfNull(questionIds);

        var questionIdArray = questionIds.Distinct().ToArray();
        if (questionIdArray.Length == 0)
        {
            return new List<UserResult>();
        }

        return await _db.UserResults
            .AsNoTracking()
            .Where(r => r.UserId == userId && questionIdArray.Contains(r.QuestionId))
            .GroupBy(r => r.QuestionId)
            .Select(g => g.OrderByDescending(x => x.Id).First())
            .ToListAsync(cancellationToken);
    }

    // Adds a new UserResult to the database, but does not save changes (SaveChanges is called in UnitOfWork)
    public async Task AddAsync(UserResult result)
    {
        ArgumentNullException.ThrowIfNull(result);
        await _db.UserResults.AddAsync(result);
    }

    // Removes an existing UserResult, so that it will be deleted from the database when SaveChanges is called
    public void Remove(UserResult result)
    {
        ArgumentNullException.ThrowIfNull(result);
        _db.UserResults.Remove(result);
    }
}

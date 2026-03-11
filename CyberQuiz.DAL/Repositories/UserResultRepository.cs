using CyberQuiz.DAL.Data;
using CyberQuiz.DAL.Entities;
using CyberQuiz.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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


    // Returns all matching results for a specific user in one subcategory
    public async Task<List<UserResult>> GetByUserAndSubCategoryAsync(string userId, int subCategoryId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);

        return await _db.UserResults
            .AsNoTracking()
            .Where(r => r.UserId == userId && r.Question.SubCategoryId == subCategoryId)
            .OrderBy(r => r.Id)
            .ToListAsync(cancellationToken);
    }


    // Returns matching results with the related question, subcategory and selected answer option
    public async Task<List<UserResult>> GetByUserAndQuestionIdsWithDetailsAsync(string userId, IEnumerable<int> questionIds, CancellationToken cancellationToken = default)
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
            .Include(r => r.Question)
                .ThenInclude(q => q.SubCategory)
            .Include(r => r.AnswerOption)
            .OrderBy(r => r.Id)
            .ToListAsync(cancellationToken);
    }


    // Returns question ids that the user has answered correctly at least once
    public async Task<HashSet<int>> GetCorrectQuestionIdsAsync(string userId, IEnumerable<int> questionIds, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        ArgumentNullException.ThrowIfNull(questionIds);

        var questionIdArray = questionIds.Distinct().ToArray();
        if (questionIdArray.Length == 0)
        {
            return new HashSet<int>();
        }

        var correctQuestionIds = await _db.UserResults
            .AsNoTracking()
            .Where(r => r.UserId == userId && r.IsCorrect && questionIdArray.Contains(r.QuestionId))
            .Select(r => r.QuestionId)
            .Distinct()
            .ToListAsync(cancellationToken);

        return correctQuestionIds.ToHashSet();
    }


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

        var latestIds = await GetLatestResultIdsForUserAndQuestionIdsAsync(userId, questionIdArray, cancellationToken);

        if (latestIds.Count == 0)
        {
            return new List<UserResult>();
        }

        return await _db.UserResults
            .AsNoTracking()
            .Where(r => latestIds.Contains(r.Id))
            .OrderBy(r => r.QuestionId)
            .ToListAsync(cancellationToken);
    }


    // Returns the latest UserResult per QuestionId for a specific user in one subcategory
    public async Task<List<UserResult>> GetLatestResultsForUserAndSubCategoryAsync(string userId, int subCategoryId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);

        var latestIds = await GetLatestResultIdsForUserAndSubCategoryAsync(userId, subCategoryId, cancellationToken);
        if (latestIds.Count == 0)
        {
            return new List<UserResult>();
        }

        return await _db.UserResults
            .AsNoTracking()
            .Where(r => latestIds.Contains(r.Id))
            .OrderBy(r => r.QuestionId)
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


    // Finds the latest result id per question for a user and a selected set of question ids
    private async Task<List<int>> GetLatestResultIdsForUserAndQuestionIdsAsync(string userId, IReadOnlyCollection<int> questionIds, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        ArgumentNullException.ThrowIfNull(questionIds);

        if (questionIds.Count == 0)
        {
            return new List<int>();
        }

        return await _db.UserResults
            .AsNoTracking()
            .Where(r => r.UserId == userId && questionIds.Contains(r.QuestionId))
            .GroupBy(r => r.QuestionId)
            .Select(g => g.Max(r => r.Id))
            .ToListAsync(cancellationToken);
    }


    // Finds the latest result id per question for a user inside one subcategory
    private async Task<List<int>> GetLatestResultIdsForUserAndSubCategoryAsync(string userId, int subCategoryId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);

        return await _db.UserResults
            .AsNoTracking()
            .Where(r => r.UserId == userId && r.Question.SubCategoryId == subCategoryId)
            .GroupBy(r => r.QuestionId)
            .Select(g => g.Max(r => r.Id))
            .ToListAsync(cancellationToken);
    }
}



// HashSet is C#'s built-in collection for storing unique values with fast lookup.
// In this case, it is used to store question ids that the user has answered correctly at least once,
// check if id exists
// Avoids duplicate database queries for the same question id when checking progress or correctness of answers.



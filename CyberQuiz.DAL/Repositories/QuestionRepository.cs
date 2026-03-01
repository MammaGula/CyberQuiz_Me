using CyberQuiz.DAL.Data;
using CyberQuiz.DAL.Entities;
using CyberQuiz.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Linq;

namespace CyberQuiz.DAL.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly CyberQuizDbContext _db;

    public QuestionRepository(CyberQuizDbContext db)
    {
        _db = db;
    }


    // Returns all questions from the database
    public async Task<List<Question>> GetAllAsync()
        => await _db.Questions
            .AsNoTracking()
            .OrderBy(q => q.SubCategoryId)
            .ThenBy(q => q.Id)
            .ToListAsync();

    // Returns 1 question with given id, or null if not found
    public async Task<Question?> GetByIdAsync(int id)
        => await _db.Questions.FindAsync(id);

    // Returns all questions that belong to given subCategoryId
    public async Task<List<Question>> GetBySubCategoryAsync(int subCategoryId, CancellationToken cancellationToken = default)
        => await _db.Questions
            .AsNoTracking()
            .Where(q => q.SubCategoryId == subCategoryId)
            .OrderBy(q => q.Id)
            .ToListAsync(cancellationToken);

    // Returns counts of questions grouped by SubCategoryId for given sub ids
    public async Task<Dictionary<int,int>> GetQuestionCountsBySubIdsAsync(IEnumerable<int> subIds, CancellationToken cancellationToken = default)
        => await _db.Questions
            .AsNoTracking()
            .Where(q => subIds.Contains(q.SubCategoryId))
            .GroupBy(q => q.SubCategoryId)
            .Select(g => new { SubId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.SubId, x => x.Count, cancellationToken);

    public async Task AddAsync(Question question)
        => await _db.Questions.AddAsync(question);

    public void Remove(Question question)
        => _db.Questions.Remove(question);
}
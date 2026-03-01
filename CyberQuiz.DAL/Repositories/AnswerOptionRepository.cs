using CyberQuiz.DAL.Data;
using CyberQuiz.DAL.Entities;
using CyberQuiz.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;

namespace CyberQuiz.DAL.Repositories;

public class AnswerOptionRepository : IAnswerOptionRepository
{
    private readonly CyberQuizDbContext _db;

    public AnswerOptionRepository(CyberQuizDbContext db)
    {
		ArgumentNullException.ThrowIfNull(db);
        _db = db;
    }


    // Returns all answer options in the database(For Admin ) >> ALL
    public async Task<List<AnswerOption>> GetAllAsync()
        => await _db.AnswerOptions
            .AsNoTracking()
            .OrderBy(a => a.QuestionId)
            .ThenBy(a => a.DisplayOrder)
            .ThenBy(a => a.Id)
            .ToListAsync();


    // Returns all answer options for a specific question >> All for 1 question
    public async Task<List<AnswerOption>> GetByQuestionIdAsync(int questionId, CancellationToken cancellationToken = default) // Default is NONE
        => await _db.AnswerOptions
            .AsNoTracking()
            .Where(o => o.QuestionId == questionId)
            .OrderBy(o => o.DisplayOrder)
            .ToListAsync(cancellationToken);

    // Returns a specific answer option by its ID 
    public async Task<AnswerOption?> GetByIdAsync(int id)
        => await _db.AnswerOptions.FindAsync(id); // for PK only!!!

    public async Task AddAsync(AnswerOption option)
	{
		ArgumentNullException.ThrowIfNull(option);
		await _db.AnswerOptions.AddAsync(option);
	}

    public void Remove(AnswerOption option)
	{
		ArgumentNullException.ThrowIfNull(option);
		_db.AnswerOptions.Remove(option);
	}
}

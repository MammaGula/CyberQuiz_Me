using CyberQuiz.DAL.Entities;
using System.Threading;

namespace CyberQuiz.DAL.Repositories.Interfaces;

public interface IAnswerOptionRepository
{
    // Returns all answer options in the database(For Admin ) >> ALL
    Task<List<AnswerOption>> GetAllAsync();


    // Returns all answer options for a specific question >> All for 1 question
    Task<List<AnswerOption>> GetByQuestionIdAsync(int questionId, CancellationToken cancellationToken = default);
    
    // Returns a specific answer option by its ID 
    Task<AnswerOption?> GetByIdAsync(int id);
    Task AddAsync(AnswerOption option);
    void Remove(AnswerOption option);
}


// CancellationToken: Contacts the cancellation request and propagates notification that operations should be canceled. It is used to gracefully handle task cancellations,
// allowing you to stop ongoing operations when they are no longer needed or when a timeout occurs.

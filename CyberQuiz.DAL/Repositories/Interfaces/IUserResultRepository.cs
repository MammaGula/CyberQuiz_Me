using CyberQuiz.DAL.Entities;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace CyberQuiz.DAL.Repositories.Interfaces;

public interface IUserResultRepository
{
    Task<List<UserResult>> GetAllAsync();
    Task<UserResult?> GetByIdAsync(int id);

    // Returns latest UserResult per QuestionId for a given user and set of questionIds
    Task<List<UserResult>> GetLatestResultsForUserAndQuestionIdsAsync(string userId, IEnumerable<int> questionIds, CancellationToken cancellationToken = default);
    Task AddAsync(UserResult result);
    void Remove(UserResult result);
}
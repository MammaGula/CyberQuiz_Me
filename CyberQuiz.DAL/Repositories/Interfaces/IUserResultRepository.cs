using CyberQuiz.DAL.Entities;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace CyberQuiz.DAL.Repositories.Interfaces;

public interface IUserResultRepository
{
    Task<List<UserResult>> GetAllAsync(CancellationToken cancellationToken = default);

    // Returns all results for a given user in a specific subcategory
    Task<List<UserResult>> GetByUserAndSubCategoryAsync(string userId, int subCategoryId, CancellationToken cancellationToken = default);

    
    // Returns matching results with the related question, subcategory and selected answer option
    Task<List<UserResult>> GetByUserAndQuestionIdsWithDetailsAsync(string userId, IEnumerable<int> questionIds, CancellationToken cancellationToken = default);

   
    // Returns only question ids that the user has answered correctly at least once
    Task<HashSet<int>> GetCorrectQuestionIdsAsync(string userId, IEnumerable<int> questionIds, CancellationToken cancellationToken = default);

    
    // Returns latest UserResult per QuestionId for a given user and set of questionIds
    Task<List<UserResult>> GetLatestResultsForUserAndQuestionIdsAsync(string userId, IEnumerable<int> questionIds, CancellationToken cancellationToken = default);

    
    // Returns latest UserResult per QuestionId for a given user in a specific subcategory
    Task<List<UserResult>> GetLatestResultsForUserAndSubCategoryAsync(string userId, int subCategoryId, CancellationToken cancellationToken = default);

    Task AddAsync(UserResult result);
    void Remove(UserResult result);
}


// HashSet is C#'s built-in collection for storing unique values with fast lookup.
// In this case, it is used to store question ids that the user has answered correctly at least once,
// check if id exists
// Avoids duplicate database queries for the same question id when checking progress or correctness of answers.
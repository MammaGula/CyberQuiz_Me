using CyberQuiz.DAL.Entities;

namespace CyberQuiz.DAL.Repositories.Interfaces;

public interface IQuestionRepository
{

    // Returns all questions from the database
    Task<List<Question>> GetAllAsync();

    // Returns 1 question with given id, or null if not found
    Task<Question?> GetByIdAsync(int id);

    // Returns all questions that belong to given subCategoryId
    Task<List<Question>> GetBySubCategoryAsync(int subCategoryId, CancellationToken cancellationToken = default);
    
    // Returns counts of questions grouped by SubCategoryId for given sub ids
    Task<Dictionary<int,int>> GetQuestionCountsBySubIdsAsync(IEnumerable<int> subIds, CancellationToken cancellationToken = default);
    
    Task AddAsync(Question question);
   
    void Remove(Question question);
}
using CyberQuiz.DAL.Repositories.Interfaces;

namespace CyberQuiz.DAL.Repositories.Interfaces;


public interface IUnitOfWork
{
    ICategoryRepository Categories { get; }
    ISubCategoryRepository SubCategories { get; }
    IQuestionRepository Questions { get; }
    IAnswerOptionRepository AnswerOptions { get; }
    IUserResultRepository UserResults { get; }

    Task<int> SaveAsync();
    Task<int> SaveAsync(CancellationToken cancellationToken);
}
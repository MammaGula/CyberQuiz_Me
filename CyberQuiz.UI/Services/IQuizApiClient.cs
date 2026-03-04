using CyberQuiz.Shared.DTOs;

namespace CyberQuiz.UI.Services
{
    public interface IQuizApiClient
    {
        Task<List<CategoryDto>> GetCategoriesAsync(string userId);
        Task<List<SubCategoryDto>> GetSubCategoriesAsync(int categoryId, string userId);
        Task<List<QuestionDto>> GetQuestionsAsync(int subCategoryId, string userId);
        Task<SubmitAnswerResponseDto> SubmitAnswerAsync(string userId, SubmitAnswerRequestDto request);
        Task<UserProgressDto> GetUserProgressAsync(string userId);
    }
}
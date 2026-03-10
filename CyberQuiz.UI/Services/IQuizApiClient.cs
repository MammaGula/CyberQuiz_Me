using CyberQuiz.Shared.AI;
using CyberQuiz.Shared.DTOs;

namespace CyberQuiz.UI.Services
{

    //Interface som definierar vilka API-metoder som UI kan använda
    //ApiService implementerar detta interface
    public interface IQuizApiClient
    {
        //Hämtar alla kategorier för en användare
        Task<List<CategoryDto>> GetCategoriesAsync(string userId);

        //hämtar alla subcategories för en specifik kategori
        Task<List<SubCategoryDto>> GetSubCategoriesAsync(int categoryId, string userId);

        //hämtar quizfrågor för en specifik subcategory
        Task<List<QuestionDto>> GetQuestionsAsync(int subCategoryId, string userId);

        //skickar användarens svar till API:t för att kontrollera rätt/fel
        Task<SubmitAnswerResponseDto> SubmitAnswerAsync(string userId, SubmitAnswerRequestDto request);

        //Hämtar användarens progression
        Task<UserProgressDto> GetUserProgressAsync(string userId);

        //Skickar en fråga till AI-endpoint och return AI: Svar
        Task<AiChatResponseDto> AskAiAsync(AiChatRequestDto request);
    }
}
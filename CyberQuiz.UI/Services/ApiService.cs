// ApiService fungerar som ett mellanlager mellan Blazor UI och backend API.
// Den ansvarar för att skicka och ta emot HTTP-requests via HttpClient.

using CyberQuiz.Shared.DTOs;
using CyberQuiz.Shared.AI;

namespace CyberQuiz.UI.Services
{
    public class ApiService : IQuizApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //Hämta kategorier från API med GET-Request
        public async Task<List<CategoryDto>> GetCategoriesAsync(string userId)
        {
            return await _httpClient.GetFromJsonAsync<List<CategoryDto>>(
                $"api/quiz/categories?userId={userId}");
        }

        //Hämtar subkategorier från rätt kategori med categoryId från API med GET-Request
        public async Task<List<SubCategoryDto>> GetSubCategoriesAsync(int categoryId, string userId)
        {
            return await _httpClient.GetFromJsonAsync<List<SubCategoryDto>>(
                $"api/quiz/subcategories?categoryId={categoryId}&userId={userId}");
        }

        //Hämtar alla frågor från en viss subkategori
        public async Task<List<QuestionDto>> GetQuestionsAsync(int subCategoryId, string userId)
        {
            return await _httpClient.GetFromJsonAsync<List<QuestionDto>>(
                $"api/quiz/questions?subCategoryId={subCategoryId}&userId={userId}");
        }

        //Skickar användarens svar med POST-request till API:t.
        public async Task<SubmitAnswerResponseDto> SubmitAnswerAsync(string userId, SubmitAnswerRequestDto request)
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"api/quiz/submit-answer?userId={userId}", request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<SubmitAnswerResponseDto>()
                   ?? throw new Exception("Failed to deserialize SubmitAnswerResponseDto");
        }

        //Hämtar användarens progression (vilka subcategories är klarade ex. 3/6) och upplåsta.
        public async Task<UserProgressDto> GetUserProgressAsync(string userId)
        {
            return await _httpClient.GetFromJsonAsync<UserProgressDto>(
                $"api/quiz/user-progress?userId={userId}");
        }

        //UI skickar användarens fråga och context till API:t. 
        public async Task<AiChatResponseDto> AskAiAsync(AiChatRequestDto request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/ai/chat", request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<AiChatResponseDto>()
                   ?? throw new Exception("Failed to deserialize AiChatResponseDto");
        }
    }
}

using CyberQuiz.Shared.DTOs;

namespace CyberQuiz.UI.Services
{
    public class ApiService : IQuizApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync(string userId)
        {
            return await _httpClient.GetFromJsonAsync<List<CategoryDto>>(
                $"api/quiz/categories?userId={userId}");
        }

        public async Task<List<SubCategoryDto>> GetSubCategoriesAsync(int categoryId, string userId)
        {
            return await _httpClient.GetFromJsonAsync<List<SubCategoryDto>>(
                $"api/quiz/subcategories?categoryId={categoryId}&userId={userId}");
        }

        public async Task<List<QuestionDto>> GetQuestionsAsync(int subCategoryId, string userId)
        {
            return await _httpClient.GetFromJsonAsync<List<QuestionDto>>(
                $"api/quiz/questions?subCategoryId={subCategoryId}&userId={userId}");
        }

        public async Task<SubmitAnswerResponseDto> SubmitAnswerAsync(string userId, SubmitAnswerRequestDto request)
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"api/quiz/submit-answer?userId={userId}", request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<SubmitAnswerResponseDto>()
                   ?? throw new Exception("Failed to deserialize SubmitAnswerResponseDto");
        }
    }
}

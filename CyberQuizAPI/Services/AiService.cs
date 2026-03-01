using System.Net.Http.Json;

namespace CyberQuiz.API.Services;

public class AiService
{
    private readonly HttpClient _httpClient;

    public AiService(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        _httpClient = httpClient;
    }

    private class OllamaRequest
    {
        public string model { get; set; } = "phi3";
        public string prompt { get; set; } = string.Empty;
        public bool stream { get; set; } = false;
    }

    private class OllamaResponse
    {
        public string? model { get; set; }
        public string? created_at { get; set; }
        public string? response { get; set; }
        public bool done { get; set; }
    }

    public async Task<string> AskAsync(string prompt, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(prompt);

        var request = new OllamaRequest
        {
            prompt = prompt,
            stream = false
        };

        var response = await _httpClient.PostAsJsonAsync("/api/generate", request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<OllamaResponse>(cancellationToken);
        return result?.response ?? string.Empty;
    }
}

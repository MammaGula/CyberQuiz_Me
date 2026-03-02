using System.ComponentModel.DataAnnotations;

namespace CyberQuiz.Shared.AI;

public sealed record AiChatResponseDto
{
    [Required]
    public string Answer { get; init; } = string.Empty;
}

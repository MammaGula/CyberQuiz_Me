using System.ComponentModel.DataAnnotations;

namespace CyberQuiz.Shared.AI;

public sealed record AiCoachResponseDto
{
    [Required]
    public string Feedback { get; init; } = string.Empty;
}

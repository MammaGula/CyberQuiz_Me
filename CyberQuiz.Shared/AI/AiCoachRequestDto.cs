using System.ComponentModel.DataAnnotations;

namespace CyberQuiz.Shared.AI;

public sealed record AiCoachRequestDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(8000)]
    public string Summary { get; init; } = string.Empty;
}

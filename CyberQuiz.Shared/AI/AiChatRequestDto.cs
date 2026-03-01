using System.ComponentModel.DataAnnotations;

namespace CyberQuiz.Shared.AI;

public sealed record AiChatRequestDto
{
    [Required]
    [MinLength(1)]
    [MaxLength(2000)]
    public string Prompt { get; init; } = string.Empty;

    [MaxLength(4000)]
    public string? Context { get; init; }
}

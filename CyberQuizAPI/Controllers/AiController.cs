using System.Security.Claims;
using CyberQuiz.API.Services;
using CyberQuiz.Shared.AI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyberQuiz.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AiController : ControllerBase
{
    private readonly AiService _ai;

    public AiController(AiService ai)
    {
        _ai = ai;
    }

    [HttpPost("chat")]
    public async Task<ActionResult<AiChatResponseDto>> Chat([FromBody] AiChatRequestDto req)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var finalPrompt = req.Context is null
            ? req.Prompt
            : $"UserId: {userId}\nContext (quiz): {req.Context}\n\nUser question: {req.Prompt}";

        var answer = await _ai.AskAsync(finalPrompt);

        return Ok(new AiChatResponseDto
        {
            Answer = answer
        });
    }

    [HttpPost("coach")]
    public async Task<ActionResult<AiCoachResponseDto>> Coach([FromBody] AiCoachRequestDto req)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var prompt =
            $"Du är en personlig cybersäkerhetscoach.\n" +
            $"UserId: {userId}\n" +
            $"Quiz summary: {req.Summary}\n\n" +
            "Analysera styrkor och svagheter och ge konkreta rekommendationer på svenska.";

        var feedback = await _ai.AskAsync(prompt);

        return Ok(new AiCoachResponseDto
        {
            Feedback = feedback
        });
    }
}












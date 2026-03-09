using CyberQuiz.API.Services;
using CyberQuiz.Shared.AI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyberQuiz.API.Controllers;

[ApiController]
[Route("api/[controller]")]
//[AllowAnonymous]
public class AiController : ControllerBase
{
    private readonly AiService _ai;

    public AiController(AiService ai)
    {
        _ai = ai;
    }

    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] AiChatRequestDto? req, CancellationToken cancellationToken)
    {
        // Validate the request body, recieve the promp from the user
        if (req is null)
        {
            return BadRequest("Request body is required.");
        }

        // finalPrompt will be the prompt we send to the AI.
        // If context is provided, we include it in the prompt.

        var finalPrompt = req.Prompt;

        //Promt: inputText from the user
        // Context: additional information that can help the AI provide a better answer(ex. quiz questions, code snippets, etc.)
        if (!string.IsNullOrWhiteSpace(req.Context))
        {
            finalPrompt = $"Context (quiz): {req.Context}\n\n User question: {req.Prompt}";
        }


        // Call AiService (AskAsyn in Service: Send promt to AI and Recieve data from AI as string)
        var answer = await _ai.AskAsync(finalPrompt, cancellationToken);


        // Return an answer to the Client as JSON (Answer from AI)
        return Ok(new AiChatResponseDto
        {
            Answer = answer
        });
    }
}




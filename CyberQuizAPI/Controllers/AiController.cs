//using System.Security.Claims;
//using CyberQuiz.API.Services;
//using CyberQuiz.Shared.AI;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace CyberQuiz.API.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//[Authorize]
//public class AiController : ControllerBase
//{
//    private readonly AiService _ai;

//    public AiController(AiService ai)
//    {
//        _ai = ai;
//    }

//    [HttpPost("chat")]
//	public async Task<ActionResult<AiChatResponseDto>> Chat([FromBody] AiChatRequestDto req, CancellationToken cancellationToken)
//    {
//		if (req is null)
//		{
//			return BadRequest("Request body is required.");
//		}

//		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//		if (string.IsNullOrWhiteSpace(userId))
//		{
//			return Unauthorized("User identity not found.");
//		}

//        var finalPrompt = req.Context is null
//            ? req.Prompt
//            : $"UserId: {userId}\nContext (quiz): {req.Context}\n\nUser question: {req.Prompt}";

//		var answer = await _ai.AskAsync(finalPrompt, cancellationToken);

//        return Ok(new AiChatResponseDto
//        {
//            Answer = answer
//        });
//    }

//    [HttpPost("coach")]
//	public async Task<ActionResult<AiCoachResponseDto>> Coach([FromBody] AiCoachRequestDto req, CancellationToken cancellationToken)
//    {
//		if (req is null)
//		{
//			return BadRequest("Request body is required.");
//		}

//		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//		if (string.IsNullOrWhiteSpace(userId))
//		{
//			return Unauthorized("User identity not found.");
//		}

//        var prompt =
//            $"Du är en personlig cybersäkerhetscoach.\n" +
//            $"UserId: {userId}\n" +
//            $"Quiz summary: {req.Summary}\n\n" +
//            "Analysera styrkor och svagheter och ge konkreta rekommendationer på svenska.";

//		var feedback = await _ai.AskAsync(prompt, cancellationToken);

//        return Ok(new AiCoachResponseDto
//        {
//            Feedback = feedback
//        });
//    }
//}












using CyberQuiz.API.Services;
using CyberQuiz.DAL.Data;
using CyberQuiz.Shared.AI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CyberQuiz.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AiController : ControllerBase
{
    private readonly AiService _ai;
    private readonly CyberQuizDbContext _db;

    public AiController(AiService ai, CyberQuizDbContext db)
    {
        ArgumentNullException.ThrowIfNull(ai);
        ArgumentNullException.ThrowIfNull(db);
        _ai = ai;
        _db = db;
    }

    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] AiChatRequestDto? req, CancellationToken cancellationToken)
    {
        //Kontrollera att prompt inte är null eller tom
        if (req is null || string.IsNullOrWhiteSpace(req.Prompt))
        {
            return BadRequest("Prompt is required.");
        }

        //Hämta kategorier och subkategorier från databasen för att skapa en kontext för AI
        var categories = await _db.Categories.ToListAsync(cancellationToken);
        var subCategories = await _db.SubCategories.ToListAsync(cancellationToken);

        var context = string.Join("\n", categories
            .OrderBy(c => c.Id)
            .Select(c =>
            {
                var subs = subCategories
                    .Where(sc => sc.CategoryId == c.Id)
                    .OrderBy(sc => sc.Id)
                    .Select(sc => $"- {sc.Name}");

                return $"Category: {c.Name}\n{string.Join("\n", subs)}";
            })
        );

        //Skapa en prompt för AI som inkluderar både användarens fråga och den kontext som hämtats från databasen
        var finalPrompt = $@"
        You are a CyberQuiz chatbot.
        Only answer questions related to the topics listed below.
        If outside topics, say you only help with CyberQuiz topics.
        Answer clearly and simply for a student.

        Topics:
        {context}

        User question:
        {req.Prompt}
        ";


        //Skicka prompt och context till AI-tjänsten och få ett svar
        var answer = await _ai.AskAsync(finalPrompt, cancellationToken);

        return Ok(new AiChatResponseDto
        {
            Answer = answer
        });
    }
}




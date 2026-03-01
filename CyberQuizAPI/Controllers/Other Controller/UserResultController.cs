//using CyberQuiz.BLL.Interfaces;
//using CyberQuiz.DAL.Entities;
//using CyberQuiz.Shared.DTOs;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace CyberQuiz.API.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//[Authorize]
//public class UserResultController : ControllerBase
//{
//    private readonly IUserResultService _resultService;
//    private readonly IQuestionService _questionService;

//    public UserResultController(IUserResultService resultService, IQuestionService questionService)
//    {
//        _resultService = resultService;
//        _questionService = questionService;
//    }

//    [HttpGet("user/{userId}")]
//    public async Task<IActionResult> GetByUser(string userId)
//        => Ok(await _resultService.GetByUserIdAsync(userId));

//    [HttpPost]
//    public async Task<IActionResult> Submit([FromBody] UserResultDto dto)
//    {
//        var question = await _questionService.GetByIdAsync(dto.QuestionId);
//        if (question == null) return NotFound();

//        var correctOption = question.AnswerOptions.FirstOrDefault(a => a.IsCorrect);
//        var isCorrect = correctOption?.Id == dto.AnswerOptionId;

//        var result = new UserResult
//        {
//            UserId = dto.UserId,
//            QuestionId = dto.QuestionId,
//            AnswerOptionId = dto.AnswerOptionId,
//            IsCorrect = isCorrect
//        };

//        await _resultService.CreateAsync(result);

//        return Ok(new SubmitAnswerResponseDto
//        {
//            IsCorrect = isCorrect,
//            CorrectOptionId = correctOption?.Id ?? 0
//        });
//    }

//    [HttpGet("progress/{userId}/{subCategoryId}")]
//    public async Task<IActionResult> GetProgress(string userId, int subCategoryId)
//    {
//        var percent = await _resultService.GetProgressAsync(userId, subCategoryId);
//        return Ok(new { percent });
//    }

//    [HttpGet("unlocked/{userId}/{subCategoryId}")]
//    public async Task<IActionResult> IsUnlocked(string userId, int subCategoryId)
//    {
//        var unlocked = await _resultService.IsUnlockedAsync(userId, subCategoryId);
//        return Ok(new { unlocked });
//    }
//}

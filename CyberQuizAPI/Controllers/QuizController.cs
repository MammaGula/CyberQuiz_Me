using CyberQuiz.BLL.Interfaces;
using CyberQuiz.DAL.Entities;
using CyberQuiz.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CyberQuiz.API.Controllers
{
    [ApiController]
    [Route("api/quiz")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;
        private readonly UserManager<AppUser> _userManager;

        public QuizController(IQuizService quizService, UserManager<AppUser> userManager)
        {
            _quizService = quizService;
            _userManager = userManager;
        }

        // GET: api/quiz/categories
        [HttpGet("categories")]
        public async Task<ActionResult<List<CategoryDto>>> GetCategories([FromQuery] string? userId)
        {
            var resolvedUserId = await ResolveUserIdAsync(userId);
            if (string.IsNullOrWhiteSpace(resolvedUserId))
                return CreateMissingUserIdResult();

            try
            {
                var result = await _quizService.GetCategoriesAsync(resolvedUserId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/quiz/subcategories?categoryId=1
        [HttpGet("subcategories")]
        public async Task<ActionResult<List<SubCategoryDto>>> GetSubCategories([FromQuery] int categoryId, [FromQuery] string? userId)
        {
            if (categoryId <= 0)
                return BadRequest("categoryId must be greater than 0.");

            var resolvedUserId = await ResolveUserIdAsync(userId);
            if (string.IsNullOrWhiteSpace(resolvedUserId))
                return CreateMissingUserIdResult();

            try
            {
                var result = await _quizService.GetSubCategoriesAsync(categoryId, resolvedUserId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/quiz/questions?subCategoryId=1
        [HttpGet("questions")]
        public async Task<ActionResult<List<QuestionDto>>> GetQuestions([FromQuery] int subCategoryId, [FromQuery] string? userId)
        {
            if (subCategoryId <= 0)
                return BadRequest("subCategoryId must be greater than 0.");

            var resolvedUserId = await ResolveUserIdAsync(userId);
            if (string.IsNullOrWhiteSpace(resolvedUserId))
                return CreateMissingUserIdResult();

            try
            {
                var result = await _quizService.GetQuestionsAsync(subCategoryId, resolvedUserId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/quiz/submit-answer
        [HttpPost("submit-answer")]
        public async Task<ActionResult<SubmitAnswerResponseDto>> SubmitAnswer([FromQuery] string? userId, [FromBody] SubmitAnswerRequestDto request)
        {
            var resolvedUserId = await ResolveUserIdAsync(userId);
            if (string.IsNullOrWhiteSpace(resolvedUserId))
                return CreateMissingUserIdResult();

            if (request is null)
                return BadRequest("Request body is required.");

            if (request.QuestionId <= 0)
                return BadRequest("QuestionId must be greater than 0.");

            if (request.AnswerOptionId <= 0)
                return BadRequest("AnswerOptionId must be greater than 0.");

            try
            {
                var result = await _quizService.SubmitAnswerAsync(resolvedUserId, request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Returns 401 if the caller is authenticated but has no usable identity claim.
        // Returns 400 if the caller is anonymous and did not provide a userId.
        private ActionResult CreateMissingUserIdResult()
        {
            if (User.Identity?.IsAuthenticated == true)
                return Unauthorized("User identity not found.");

            return BadRequest("userId is required.");
        }

        // Resolve the effective user id used by the BLL/DAL.
        // - Prefer authenticated identity claims (NameIdentifier)
        // - Otherwise (development fallback), accept userId as either Identity user id or username
        private async Task<string?> ResolveUserIdAsync(string? userId)
        {
            var nameIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrWhiteSpace(nameIdentifier))
                return nameIdentifier;

            if (User.Identity?.IsAuthenticated == true)
                return null;

            if (string.IsNullOrWhiteSpace(userId))
                return null;

            var userById = await _userManager.FindByIdAsync(userId);
            if (userById is not null)
                return userById.Id;

            var userByName = await _userManager.FindByNameAsync(userId);
            if (userByName is not null)
                return userByName.Id;

            return null;
        }
    }
}

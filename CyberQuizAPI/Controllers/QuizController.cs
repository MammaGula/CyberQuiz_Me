using CyberQuiz.BLL.Interfaces;
using CyberQuiz.DAL.Entities;
using CyberQuiz.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CyberQuiz.API.Controllers
{
    //  API Controller for “Quiz endpoints” 
    [ApiController]
    [Route("api/quiz")]
    [Authorize]

    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public QuizController(IQuizService quizService, UserManager<AppUser> userManager, IWebHostEnvironment environment)
        {
            _quizService = quizService;
            _userManager = userManager;
            _environment = environment;
        }

        // 1. Returns all Categories

        // GET: /api/quiz/categories
        [HttpGet("categories")]
        public async Task<ActionResult<List<CategoryDto>>> GetCategories()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User identity not found.");

            try
            {
                var result = await _quizService.GetCategoriesAsync(userId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // 2. Return SubCategories of this category

        // GET: /api/quiz/categories/{categoryId}/subcategories
        [HttpGet("categories/{categoryId:int}/subcategories")]
        public async Task<ActionResult<List<SubCategoryDto>>> GetSubCategoriesByCategoryId([FromRoute] int categoryId)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User identity not found.");

            try
            {
                var result = await _quizService.GetSubCategoriesAsync(categoryId, userId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }



        // GET /api/quiz/subcategories?categoryId=1 (Temporary)
        [HttpGet("subcategories")]
        public async Task<ActionResult<List<SubCategoryDto>>> GetSubCategories([FromQuery] int categoryId)
        {
            if (!_environment.IsDevelopment())
                return NotFound();
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User identity not found.");

            try
            {
                var result = await _quizService.GetSubCategoriesAsync(categoryId, userId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // 3. Return Questions of this subcategory

        // GET: /api/quiz/subcategories/{subCategoryId}/questions
        [HttpGet("subcategories/{subCategoryId:int}/questions")]
        public async Task<ActionResult<List<QuestionDto>>> GetQuestionsBySubCategoryId([FromRoute] int subCategoryId)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User identity not found.");

            try
            {
                var result = await _quizService.GetQuestionsAsync(subCategoryId, userId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // GET /api/quiz/questions?subCategoryId=1 (Temporary)
        [HttpGet("questions")]
        public async Task<ActionResult<List<QuestionDto>>> GetQuestions([FromQuery] int subCategoryId)
        {
            if (!_environment.IsDevelopment())
                return NotFound();
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User identity not found.");

            try
            {
                var result = await _quizService.GetQuestionsAsync(subCategoryId, userId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 4. Submit an answer 

        // POST: /api/quiz/submissions
        [HttpPost("submissions")]
        public async Task<ActionResult<SubmitAnswerResponseDto>> CreateSubmission([FromBody] SubmitAnswerRequestDto request)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User identity not found.");

            try
            {
                var result = await _quizService.SubmitAnswerAsync(userId, request);
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


        // POST /api/quiz/submit-answer (Temporary)
        [HttpPost("submit-answer")]
        public async Task<ActionResult<SubmitAnswerResponseDto>> SubmitAnswer([FromBody] SubmitAnswerRequestDto request)
        {
            if (!_environment.IsDevelopment())
                return NotFound();
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User identity not found.");

            try
            {
                var result = await _quizService.SubmitAnswerAsync(userId, request);
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


    }
}

using CyberQuiz.BLL.Interfaces;
//using CyberQuiz.DAL.Entities;
using CyberQuiz.Shared.DTOs;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CyberQuiz.API.Controllers
{
    //  API Controller for “Quiz endpoints” 
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]

    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;
        //private readonly UserManager<AppUser> _userManager;
        //private readonly IWebHostEnvironment _environment;

        public QuizController(IQuizService quizService/*, UserManager<AppUser> userManager, IWebHostEnvironment environment*/)
        {
            _quizService = quizService;
            //_userManager = userManager;
            //_environment = environment;
        }

        // 1. Returns all Categories

        // GET: /api/quiz/categories
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories([FromQuery] string userId)
        {
            // Validering (bra att ha i API också)
            if (string.IsNullOrEmpty(userId))
                return BadRequest("userId is required");

            var result = await _quizService.GetCategoriesAsync(userId);

            return Ok(result); // returnerar JSON till UI
        }


        // 2. Return SubCategories of this category

        // GET: api/quiz/subcategories?categoryId=1&userId=xxx
        [HttpGet("subcategories")]
        public async Task<IActionResult> GetSubCategories(
            [FromQuery] int categoryId,
            [FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("userId is required");

            var result = await _quizService.GetSubCategoriesAsync(categoryId, userId);

            return Ok(result);
        }



        //3. Return Questions of this subcategory
        // GET: api/quiz/questions?subCategoryId=1&userId=xxx
        [HttpGet("questions")]
        public async Task<IActionResult> GetQuestions(
            [FromQuery] int subCategoryId,
            [FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("userId is required");

            var result = await _quizService.GetQuestionsAsync(subCategoryId, userId);

            return Ok(result);
        }


        // 4. Submit an answer
        // POST: api/quiz/submit-answer?userId=xxx
        [HttpPost("submit-answer")]
        public async Task<IActionResult> SubmitAnswer(string userId, SubmitAnswerRequestDto request)
        {
            try
            {
                var result = await _quizService.SubmitAnswerAsync(userId, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message }); // 🔥 JSON istället
            }
        }


        // 5. Get user progress for all subcategories (for progress bar in UI)
        //hämta (GET) userprogress : 
        [HttpGet("user-progress")]
        public async Task<ActionResult<UserProgressDto>> GetUserProgress(string userId)
        {
            var progress = await _quizService.GetUserProgressAsync(userId);
            return Ok(progress);
        }
    }
}













// 3. Return Questions of this subcategory

// GET: /api/quiz/subcategories/{subCategoryId}/questions
//[HttpGet("subcategories/{subCategoryId:int}/questions")]
//public async Task<ActionResult<List<QuestionDto>>> GetQuestionsBySubCategoryId([FromRoute] int subCategoryId)
//{
//    var userId = _userManager.GetUserId(User);
//    if (string.IsNullOrWhiteSpace(userId))
//        return Unauthorized("User identity not found.");

//    try
//    {
//        var result = await _quizService.GetQuestionsAsync(subCategoryId, userId);
//        return Ok(result);
//    }
//    catch (ArgumentException ex)
//    {
//        return BadRequest(ex.Message);
//    }
//}


// GET /api/quiz/questions?subCategoryId=1 (Temporary)
//[HttpGet("questions")]
//public async Task<ActionResult<List<QuestionDto>>> GetQuestions([FromQuery] int subCategoryId)
//{
//    if (!_environment.IsDevelopment())
//        return NotFound();
//    var userId = _userManager.GetUserId(User);
//    if (string.IsNullOrWhiteSpace(userId))
//        return Unauthorized("User identity not found.");

//    try
//    {
//        var result = await _quizService.GetQuestionsAsync(subCategoryId, userId);
//        return Ok(result);
//    }
//    catch (ArgumentException ex)
//    {
//        return BadRequest(ex.Message);
//    }
//}

// 4. Submit an answer 

// POST: /api/quiz/submissions
//[HttpPost("submissions")]
//public async Task<ActionResult<SubmitAnswerResponseDto>> CreateSubmission([FromBody] SubmitAnswerRequestDto request)
//{
//    var userId = _userManager.GetUserId(User);
//    if (string.IsNullOrWhiteSpace(userId))
//        return Unauthorized("User identity not found.");

//    try
//    {
//        var result = await _quizService.SubmitAnswerAsync(userId, request);
//        return Ok(result);
//    }
//    catch (ArgumentException ex)
//    {
//        return BadRequest(ex.Message);
//    }
//    catch (InvalidOperationException ex)
//    {
//        return BadRequest(ex.Message);
//    }
//}


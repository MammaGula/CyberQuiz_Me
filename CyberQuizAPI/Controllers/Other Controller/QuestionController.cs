//using CyberQuiz.BLL.Interfaces;
//using CyberQuiz.DAL.Entities;
//using CyberQuiz.Shared.DTOs;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace CyberQuiz.API.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//[Authorize]
//public class QuestionController : ControllerBase
//{
//    private readonly IQuestionService _service;

//    public QuestionController(IQuestionService service)
//    {
//        _service = service;
//    }

//    [HttpGet]
//    public async Task<IActionResult> GetAll()
//        => Ok(await _service.GetAllAsync());

//    [HttpGet("{id}")]
//    public async Task<IActionResult> GetById(int id)
//    {
//        var q = await _service.GetByIdAsync(id);
//        return q == null ? NotFound() : Ok(q);
//    }

//    [HttpGet("subcategory/{subCategoryId}")]
//    public async Task<IActionResult> GetBySubCategory(int subCategoryId)
//    {
//        var questions = (await _service.GetAllAsync())
//            .Where(q => q.SubCategoryId == subCategoryId)
//            .Select(q => new QuestionDto
//            {
//                Id = q.Id,
//                Text = q.Text,
//                Options = q.AnswerOptions.Select(a => new AnswerOptionDto
//                {
//                    Id = a.Id,
//                    Text = a.Text
//                }).ToList()
//            });
//        return Ok(questions);
//    }

//    [HttpPost]
//    public async Task<IActionResult> Create(Question question)
//    {
//        var created = await _service.CreateAsync(question);
//        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
//    }

//    [HttpDelete("{id}")]
//    public async Task<IActionResult> Delete(int id)
//    {
//        var success = await _service.DeleteAsync(id);
//        return success ? NoContent() : NotFound();
//    }
//}

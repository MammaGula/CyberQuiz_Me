//using CyberQuiz.BLL.Interfaces;
//using CyberQuiz.DAL.Entities;
//using CyberQuiz.Shared.DTOs;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace CyberQuiz.API.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//[Authorize]
//public class SubCategoryController : ControllerBase
//{
//    private readonly ISubCategoryService _service;

//    public SubCategoryController(ISubCategoryService service)
//    {
//        _service = service;
//    }

//    [HttpGet]
//    public async Task<IActionResult> GetAll()
//        => Ok(await _service.GetAllAsync());

//    [HttpGet("{id}")]
//    public async Task<IActionResult> GetById(int id)
//    {
//        var sub = await _service.GetByIdAsync(id);
//        if (sub == null) return NotFound();

//        return Ok(new SubCategoryDto
//        {
//            Id = sub.Id,
//            Name = sub.Name,
//            CategoryId = sub.CategoryId,
//            QuestionCount = sub.Questions.Count
//        });
//    }

//    [HttpGet("category/{categoryId}")]
//    public async Task<IActionResult> GetByCategory(int categoryId)
//    {
//        var subs = (await _service.GetAllAsync())
//            .Where(s => s.CategoryId == categoryId)
//            .Select(s => new SubCategoryDto
//            {
//                Id = s.Id,
//                Name = s.Name,
//                CategoryId = s.CategoryId,
//                QuestionCount = s.Questions.Count
//            });
//        return Ok(subs);
//    }

//    [HttpPost]
//    public async Task<IActionResult> Create(SubCategory subCategory)
//    {
//        var created = await _service.CreateAsync(subCategory);
//        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
//    }

//    [HttpDelete("{id}")]
//    public async Task<IActionResult> Delete(int id)
//    {
//        var success = await _service.DeleteAsync(id);
//        return success ? NoContent() : NotFound();
//    }
//}

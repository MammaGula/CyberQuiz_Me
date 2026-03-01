//using CyberQuiz.BLL.Interfaces;
//using CyberQuiz.DAL.Entities;
//using Microsoft.AspNetCore.Mvc;

//namespace CyberQuiz.API.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//public class CategoryController : ControllerBase
//{
//    private readonly ICategoryService _service;

//    public CategoryController(ICategoryService service)
//    {
//        _service = service;
//    }

//    [HttpGet]
//    public async Task<IActionResult> GetAll()
//        => Ok(await _service.GetAllAsync());

//    [HttpGet("{id}")]
//    public async Task<IActionResult> GetById(int id)
//    {
//        var category = await _service.GetByIdAsync(id);
//        return category == null ? NotFound() : Ok(category);
//    }

//    [HttpPost]
//    public async Task<IActionResult> Create(Category category)
//    {
//        var created = await _service.CreateAsync(category);
//        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
//    }

//    [HttpDelete("{id}")]
//    public async Task<IActionResult> Delete(int id)
//    {
//        var success = await _service.DeleteAsync(id);
//        return success ? NoContent() : NotFound();
//    }
//}
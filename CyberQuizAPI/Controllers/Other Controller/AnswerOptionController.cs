//using CyberQuiz.BLL.Interfaces;
//using CyberQuiz.DAL.Entities;
//using Microsoft.AspNetCore.Mvc;

//namespace CyberQuiz.API.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//public class AnswerOptionController : ControllerBase
//{
//    private readonly IAnswerOptionService _service;

//    public AnswerOptionController(IAnswerOptionService service)
//    {
//        _service = service;
//    }

//    [HttpGet]
//    public async Task<IActionResult> GetAll()
//        => Ok(await _service.GetAllAsync());

//    [HttpGet("{id}")]
//    public async Task<IActionResult> GetById(int id)
//    {
//        var opt = await _service.GetByIdAsync(id);
//        return opt == null ? NotFound() : Ok(opt);
//    }

//    [HttpPost]
//    public async Task<IActionResult> Create(AnswerOption option)
//    {
//        var created = await _service.CreateAsync(option);
//        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
//    }

//    [HttpDelete("{id}")]
//    public async Task<IActionResult> Delete(int id)
//    {
//        var success = await _service.DeleteAsync(id);
//        return success ? NoContent() : NotFound();
//    }
//}
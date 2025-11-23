using HabitTracker.Models;
using HabitTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Controllers;
[Route("api/habit")]
[ApiController]
public class HabitController : ControllerBase
{
    private readonly IHabitService _habitService;

    public HabitController(IHabitService habitService)
    {
        _habitService = habitService;
    }


    [HttpPost]
    public ActionResult Create([FromBody] CreateHabitDto dto)
    {
        var id  = _habitService.Create(dto);

        return Created($"/api/habit/{id}", null);
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<HabitDto>> GetAll()
    {
        var habitsDtos = _habitService.GetAll();
        return Ok(habitsDtos); 
    }

    
    [HttpGet("{id}")]
    public ActionResult<HabitDto> GetById([FromRoute] int id)
    {
        var habit = _habitService.GetById(id);
        
        return Ok(habit);
    }

    [HttpPut("{id}")]
    public ActionResult Update([FromBody] UpdateHabitDto dto, [FromRoute] int id)
    {
        _habitService.Update(id,dto);
        return Ok();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete([FromRoute] int id)
    {
        _habitService.Delete(id);
        return NoContent();
    }

}
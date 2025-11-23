using AutoMapper;
using HabitTracker.Entity;
using HabitTracker.Exceptions;
using HabitTracker.Models;

namespace HabitTracker.Services;

public interface IHabitService
{
    IEnumerable<HabitDto> GetAll();
    HabitDto GetById(int id);
    int Create(CreateHabitDto dto);
    void Update(int id, UpdateHabitDto dto);
    void Delete(int id);
}
public class HabitService : IHabitService
{
    private readonly TrackerDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<HabitService> _logger;

    public HabitService(TrackerDbContext dbContext, IMapper mapper, ILogger<HabitService> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public void Update(int id, UpdateHabitDto dto)
    {
        var habit = _dbContext
            .Habits
            .FirstOrDefault(h => h.Id == id);
        if(habit == null)  throw new NotFoundException("Habit not found");
        
        habit.Title = dto.Title;
        habit.Description = dto.Description;
        habit.Frequency = dto.Frequency;
        _dbContext.SaveChanges();
        
    }
    public void Delete(int id)
    {
        _logger.LogError($"Habit with id {id} DELETE action invoked ");
        
        var habit = _dbContext
            .Habits
            .FirstOrDefault(h => h.Id == id);
        if(habit == null)  throw new NotFoundException("Habit not found");
        
        _dbContext.Habits.Remove(habit);
        _dbContext.SaveChanges();
    }
   
    public IEnumerable<HabitDto> GetAll()
    {
        var habits = _dbContext
            .Habits
            .ToList();
        
        var habitsDtos = _mapper.Map<List<HabitDto>>(habits);
        return habitsDtos;
    }

    public HabitDto GetById(int id)
    {
        var habit = _dbContext
            .Habits
            .FirstOrDefault(h => h.Id == id);
        if(habit == null)  throw new NotFoundException("Habit not found");
        
        var result = _mapper.Map<HabitDto>(habit);
        return result;
    }

    public int Create(CreateHabitDto dto)
    {
        var habit = _mapper.Map<Habit>(dto);
        _dbContext.Habits.Add(habit);
        _dbContext.SaveChanges();
        return habit.Id;
    }
}
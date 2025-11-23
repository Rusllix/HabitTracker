using AutoMapper;
using HabitTracker.Entity;
using HabitTracker.Models;

namespace HabitTracker;

public class HabitMappingProfile :Profile
{
    public HabitMappingProfile()
    {
        CreateMap<Habit, HabitDto>();
        CreateMap<CreateHabitDto, Habit>();
        




    }
}
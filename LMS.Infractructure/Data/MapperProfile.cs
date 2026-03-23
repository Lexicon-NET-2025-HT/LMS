using AutoMapper;
using Domain.Models.Entities;
using LMS.Shared.DTOs.AuthDtos;
using LMS.Shared.DTOs.Course;

namespace LMS.Infractructure.Data;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<UserRegistrationDto, ApplicationUser>();

        // Course mappings
        CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.StudentCount, opt => opt.MapFrom(src => src.Students.Count))
            .ForMember(dest => dest.ModuleCount, opt => opt.MapFrom(src => src.Modules.Count))
            .ForMember(dest => dest.TeacherIds, opt => opt.MapFrom(src =>
                src.CourseTeachers.Select(ct => ct.TeacherId).ToList()));
        CreateMap<Course, CourseDetailDto>()
            .ForMember(dest => dest.StudentCount, opt => opt.MapFrom(src => src.Students.Count))
            .ForMember(dest => dest.ModuleCount, opt => opt.MapFrom(src => src.Modules.Count))
            .ForMember(dest => dest.TeacherIds, opt => opt.MapFrom(src =>
                src.CourseTeachers.Select(ct => ct.TeacherId).ToList()));

        CreateMap<CreateCourseDto, Course>();

        CreateMap<UpdateCourseDto, Course>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}

using AutoMapper;
using Domain.Models.Entities;
using LMS.Shared.DTOs.Activity;
using LMS.Shared.DTOs.AuthDtos;
using LMS.Shared.DTOs.Course;
using LMS.Shared.DTOs.Document;
using LMS.Shared.DTOs.Module;
using LMS.Shared.DTOs.Submission;
using LMS.Shared.DTOs.SubmissionComment;

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

        CreateMap<Module, ModuleDto>()
            .ForMember(dest => dest.ActivityCount, opt => opt.MapFrom(src => src.Activities.Count));

        CreateMap<Module, ModuleDetailDto>();

        CreateMap<CreateModuleDto, Module>();

        CreateMap<Activity, ActivityDto>()
            .ForMember(dest => dest.DocumentCount, opt => opt.MapFrom(src => src.Documents.Count))
            .ForMember(dest => dest.SubmissionCount, opt => opt.MapFrom(src => src.Submissions.Count));

        CreateMap<Document, DocumentDto>()
            .ForMember(dest => dest.Scope, opt => opt.MapFrom(
                src =>
                src.CourseId != null ? nameof(Course) :
                src.ModuleId != null ? nameof(Module) :
                src.ActivityId != null ? nameof(Activity) :
                src.SubmissionId != null ? nameof(Submission) :
                "Unknown"));

        CreateMap<Submission, SubmissionDto>()
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.UserName))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Comments.Count))
            .ForMember(dest => dest.ActivityName, opt => opt.MapFrom(src => src.Activity.Name));

        CreateMap<SubmissionComment, SubmissionCommentDto>()
            .ForMember(dest => dest.AuthorName,
                opt => opt.MapFrom(src => src.Author.UserName));

    }
}

using AutoMapper;
using Domain.Models.Entities;
using LMS.Shared.DTOs.Activity;
using LMS.Shared.DTOs.AuthDtos;
using LMS.Shared.DTOs.Course;
using LMS.Shared.DTOs.Module;
using LMS.Shared.DTOs.User;
using LMS.Shared.DTOs.Document;
using LMS.Shared.DTOs.Submission;
using LMS.Shared.DTOs.SubmissionComment;

namespace LMS.Infractructure.Data;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        // user mappings
        CreateMap<UserRegistrationDto, ApplicationUser>();

        // User mappings
        CreateMap<ApplicationUser, UserBasicDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

        CreateMap<ApplicationUser, StudentBasicDto>()
            .IncludeBase<ApplicationUser, UserBasicDto>()
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course != null ? src.Course.Name : null));

        CreateMap<ApplicationUser, UserDto>()
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course != null ? src.Course.Name : null));

        // Module mapping
        CreateMap<Module, ModuleDto>()
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course != null ? src.Course.Name : string.Empty))
            .ForMember(dest => dest.ActivityCount, opt => opt.MapFrom(src => src.Activities.Count));

        // Course mappings
        CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.StudentCount, opt => opt.MapFrom(src => src.Students.Count))
            .ForMember(dest => dest.ModuleCount, opt => opt.MapFrom(src => src.Modules.Count))
            .ForMember(dest => dest.TeacherIds, opt => opt.MapFrom(src =>
                src.CourseTeachers.Select(ct => ct.TeacherId).ToList()));

        CreateMap<Course, CourseDetailDto>()
            .IncludeBase<Course, CourseDto>()
            .ForMember(dest => dest.Students, opt => opt.MapFrom(src =>
                src.Students.Select(s => new StudentBasicDto
                {
                    Id = s.Id,
                    UserName = s.UserName ?? string.Empty,
                    Email = s.Email ?? string.Empty,
                    CourseId = src.Id,
                    CourseName = src.Name
                }).ToList()))
            .ForMember(dest => dest.Modules, opt => opt.MapFrom(src => src.Modules));

        CreateMap<CreateCourseDto, Course>();

        CreateMap<UpdateCourseDto, Course>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // module mappings
        CreateMap<Module, ModuleDto>()
            .ForMember(dest => dest.ActivityCount, opt => opt.MapFrom(src => src.Activities.Count));

        CreateMap<Module, ModuleDetailDto>();

        CreateMap<CreateModuleDto, Module>();

        // activity mappings
        CreateMap<Activity, ActivityDto>()
            .ForMember(dest => dest.DocumentCount, opt => opt.MapFrom(src => src.Documents.Count))
            .ForMember(dest => dest.SubmissionCount, opt => opt.MapFrom(src => src.Submissions.Count));

        // document mappings
        CreateMap<Document, DocumentDto>()
            .ForMember(dest => dest.Scope, opt => opt.MapFrom(
                src =>
                src.CourseId != null ? nameof(Course) :
                src.ModuleId != null ? nameof(Module) :
                src.ActivityId != null ? nameof(Activity) :
                src.SubmissionId != null ? nameof(Submission) :
                "Unknown"));

        CreateMap<CreateDocumentDto, Document>();

        // Activity mappings
        CreateMap<CreateActivityDto, Activity>();

        CreateMap<Activity, ActivityDto>()
            .ForMember(dest => dest.DocumentCount, opt => opt.MapFrom(src => src.Documents.Count))
            .ForMember(dest => dest.SubmissionCount, opt => opt.MapFrom(src => src.Submissions.Count));

        CreateMap<UpdateActivityDto, Activity>();

        CreateMap<PatchActivityDto, Activity>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Activity, ActivityDto>()
            .IncludeAllDerived();

        CreateMap<Activity, ActivityDetailDto>();

        // submission mappings
        CreateMap<Submission, SubmissionDto>()
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.UserName))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Comments.Count))
            .ForMember(dest => dest.ActivityName, opt => opt.MapFrom(src => src.Activity.Name));

        CreateMap<Submission, SubmissionDetailDto>()
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.UserName))
            .ForMember(dest => dest.ActivityName, opt => opt.MapFrom(src => src.Activity.Name));

        CreateMap<CreateSubmissionDto, Submission>()
            .ForMember(dest => dest.StudentId, opt => opt.Ignore())
            .ForMember(dest => dest.SubmittedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsLate, opt => opt.Ignore());

        CreateMap<SubmissionComment, SubmissionCommentDto>()
            .ForMember(dest => dest.AuthorName,
                opt => opt.MapFrom(src => src.Author.UserName));

    }
}

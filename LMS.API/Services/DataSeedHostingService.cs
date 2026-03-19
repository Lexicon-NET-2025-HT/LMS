using Bogus;
using LMS.Infractructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace LMS.API.Services;

public class DataSeedHostingService : IHostedService
{
    private readonly IServiceProvider serviceProvider;
    private readonly IConfiguration configuration;
    private readonly ILogger<DataSeedHostingService> logger;
    private UserManager<ApplicationUser> userManager = null!;
    private RoleManager<IdentityRole> roleManager = null!;
    private ApplicationDbContext dbContext = null!;
    private const string TeacherRole = "Teacher";
    private const string StudentRole = "Student";
    private Random random = new Random();

    public DataSeedHostingService(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<DataSeedHostingService> logger)
    {
        this.serviceProvider = serviceProvider;
        this.configuration = configuration;
        this.logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        if (!env.IsDevelopment()) return;

        dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (await dbContext.Users.AnyAsync(cancellationToken)) return;

        userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        ArgumentNullException.ThrowIfNull(roleManager, nameof(roleManager));
        ArgumentNullException.ThrowIfNull(userManager, nameof(userManager));

        try
        {
            await AddRolesAsync([TeacherRole, StudentRole]);
            await AddDemoUsersAsync();
            await AddUsersAsync(20);

            var courses = FakeCourses(5);
            await AddCoursesToDb(courses);

            var modules = FakeModules(2, 7, courses);
            await AddModulesToDb(modules);

            logger.LogInformation("Seed complete");
        }
        catch (Exception ex)
        {
            logger.LogError($"Data seed fail with error: {ex.Message}");
            throw;
        }
    }

    private async Task AddRolesAsync(string[] rolenames)
    {
        foreach (string rolename in rolenames)
        {
            if (await roleManager.RoleExistsAsync(rolename)) continue;
            var role = new IdentityRole { Name = rolename };
            var res = await roleManager.CreateAsync(role);

            if (!res.Succeeded) throw new Exception(string.Join("\n", res.Errors));
        }
    }
    private async Task AddDemoUsersAsync()
    {
        var teacher = new ApplicationUser
        {
            UserName = "teacher@test.com",
            Email = "teacher@test.com"
        };

        var student = new ApplicationUser
        {
            UserName = "student@test.com",
            Email = "student@test.com"
        };

        await AddUserToDb([teacher, student]);

        var teacherRoleResult = await userManager.AddToRoleAsync(teacher, TeacherRole);
        if (!teacherRoleResult.Succeeded) throw new Exception(string.Join("\n", teacherRoleResult.Errors));

        var studentRoleResult = await userManager.AddToRoleAsync(student, StudentRole);
        if (!studentRoleResult.Succeeded) throw new Exception(string.Join("\n", studentRoleResult.Errors));
    }

    private async Task AddUsersAsync(int nrOfUsers)
    {
        var faker = new Faker<ApplicationUser>("sv").Rules((f, e) =>
        {
            e.Email = f.Person.Email;
            e.UserName = f.Person.Email;
        });

        await AddUserToDb(faker.Generate(nrOfUsers));
    }

    private async Task AddUserToDb(IEnumerable<ApplicationUser> users)
    {
        var passWord = configuration["password"];
        ArgumentNullException.ThrowIfNull(passWord, nameof(passWord));

        foreach (var user in users)
        {
            var result = await userManager.CreateAsync(user, passWord);
            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
        }
    }

    private IEnumerable<Course> FakeCourses(int nrOfCourses)
    {
        var subjects = new[]
            {
                "C#", ".NET", "Webbutveckling", "Databaser", "JavaScript",
                "React", "API-utveckling", "Molntjänster", "AI", "Testning"
            };

        var faker = new Faker<Course>("sv")
            .RuleFor(c => c.Name, f => $"{f.PickRandom(subjects)} {f.PickRandom(new[] { "Grund", "Fortsättning", "Avancerad" })}")
            .RuleFor(c => c.Description, f => f.Lorem.Paragraph(2))
            .RuleFor(c => c.StartDate, f => EnsureWeekday(f.Date.Soon(90)));

        return faker.Generate(nrOfCourses);
    }

    private async Task AddCoursesToDb(IEnumerable<Course> courses)
    {
        if (dbContext is null)
        {
            throw new InvalidOperationException("DbContext is not initialized");
        }
        foreach (var course in courses)
        {
            bool exists = await dbContext.Courses
                .AnyAsync(c => c.Name == course.Name);

            if (!exists)
            {
                await dbContext.Courses.AddAsync(course);
            }
        }

        await dbContext.SaveChangesAsync();
    }

    private IEnumerable<Domain.Models.Entities.Module> FakeModules(int nrOfModulesAtLEast, int nrOfModulesTops, IEnumerable<Course> courses)
    {
        var progression = new[]
            {
                "Introduktion",
                "Grundläggande koncept",
                "Fördjupning",
                "Praktisk tillämpning",
                "Avancerade tekniker",
                "Integration och helhet",
                "Projekt"
            };

        var modules = new List<Domain.Models.Entities.Module>();

        var textFaker = new Faker("sv");

        foreach (var course in courses)
        {
            int moduleCount = random.Next(nrOfModulesAtLEast, nrOfModulesTops);

            var moduleStart = course.StartDate;

            for (int i = 0; i < moduleCount; i++)
            {
                var moduleLength = random.Next(2, 21);
                var moduleEnd = EnsureWeekday(moduleStart.AddDays(moduleLength), 17);

                modules.Add(new Domain.Models.Entities.Module
                {
                    CourseId = course.Id,
                    Name = progression[i % 7],
                    Description = textFaker.Lorem.Paragraph(random.Next(1, 3)),
                    StartDate = moduleStart,
                    EndDate = moduleEnd
                });

                moduleStart = EnsureWeekday(moduleEnd.AddDays(1));
            }
        }

        return modules;
    }

    private async Task AddModulesToDb(IEnumerable<Module> modules)
    {
        if (dbContext is null)
        {
            throw new InvalidOperationException("DbContext is not initialized");
        }
        foreach (var module in modules)
        {
            bool exists = await dbContext.Modules
                .AnyAsync(c => c.Name == module.Name);

            if (!exists)
            {
                await dbContext.Modules.AddAsync(module);
            }
        }

        await dbContext.SaveChangesAsync();
    }

    private static DateTime EnsureWeekday(DateTime date, int atHour = 8)
    {
        var newDate = date.DayOfWeek switch
        {
            DayOfWeek.Saturday => date.AddDays(2),
            DayOfWeek.Sunday => date.AddDays(1),
            _ => date
        };
        return WithHour(atHour, newDate);
    }

    private static DateTime WithHour(int hour, DateTime date)
    {
        return new DateTime(
            date.Year,
            date.Month,
            date.Day,
            hour, // h
            0,  // m
            0   // s
        );
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

}

using LMS.Shared.DTOs.Activity;
using LMS.Shared.DTOs.ActivityType;
using LMS.Shared.DTOs.Course;
using LMS.Shared.DTOs.Module;
using Microsoft.AspNetCore.Components;

namespace LMS.Blazor.Client.Components.Activity;

public partial class ActivityGrid
{
    [Parameter] public int? ModuleId { get; set; }
    [Parameter] public bool ShowCreate { get; set; } = true;
    [Parameter] public bool ShowEdit { get; set; } = true;
    [Parameter] public EventCallback<ActivityDto> OnActivitySelected { get; set; }
    [Parameter] public int SelectedCourseId { get; set; }
    [Parameter] public int SelectedModuleId { get; set; }

    private List<CourseDto> activeCourses = [];
    private List<ModuleDto> availableModules = [];
    private List<ActivityDto> activities = [];
    private List<ActivityTypeDto> activityTypes = [];
    private bool isLoading;
    private string? loadError;

    private bool dialogOpen;
    private bool isEditing;
    private ActivityDto? editingActivity;
    private ActivityDialog.ActivityFormModel formModel = new();

    private bool activityTypeDialogOpen;
    private string newActivityTypeName = string.Empty;
    private string newActivityTypeDescription = string.Empty;
    private string? activityTypeError;
    private bool isCreatingActivityType;

    // Pagination
    private int currentPage = 1;
    private int pageSize = 9;
    private int totalCount = 0;
    private int totalPages = 0;
    private int _lastSelectedCourseId = -1;
    private int _lastSelectedModuleId = -1;

    protected override async Task OnParametersSetAsync()
    {
        if (_lastSelectedCourseId != SelectedCourseId ||
            _lastSelectedModuleId != SelectedModuleId)
        {
            _lastSelectedCourseId = SelectedCourseId;
            _lastSelectedModuleId = SelectedModuleId;
            currentPage = 1;
            await LoadAsync();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        isLoading = true;
        loadError = null;
        StateHasChanged();

        try
        {
            var typesResult = await ActivityTypeService.GetAllActivityTypesAsync();
            activityTypes = typesResult?.ToList() ?? [];

            var coursesResult = await CourseService.GetAllCoursesAsync(1, 200);
            activeCourses = coursesResult?.Items?
                // .Where(c => c.IsActive)
                .ToList() ?? [];

            var moduleFilter = SelectedModuleId > 0 ? SelectedModuleId : ModuleId;

            var result = await ActivityService.GetAllActivitiesAsync(
                page: currentPage,
                pageSize: pageSize,
                moduleId: moduleFilter);

            if (result is not null)
            {
                activities = result.Items ?? [];
                totalCount = result.TotalCount;
                totalPages = result.TotalPages;
            }
            else
            {
                activities = [];
                totalCount = 0;
                totalPages = 0;
            }
        }
        catch (Exception ex)
        {
            loadError = $"Failed to load activities. {ex.Message}";
        }
        finally
        {
            isLoading = false;
        }
    }

    private async Task LoadModulesForCourseAsync(int courseId)
    {
        availableModules = [];

        if (courseId <= 0)
            return;

        try
        {
            // Adjust this if your real service method is named differently
            var result = await ModuleService.GetAllModulesAsync(page: 1, pageSize: 500);

            availableModules = result?.Items?
                // .Where(m => m.CourseId == courseId && m.IsActive)
                .Where(m => m.CourseId == courseId)
                .ToList() ?? [];
        }
        catch (Exception ex)
        {
            loadError = $"Failed to load modules. {ex.Message}";
        }
    }

    private async Task HandleCourseChanged(int courseId)
    {
        formModel.CourseId = courseId;
        formModel.ModuleId = 0;

        await LoadModulesForCourseAsync(courseId);
        StateHasChanged();
    }

    private async Task GoToPageAsync(int page)
    {
        if (page < 1 || page > totalPages)
            return;

        currentPage = page;
        await LoadAsync();
    }

    private async Task OpenCreateDialog()
    {
        isEditing = false;
        editingActivity = null;
        activityTypeError = null;

        formModel = new()
        {
            CourseId = 0,
            ModuleId = 0,
            ActivityTypeId = activityTypes.FirstOrDefault()?.Id ?? 0
        };

        availableModules = [];
        dialogOpen = true;

        await Task.CompletedTask;
    }

    private async Task OpenEditDialog(ActivityDto activity)
    {
        isEditing = true;
        editingActivity = activity;
        activityTypeError = null;

        var courseId = 0;
        availableModules = [];

        if (activity.ModuleId > 0)
        {
            var modulesResult = await ModuleService.GetAllModulesAsync(page: 1, pageSize: 500);
            var module = modulesResult?.Items?.FirstOrDefault(m => m.Id == activity.ModuleId);

            if (module is not null)
            {
                courseId = module.CourseId;
                availableModules = modulesResult?.Items?
                    .Where(m => m.CourseId == courseId)
                    .ToList() ?? [];
            }
        }

        formModel = new ActivityDialog.ActivityFormModel
        {
            CourseId = courseId,
            ModuleId = activity.ModuleId,
            Name = activity.Name,
            Description = activity.Description,
            ActivityTypeId = activity.ActivityTypeId,
            StartTime = activity.StartTime,
            EndTime = activity.EndTime
        };

        dialogOpen = true;
    }

    private async Task HandleSave()
    {
        try
        {
            if (!isEditing)
            {
                if (formModel.ModuleId <= 0)
                {
                    loadError = "Please select a module before creating an activity.";
                    return;
                }

                var dto = new CreateActivityDto
                {
                    ModuleId = formModel.ModuleId,
                    Name = formModel.Name,
                    Description = formModel.Description,
                    ActivityTypeId = formModel.ActivityTypeId,
                    StartTime = formModel.StartTime ?? DateTime.Today.AddHours(9),
                    EndTime = formModel.EndTime
                };

                await ActivityService.CreateActivityAsync(dto);
            }
            else if (editingActivity is not null)
            {
                var dto = new UpdateActivityDto
                {
                    Name = formModel.Name,
                    Description = formModel.Description,
                    ActivityTypeId = formModel.ActivityTypeId,
                    StartTime = formModel.StartTime ?? DateTime.Today.AddHours(9),
                    EndTime = formModel.EndTime
                };

                await ActivityService.UpdateActivityAsync(editingActivity.Id, dto);
            }

            dialogOpen = false;
            await LoadAsync();
        }
        catch (Exception ex)
        {
            loadError = $"Failed to save. {ex.Message}";
        }
    }

    private async Task HandleDelete(int id)
    {
        await ActivityService.DeleteActivityAsync(id);

        if (activities.Count == 1 && currentPage > 1)
            currentPage--;

        await LoadAsync();
    }

    private void OpenActivityTypeDialog()
    {
        newActivityTypeName = string.Empty;
        newActivityTypeDescription = string.Empty;
        activityTypeError = null;

        dialogOpen = false;
        activityTypeDialogOpen = true;
    }

    private async Task HandleCreateActivityType()
    {
        activityTypeError = null;

        if (string.IsNullOrWhiteSpace(newActivityTypeName))
        {
            activityTypeError = "Please enter an activity type name.";
            return;
        }
        if (string.IsNullOrWhiteSpace(newActivityTypeDescription))
        {
            activityTypeError = "Please enter an activity type description.";
            return;
        }

        isCreatingActivityType = true;

        try
        {
            var created = await ActivityTypeService.CreateActivityTypeAsync(
                new CreateActivityTypeDto
                {
                    Name = newActivityTypeName.Trim(),
                    Description = newActivityTypeDescription
                });

            if (created is null)
            {
                activityTypeError = "Failed to create activity type.";
                return;
            }

            var typesResult = await ActivityTypeService.GetAllActivityTypesAsync();
            activityTypes = typesResult?.ToList() ?? [];

            formModel.ActivityTypeId = created.Id;

            activityTypeDialogOpen = false;
            newActivityTypeName = string.Empty;
            newActivityTypeDescription = string.Empty;
        }
        catch (Exception ex)
        {
            activityTypeError = $"Failed to create activity type. {ex.Message}";
        }
        finally
        {
            isCreatingActivityType = false;
        }
    }

    private async Task HandleDeleteActivityType(int activityTypeId)
    {
        try
        {
            await ActivityTypeService.DeleteActivityTypeAsync(activityTypeId);

            var typesResult = await ActivityTypeService.GetAllActivityTypesAsync();
            activityTypes = typesResult?.ToList() ?? [];
            formModel.ActivityTypeId = 0;
        }
        catch (Exception ex)
        {
            dialogOpen = true;
            activityTypeError = $"Cannot delete this activity type: {ex.Message}";
        }
    }
}
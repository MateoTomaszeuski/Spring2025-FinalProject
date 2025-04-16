using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Consilium.Shared.Models;
using Consilium.Shared.Services;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consilium.Shared.ViewModels;

public partial class AssignmentsViewModel : ObservableObject {

    [ObservableProperty]
    private ObservableCollection<Course> courses = new();

    [ObservableProperty]
    private ObservableCollection<Assignment> assignments = new();

    [ObservableProperty]
    private Course selectedCourse = new();

    [ObservableProperty]
    private bool showAssignmentForm;

    [ObservableProperty]
    private string newAssignmentTitle = String.Empty;

    [ObservableProperty]
    private string? newAssignmentDescription;

    [ObservableProperty]
    private DateTime newAssignmentDueDate;

    [ObservableProperty]
    private bool showCourseForm;

    [ObservableProperty]
    private string newCourseName = string.Empty;

    private readonly IAssignmentService service;

    public AssignmentsViewModel(IAssignmentService service) {
        this.service = service;
    }

    [RelayCommand]
    public async Task StartAssignment(Assignment a) {
        a.DateStarted = new DateTime();
        await service.UpdateAssignmentAsync(a);
    }

    [RelayCommand]
    private void ToggleAssignmentForm() {
        ShowAssignmentForm = !ShowAssignmentForm;
    }

    [RelayCommand]
    private void ToggleCourseForm() {
        ShowCourseForm = !ShowCourseForm;
    }

    [RelayCommand]
    private async Task AddCourse() {
        if (string.IsNullOrWhiteSpace(NewCourseName))
            return;

        var newCourse = new Course {
            CourseName = NewCourseName
        };

        await service.AddCourseAsync(newCourse);
        Courses.Add(newCourse);
        Courses = new(await service.GetAllCoursesAsync());
        ShowCourseForm = false;

        SelectedCourse = Courses.FirstOrDefault(c => c.CourseName == newCourse.CourseName)!;
        ResetCourseFormValues();
    }



    [RelayCommand]
    public async Task AddAssignment() {
        if (string.IsNullOrWhiteSpace(NewAssignmentTitle) || SelectedCourse is null)
            return;

        var newAssignment = new Assignment {
            Name = NewAssignmentTitle,
            Description = NewAssignmentDescription,
            CourseId = SelectedCourse.Id,
            DueDate = NewAssignmentDueDate
        };

        await service.AddAssignmentAsync(newAssignment);

        service.AllAssignments = new(await service.GetAllAssignmentsAsync());
        Assignments.Add(newAssignment);
        Assignments = new(FilterAssignmentsOnCourse(SelectedCourse));
        ResetAssignmentFormValues();
    }


    partial void OnSelectedCourseChanged(Course value) {
        if (value is not null && service.AllAssignments is not null && value.Id != -1) {
            var newAssignments = FilterAssignmentsOnCourse(value);
            Assignments = new ObservableCollection<Assignment>(newAssignments);
        } else {
            Assignments = new();
        }
    }

    private IEnumerable<Assignment> FilterAssignmentsOnCourse(Course course) {
        return service.AllAssignments.Where(a => a.CourseId == course.Id);
    }

    public async Task InitializeViewModelAsync() {
        Courses = new(await service.GetAllCoursesAsync());
        service.AllAssignments = new(await service.GetAllAssignmentsAsync());

        if (Courses.Count < 1) {
            Assignments = new();
        } else {
            Assignments = new(FilterAssignmentsOnCourse(Courses[0]));
        }
    }

    private void ResetAssignmentFormValues() {
        NewAssignmentTitle = string.Empty;
        NewAssignmentDescription = string.Empty;
        NewAssignmentDueDate = DateTime.Today;
        ShowAssignmentForm = false;
    }

    private void ResetCourseFormValues() {
        NewCourseName = string.Empty;
        ShowCourseForm = false;
    }


    // relay command methods:
    // start assignment
    // complete assignment
    // delete assignment
    // add assignment
    // add to to-do list


    // once a student has started working on an assignment, do we want to give them the option to "pause" it?
    // or is the button just there to mark the first time they started working on it?
}
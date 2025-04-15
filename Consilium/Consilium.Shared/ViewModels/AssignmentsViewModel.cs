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

    private readonly IAssignmentService service;

    public AssignmentsViewModel(IAssignmentService service) {
        this.service = service;
    }

    [RelayCommand]
    public async Task StartAssignment(Assignment a) {
        a.DateStarted = new DateTime();
        await service.UpdateAssignment(a);
    }

    partial void OnSelectedCourseChanged(Course value) {
        if (value is not null && service.AllAssignments is not null && value.CourseId != -1) {
            var newAssignments = FilterAssignmentsOnCourse(value);
            Assignments = new ObservableCollection<Assignment>(newAssignments);
        } else {
            Assignments = new();
        }
    }

    private IEnumerable<Assignment> FilterAssignmentsOnCourse(Course course) {
        return service.AllAssignments.Where(a => a.CourseId == course.CourseId);
    }

    public async Task InitializeViewModelAsync() {
        Courses = new(await service.GetAllCourses());
        service.AllAssignments = new(await service.GetAllAssignmentsAsync());
        Assignments = new(FilterAssignmentsOnCourse(Courses[0]));
    }


    // relay command methods:
    // start assignment
    // complete assignment
    // delete assignment
    // add assignment


    // view assignment details
    // add to To-Do list



    // once a student has started working on an assignment, do we want to give them the option to "pause" it?
    // or is the button just there to mark the first time they started working on it?

    // are we clearing assignments automatically when they are marked completed?
    // or are we going to let the user hide it from the list?
}
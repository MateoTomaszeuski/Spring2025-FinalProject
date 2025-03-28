using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Consilium.Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consilium.Shared.ViewModels;

public partial class AssignmentsViewModel : ObservableObject {

    [ObservableProperty]
    private ObservableCollection<Course> courses;

    [ObservableProperty]
    private ObservableCollection<Assignment> assignments;

    public AssignmentsViewModel() {

        Assignment a1 = new() { Title = "Assignment 1", Description = "Assignment 1 Description", Category = "Homework", IsCompleted = false, CourseId = 1 };
        Assignment a2 = new() { Title = "Assignment 2", Description = "Assignment 2 Description", Category = "Homework", IsCompleted = false, CourseId = 1 };
        Assignment a3 = new() { Title = "Assignment 3", Description = "Assignment 3 Description", Category = "Homework", IsCompleted = false, CourseId = 2 };

        Course c1 = new() { CourseName = "Math", CourseId = 1 };
        Course c2 = new() { CourseName = "English", CourseId = 2 };
        Courses = new() { c1, c2 };

        Assignments = new() { a1, a2, a3 };
    }

    [ObservableProperty]
    private int selectedCourseId;

    [RelayCommand]
    public void FilterOnCourseId() {
        var filteredAssignments = Assignments.Where(a => a.CourseId == 1);
        Assignments = new(filteredAssignments);
    }


}
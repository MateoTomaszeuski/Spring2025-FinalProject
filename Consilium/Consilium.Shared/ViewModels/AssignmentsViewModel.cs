using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Consilium.Shared.Models;
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
    private ObservableCollection<Course> courses;

    [ObservableProperty]
    private ObservableCollection<Assignment> assignments;

    [ObservableProperty]
    private Course selectedCourse;

    private List<Assignment> allAssignments; // could this get extracted to a service?

    partial void OnSelectedCourseChanged(Course value) {
        if (value is not null && allAssignments is not null && value.CourseId != -1) {
            var newAssignments = FilterAssignmentsOnCourse(value);
            Assignments = new ObservableCollection<Assignment>(newAssignments);
        } else {
            Assignments = new();
        }
    }

    private IEnumerable<Assignment> FilterAssignmentsOnCourse(Course course) {
        return allAssignments.Where(a => a.CourseId == course.CourseId);
    }

    public AssignmentsViewModel() {
        Courses = PopulateSampleCourses();
        SelectedCourse = Courses[0];
        allAssignments = PopulateSampleAssignments().ToList();
        Assignments = new ObservableCollection<Assignment>(allAssignments.Where(a => a.CourseId == Courses[0].CourseId));
    }

    private ObservableCollection<Course> PopulateSampleCourses() {
        return new ObservableCollection<Course> {
            new Course { CourseId = -1, CourseName = "Select a course" },
            new Course { CourseName = "Math", CourseId = 1 },
            new Course { CourseName = "History", CourseId = 2 },
            new Course { CourseName = "Science", CourseId = 3 }
        };
    }

    private ObservableCollection<Assignment> PopulateSampleAssignments() {
        var assignment1 = new Assignment {
            Name = "Math Homework",
            Description = "do math homework stuff",
            CourseId = 1,
            DueDate = new DateTime(2023, 11, 15),
            DateStarted = new DateTime(2023, 11, 10),
            DateCompleted = null
        };

        var assignment2 = new Assignment {
            Name = "Math Homework 2",
            Description = "do math homework stuff",
            CourseId = 1,
            DueDate = new DateTime(2023, 12, 1),
            DateStarted = new DateTime(2023, 11, 20),
            DateCompleted = null
        };

        var assignment3 = new Assignment {
            Name = "History Essay",
            Description = "write an essay",
            CourseId = 2,
            DueDate = new DateTime(2023, 11, 30),
            DateStarted = new DateTime(2023, 11, 15),
            DateCompleted = new DateTime(2023, 11, 25)
        };

        var assignment4 = new Assignment {
            Name = "Science Project",
            Description = "do sciency stuff",
            CourseId = 3,
            DueDate = new DateTime(2023, 11, 30),
            DateStarted = new DateTime(2023, 11, 15),
            DateCompleted = new DateTime(2023, 11, 25)
        };

        return new() { assignment1, assignment2, assignment3, assignment4 };
    }


    // relay command methods:
    // start assignment
    // complete assignment
    // delete assignment
    // add assignment
    // add to To-Do list
    // view assignment details



    // once a student has started working on an assignment, do we want to give them the option to "pause" it?
    // or is the button just there to mark the first time they started working on it?

    // are we clearing assignments automatically when they are marked completed?
    // or are we going to let the user hide it from the list?
}
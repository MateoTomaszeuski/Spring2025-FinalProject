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

    private List<Assignment> allAssignments; // this would go in the service

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
            Description = "",
            CourseId = 1,
            DueDate = new DateTime(2023, 12, 1),
            DateStarted = new DateTime(2023, 11, 20),
            DateCompleted = null
        };

        var assignment3 = new Assignment
        {
            Name = "History Essay",
            Description = """
            Four score and seven years ago our fathers brought forth on this continent a new nation, conceived in liberty, and dedicated to the proposition that all men are created equal. 
            “Now we are engaged in a great civil war, testing whether that nation, or any nation so conceived and so dedicated, can long endure. We are met on a great battlefield of that war. 
            We have come to dedicate a portion of that field as a final resting place for those who here gave their lives that that nation might live. It is altogether fitting and proper that we should do this. 
            “But in a larger sense we cannot dedicate, we cannot consecrate, we cannot hallow this ground. The brave men, living and dead, who struggled here have consecrated it, far above our poor power to add or detract. 
            The world will little note, nor long remember, what we say here, but it can never forget what they did here. It is for us the living, rather, to be dedicated here to the unfinished work which they who fought here have thus far so nobly advanced. 
            It is rather for us to be here dedicated to the great task remaining before us, that from these honored dead we take increased devotion to that cause for which they gave the last full measure of devotion, 
            that we here highly resolve that these dead shall not have died in vain, that this nation, under God, shall have a new birth of freedom, and that government of the people, by the people, for the people, shall not perish from the earth.
            """,
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


    // view assignment details
    // add to To-Do list



    // once a student has started working on an assignment, do we want to give them the option to "pause" it?
    // or is the button just there to mark the first time they started working on it?

    // are we clearing assignments automatically when they are marked completed?
    // or are we going to let the user hide it from the list?
}
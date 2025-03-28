using Consilium.Shared.Models;
using Consilium.Shared.Services;
using Consilium.Shared.ViewModels;
using NSubstitute;
using Shouldly;
using System.Threading.Tasks;
using TUnit.Assertions.Extensions;
namespace Consilium.Tests;


public class AssignmentsVmTest {
    private AssignmentsViewModel viewModel;

    public AssignmentsVmTest() {
        viewModel = new AssignmentsViewModel();
    }

    [Before(Test)]
    public void Setup() {
        viewModel = new AssignmentsViewModel();
    }

    [Test]
    public async Task CanCreateViewModel() {
        await Assert.That(viewModel).IsNotNull();
    }

    [Test]
    public async Task CanGetAssignment() {
        await Assert.That(viewModel.Assignments.Count).IsEqualTo(3);
    }


    [Test]
    public async Task CanFilterAssignmentsWithCourseId() {

        Assignment a1 = new() { Title = "Assignment 1", Description = "Assignment 1 Description", Category = "Homework", IsCompleted = false, CourseId = 1 };
        Assignment a2 = new() { Title = "Assignment 2", Description = "Assignment 2 Description", Category = "Homework", IsCompleted = false, CourseId = 1 };
        Assignment a3 = new() { Title = "Assignment 3", Description = "Assignment 3 Description", Category = "Homework", IsCompleted = false, CourseId = 2 };

        Course c1 = new() { CourseName = "Math", CourseId = 1 };
        Course c2 = new() { CourseName = "English", CourseId = 2 };


        viewModel.Assignments = new() { a1, a2, a3 };
        viewModel.SelectedCourseId = 1;
        viewModel.FilterOnCourseIdCommand.Execute(null);

        await Assert.That(viewModel.Assignments.Count).IsEqualTo(2);

    }
}
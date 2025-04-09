using Consilium.Shared.Models;
using Consilium.Shared.Services;
using Consilium.Shared.ViewModels;
using NSubstitute;
using Shouldly;
using System.Collections.ObjectModel;
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

    //[Test]
    //public async Task WhenCourseIsSelected_AssignmentsAreFiltered() {
    //    viewModel.AllAssignments = new List<Assignment> {
    //        new Assignment { CourseId = 1, Name = "Math Homework", Description = "do math homework stuff" },
    //        new Assignment { CourseId = 2, Name = "History Essay", Description = "write history essay" }
    //    };

    //    viewModel.SelectedCourse = new Course { CourseId = 1, CourseName = "Math" };
    //    await Assert.That(viewModel.Assignments.Count).IsEqualTo(1);
    //}        


    }
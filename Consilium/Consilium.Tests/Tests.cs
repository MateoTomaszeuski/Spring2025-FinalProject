using Consilium.Shared.Models;
using Consilium.Shared.Services;
using Consilium.Shared.ViewModels;
using NSubstitute;
using System.Threading.Tasks;
using TUnit.Assertions.Extensions;
namespace Consilium.Tests;


public class ToDoListVMTests {
    private readonly TodoListViewModel viewModel;

    public ToDoListVMTests() {
        viewModel = new TodoListViewModel(Substitute.For<IToDoService>());
    }

    [Test]
    public async Task CanCreateViewModel() {
        await Assert.That(viewModel).IsNotNull();
    }

    [Test]
    public async Task CanAddTodo() {
        viewModel.NewTodoTitle = "Test Todo";
        viewModel.AddTodoCommand.Execute(null);
        await Assert.That(viewModel.TodoItems.Count).IsEqualTo(1);
    }

    [Test]
    public async Task CanRemoveTodo() {
        viewModel.NewTodoTitle = "Test Todo";
        viewModel.AddTodoCommand.Execute(null);
        viewModel.RemoveTodoCommand.Execute(viewModel.TodoItems[0]);
        await Assert.That(viewModel.TodoItems.Count).IsEqualTo(0);
    }

    [Test]
    public async Task CantAddEmptyTodo() {
        viewModel.NewTodoTitle = "";
        viewModel.AddTodoCommand.Execute(null);
        await Assert.That(viewModel.TodoItems.Count).IsEqualTo(0);
    }

    [Test]

    public async Task CheckCorrectTodoItemIsAdded() {
        viewModel.NewTodoTitle = "Test Todo";
        viewModel.AddTodoCommand.Execute(null);
        await Assert.That(viewModel.TodoItems[0].Title).IsEqualTo("Test Todo");
    }

    [Test]

    public async Task CanAddMultipleItems() {
        viewModel.NewTodoTitle = "Test Todo";
        viewModel.AddTodoCommand.Execute(null);
        viewModel.NewTodoTitle = "Test Todo 2";
        viewModel.AddTodoCommand.Execute(null);
        await Assert.That(viewModel.TodoItems.Count).IsEqualTo(2);
    }

    [Test]

    public async Task GetTodoItemsList() {
        viewModel.NewTodoTitle = "Test Todo";
        viewModel.AddTodoCommand.Execute(null);
        viewModel.NewTodoTitle = "Test Todo 2";
        viewModel.AddTodoCommand.Execute(null);

        List<TodoItem> TodoItems = new List<TodoItem>(viewModel.TodoItems);
        await Assert.That(TodoItems[0]).IsEqualTo(
        new TodoItem() { Title = "Test Todo" }
        );

        await Assert.That(TodoItems[1]).IsEqualTo(
        new TodoItem() { Title = "Test Todo 2" }
        );

    }

    [Test]

    public async Task CanDeleteItem() {
        viewModel.NewTodoTitle = "Test Todo";
        viewModel.AddTodoCommand.Execute(null);
        viewModel.NewTodoTitle = "Test Todo 2";
        viewModel.AddTodoCommand.Execute(null);
        viewModel.RemoveTodoCommand.Execute(viewModel.TodoItems[0]);
        List<TodoItem> TodoItems = new List<TodoItem>(viewModel.TodoItems);
        await Assert.That(TodoItems.Count).IsEqualTo(1);
        await Assert.That(TodoItems[0]).IsEqualTo(
        new TodoItem() { Title = "Test Todo 2" }
        );
    }

    [Test]
    public async Task CanSetTodoCategory() {
        viewModel.NewTodoTitle = "Test Todo";
        viewModel.AddTodoCommand.Execute(null);
        viewModel.NewCategoryInput = "Test Category";
        viewModel.SetCategoryForTodoItemCommand.Execute(viewModel.TodoItems[0]);
        await Assert.That(viewModel.TodoItems[0].Category).IsEqualTo("Test Category");
    }

    [Test]
    public async Task CanSortTodoItemByCategory() {
        viewModel.NewTodoTitle = "Test Todo";
        viewModel.NewCategoryInput = "Test Category A";
        viewModel.AddTodoCommand.Execute(null);
        viewModel.SetCategoryForTodoItemCommand.Execute(viewModel.TodoItems[0]);
        viewModel.NewTodoTitle = "Test Todo 2";
        viewModel.NewCategoryInput = "Test Category B";
        viewModel.AddTodoCommand.Execute(null);
        viewModel.SetCategoryForTodoItemCommand.Execute(viewModel.TodoItems[1]);
        viewModel.SortByCategoryCommand.Execute(null);
        await Assert.That(viewModel.TodoItems[0].Category).IsEqualTo("Test Category A");
        await Assert.That(viewModel.TodoItems[1].Category).IsEqualTo("Test Category B");
    }

    [Test]
    public async Task CanSortTodoItemsByCategory() {
        viewModel.NewTodoTitle = "Task 1";
        viewModel.NewCategoryInput = "Work";
        viewModel.AddTodoCommand.Execute(null);
        viewModel.SetCategoryForTodoItemCommand.Execute(viewModel.TodoItems[0]);
        viewModel.NewTodoTitle = "Task 2";
        viewModel.NewCategoryInput = "Personal";
        viewModel.AddTodoCommand.Execute(null);
        viewModel.SetCategoryForTodoItemCommand.Execute(viewModel.TodoItems[1]);
        viewModel.NewTodoTitle = "Task 3";
        viewModel.NewCategoryInput = "School";
        viewModel.AddTodoCommand.Execute(null);
        viewModel.SetCategoryForTodoItemCommand.Execute(viewModel.TodoItems[2]);
        viewModel.SortByCategoryCommand.Execute(null);
        await Assert.That(viewModel.TodoItems[0].Category).IsEqualTo("Personal");
        await Assert.That(viewModel.TodoItems[1].Category).IsEqualTo("School");
        await Assert.That(viewModel.TodoItems[2].Category).IsEqualTo("Work");
    }

    [Test]
    public async Task CanSortTodoItemsByIsComplete() {
        viewModel.NewTodoTitle = "Task 1";
        viewModel.AddTodoCommand.Execute(null);
        viewModel.NewTodoTitle = "Task 2";
        viewModel.AddTodoCommand.Execute(null);
        viewModel.NewTodoTitle = "Task 3";
        viewModel.AddTodoCommand.Execute(null);
        viewModel.TodoItems[0].IsCompleted = true;
        viewModel.TodoItems[1].IsCompleted = false;
        viewModel.TodoItems[2].IsCompleted = true;
        viewModel.SortByCompletionCommand.Execute(null);
        await Assert.That(viewModel.TodoItems[0].IsCompleted).IsTrue();
        await Assert.That(viewModel.TodoItems[1].IsCompleted).IsTrue();
        await Assert.That(viewModel.TodoItems[2].IsCompleted).IsFalse();
    }
    [Test]
    public async Task CanCreateSubTask() {
        viewModel.NewTodoTitle = "Task 1";
        viewModel.AddTodoCommand.Execute(null);
        var parentTask = viewModel.TodoItems.FirstOrDefault();
        await Assert.That(parentTask).IsNotNull();
        viewModel.NewTodoTitle = "Sub Task 1";
        viewModel.AddSubTaskCommand.Execute(parentTask);
        await Assert.That(parentTask!.SubTasks.Count).IsEqualTo(1);
        await Assert.That(parentTask.SubTasks[0].Title).IsEqualTo("Sub Task 1");
    }

    //[Test]
    //public async Task InitSetsIsLoading() {
    //    await Task.Delay(1);
    //}

}
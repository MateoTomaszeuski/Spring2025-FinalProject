using Consilium.Maui.ViewModels;
using System.Threading.Tasks;
using TUnit.Assertions.Extensions;

namespace Consilium.Tests;


public class Tests {
    [Test]
    public void Basic() {
        Console.WriteLine("This is a basic test");
    }

    // These are for the ToDoListViewModel

    [Test]
    public async Task CanCreateViewModel() {
        TodoListViewModel viewModel = new TodoListViewModel();
        await Assert.That(viewModel).IsNotNull();
    }

    [Test]
    public async Task CanAddTodo() {
        TodoListViewModel viewModel = new TodoListViewModel();
        viewModel.NewTodoTitle = "Test Todo";
        viewModel.AddTodoCommand.Execute(null);
        await Assert.That(viewModel.TodoItems.Count).IsEqualTo(1);
    }

    [Test]
    public async Task CanRemoveTodo() {
        TodoListViewModel viewModel = new TodoListViewModel();
        viewModel.NewTodoTitle = "Test Todo";
        viewModel.AddTodoCommand.Execute(null);
        viewModel.RemoveTodoCommand.Execute(viewModel.TodoItems[0]);
        await Assert.That(viewModel.TodoItems.Count).IsEqualTo(0);
    }

    [Test]
    public async Task CantAddEmptyTodo() {
        TodoListViewModel viewModel = new TodoListViewModel();
        viewModel.NewTodoTitle = "";
        viewModel.AddTodoCommand.Execute(null);
        await Assert.That(viewModel.TodoItems.Count).IsEqualTo(0);
    }

    [Test]

    public async Task CheckCorrectTodoItemIsAdded() {
        TodoListViewModel viewModel = new TodoListViewModel();
        viewModel.NewTodoTitle = "Test Todo";
        viewModel.AddTodoCommand.Execute(null);
        await Assert.That(viewModel.TodoItems[0].Title).IsEqualTo("Test Todo");
    }

    [Test]

    public async Task CanAddMultipleItems() {
        TodoListViewModel viewModel = new TodoListViewModel();
        viewModel.NewTodoTitle = "Test Todo";
        viewModel.AddTodoCommand.Execute(null);
        viewModel.NewTodoTitle = "Test Todo 2";
        viewModel.AddTodoCommand.Execute(null);
        await Assert.That(viewModel.TodoItems.Count).IsEqualTo(2);
    }

    [Test]

    public async Task GetTodoItemsList() {
        TodoListViewModel viewModel = new TodoListViewModel();
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

}
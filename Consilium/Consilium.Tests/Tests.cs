using Consilium.Maui.ViewModels;
using System.Threading.Tasks;

namespace Consilium.Tests;


public class Tests
{
    [Test]
    public void Basic() {
        Console.WriteLine("This is a basic test");
    }

    // These are for the ToDoListViewModel

    [Test]
    public async Task CanCreateViewModel()
    {
        TodoListViewModel viewModel = new TodoListViewModel();
        await Assert.That(viewModel).IsNotNull();
    }

    [Test]
    public async Task CanAddTodo()
    {
        TodoListViewModel viewModel = new TodoListViewModel();
        viewModel.NewTodoTitle = "Test Todo";
        viewModel.AddTodoItem();
        await Assert.That(viewModel.TodoItems.Count).IsEqualTo(1);
    }


}
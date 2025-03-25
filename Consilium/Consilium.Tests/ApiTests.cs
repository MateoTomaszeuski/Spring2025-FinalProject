using Consilium.API.InMemoryServices;
using Consilium.Shared.ViewModels;

namespace Consilium.Tests;

public class ApiTests {
    private ToDoServiceIM service = new();
    [Before(Test)]
    public void Setup() {
        service = new ToDoServiceIM();
    }

    [Test]
    public async Task InMemServiceCanBeBuilt() {
        service.AddTask(new TodoItem());
        await Assert.That(service.TaskCount).IsEqualTo(0);
    }
}
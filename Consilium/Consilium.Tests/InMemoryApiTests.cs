using Consilium.API.DBServices;
using Consilium.API.InMemoryServices;
using Consilium.Shared.Models;

namespace Consilium.Tests;

public class InMemoryApiTests {
    private DBServiceIM service = new();
    [Before(Test)]
    public void Setup() {
        service = new DBServiceIM();
    }

    [Test]
    public void InMemServiceCanBeBuilt() {
    }

    [Test]
    public async Task InMemServiceCanStoreOneItem() {
        service.AddToDo(new TodoItem(), "cody");
        await Assert.That(service.ToDoCount("cody")).IsEqualTo(1);
    }

    [Test]
    public async Task InMemServiceCanStoreTwoItems() {
        service.AddToDo(new TodoItem() { Title = "Lorem" }, "cody");
        service.AddToDo(new TodoItem() { Title = "Lorem2" }, "cody");

        await Assert.That(service.ToDoCount("cody")).IsEqualTo(2);

        List<TodoItem> items = service.GetToDos("cody").ToList();
        await Assert.That(items[0].Title).IsEqualTo("Lorem");
        await Assert.That(items[1].Title).IsEqualTo("Lorem2");
    }

    [Test]
    public async Task InMemServiceCanStoreTwoPeople() {
        service.AddToDo(new TodoItem() { Title = "LoremC" }, "cody");
        service.AddToDo(new TodoItem() { Title = "LoremA" }, "audrey");

        await Assert.That(service.ToDoCount("cody")).IsEqualTo(1);

        List<TodoItem> Citems = service.GetToDos("cody").ToList();
        List<TodoItem> Aitems = service.GetToDos("audrey").ToList();
        await Assert.That(Citems[0].Title).IsEqualTo("LoremC");
        await Assert.That(Aitems[0].Title).IsEqualTo("LoremA");
    }

    [Test]
    public async Task InMemServiceCanUpdateItem() {
        service.AddToDo(new TodoItem() { Title = "Lorem" }, "cody");
        service.UpdateToDo(0, new TodoItem() { Title = "Cody" }, "cody");

        List<TodoItem> items = service.GetToDos("cody").ToList();
        await Assert.That(items[0].Title).IsEqualTo("Cody");
    }

    [Test]
    public async Task InMemServiceCanRemoveItem() {
        service.AddToDo(new TodoItem() { Title = "Lorem" }, "cody");
        service.RemoveToDo(0, "cody");

        List<TodoItem> items = service.GetToDos("cody").ToList();
        await Assert.That(items.Count).IsEqualTo(0);
    }
}
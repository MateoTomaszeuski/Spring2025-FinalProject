using Consilium.Shared.Models;
using Consilium.Shared.ViewModels;
using Dapper;
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace Consilium.API.DBServices;

public class DBService(IDbConnection conn) : IDBService {
    #region ToDos
    public void AddToDo(TodoItem Todo, string email) {
        throw new NotImplementedException();
    }

    public IEnumerable<TodoList> GetToDoLists(string email) {
        string sql = $"SELECT id, listname FROM todolist tl INNER JOIN \"HowlDev.User\" h on (tl.account_email = h.email) WHERE h.email = @email";
        var todolists = conn.Query<TodoList>(sql, new { email }).ToList();
        foreach (TodoList list in todolists) {
            List<TodoItem> items = [.. GetTodoItems(list.Id)];
            list.TodoItems = items;
        }
        return todolists;
    }

    public TodoList GetTodoList(int tableid) {
        string sql = $"SELECT id, listname FROM todolist WHERE id = @tableid";
        var todolist = conn.Query<TodoList>(sql, new { tableid }).FirstOrDefault();

        sql = $"SELECT id, categoryId, parentId, assignmentId,toDoName FROM todoitem WHERE toDoListId = @tableid";
        var todoitems = conn.Query<TodoItem>(sql, new { tableid });

        if (todolist is null)
            throw new SqlNullValueException();
        todolist.TodoItems = new(todoitems);
        return todolist;
    }

    public void RemoveToDo(TodoItem item, string email) {
        if (ValidateControlOverItem(item.Id, email)) {
            string removeItem = """"
                delete from todoitem t where t.id = @id;
                """";
            conn.Execute(removeItem, new { id = item.Id });
        }
        throw new Exception("You don't have control over this item.");
    }

    public int ToDoCount(string email) {
        throw new NotImplementedException();
    }

    public void UpdateToDo(TodoItem Todo, string email) {
        if (ValidateControlOverItem(Todo.Id, email)) {
            string updateItem = """"
                update todoitem t set completionDate = @time where id = @id
                """";
            DateTime now = DateTime.Now;
            conn.Execute(updateItem, new { time = now, id = Todo.Id });
        }
        throw new Exception("You don't have control over this item.");
    }

    private bool ValidateControlOverItem(int itemId, string email) {
        string getCountOfItems = """"
            SELECT count(*) FROM todolist tl 
            INNER JOIN todoitem ti on (ti.todolistid = tl.id)
            WHERE tl.account_email = @email
            	AND ti.id = @itemId
            """";
        int count = conn.QuerySingle(getCountOfItems, new { itemId, email });
        return count == 1;
    }

    private IEnumerable<TodoItem> GetTodoItems(int id) {
        string items = """"
            SELECT id, @id as todolistid, categoryId as category, parentId, assignmentId, todoName as title, completionDate
            FROM todoitem t WHERE t.todolistid = @id
            """";
        return conn.Query<TodoItem>(items, new { id });
    }
    #endregion 
}
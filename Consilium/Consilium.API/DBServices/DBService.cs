using Consilium.Shared.Models;
using Dapper;
using System.Data;

namespace Consilium.API.DBServices;

public class DBService(IDbConnection conn) : IDBService {
    public void AddToDo(TodoItem Todo, string username) {
        throw new NotImplementedException();
    }

    public IEnumerable<User> GetAllUsers() {
        string sql = "select id, email, displayname from account";
        return conn.Query<User>(sql);
    }

    public List<TodoItem> GetToDos(string username) {
        throw new NotImplementedException();
    }

    public void RemoveToDo(int index, string username) {
        throw new NotImplementedException();
    }

    public int ToDoCount(string username) {
        throw new NotImplementedException();
    }

    public void UpdateToDo(int index, TodoItem Todo, string username) {
        throw new NotImplementedException();
    }
}
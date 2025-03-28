using Consilium.Shared.Models;
using Dapper;
using System.Data;

namespace Consilium.API.DBServices;

public class DBService(IDbConnection conn) {
    public IEnumerable<User> GetAllUsers() {
        string sql = "select id, email, displayname from account";
        return conn.Query<User>(sql);
    }
}
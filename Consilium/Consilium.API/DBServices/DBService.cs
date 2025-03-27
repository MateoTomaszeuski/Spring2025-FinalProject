using Consilium.Shared.Models;
using Dapper;
using System.Data;

namespace Consilium.API.DBServices;

public class DBService(IDbConnection conn) {
    public IEnumerable<string> GetAllUsers() {
        string sql = "select * from account";
        return conn.Query<string>(sql);
    }
}
namespace Consilium.API;

public interface IAuthService {
    public bool AddUser(string s);
    public bool IsValidUser(string s);

}
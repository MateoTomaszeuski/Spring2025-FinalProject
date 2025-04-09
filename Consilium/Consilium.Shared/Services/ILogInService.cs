namespace Consilium.Shared.Services;
public interface ILogInService {
    Task<string> LogIn(string email);
}
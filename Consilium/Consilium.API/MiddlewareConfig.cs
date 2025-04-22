using EmailAuthenticator;

public class MiddlewareConfig : IIDMiddlewareConfig {
    public List<string> Paths => new List<string>() { "/", "/account", "/health", "/account/validate", "/timecheck" };

    public TimeSpan? ExpirationDate => new TimeSpan(90, 0, 0, 0);

    public TimeSpan? ReValidationDate => new TimeSpan(10, 0, 0, 0);
}
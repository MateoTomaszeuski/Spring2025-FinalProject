using EmailAuthenticator;

public class MiddlewareConfig : IIDMiddlewareConfig {
    public List<string> Paths => new List<string>() { "", "/health" };

    public TimeSpan ExpirationDate => new TimeSpan(90, 0, 0, 0);
}
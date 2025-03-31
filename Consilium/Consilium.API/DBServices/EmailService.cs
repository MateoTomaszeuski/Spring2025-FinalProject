using EmailAuthenticator;

namespace Consilium.API.DBServices;

public class EmailService : IEmailService {
    public Task SendValidationEmail(string email, string validationToken) {
        throw new NotImplementedException();
    }
}
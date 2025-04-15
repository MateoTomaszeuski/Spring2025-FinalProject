using Consilium.Shared.Models;
using System.Collections.ObjectModel;

namespace Consilium.Shared.Services;
public interface IMessageService {
    Task<IEnumerable<string>> GetConversations();
    Task<IEnumerable<Message>> InitializeMessagesAsync(string otherUser);
    Task<bool> SendMessageAsync(Message message);
}
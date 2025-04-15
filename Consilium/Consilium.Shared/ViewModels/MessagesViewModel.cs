using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Consilium.Shared.Models;
using System.Collections.ObjectModel;

namespace Consilium.Shared.ViewModels;

public partial class MessagesViewModel : ObservableObject {
    [ObservableProperty]
    private ObservableCollection<Message> allMessges = new();

    [ObservableProperty]
    private string conversationWith = "string.Empty";
    [ObservableProperty]
    private string messageContent = string.Empty;

    [RelayCommand]
    public void SendMessage() {
        var message = new Message {
            Sender = "Me",
            Receiver = ConversationWith,
            Content = MessageContent,
            TimeSent = DateTime.Now
        };
        MessageContent = string.Empty;
        AllMessges.Add(message);
    }
}
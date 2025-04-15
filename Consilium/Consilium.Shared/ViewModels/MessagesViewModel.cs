using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Consilium.Shared.Models;
using Consilium.Shared.Services;
using System.Collections.ObjectModel;

namespace Consilium.Shared.ViewModels;

public partial class MessagesViewModel(IMessageService messageService) : ObservableObject {
    [ObservableProperty]
    private ObservableCollection<Message> allMessages = new();

    [ObservableProperty]
    private string conversationWith = "string.Empty";
    [ObservableProperty]
    private string messageContent = string.Empty;

    [RelayCommand]
    public async Task SendMessage() {
        var message = new Message {
            Sender = "Me",
            Receiver = ConversationWith,
            Content = MessageContent,
            TimeSent = DateTime.Now
        };
        var sent = await messageService.SendMessageAsync(message);
        if (sent) {
            AllMessages.Add(message);
            MessageContent = string.Empty;
        }
    }

    [RelayCommand]
    public async Task InitializeMessagesAsync() {
        var messages = await messageService.InitializeMessagesAsync(ConversationWith);
        AllMessages.Clear();
        foreach (var message in messages) {
            AllMessages.Add(message);
        }
    }
}
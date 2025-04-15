using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Consilium.Shared.Models;
using Consilium.Shared.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Consilium.Shared.ViewModels;

public partial class MessagesViewModel : ObservableObject {
    public MessagesViewModel(IMessageService messageService, IPersistenceService persistenceService) {
        this.messageService = messageService;
        messageService.PropertyChanged += MessageService_PropertyChanged;
        MyUserName = persistenceService.GetUserName();
    }

    private async void MessageService_PropertyChanged(object? sender, PropertyChangedEventArgs e) {
        if (e.PropertyName == nameof(messageService.CurrentChat)) {
            if (messageService.CurrentChat != null) {
                ConversationWith = messageService.CurrentChat;
                await InitializeMessagesAsync();
            }
        }
    }
    [ObservableProperty]
    private string myUserName = string.Empty;

    [ObservableProperty]
    private ObservableCollection<Message> allMessages = new();

    [ObservableProperty]
    private string conversationWith = string.Empty;
    [ObservableProperty]
    private string messageContent = string.Empty;
    private readonly IMessageService messageService;

    [RelayCommand]
    public async Task SendMessage() {
        var message = new Message {
            Sender = MyUserName,
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
        if (!string.IsNullOrEmpty(ConversationWith)) {
            var messages = await messageService.InitializeMessagesAsync(ConversationWith);
            AllMessages.Clear();
            foreach (var message in messages) {
                message.IsMyMessage = message.Sender == MyUserName;
                AllMessages.Add(message);
            }
        }
    }
}
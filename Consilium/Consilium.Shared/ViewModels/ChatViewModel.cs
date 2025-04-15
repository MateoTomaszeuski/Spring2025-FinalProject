using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Consilium.Shared.Services;
using System.Collections.ObjectModel;

namespace Consilium.Shared.ViewModels;
public partial class ChatViewModel(IMessageService messageService) : ObservableObject {

    [ObservableProperty]
    private ObservableCollection<string> conversations = new();
    [ObservableProperty]
    private bool showConversations = true;
    [ObservableProperty]
    private string selectedConversation = string.Empty;
    [ObservableProperty]
    private bool showChat = false;

    public async Task InitConversations() {
        Conversations = new(await messageService.GetConversations());
    }
    [RelayCommand]
    private void SelectConversation(string conversation) {
        if (Conversations.Contains(conversation)) {
            SelectedConversation = conversation;
            ShowConversations = false;
            ShowChat = true;
        }
    }
}
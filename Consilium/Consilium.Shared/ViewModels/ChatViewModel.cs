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
    [ObservableProperty]
    private bool isNotCreatingNewConversation = true;

    [ObservableProperty]
    private bool isCreatingNewConversation = false;
    [ObservableProperty]
    private string newConversationName = string.Empty;
    public async Task InitConversations() {
        Conversations = new(await messageService.GetConversations());
    }
    [RelayCommand]
    private void SelectConversation(string conversation) {
        if (Conversations.Contains(conversation)) {
            messageService.CurrentChat = conversation;
            ShowConversations = false;
            ShowChat = true;
        }
    }
    [RelayCommand]
    private void Back() {
        ShowConversations = true;
        ShowChat = false;
        messageService.CurrentChat = string.Empty;
    }
    [RelayCommand]
    private void ActivateNewConversation() {
        IsCreatingNewConversation = true;
        IsNotCreatingNewConversation = false;
    }
    [RelayCommand]
    private void CreateNewConversation() {
        if (string.IsNullOrWhiteSpace(NewConversationName)) {
            return;
        }
        if (Conversations.Contains(NewConversationName)) {
            return;
        }
        Conversations.Add(NewConversationName);
        SelectConversation(NewConversationName);

        IsCreatingNewConversation = false;
        IsNotCreatingNewConversation = true;
    }
}
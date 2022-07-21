using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using FirebaseTest.Authentication;
using UnityEngine;

namespace FirebaseTest.Chats {
    public class ChatManager : MonoBehaviour {
        [SerializeField] 
        private Chat chatPrefab;
        [SerializeField]
        private RectTransform spawnOrigin;

        private List<Chat> _activeChats;
        private IAddChatInput _addChatInput;
        private IAuthManager _authManager;
        private string userId;

        public void Initialize(IAuthManager authManager, IAddChatInput addChatInput) {
            _addChatInput = addChatInput;
            _authManager = authManager;
            
            _authManager.onUserLogIn += OnLogIn;
            _authManager.onUserSignOut += OnLogOut;
            _addChatInput.onAddChatInput += OnAddChatInput;
            _activeChats = new List<Chat>();
        }

        private void OnAddChatInput() {
            CreateNewChat();
        }

        private void OnLogOut() {
            _addChatInput.OnUserLogOut();
        }

        private async void OnLogIn() {
            userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
            _addChatInput.OnUserLogIn();
            ClearChats();
            await RestoreChats();
        }

        private async UniTask CreateNewChat() {
            var newChatID = await DatabaseUtils.PushAndSetValue(Paths.ChatsPath, true);
            var chatMemberPath = Paths.ChatMemberPath(newChatID, userId);
            var addChatMember = DatabaseUtils.AddOrOverrideValue(chatMemberPath, true);
            var userChatPath = Paths.UserChatPath(userId, newChatID);
            var addUserChat = DatabaseUtils.AddOrOverrideValue(userChatPath, true);
            await UniTask.WhenAll(addChatMember, addUserChat);
            await CreateChatWindow(newChatID);
        }

        private async UniTask RestoreChats() {
            var userChats = await DatabaseUtils.GetValue(Paths.UserChatsPath(userId));
            if (!userChats.Exists) {
                Debug.LogWarning("There are no chats fo this user");
                return;
            }
            foreach (var userChat in userChats.Children) {
                await CreateChatWindow(userChat.Key);
            }
        }

        private async UniTask CreateChatWindow(string chatID) {
            var newChatWindow = Instantiate(chatPrefab, spawnOrigin);
            await newChatWindow.Initialize(chatID);
            newChatWindow.onLeaveChat += OnLeaveChat;
            _activeChats.Add(newChatWindow);
        }

        private async void OnLeaveChat(Chat chat) {
            chat.onLeaveChat -= OnLeaveChat;
            var chatMembers = await DatabaseUtils.GetValue(Paths.ChatMembersPath(chat.ChatId));
            if (!chatMembers.Exists || chatMembers.ChildrenCount == 0) {
                var removeChatMembers = DatabaseUtils.RemoveKey(Paths.ChatMembersPath(chat.ChatId));
                var removeChat =  DatabaseUtils.RemoveKey(Paths.ChatPath(chat.ChatId));
                var removeMessages = DatabaseUtils.RemoveKey(Paths.ChatMessagesPath(chat.ChatId));
                await UniTask.WhenAll(removeChatMembers, removeChat, removeMessages);
            }
            Destroy(chat.gameObject);
            _activeChats.Remove(chat);
        }

        private void ClearChats() {
            if (_activeChats == null) return;
            foreach (var chat in _activeChats) {
                Destroy(chat.gameObject);
            }
            _activeChats.Clear();
        }

        private void OnDestroy() {
            _authManager.onUserLogIn -= OnLogIn;
            _authManager.onUserSignOut -= OnLogOut;
            _addChatInput.onAddChatInput -= OnAddChatInput;
        }
    }
}


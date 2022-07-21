using System;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

namespace FirebaseTest.Chats {
    [RequireComponent(typeof(IChatInput), typeof(IChatView))]
    public class Chat : MonoBehaviour {
        public event Action<Chat> onLeaveChat;
        public string ChatId { get; private set; }

        private IChatInput _chatInput;
        private IChatView _chatView;

        public void Initialize(string chatID) {
            _chatInput = GetComponent<IChatInput>();
            _chatView = GetComponent<IChatView>();
            ChatId = chatID;

            _chatInput.onMessageSendInput += OnMessagePostInput;
            _chatInput.onInviteInput += OnAddUserInput;
            _chatInput.onLeaveChatInput += OnLeaveChatInput;
            AddListenerToNewMessages();
            AddListenerToNewMemberAdded();
        }

        #region Members

        private async UniTask AddMember(string userEmail) {
            var userId = await DatabaseUtils.GetIDByEmail(userEmail);
            var userRegistered = await DatabaseUtils.IsUserRegistered(userId);
            if (!userRegistered) {
                Debug.LogWarning("User is not registered");
                return;
            }
            var chatMemberPath = Paths.ChatMemberPath(ChatId, userId);
            var member= await DatabaseUtils.GetValue(chatMemberPath);
            if (member.Exists) {
                Debug.LogWarning("Member already joined chat");
            } else {
                var addChatMember = DatabaseUtils.AddKey(chatMemberPath);
                var addUserChat = DatabaseUtils.AddKey(Paths.UserChatPath(userId, ChatId));
                await UniTask.WhenAll(addChatMember, addUserChat);
                Debug.Log($"New member {userId} successfully added to chat {ChatId}");
            }
        }

        private void OnLeaveChatInput() {
            LeaveChat();
        }
        
        private void OnAddUserInput(string userEmail) {
            AddMember(userEmail);
        }
        
        private async UniTask LeaveChat() {
            var userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
            await RemoveMember(userId);
            onLeaveChat?.Invoke(this);
        }

        private async UniTask RemoveMember(string userId) {
            var removeChatMember = DatabaseUtils.RemoveKey(Paths.ChatMemberPath(ChatId, userId));
            var removeUserChat =  DatabaseUtils.RemoveKey(Paths.UserChatPath(userId, ChatId));
            await UniTask.WhenAll(removeChatMember, removeUserChat);
        }
        
        private void AddListenerToNewMemberAdded() {
            var membersRef = DatabaseUtils.GetReference(Paths.ChatMembersPath(ChatId));
            membersRef.ChildAdded += OnMemberListChanged;
        }
        
        private void RemoveListenerToNewMemberAdded() {
            var membersRef = DatabaseUtils.GetReference(Paths.ChatMembersPath(ChatId));
            membersRef.ChildAdded -= OnMemberListChanged;
        }

        private async void OnMemberListChanged(object sender, ChildChangedEventArgs args) {
            var addedMember = args.Snapshot;
            var addedMemberId = addedMember.Key;
            string email = await DatabaseUtils.GetEmailByID(addedMemberId);
            _chatView.AddMember(email);
        }

        /*private async UniTask RestoreMembers(string chatID) {
            var members = await DatabaseUtils.GetValue(Paths.ChatMembersPath(chatID));
            if (!members.Exists || members.ChildrenCount == 0) {
                Debug.LogWarning($"No members for chat {chatID}");
                return;
            }
            
            _chatView.ClearMembers();
            foreach (var child in members.Children) {
                string email = await DatabaseUtils.GetEmailByID(child.Key);
                _chatView.AddMember(email);
            }
        }*/

        #endregion

        #region Messages
        
        [Serializable]
        private struct MessageData {
            public string sender;
            public string dateTime;
            public string message;
        }

        private async UniTask PostMessage(string message) {
            var user = FirebaseAuth.DefaultInstance.CurrentUser;
            if (user == null) {
                Debug.LogWarning("Log in first");
                return;
            }
            if (message == string.Empty) {
                return;
            }
            var messageData = new MessageData() {
                message = message,
                sender = user.Email,
                dateTime = DateTime.Now.ToString("g")
            };
            var value = JsonUtility.ToJson(messageData);
            var path = Paths.ChatMessagesPath(ChatId);
            await DatabaseUtils.PushAndSetValue(path, value);
        }

        /*private async UniTask RestoreMessages() {
            var messages = await DatabaseUtils.GetValue(Paths.ChatMessagesPath(ChatId));
            if (!messages.Exists || messages.ChildrenCount == 0) {
                Debug.LogWarning($"No messages for chat {ChatId}");
                return;
            }

            _chatView.ClearMessages();
            foreach (var child in messages.Children) {
                var data = (MessageData) JsonUtility.FromJson(child.Value.ToString(), typeof(MessageData));
                _chatView.AddMessage(data.message, data.sender, data.dateTime);
                print("Message added: " + data.message);
            }
        }*/

        private void OnMessagePostInput(string input) {
            PostMessage(input);
        }

        private void AddListenerToNewMessages() {
            var messagesRef = DatabaseUtils.GetReference(Paths.ChatMessagesPath(ChatId));
            messagesRef.ChildAdded += OnNewMessagePosted;
        }

        private void RemoveListenerToNewMessages() {
            var messagesRef = DatabaseUtils.GetReference(Paths.ChatMessagesPath(ChatId));
            messagesRef.ChildAdded -= OnNewMessagePosted;
        }

        private void OnNewMessagePosted(object sender, ChildChangedEventArgs args) {
            var postedMessage = args.Snapshot;
            var data = (MessageData) JsonUtility.FromJson(postedMessage.Value.ToString(), typeof(MessageData));
            _chatView.AddMessage(data.message, data.sender, data.dateTime);
            print("New Message added: " + data.message);
        }

        #endregion
        
        private void OnDestroy() {
            _chatView.ClearMessages();
            _chatView.ClearMembers();
            _chatInput.onMessageSendInput -= OnMessagePostInput;
            _chatInput.onInviteInput -= OnAddUserInput;
            _chatInput.onLeaveChatInput -= OnLeaveChatInput;
            RemoveListenerToNewMessages();
            RemoveListenerToNewMemberAdded();
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FirebaseTest.Chats {
    public class ChatView : MonoBehaviour, IChatView {
        [SerializeField]
        private RectTransform messagesSpawnOrigin;
        [SerializeField]
        private RectTransform membersSpawnOrigin;
        [SerializeField]
        private ScrollRect scrollView;
        [SerializeField]
        private Message messagePrefab;
        [SerializeField]
        private ChatMember chatMemberPrefab;

        private List<Message> _messages;
        private List<ChatMember> _members;
        
        public void AddMessage(string text, string sender, string dateTime) {
            var newMessage = Instantiate(messagePrefab, messagesSpawnOrigin);
            newMessage.SetText(text);
            newMessage.SetSender(sender);
            newMessage.SetDateTime(dateTime);
            
            _messages ??= new List<Message>();
            _messages.Add(newMessage);
            ScrollToBottom();
        }

        public void AddMember(string memberName) {
            var newMember = Instantiate(chatMemberPrefab, membersSpawnOrigin);
            newMember.SetMemberName(memberName);

            _members ??= new List<ChatMember>();
            _members.Add(newMember);
        }

        public void ClearMessages() {
            if (_messages == null) return;
            foreach (var message in _messages) {
                Destroy(message.gameObject);
            }
            _messages.Clear();
        }

        public void ClearMembers() {
            if (_members == null) return;
            foreach (var member in _members) {
                Destroy(member.gameObject);
            }
            _members.Clear();
        }

        private void ScrollToBottom() {
            Canvas.ForceUpdateCanvases();
            scrollView.verticalNormalizedPosition = 0f;
        }
    }
}


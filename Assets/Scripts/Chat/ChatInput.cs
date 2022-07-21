using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FirebaseTest.Chats {
    public class ChatInput : MonoBehaviour, IChatInput {
        [SerializeField]
        private Button sendButton;
        [SerializeField]
        private Button inviteButton;
        [SerializeField]
        private Button leaveButton;
        [SerializeField]
        private TMP_InputField inputField;

        private void OnEnable() {
            sendButton.onClick.AddListener(OnSendInput);
            inviteButton.onClick.AddListener(OnInviteInput);
            leaveButton.onClick.AddListener(OnLeaveInput);
        }

        private void OnSendInput() {
            onMessageSendInput?.Invoke(inputField.text);
            ClearInputField();
        }

        private void OnInviteInput() {
            onInviteInput?.Invoke(inputField.text);
            ClearInputField();
        }

        private void OnLeaveInput() {
            onLeaveChatInput?.Invoke();
        }

        private void ClearInputField() {
            inputField.text = string.Empty;
        }

        #region IChatInput
        public event Action<string> onMessageSendInput;
        public event Action<string> onInviteInput;
        public event Action onLeaveChatInput;
        #endregion
    }
}


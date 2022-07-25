using System;
using FirebaseTest.Chats;
using UnityEngine;
using UnityEngine.UI;

namespace FirebaseTest.Chats {
    public class AddChatInput : MonoBehaviour, IAddChatInput {
        public event Action onAddChatInput;

        [SerializeField]
        private Button button;

        public void OnUserLogIn() {
            button.interactable = true;
            print("Log in");
        }

        public void OnUserLogOut() {
            button.interactable = false;
            print("Log out");
        }

        private void OnEnable() {
            button.onClick.AddListener(() => onAddChatInput?.Invoke());
        }
    }
}


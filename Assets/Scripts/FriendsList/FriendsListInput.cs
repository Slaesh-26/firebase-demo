using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FirebaseTest.Friends {
    public class FriendsListInput : MonoBehaviour, IFriendsListInput {
        public event Action<string> onAddFriendInput;
        public event Action<string> onRemoveFriendInput;

        [SerializeField]
        private TMP_InputField inputField;
        [SerializeField]
        private Button addFriend;
        [SerializeField]
        private Button removeFriend;

        private void Start() {
            addFriend.onClick.AddListener(() => {
                onAddFriendInput?.Invoke(inputField.text);
            });
            removeFriend.onClick.AddListener(() => {
                onRemoveFriendInput?.Invoke(inputField.text);
            });
        }
    }
}


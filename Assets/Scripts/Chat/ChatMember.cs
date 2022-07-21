using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FirebaseTest.Chats {
    public class ChatMember : MonoBehaviour {
        [SerializeField]
        private TextMeshProUGUI tmPro;

        public void SetMemberName(string memberName) {
            tmPro.text = memberName;
        }
    }
}


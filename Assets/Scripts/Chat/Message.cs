using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FirebaseTest.Chats {
    public class Message : MonoBehaviour {
        [SerializeField]
        private TextMeshProUGUI messageTmPro;
        [SerializeField]
        private TextMeshProUGUI senderTmPro;
        [SerializeField]
        private TextMeshProUGUI dateTimeTmPro;
        
        public void SetText(string text) {
            messageTmPro.text = text;
        }

        public void SetSender(string sender) {
            senderTmPro.text = sender;
        }

        public void SetDateTime(string dateTime) {
            dateTimeTmPro.text = dateTime;
        }
    }
}


using Firebase.Auth;
using TMPro;
using UnityEngine;

namespace FirebaseTest.Authentication {
    public class CurrentUserPanel : MonoBehaviour, ICurrentUserView {
        [SerializeField]
        private TextMeshProUGUI currentUserEmail;

        public void SetUser(string userName) {
            currentUserEmail.text = userName;
        }

        public void ResetUser() {
            currentUserEmail.text = "Sign in or register";
        }
    }
}


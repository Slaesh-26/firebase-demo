using System;
using UnityEngine;
using UnityEngine.UI;
using FirebaseTest.Authentication.Input;

namespace FirebaseTest.Authentication.UI {
    internal class EmailAuthenticationInputPanel : MonoBehaviour, IEmailAuthenticationInput {
        public event Action<string, string> onRegister;
        public event Action<string, string> onLogIn;
    
        [SerializeField]
        private InputFieldValidated emailInputField;
        [SerializeField]
        private InputFieldValidated passwordInputField;
        [SerializeField]
        private Button registerButton;
        [SerializeField]
        private Button logInButton;
    
        private void Start() {
            registerButton.onClick.AddListener(OnRegisterPressed);
            logInButton.onClick.AddListener(OnLogInPressed);
        }
    
        private void OnRegisterPressed() {
            var email = emailInputField.GetText();
            var password = passwordInputField.GetText();
            onRegister?.Invoke(email, password);
        }
    
        private void OnLogInPressed() {
            var email = emailInputField.GetText();
            var password = passwordInputField.GetText();
            onLogIn?.Invoke(email, password);
        }
    }
}
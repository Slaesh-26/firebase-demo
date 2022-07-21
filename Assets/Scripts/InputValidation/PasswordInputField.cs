using UnityEngine;

namespace FirebaseTest.Authentication.Input {
    public class PasswordInputField : InputFieldValidated {
        [SerializeField]
        private int passwordLength = 6;
        
        protected override bool IsInputValid() {
            return InputField.text.Length >= passwordLength;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirebaseTest.Authentication.Input {
    public class EmailInputField : InputFieldValidated {
        protected override bool IsInputValid() {
            var email = InputField.text;
            var trimmedEmail = email.Trim();
    
            if (trimmedEmail.EndsWith(".")) {
                return false;
            }
            try {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch {
                return false;
            }
        }
    }
}


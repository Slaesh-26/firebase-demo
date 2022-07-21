using System.Collections.Generic;
using FirebaseTest;
using FirebaseTest.Authentication.Input;
using UnityEngine;
using UnityEngine.UI;

namespace FirebaseTest {
    [RequireComponent(typeof(Button))]
    public class ButtonValidated : ValidatedBehaviour {
        [SerializeField] 
        private List<InputFieldValidated> inputFieldsValidated;

        private Button _button;
    
        private void OnEnable() {
            _button = GetComponent<Button>();
            foreach (var inputField in inputFieldsValidated) {
                AddValidatable(inputField);
            }
        }
    
        protected override void OnAllValid() {
            _button.interactable = true;
        }
    
        protected override void OnAnyInvalid() {
            _button.interactable = false;
        }
    }
}


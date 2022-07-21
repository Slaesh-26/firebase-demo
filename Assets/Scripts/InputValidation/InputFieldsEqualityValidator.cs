using System;
using System.Collections;
using System.Collections.Generic;
using FirebaseTest.Authentication.Input;
using UnityEngine;

namespace FirebaseTest {
    [Obsolete]
    public class InputFieldsEqualityValidator : MonoBehaviour, IValidatable {
        [SerializeField]
        private List<InputFieldValidated> inputFields;

        private void OnEnable() {
            foreach (var inputField in inputFields) {
                inputField.onStateChanged += OnStateChanged;
            }
        }

        private void OnDisable() {
            foreach (var inputField in inputFields) {
                inputField.onStateChanged -= OnStateChanged;
            }
        }

        private bool IsAllInputSame() {
            if (inputFields.Count == 0) {
                Debug.LogError("No input fields to check");
                return false;
            }
            
            var firstInput = inputFields[0].GetText();
            for (int i = 1; i < inputFields.Count; i++) {
                var input = inputFields[i].GetText();
                if (input != firstInput) {
                    return false;
                }
            }
            return true;
        }

        private void OnStateChanged() {
            onStateChanged?.Invoke();
        }

        #region IValidatable
        public event Action onStateChanged;
        public bool IsValid() {
            return IsAllInputSame();
        }
        #endregion
    }
}


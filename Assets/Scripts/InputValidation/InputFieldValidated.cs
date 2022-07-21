using System;
using TMPro;
using UnityEngine;

namespace FirebaseTest.Authentication.Input {
    [RequireComponent(typeof(TMP_InputField))]
    public abstract class InputFieldValidated : MonoBehaviour, IValidatable {
        protected TMP_InputField InputField {
            get {
                if (_inputField == null) _inputField = GetComponent<TMP_InputField>();
                return _inputField;
            }
        }

        private TMP_InputField _inputField;

        public string GetText() {
            if (!IsInputValid()) {
                Debug.LogError($"Invalid input:\"{InputField.text}\"");
            }
            return InputField.text;
        }

        protected abstract bool IsInputValid();

        private void OnEnable() {
            InputField.onValueChanged.AddListener(OnInputValueChanged);
        }

        private void OnDisable() {
            InputField.onValueChanged.RemoveListener(OnInputValueChanged);
        }

        private void OnInputValueChanged(string value) {
            onStateChanged?.Invoke();
        }

        #region IValidatable
        public event Action onStateChanged;
        public bool IsValid() {
            return IsInputValid();
        }
        #endregion
    }
}


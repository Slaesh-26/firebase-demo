using System;
using System.Collections.Generic;
using UnityEngine;

namespace FirebaseTest {
    public abstract class ValidatedBehaviour : MonoBehaviour {
        private List<IValidatable> _validatables;
        
        public void AddValidatable(IValidatable validatable) {
            _validatables ??= new List<IValidatable>();
            if (_validatables.Contains(validatable)) return;
            _validatables.Add(validatable);
            validatable.onStateChanged += OnValidatableStateChanged;
            
            OnValidatableStateChanged();
        }
    
        /// <summary>
        /// Вызывается, когда все елементы прошли проверку
        /// </summary>
        protected abstract void OnAllValid();
        
        /// <summary>
        /// Вызывается при изменении состояния одного из элементов, при этом среди
        /// всех элементов есть хотя бы один не прошедший проверку
        /// </summary>
        protected abstract void OnAnyInvalid();
    
        private void OnValidatableStateChanged() {
            foreach (var validatable in _validatables) {
                if (!validatable.IsValid()) {
                    OnAnyInvalid();
                    return;
                }
            }
            
            OnAllValid();
        }

        private void OnDestroy() {
            foreach (var validatable in _validatables) {
                validatable.onStateChanged -= OnValidatableStateChanged;
            }
        }
    }
}


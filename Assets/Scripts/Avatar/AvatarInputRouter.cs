using System;
using UnityEngine;
using UnityEngine.UI;

namespace FirebaseTest.Avatar.UI {
    public class AvatarInputRouter : MonoBehaviour, IAvatarInput {
        public event Action onAvatarSaveInput;
        public event Action onAvatarLoadInput;
        public event Action onAvatarGenerateInput;
        
        [SerializeField]
        private Button save;
        [SerializeField]
        private Button load;
        [SerializeField]
        private Button generate;
        [SerializeField]
        private RawImage avatarImage;

        public void SetAvatar(Texture2D avatarTex) {
            avatarImage.texture = avatarTex;
        }
        
        private void Start() {
            save.onClick.AddListener(OnSavePressed);
            load.onClick.AddListener(OnLoadPressed);
            generate.onClick.AddListener(OnGeneratePressed);
        }

        private void OnSavePressed() {
            onAvatarSaveInput?.Invoke();
        }
        
        private void OnLoadPressed() {
            onAvatarLoadInput?.Invoke();
        }
        
        private void OnGeneratePressed() {
            onAvatarGenerateInput?.Invoke();
        }
    }
}


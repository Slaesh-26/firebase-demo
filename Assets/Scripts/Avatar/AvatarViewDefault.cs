using UnityEngine;
using UnityEngine.UI;

namespace FirebaseTest.Avatar.UI {
    public class AvatarViewDefault : MonoBehaviour, IAvatarView {
        [SerializeField]
        private RawImage rawImage;
        
        public void SetAvatar(Texture2D texture) {
            rawImage.texture = texture;
        }
    }
}


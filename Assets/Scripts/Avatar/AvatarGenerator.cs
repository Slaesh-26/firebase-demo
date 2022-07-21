using UnityEngine;

namespace FirebaseTest.Avatar {
    public class AvatarGenerator {
        private Vector2Int _resolution;
        
        public AvatarGenerator(Vector2Int resolution) {
            _resolution = resolution;
        }
        
        public Texture2D GetAvatar() {
            var newTex = new Texture2D(_resolution.x, _resolution.y);
            var color1 = GetRandomColor();
            var color2 = GetRandomColor();
            for (int i = 0; i < _resolution.x; i++) {
                for (int j = 0; j < _resolution.y; j++) {
                    var color = i > j ? color1 : color2;
                    newTex.SetPixel(i, j, color);
                }
            }
            newTex.Apply();
            return newTex;
        }

        private Color GetRandomColor() {
            var r = Random.Range(0f, 1f);
            var g = Random.Range(0f, 1f);
            var b = Random.Range(0f, 1f);
            return new Color(r, g, b, 1f);
        }
    }
}


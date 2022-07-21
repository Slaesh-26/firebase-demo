using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.Networking;

namespace FirebaseTest.Avatar {
    internal class AvatarDownloaderDefault : MonoBehaviour, IAvatarDownloader {
        [SerializeField]
        private Texture2D avatarNotFoundTex;

        /// <summary>
        /// Загружает текстуру по userID текущего пользователя.
        /// Если текстура не существует или при загрузке
        /// возникла ошибка - возвращает дефолтную.
        /// </summary>
        public async UniTask<Texture2D> LoadAvatarAsync(string userID) {
            var storage = FirebaseStorage.DefaultInstance;
            var reference = storage.GetReferenceFromUrl(GetAvatarPath(userID));
            var getURLTask = reference.GetDownloadUrlAsync();
            await getURLTask;

            if (getURLTask.IsCanceled) {
                Debug.LogError("GetDownloadUrlAsync was canceled.");
                return avatarNotFoundTex;
            }
            if (getURLTask.IsFaulted) {
                Debug.LogError("GetDownloadUrlAsync encountered an error: " + getURLTask.Exception);
                return avatarNotFoundTex;
            }

            var downloadURL = getURLTask.Result;
            var webRequest = UnityWebRequest.Get(downloadURL);
            await webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success) {
                var rawData = webRequest.downloadHandler.data;
                // Texture2D.LoadImage автоматически заменит размер текстуры
                var texture = new Texture2D(16, 16);
                texture.LoadImage(rawData);
                texture.Apply();
                Debug.Log($"User {userID} avatar loaded");
                return texture;
            }
            
            Debug.LogError($"WebRequest failed: {webRequest.error}");
            return avatarNotFoundTex;
        }

        private string GetAvatarPath(string userID) {
            return $"{Paths.AvatarsPath}{userID}.png";
        }
    }
}


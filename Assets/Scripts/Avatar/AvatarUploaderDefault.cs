using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Storage;
using FirebaseTest.Avatar;
using UnityEngine;

namespace FirebaseTest.Avatar {
    public class AvatarUploaderDefault : MonoBehaviour, IAvatarUploader {
        public async UniTaskVoid Save(Texture2D texture, string userID) {
            var storage = FirebaseStorage.DefaultInstance;
            var reference = storage.GetReferenceFromUrl(GetAvatarPath(userID));
            var rawData = texture.EncodeToPNG();
            await reference.PutBytesAsync(rawData);
            Debug.Log($"User {userID} avatar saved");
        }
        
        private string GetAvatarPath(string userID) {
            return $"{Paths.AvatarsPath}{userID}.png";
        }
    }
}


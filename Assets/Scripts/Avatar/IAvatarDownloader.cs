using Cysharp.Threading.Tasks;
using UnityEngine;

namespace FirebaseTest.Avatar {
    public interface IAvatarDownloader {
        UniTask<Texture2D> LoadAvatarAsync(string userID);
    }
}


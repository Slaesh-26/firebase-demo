using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace FirebaseTest.Avatar {
    public interface IAvatarUploader {
        UniTaskVoid Save(Texture2D texture, string userID);
    }
}


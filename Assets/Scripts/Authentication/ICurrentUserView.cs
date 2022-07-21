using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;

namespace FirebaseTest.Authentication {
    public interface ICurrentUserView {
        void SetUser(string userName);
        void ResetUser();
    }
}


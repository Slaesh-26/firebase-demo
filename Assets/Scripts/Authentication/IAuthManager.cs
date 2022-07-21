using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirebaseTest.Authentication {
    public interface IAuthManager {
        event Action onUserLogIn;
        event Action onUserRegister;
        event Action onUserSignOut;
    }
}


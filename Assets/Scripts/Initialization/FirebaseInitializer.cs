using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Storage;
using UnityEngine;

namespace FirebaseTest.AppInitialization {
    internal class FirebaseInitializer : MonoBehaviour {
        public async UniTask<bool> Initialize() {
            var status = await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();
            if (status == DependencyStatus.Available) {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                return true;
            }
            FirebaseDatabase.DefaultInstance.GoOnline();
            return false;
        }
    }
}


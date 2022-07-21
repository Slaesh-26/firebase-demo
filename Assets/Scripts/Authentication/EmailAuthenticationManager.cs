using System;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using FirebaseTest.Authentication.Input;

namespace FirebaseTest.Authentication {
    public class EmailAuthenticationManager : MonoBehaviour, IAuthManager {
        [SerializeField]
        private bool signOutOnAppQuit = true;
        [SerializeField]
        private bool signOutOnAppPause = false;
        
        private IEmailAuthenticationInput _inputPanel;
        private ICurrentUserView _currentUserView;
    
        public void Initialize(IEmailAuthenticationInput inputPanel, ICurrentUserView currentUserView) {
            _currentUserView = currentUserView;
            _inputPanel = inputPanel;
            _inputPanel.onLogIn += OnLogInInput;
            _inputPanel.onRegister += OnResisterInput;

            var user = FirebaseAuth.DefaultInstance.CurrentUser;
            if (user != null) {
                _currentUserView.SetUser(user.Email);
                onUserLogIn?.Invoke();
            }
        }
    
        private async void OnLogInInput(string email, string password) {
            await LogInAsync(email, password);
            _currentUserView.SetUser(email);
            onUserLogIn?.Invoke();
        }

        private async void OnResisterInput(string email, string password) {
            await RegisterAsync(email, password);
            _currentUserView.SetUser(email);
            onUserRegister?.Invoke();
        }

        private async UniTask LogInAsync(string email, string password) {
            var authentication = FirebaseAuth.DefaultInstance;
            var logInTask = authentication.SignInWithEmailAndPasswordAsync(email, password);
            await logInTask.AsUniTask();
            
            if (logInTask.IsCanceled) {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (logInTask.IsFaulted) {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + logInTask.Exception);
                return;
            }
            
            var newUser = logInTask.Result;
            Debug.LogFormat($"User signed in successfully: {newUser.DisplayName} ({newUser.UserId})");
            
            await SaveUserDataToRealtimeDB(newUser.UserId, email);
        }

        private async UniTask RegisterAsync(string email, string password) {
            var authentication = FirebaseAuth.DefaultInstance;
            var registerTask = authentication.CreateUserWithEmailAndPasswordAsync(email, password);
            await registerTask.AsUniTask();
            
            if (registerTask.IsCanceled) {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (registerTask.IsFaulted) {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + registerTask.Exception);
                return;
            }
            
            var newUser = registerTask.Result;
            Debug.LogFormat($"Firebase user created successfully: {newUser.DisplayName} ({newUser.UserId})");

            await SaveUserDataToRealtimeDB(newUser.UserId, email);
            await LogInAsync(email, password);
        }

        private async UniTask SaveUserDataToRealtimeDB(string userID, string email) {
            var database = FirebaseDatabase.DefaultInstance;
            var reference = database.GetReference($"users/{userID}");
            await reference.SetValueAsync(email);
        }
    
        private void OnDestroy() {
            _inputPanel.onLogIn -= OnLogInInput;
            _inputPanel.onRegister -= OnResisterInput;
        }

        private void OnApplicationQuit() {
            if (signOutOnAppQuit) {
                FirebaseAuth.DefaultInstance.SignOut();
                onUserSignOut?.Invoke();
            }
        }

        private void OnApplicationPause(bool pauseStatus) {
            if (pauseStatus && signOutOnAppPause) {
                FirebaseAuth.DefaultInstance.SignOut();
                onUserSignOut?.Invoke();
            }
        }

        #region IAuthManager
        public event Action onUserLogIn;
        public event Action onUserRegister;
        public event Action onUserSignOut;
        #endregion
    }
}
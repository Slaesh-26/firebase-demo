using System;
using Firebase.Auth;
using FirebaseTest.Avatar.UI;
using UnityEngine;

namespace FirebaseTest.Avatar {
    public class AvatarController : MonoBehaviour {
        private IAvatarInput _input;
        private IAvatarDownloader _avatarDownloader;
        private IAvatarUploader _avatarUploader;
        private IAvatarView _view;
        private AvatarGenerator _avatarGenerator;
        private Texture2D _currentAvatar;
        
        public void Initialize(IAvatarInput input, IAvatarView view, IAvatarDownloader downloader, IAvatarUploader uploader) {
            _input = input;
            _view = view;
            _avatarDownloader = downloader;
            _avatarUploader = uploader;
            _avatarGenerator = new AvatarGenerator(new Vector2Int(256, 256));

            _input.onAvatarLoadInput += OnLoadInput;
            _input.onAvatarSaveInput += OnSaveInput;
            _input.onAvatarGenerateInput += OnGenerateInput;
        }

        private void OnSaveInput() {
            if (_currentAvatar == null) {
                return;
            }
            var user = FirebaseAuth.DefaultInstance.CurrentUser;
            if (user == null) {
                return;
            }
            _avatarUploader.Save(_currentAvatar, user.UserId);
        }

        private async void OnLoadInput() {
            var user = FirebaseAuth.DefaultInstance.CurrentUser;
            if (user == null) {
                return;
            }
            var remoteTex = await _avatarDownloader.LoadAvatarAsync(user.UserId);
            SetCurrentAvatar(remoteTex);
        }

        private void OnGenerateInput() {
            var generated = _avatarGenerator.GetAvatar();
            SetCurrentAvatar(generated);
        }

        private void SetCurrentAvatar(Texture2D newAvatar) {
            _currentAvatar = newAvatar;
            _view.SetAvatar(_currentAvatar);
        }

        private void OnDestroy() {
            _input.onAvatarLoadInput -= OnLoadInput;
            _input.onAvatarSaveInput -= OnSaveInput;
            _input.onAvatarGenerateInput -= OnGenerateInput;
        }
    }
}


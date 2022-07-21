using FirebaseTest.Authentication;
using FirebaseTest.Authentication.UI;
using FirebaseTest.Avatar;
using FirebaseTest.Avatar.UI;
using FirebaseTest.Chats;
using FirebaseTest.Friends;
using UnityEngine;

namespace FirebaseTest.AppInitialization {
    internal class AppEntryPoint : MonoBehaviour {
        [SerializeField] 
        private FirebaseInitializer firebaseInitializer;
        [SerializeField]
        private EmailAuthenticationManager emailAuthenticationManager;
        [SerializeField]
        private AvatarController avatarController;
        [SerializeField]
        private AvatarDownloaderDefault avatarDownloaderDefault;
        [SerializeField]
        private AvatarUploaderDefault avatarUploaderDefault;
        [SerializeField]
        private AvatarViewDefault avatarViewDefault;
        [SerializeField]
        private FriendsManager friendsManager;
        [SerializeField]
        private ChatManager chatManager;
        
        [Header("Input and View panels")]
        [SerializeField]
        private EmailAuthenticationInputPanel emailAuthenticationInput;
        [SerializeField]
        private AvatarInputRouter avatarInputRouter;
        [SerializeField]
        private CurrentUserPanel currentUserPanel;
        [SerializeField]
        private FriendsListPanel friendsListPanel;
        [SerializeField]
        private FriendsListInput friendsListInput;
        [SerializeField]
        private AddChatInput addChatInput;

        private async void Awake() {
            var success = await firebaseInitializer.Initialize();
            if (success) {
                Debug.Log("Firebase Initialized");
            } else {
                Debug.LogError("Firebase initialization error");
                return;
            }
            
            emailAuthenticationManager.Initialize(
                emailAuthenticationInput, 
                currentUserPanel
            );
            avatarController.Initialize(
                avatarInputRouter, 
                avatarViewDefault, 
                avatarDownloaderDefault, 
                avatarUploaderDefault
            );
            friendsManager.Initialize(
                friendsListPanel, 
                friendsListInput, 
                emailAuthenticationManager
            );
            chatManager.Initialize(
                emailAuthenticationManager, 
                addChatInput
            );
            
            print(await DatabaseUtils.GetIDByEmail("sss@ddd.com"));
        }
    }
}


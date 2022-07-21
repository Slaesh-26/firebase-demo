using Cysharp.Threading.Tasks;
using Firebase.Auth;
using FirebaseTest.Authentication;
using UnityEngine;

namespace FirebaseTest.Friends {
    public class FriendsManager : MonoBehaviour {
        private IFriendsListView _friendsListView;
        private IFriendsListInput _friendsListInput;
        private IAuthManager _authManager;
        private FirebaseUser _user;

        public void Initialize(IFriendsListView friendsListView, IFriendsListInput friendsListInput, IAuthManager authManager) {
            _friendsListView = friendsListView;
            _friendsListInput = friendsListInput;
            _authManager = authManager;
            _friendsListInput.onAddFriendInput += OnAddFriendInput;
            _friendsListInput.onRemoveFriendInput += OnRemoveFriendInput;
            _authManager.onUserRegister += OnAuthStateChanged;
            _authManager.onUserLogIn += OnAuthStateChanged;
            _authManager.onUserSignOut += OnAuthStateChanged;
            _user = FirebaseAuth.DefaultInstance.CurrentUser;
        }
        
        private async void OnAuthStateChanged() {
            _user = FirebaseAuth.DefaultInstance.CurrentUser;
            if (_user == null) return;
            var data = await DatabaseUtils.GetValue(Paths.UserFriends(_user.UserId));

            _friendsListView.RemoveAllFriends();

            if (!data.HasChildren) {
                print("No friends for this user");
                return;
            }
            
            foreach (var friend in data.Children) {
                print(friend.Value);
                _friendsListView.AddFriend(new FriendData {
                    name = friend.Key
                });
            }
        }

        private async void OnAddFriendInput(string friendID) {
            var success = await AddFriend(friendID);
            if (success) {
                OnAuthStateChanged();
            }
        }

        private async void OnRemoveFriendInput(string friendID) {
            bool success = await RemoveFriend(friendID);
            if (success) {
                OnAuthStateChanged();
            }
        }

        private async UniTask<bool> AddFriend(string friendEmail) {
            string friendID = await DatabaseUtils.GetIDByEmail(friendEmail);
            if (_user.UserId == friendID) {
                Debug.LogWarning("Trying to add self as friend");
                return false;
            }
            var friendData = await DatabaseUtils.GetValue(Paths.UserPath(friendID));
            if (!friendData.Exists) {
                Debug.Log($"user {friendID} is not registered in DB");
                return false;
            }
            var friendInListRef = DatabaseUtils.GetReference(Paths.UserFriend(_user.UserId, friendID));
            var addFriendTask = friendInListRef.SetValueAsync("friend");
            await addFriendTask;

            if (addFriendTask.IsCanceled || addFriendTask.IsFaulted) {
                Debug.LogError("Error while adding friend");
                return false;
            }

            return true;
        }

        private async UniTask<bool> RemoveFriend(string friendID) {
            var path = Paths.UserFriend(_user.UserId, friendID);
            var friendData = await DatabaseUtils.GetValue(path);
            if (friendData.Exists) {
                await DatabaseUtils.RemoveKey(path);
                return true;
            } 
            return false;
        }

        private void OnDestroy() {
            _friendsListInput.onAddFriendInput -= OnAddFriendInput;
            _friendsListInput.onRemoveFriendInput -= OnRemoveFriendInput;
            _authManager.onUserRegister -= OnAuthStateChanged;
            _authManager.onUserLogIn -= OnAuthStateChanged;
            _authManager.onUserSignOut -= OnAuthStateChanged;
        }
    }
}
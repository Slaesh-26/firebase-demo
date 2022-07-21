using System.Collections;
using System.Collections.Generic;
using FirebaseTest.Friends;
using TMPro;
using UnityEngine;

namespace FirebaseTest.Friends {
    public class FriendsListPanel : MonoBehaviour, IFriendsListView {
        [SerializeField]
        private TextMeshProUGUI text;
        
        public void AddFriend(FriendData friend) {
            text.text += $"{friend.name}\n";
        }
    
        public void RemoveFriend(FriendData friend) {
            
        }

        public void RemoveAllFriends() {
            text.text = string.Empty;
        }
    }
}


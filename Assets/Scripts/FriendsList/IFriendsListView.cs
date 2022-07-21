using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirebaseTest.Friends {
    public interface IFriendsListView {
        void AddFriend(FriendData friend);
        void RemoveFriend(FriendData friend);
        void RemoveAllFriends();
    }
}


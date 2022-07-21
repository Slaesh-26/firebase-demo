using System;

namespace FirebaseTest.Friends {
    public interface IFriendsListInput {
        /// <summary>
        /// string - user ID
        /// </summary>
        event Action<string> onAddFriendInput;
        
        /// <summary>
        /// string - user ID
        /// </summary>
        event Action<string> onRemoveFriendInput;
    }
}


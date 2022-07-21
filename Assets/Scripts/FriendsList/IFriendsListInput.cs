using System;

namespace FirebaseTest.Friends {
    public interface IFriendsListInput {
        /// <summary>
        /// user ID
        /// </summary>
        event Action<string> onAddFriendInput;
        
        /// <summary>
        /// user ID
        /// </summary>
        event Action<string> onRemoveFriendInput;
    }
}


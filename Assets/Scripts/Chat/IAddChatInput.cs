using System;

namespace FirebaseTest.Chats {
    public interface IAddChatInput {
        event Action onAddChatInput;
        void OnUserLogIn();
        void OnUserLogOut();
    } 
}


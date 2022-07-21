using System;

namespace FirebaseTest.Chats {
    public interface IChatInput {
        event Action<string> onMessageSendInput;
        event Action<string> onInviteInput;
        event Action onLeaveChatInput;
    }
}


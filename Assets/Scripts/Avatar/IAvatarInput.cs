using System;

namespace FirebaseTest.Avatar.UI {
    public interface IAvatarInput {
        event Action onAvatarSaveInput;
        event Action onAvatarLoadInput;
        event Action onAvatarGenerateInput;
    }
}


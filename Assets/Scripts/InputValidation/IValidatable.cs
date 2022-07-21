using System;

public interface IValidatable {
    event Action onStateChanged;
    bool IsValid();
}

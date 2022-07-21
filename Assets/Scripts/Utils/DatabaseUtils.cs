using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Firebase.Database;
using FirebaseTest;
using UnityEngine;

public static class DatabaseUtils {
    public static FirebaseDatabase Database => DatabaseInternal ??= FirebaseDatabase.DefaultInstance;
    private static FirebaseDatabase DatabaseInternal;
    
    public static async UniTask AddKey(string path) {
        var database = Database;
        var reference = database.GetReference(path);
        await reference.SetValueAsync(true);
    }

    public static async UniTask RemoveKey(string path) {
        var database = Database;
        var reference = database.GetReference(path);
        await reference.RemoveValueAsync();
    }

    public static async UniTask AddOrOverrideValue(string path, object value) {
        var database = Database;
        var reference = database.GetReference(path);
        await reference.SetValueAsync(value);
    }

    public static async UniTask<DataSnapshot> GetValue(string path) {
        var database = Database;
        var reference = database.GetReference(path);
        var data = await reference.GetValueAsync();
        return data;
    }

    public static async UniTask<string> PushAndSetValue(string path, object value) {
        var database = Database;
        var reference = database.GetReference(path);
        var key = reference.Push().Key;
        await reference.Child(key).SetValueAsync(value);
        return key;
    }

    public static DatabaseReference GetReference(string path) {
        var database = Database;
        var reference = database.GetReference(path);
        return reference;
    }

    public static async UniTask<bool> IsUserRegistered(string userId) {
        var user = await GetValue(Paths.UserPath(userId));
        if (!user.Exists) {
            return false;
        }
        return true;
    }

    public static async UniTask<string> GetEmailByID(string userId) {
        var path = Paths.UserPath(userId);
        var data = await GetValue(path);
        var email = data.Value.ToString();
        return email;
    }

    public static async UniTask<string> GetIDByEmail(string email) {
        var reference = GetReference(Paths.UsersPath);
        var data = await reference
            .OrderByValue()
            .EqualTo(email)
            .LimitToFirst(1)
            .GetValueAsync();
        if (data == null || data.ChildrenCount == 0) {
            Debug.LogWarning($"No user found with email {email}");
            return string.Empty;
        }
        return data.Children.First().Key;
    }
}

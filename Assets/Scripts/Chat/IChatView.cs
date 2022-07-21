using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirebaseTest.Chats {
    public interface IChatView {
        void AddMessage(string text, string sender, string dateTime);
        void AddMember(string memberName);
        void ClearMessages();
        void ClearMembers();
    } 
}


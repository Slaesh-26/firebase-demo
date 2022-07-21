namespace FirebaseTest {
    public static class Paths {
        public static readonly string AvatarsPath = "gs://fir-test-6010a.appspot.com/avatars/";
        public static readonly string ChatsPath = "chats";
        public static readonly string UsersPath = "users";
        public static string UserChatPath(string userId, string chatId) => $"userChats/{userId}/{chatId}";
        public static string UserChatsPath(string userId) => $"userChats/{userId}";
        public static string UserPath(string userId) => $"users/{userId}";
        public static string UserFriends(string userId) => $"friends/{userId}";
        public static string UserFriend(string userId, string friendId) => $"friends/{userId}/{friendId}";
        public static string ChatMemberPath(string chatId, string userId) => $"members/{chatId}/{userId}";
        public static string ChatMembersPath(string chatId) => $"members/{chatId}";
        public static string ChatMessagesPath(string chatId) => $"messages/{chatId}";
        public static string ChatPath(string chatId) => $"chats/{chatId}";
    }
}


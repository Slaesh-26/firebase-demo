using System;

namespace FirebaseTest.Authentication.Input {
    public interface IEmailAuthenticationInput {
        /// <summary>
        /// Email, password
        /// </summary>
        event Action<string, string> onLogIn;
        
        /// <summary>
        /// Email, password
        /// </summary>
        event Action<string, string> onRegister;
    }
}


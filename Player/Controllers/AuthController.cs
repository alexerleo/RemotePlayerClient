using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Player.Database;
using Player.Utils;

namespace Player.Controllers
{
    public class AuthController
    {
        public AuthController(SuccessEvent success, FailedEvent failed)
        {
            onSuccess += success;
            onFailed += failed;
            User user = Library.collection.db.users.FirstOrDefault();
            if (user != null)
                onSuccess();
        }

        public delegate void SuccessEvent();
        public event SuccessEvent onSuccess;

        public delegate void FailedEvent();
        public event FailedEvent onFailed;
    
        public string email { get; set; }
        public string password { get; set; }
        public void Submit()
        {
            SocketController.instance.Auth(email, password);
            SocketController.instance.callbacks.onAuth += OnAuth;
        }
        private void OnAuth(string message)
        {
            SocketController.instance.ClearCallbacks();
            try
            {
                if (message == "401")
                    onFailed();
                else
                {
                    login(new User()
                    {
                        email = email,
                        password = password
                    });
                    
                    onSuccess();
                }
                    
            }
            catch (Exception ex)
            {
                //no binding listerners
            }            
        }  
        private void login(User user)
        {
            Library.collection.db.users.Add(user);
            Library.collection.db.SaveChanges();
        }      
    }
}

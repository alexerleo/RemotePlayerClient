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
            SocketController.instance.callbacks.onAuth += OnAuth;
            user = Library.collection.db.users.FirstOrDefault();
            if (user != null)
                SocketController.instance.Auth(user.email, user.password);
        }

        public delegate void SuccessEvent();
        public event SuccessEvent onSuccess;

        public delegate void FailedEvent();
        public event FailedEvent onFailed;
    
        public string email { get; set; }
        public string password { get; set; }
        private Library Library = Library.instance;
        private User user;

        public void Submit()
        {
            SocketController.instance.Auth(email, password);            
        }

        private void OnAuth(string message)
        {
            //SocketController.instance.ClearCallbacks();
            try
            {
                if (message == "401")
                    onFailed();
                else
                {
                    if(user == null)
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

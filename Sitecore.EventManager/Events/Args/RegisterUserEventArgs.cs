using System;
using Sitecore.Modules.EventManager.Entities;
using Sitecore.Security.Accounts;

namespace Sitecore.Modules.EventManager.Events.Args
{
    public class RegisterUserEventArgs : EventArgs
    {
        public User User { get; set; }

        public EventItem EventItem { get; set; }
    }
}
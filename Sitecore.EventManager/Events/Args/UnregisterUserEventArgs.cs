using Sitecore.Modules.EventManager.Entities;
using Sitecore.Security.Accounts;

namespace Sitecore.Modules.EventManager.Events.Args
{
    public class UnregisterUserEventArgs
    {
        public UnregisterUserEventArgs()
        {

            this.Error = false;
        }

        public EventItem EventItem { get; set; }
        public User User { get; set; }
        public bool Error { get; set; }
        public bool FromCommand { get; set; }
    }
}
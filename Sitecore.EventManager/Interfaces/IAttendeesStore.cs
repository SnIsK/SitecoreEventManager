using System;
using Sitecore.Modules.EventManager.Entities;
using Sitecore.Security.Accounts;

namespace Sitecore.Modules.EventManager.Interfaces
{
    public interface IAttendeesStore
    {
        bool AddUser(EventItem eventItem, User user);
        bool RemoveUser(EventItem eventItem, User user);
        void GetRegistered(Guid guid);
    }
}
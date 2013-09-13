using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Sitecore.Modules.EventManager.App.Entities;
using Sitecore.Modules.EventManager.Entities;
using Sitecore.Security.Accounts;
using Sitecore.Web;

namespace Sitecore.Modules.EventManager.App.Repositories
{
    public class RegisteredRepository
    {
        public List<RegisteredUser> GetListData()
        {
            var queryString = WebUtil.GetQueryString("id");

            var database = Sitecore.Context.ContentDatabase ?? Sitecore.Context.Database;

            var item = database.GetItem(queryString);

            var eventItem = new EventItem(item);


            List<RegisteredUser> subsribers = new List<RegisteredUser>();
            int i = 0 ;
            foreach (var registered in eventItem.GetRegistered())
            {
                User fromName = User.FromName(registered, false);

                subsribers.Add(new RegisteredUser()
                {
                    Id = fromName.GetLocalName(),
                    Email =  fromName.Profile.Email,
                    Name = fromName.Profile.FullName
                });

            }


            return subsribers;
        }
    }
}
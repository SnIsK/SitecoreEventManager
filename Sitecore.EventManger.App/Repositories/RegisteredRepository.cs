using System.Collections.Generic;
using System.Linq;
using Sitecore.Modules.EventManager.App.Entities;
using Sitecore.Modules.EventManager.Entities;
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

            var subsribers = eventItem.GetRegistered().Select(t => new RegisteredUser()
                {
                    Name = "TODO", Email = t
                });

            return subsribers.ToList();
        }
    }
}
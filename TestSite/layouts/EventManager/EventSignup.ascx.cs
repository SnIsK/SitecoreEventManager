using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Modules.EventManager.Entities;
using Sitecore.Security.Accounts;
using Sitecore.SecurityModel.Cryptography;

namespace TestSite.layouts.EventManager
{
    public partial class EventSignup : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Signup(object sender, EventArgs e)
        {
            var eventItem = new EventItem(Sitecore.Context.Item);

            var username = Sitecore.Context.Domain.GetFullName(this.Name.Text);
            PasswordGenerator asd = new PasswordGenerator();
            var generate = asd.Generate();
            Membership.CreateUser(username, generate, this.Email.Text);
            var user = User.FromName(username, true);

            user.Profile.Email = this.Email.Text;
            user.Profile.FullName = this.Name.Text;
            user.Profile.Save();

            eventItem.RegisterUser(user);
        }
    }
}
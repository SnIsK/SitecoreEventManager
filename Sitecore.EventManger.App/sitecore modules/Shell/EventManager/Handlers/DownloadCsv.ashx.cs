using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Sitecore.Collections;
using Sitecore.Data.Items;
using Sitecore.Modules.EventManager.Entities;
using Sitecore.Security.Accounts;
using Sitecore.Shell.Feeds.Sections;

namespace Sitecore.Modules.EventManager.App.sitecore_modules.Shell.EventManager.Handlers
{
    /// <summary>
    /// Summary description for DownloadCSV
    /// </summary>
    public class DownloadCsv : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string eventId = context.Request.QueryString["EventId"];
            string settingsId = context.Request.QueryString["settingsId"];
            if (string.IsNullOrWhiteSpace(eventId) || string.IsNullOrWhiteSpace(settingsId))
            {
                return;
            }

            Item settingItem = ItemUtil.GetContentItem(Guid.Parse(settingsId));
            Item contentItem = Modules.EventManager.ItemUtil.GetContentItem(Guid.Parse(eventId));
            EventItem eventItem = new EventItem(contentItem);

            ChildList childList = settingItem.Children;

            var headers = new List<Row>();

            foreach (Item child in childList)
            {
                Row row = new Row();
                row.Title = child.Fields["HeaderFieldTitle"].Value;
                row.FieldName = child.Fields["HeaderFieldCustomFieldName"].Value;
                headers.Add(row);
            }
            StringBuilder responseString = new StringBuilder();

            context.Response.Clear();
            context.Response.ContentType = "text/csv";
            context.Response.AddHeader("content-disposition",
                string.Format("attachment;filename={0} {1}.csv", eventItem.Title.Value,
                    DateTime.Now.ToString("yyyyMMdd")));
            responseString.Append("Name;Email;");
            responseString.Append(string.Join(";", headers.Select(t => t.Title)) + Environment.NewLine);


            List<string> registered = eventItem.GetRegistered();
            List<List<string>> results = new List<List<string>>();


            foreach (var username in registered)
            {
                User fromName = User.FromName(username, false);
                var row = new List<string>();
                row.Add(string.Format("{0};{1}", fromName.Profile.FullName, fromName.Profile.Email));

                foreach (var header in headers)
                {
                    row.Add("\"" + fromName.Profile.GetCustomProperty(header.FieldName).Replace("\"", "\"\"") + "\"");
                }
                results.Add(row);
            }

            foreach (var result in results.OrderBy(t => t[0]))
            {
                responseString.Append(string.Join(";", result));

                responseString.Append(Environment.NewLine);
            }

            context.Response.Write(responseString.ToString());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
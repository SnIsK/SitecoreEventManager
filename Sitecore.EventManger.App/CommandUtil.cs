using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Links;
using Sitecore.Sites;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.UI.WebControls;

namespace Sitecore.Modules.EventManager.App
{
    public static class CommandUtil
    {
        public static void OpenPageEditor(Page page, Item item, Language language = null)
        {
            string cookieValue = WebUtil.GetCookieValue("shell#lang", item.Language.Name);
            WebUtil.SetCookieValue("shell#lang", item.Language.Name);
            UrlString urlString = new UrlString(LinkManager.GetItemUrl(Context.Database.GetItem(ID.Parse(UiItemIds.IframeDialogPageGuid))));
            urlString.Parameters["size"] = "full";
            UrlHandle urlHandle = new UrlHandle();
            urlHandle["FrameSrc"] = GetPageEditorLink(item, language);
            urlHandle["PreventPeUnloadMessage"] = true.ToString((IFormatProvider)CultureInfo.InvariantCulture);
            urlHandle.Add(urlString);
            string result;
            ScriptManager.GetCurrent(page).ShowPopup(HttpUtility.UrlPathEncode(urlString.ToString()), out result);
            if (!string.IsNullOrEmpty(result))
            {
                //EnumerableExtensions.ForEach<MessageBody>(ControlCollectionExtensions.Flatten<MessageBody>(page.Controls), (System.Action<MessageBody>)(mb => mb.Refresh()));
                ScriptManagerExtensions.RegisterStartupScript(ScriptManager.GetCurrent(page), "$('.collapsible input:visible').focus().blur();", (Control)null);
            }
            WebUtil.SetCookieValue("shell#lang", cookieValue);
        }

        private static string GetPageEditorLink(Item itemToEdit, Language language)
        {
            if (language == (Language)null)
                language = itemToEdit.Language;
            UrlString webSiteUrl = SiteContext.GetWebSiteUrl(itemToEdit.Database);
            webSiteUrl["sc_itemid"] = itemToEdit.ID.ToString();
            webSiteUrl["sc_lang"] = language.Name;
            webSiteUrl["sc_mode"] = "edit";
            return webSiteUrl.ToString();
        }
    }
}
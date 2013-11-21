using System;
using System.Web.UI.WebControls;
using Sitecore.Data.Items;
using Sitecore.Modules.EventManager.Entities;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Speak.Web.UI.WebControls;
using Sitecore.Text;

namespace Sitecore.Modules.EventManager.App.Ui.WebControls
{
    public class AddEventDialogControl : CompositeWebControl
    {
        private Button _createButton;
        private TextBox _textBox;

        public AddEventDialogControl()
        {
            this._createButton = new Button();
            this._textBox = new TextBox();
        }


        protected override void OnInit(EventArgs e)
        {
            this._textBox.Text = string.Empty;

            this._createButton.CssClass = "sc-ecm-dialog-button";
            this._createButton.ID = "SaveButton";
            this._createButton.Text = "Create";
            this._createButton.Click += new EventHandler(this.OnAcceptClick);
        }

        private void OnAcceptClick(object sender, EventArgs e)
        {
            EventRoot eventRoot = EventRoot.Current;
            Item eventItem = eventRoot.CreateEvent(this._textBox.Text);
            string linkForTaskPage = TaskPageUtils.GetLinkForTaskPage(eventItem);
            this.Page.Response.Redirect(linkForTaskPage, false);
        }


        protected override void CreateChildControls()
        {
            this.Controls.Add(this._textBox);
            this.Controls.Add(this._createButton);
        }
    }
}
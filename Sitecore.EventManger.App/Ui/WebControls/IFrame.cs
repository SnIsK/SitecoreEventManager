using System;
using System.Web.UI;
using Sitecore.Web.UI.WebControls;

namespace Sitecore.Modules.EventManager.App.Ui.WebControls
{
    public class Frame : Sitecore.Speak.Web.UI.WebControls.Frame
    {
        private bool preventPeUnloadMessage;

        public Frame()
        {
            this.preventPeUnloadMessage = false;
        }

        protected override void CreateChildControls()
        {
            this.Controls.Add((Control)this.frame);
        }

        protected override void InitializeFrame()
        {
            base.InitializeFrame();
            this.Height = this.ContentHeight;
            this.Width = this.ContentWidth;
        }

        protected override void OnInit(EventArgs e)
        {
            PopupPage popupPage = this.Page as PopupPage;
            if (popupPage != null && !string.IsNullOrEmpty(popupPage.Parameters["FrameSrc"]))
            {
                this.Src = popupPage.Parameters["FrameSrc"];
                this.preventPeUnloadMessage = MainUtil.GetBool(popupPage.Parameters["PreventPeUnloadMessage"], false);
            }
            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!this.preventPeUnloadMessage)
                return;
        }
    }
}
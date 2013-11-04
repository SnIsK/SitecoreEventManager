using System;
using System.Linq;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.Modules.EventManager.Entities;
using Sitecore.Publishing;
using Sitecore.Workflows;

namespace Sitecore.Modules.EventManager.Events
{
    public class ItemEventHandler
    {
        public void OnItemAdded(object sender, EventArgs args)
        {
            var item = Event.ExtractParameter(args, 0) as Item;

            if (item == null)
                return;

            if (item.TemplateID == Configuration.Settings.EventTemplateId || item.Template.BaseTemplates.Any(t => t.ID.Guid == Configuration.Settings.EventTemplateId.ID.Guid))
            {
                item.Editing.BeginEdit();
                this.CopyPlan(item);
                this.CopyEmailTemplate(item);
                item.Editing.EndEdit();
            }
        }

        private void CopyEmailTemplate(Item item)
        {
            var eventitem = new EventItem(item);

            EventRoot eventRoot = eventitem.EventRoot;

            item.Fields["EmailMessage"].Value = eventRoot.EmailMessage.Value;
            item.Fields["EmailSubject"].Value = eventRoot.EmailSubject.Value;
            item.Fields["EmailName"].Value = eventRoot.EmailName.Value;
            item.Fields["FromEmail"].Value = eventRoot.EmailFrom.Value;


        }

        private void CopyPlan(Item item)
        {
            var eventItem = new Entities.EventItem(item);
            var planRoot = (Sitecore.Context.ContentDatabase ?? Sitecore.Context.Database).GetItem("{963678A4-869C-48AB-915B-D18C6D2AF357}");

            //TODO: Create more unike path
            var copiedPlanItem = eventItem.EventRoot.EngangementPlanItem.CopyTo(planRoot, item.Name);

            eventItem.PlanId = copiedPlanItem.ID.Guid;

            //Set the workflow state on the plan to deploy.
            // TODO: Move this to a deploy button instead!
            IWorkflow workflow = copiedPlanItem.State.GetWorkflow();

            Item workflowCommandItem = ItemUtil.GetContentItem(Guid.Parse("{4044A9C4-B583-4B57-B5FF-2791CB0351DF}"));
            if (workflow == null)
            {
                copiedPlanItem.Database.DataManager.SetWorkflowInfo(copiedPlanItem,
                    new WorkflowInfo(workflowCommandItem.Parent.Parent.ID.ToString(), workflowCommandItem.Parent.ID.ToString()));

                workflow = copiedPlanItem.State.GetWorkflow();
            }

            WorkflowInfo workflowInfo = copiedPlanItem.Database.DataManager.GetWorkflowInfo(copiedPlanItem);
            if (workflowInfo == null || workflowInfo.StateID != workflowCommandItem.Parent.ID.ToString())
            {
                return;
            }

            WorkflowResult workflowResult = workflow.Execute("{4044A9C4-B583-4B57-B5FF-2791CB0351DF}", copiedPlanItem,
                "Executed from Events save event", false, new object());


            PublishOptions options = new PublishOptions(copiedPlanItem.Database, Database.GetDatabase("web"), PublishMode.Smart, copiedPlanItem.Language, DateTime.Now)
            {
                Deep = true,
                RootItem = copiedPlanItem
            };

            Publisher p = new Publisher(options);
            p.PublishAsync();
        }
    }
}
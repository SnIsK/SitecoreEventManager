using System;

namespace Sitecore.Modules.EventManager.Entities
{
    public class PlanData
    {
        public Guid Id { get; set; }

        [StateName("Registered")]
        public int NumberOfRegistred { get; set; }

        [StateName("Deregistered")]
        public int NumberOfDeregistered { get; set; }
    }

    public class StateNameAttribute : Attribute
    {
        public StateNameAttribute(string stateName)
        {
            this.StateName = stateName;
        }

        public string StateName { get; private set; }
    }
}
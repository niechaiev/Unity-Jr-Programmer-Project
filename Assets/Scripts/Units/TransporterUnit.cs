using System.Collections.Generic;
using Buildings;
using UnityEngine;
using Zenject;

namespace Units
{
    /// <summary>
    /// Subclass of Unit that will transport resource from a Resource Pile back to Base.
    /// </summary>
    public class TransporterUnit : Unit
    {
        public int MaxAmountTransported = 1;

        private Building currentTransportTarget;
        private Building.InventoryEntry transporting = new();
        private DropPoint dropPoint;

        // We override the GoTo function to remove the current transport target, as any go to order will cancel the transport
        public override void GoTo(Vector3 position)
        {
            base.GoTo(position);
            currentTransportTarget = null;
        }

        [Inject]
        private void Construct(DropPoint dropPointRef)
        {
            dropPoint = dropPointRef;
        }

        protected override void BuildingInRange()
        {
            if(Target==null) return;
            if (Target == dropPoint)
            {
                //we arrive at the base, unload!
                if (transporting.Count > 0)
                    Target.AddItem(transporting.ResourceId, transporting.Count);

                //we go back to the building we came from
                GoTo(currentTransportTarget);
                transporting.Count = 0;
                transporting.ResourceId = "";
            }
            else
            {
                if (Target.Inventory.Count > 0)
                {
                    transporting.ResourceId = Target.Inventory[0].ResourceId;
                    transporting.Count = Target.GetItem(transporting.ResourceId, MaxAmountTransported);
                    currentTransportTarget = Target;
                    GoTo(dropPoint);
                }
            }
        }

        //Override all the UI function to give a new name and display what it is currently transporting
        public override string GetName()
        {
            return "Transporter";
        }

        public override string GetData()
        {
            return $"Can transport up to {MaxAmountTransported}";
        }

        public override void GetContent(ref List<Building.InventoryEntry> content)
        {
            if (transporting.Count > 0)
                content.Add(transporting);
        }
    }
}
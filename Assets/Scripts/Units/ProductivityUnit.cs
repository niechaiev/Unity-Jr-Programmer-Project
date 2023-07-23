using Buildings;
using UnityEngine;

namespace Units
{
    public class ProductivityUnit : Unit
    {
        ResourcePile currentPile;
        public float ProductivityMultiplier = 2;

        protected override void BuildingInRange()
        {
            if (currentPile == null)
            {
                ResourcePile pile = Target as ResourcePile;
                if (pile != null)
                {
                    currentPile = pile;
                    currentPile.ProductionSpeed *= ProductivityMultiplier;
                }
            }
        }

        void ResetProductivity()
        {
            if (currentPile != null)
            {
                currentPile.ProductionSpeed /= ProductivityMultiplier;
                currentPile = null;
            }
        }

        public override void GoTo(Building target)
        {
            ResetProductivity();
            base.GoTo(target);
        }

        public override void GoTo(Vector3 position)
        {
            ResetProductivity();
            base.GoTo(position);
        }
    }
}
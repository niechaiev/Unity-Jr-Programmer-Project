using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductivityUnit : Unit
{
    ResourcePile m_currentPile = null;
    public float ProductiviyMultiplier = 2;

    protected override void BuildingInRange()
    {
        if (m_currentPile == null)
        {
            ResourcePile pile = m_Target as ResourcePile;
            if (pile != null)
            {
                m_currentPile = pile;
                m_currentPile.ProductionSpeed *= ProductiviyMultiplier;
            }
        }
    }
    void ResetProducitvity() 
    {
        if (m_currentPile != null)
        {
            m_currentPile.ProductionSpeed /= ProductiviyMultiplier;
            m_currentPile = null;
        }    
    }

    public override void GoTo(Building target)
    {
        ResetProducitvity();
        base.GoTo(target);
    }

    public override void GoTo(Vector3 position)
    {
        ResetProducitvity();
        base.GoTo(position);
    }

}

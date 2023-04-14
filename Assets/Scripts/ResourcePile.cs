using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A subclass of Building that produce resource at a constant rate.
/// </summary>
public class ResourcePile : Building
{
    public ResourceItem Item;

    private float productionSpeed = 0.5f;



    private float m_CurrentProduction = 0.0f;

    public float ProductionSpeed { get => productionSpeed; 
        set
        { if (value < 0.0f)
            {
                Debug.LogError("Negachin");
            }
            else
            {
                productionSpeed = value;
            }
        }
    }

    private void Update()
    {
        if (m_CurrentProduction > 1.0f)
        {
            int amountToAdd = Mathf.FloorToInt(m_CurrentProduction);
            int leftOver = AddItem(Item.Id, amountToAdd);

            m_CurrentProduction = m_CurrentProduction - amountToAdd + leftOver;
        }
        
        if (m_CurrentProduction < 1.0f)
        {
            m_CurrentProduction += ProductionSpeed * Time.deltaTime;
        }
    }

    public override string GetData()
    {
        return $"Producing at the speed of {ProductionSpeed}/s";
        
    }
    
    
}

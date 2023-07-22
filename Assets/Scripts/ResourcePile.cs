using Helpers;
using UnityEngine;

/// <summary>
/// A subclass of Building that produce resource at a constant rate.
/// </summary>
public class ResourcePile : Building
{
    public ResourceItem Item;

    private float productionSpeed = 0.5f;



    private float currentProduction;

    public float ProductionSpeed { get => productionSpeed; 
        set
        { if (value < 0.0f)
            {
                //Debug.LogError("Negachin");
            }
            else
            {
                productionSpeed = value;
            }
        }
    }

    private void Update()
    {
        if (currentProduction > 1.0f)
        {
            int amountToAdd = Mathf.FloorToInt(currentProduction);
            int leftOver = AddItem(Item.Id, amountToAdd);

            currentProduction = currentProduction - amountToAdd + leftOver;
        }
        
        if (currentProduction < 1.0f)
        {
            currentProduction += ProductionSpeed * Time.deltaTime;
        }
    }

    public override string GetData()
    {
        return $"Producing at the speed of {ProductionSpeed}/s";
        
    }
    
    
}

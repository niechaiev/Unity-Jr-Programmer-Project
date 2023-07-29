using System;
using System.Collections.Generic;
using System.Linq;
using Units;

public class Selector
{
    public List<Unit> AvailableUnits = new();
    public HashSet<Unit> SelectedUnits = new();
    public HashSet<Unit> HighlightedUnits = new();
    
    private bool isMultiSelect;

    public bool IsMultiSelect
    {
        set => isMultiSelect = value;
    }

    public Action<Unit> OnAddNewUnit;

    
    private void DeselectAll()
    {
        foreach (var selectedUnit in SelectedUnits)
            selectedUnit.Selected(false);

        SelectedUnits.Clear();
    }
    

    private void AddSelected(Unit unit)
    {
        unit.Selected(true);
        SelectedUnits.Add(unit);
    }

    private void AddSelected(List<Unit> units)
    {
        foreach (var unit in units)
            unit.Selected(true);

        SelectedUnits.UnionWith(units);
    }

    private void RemoveSelected(Unit unit)
    {
        unit.Selected(false);
        SelectedUnits.Remove(unit);
    }

    public void Highlight(Unit unit)
    {
        DeHighlightAll();
        HighlightedUnits.Add(unit);
        unit.Highlighted(true);
    }

    public void DeHighlightAll()
    {
        foreach (var highlightedUnit in HighlightedUnits)
        {
            if(!SelectedUnits.Contains(highlightedUnit)) 
                highlightedUnit.Highlighted(false);
        }
        HighlightedUnits.Clear();
    }

    public void Select(List<Unit> units)
    {
        if (!isMultiSelect)
        {
            DeselectAll();
            AddSelected(units);
            return;
        }

        if (units.Intersect(SelectedUnits).Count() != units.Count)
        {
            AddSelected(units);
            return;
        }

        foreach (var unit in units)
        {
            if (IsSelected(unit))
                RemoveSelected(unit);
            else
                AddSelected(unit);
        }
    }

    public void Select(Unit unit)
    {
        if (unit == null) return;

        if (!isMultiSelect)
        {
            DeselectAll();
            AddSelected(unit);
            return;
        }

        if (IsSelected(unit))
            RemoveSelected(unit);
        else
            AddSelected(unit);
    }

    private bool IsSelected(Unit unit)
    {
        return SelectedUnits.Contains(unit);
    }

    public void AddNewUnit(Unit unit)
    {
        AvailableUnits.Add(unit);
        OnAddNewUnit?.Invoke(unit);
    }

}
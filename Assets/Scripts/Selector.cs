using System;
using System.Collections.Generic;
using System.Linq;
using Units;

public class Selector
{
    public List<Unit> AvailableUnits = new();
    public HashSet<Unit> SelectedUnits = new();
    private bool isMultiSelect;

    public bool IsMultiSelect
    {
        get => isMultiSelect;
        set => isMultiSelect = value;
    }


    private void DeselectAll()
    {
        foreach (var selectedUnit in SelectedUnits)
            selectedUnit.OnDeselected();

        SelectedUnits.Clear();
    }

    private void AddSelected(Unit unit)
    {
        unit.OnSelected();
        SelectedUnits.Add(unit);
    }

    private void AddSelected(List<Unit> units)
    {
        foreach (var unit in units)
            unit.OnSelected();

        SelectedUnits.UnionWith(units);
    }

    private void RemoveSelected(Unit unit)
    {
        unit.OnDeselected();
        SelectedUnits.Remove(unit);
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
}
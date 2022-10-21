using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
  [SerializeField] private LayerMask selectablesLayerMask;

  private List<Unit> selectedUnits = new List<Unit>();

  private void Update()
  {
    // Listen for input that would select a unit
    if (Input.GetMouseButtonDown(0))
    {
      // Was a unit clicked?
      Unit clickedUnit = GetClickedUnit();
      if (clickedUnit == null)
      {
        // No unit was clicked; should deselect all selected units then stop
        DeselectAllUnits();
        return;
      }

      // If control was pressed while clicking
      if (Input.GetKey(KeyCode.LeftControl))
      {
        // Is this a new selection?
        bool isUnitSelected = IsUnitSelected(clickedUnit);
        if (isUnitSelected)
        {
          // Selection is subtractive
          DeselectUnit(clickedUnit);
        }
        else
        {
          // Selection is additive
          AddSelectedUnit(clickedUnit);
        }
        // No control was pressed
      }
      else
      {
        // The clicked unit replaces all selected units
        // Note: Should this be a deselect all then select one instead?
        SetSelectedUnits(new List<Unit> { clickedUnit });
      }
    }
  }

  // Returns the unit clicked, if no unit was clicked returns null
  private Unit GetClickedUnit()
  {
    // Fire a raycast to see if anything selectable was hit
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    bool didHit = Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, selectablesLayerMask);

    // See if the selectable thing hit was a unit
    if (didHit && raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
    {
      // Return the unit clicked
      return unit;
    }

    return null;
  }

  private void DeselectAllUnits()
  {
    selectedUnits.Clear();
    OnSelectionChange();
  }

  private void DeselectUnit(Unit unit)
  {
    selectedUnits.Remove(unit);
    OnSelectionChange();
  }

  private void AddSelectedUnit(Unit unit)
  {
    selectedUnits.Add(unit);
    OnSelectionChange();
  }

  private void SetSelectedUnits(List<Unit> units)
  {
    selectedUnits = units;
    OnSelectionChange();
  }

  private void OnSelectionChange()
  {
    Debug.Log($"{selectedUnits.Count} units selected");
  }

  private bool IsUnitSelected(Unit unit)
  {
    return selectedUnits.Contains(unit);
  }
}


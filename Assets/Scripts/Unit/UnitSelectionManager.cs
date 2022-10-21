using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
  This class manages which units are currently selected.
  
  Selection controls:
  - Select individual units with left mouse click
  - Add/remove units to/from selection by holding left control key while clicking
*/
public class UnitSelectionManager : MonoBehaviour
{
  public static UnitSelectionManager Instance { get; private set; }

  [SerializeField] private LayerMask selectablesLayerMask;

  private List<Unit> selectedUnits = new List<Unit>();

  private void Awake()
  {
    // Singleton setup
    if (Instance != null)
    {
      Debug.LogError("Found more than one UnitSelectionManager " + transform);
      Destroy(gameObject);
      return;
    }
    Instance = this;
  }

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

      // For now, just using single selection of units
      SingleClickSelection(clickedUnit);
    }
  }

  private void SingleClickSelection(Unit clickedUnit)
  {
    // Is this a new selection?
    bool isUnitSelected = IsUnitSelected(clickedUnit);
    if (isUnitSelected)
    {
      // Already selected; do nothing!
      return;
    }

    // It's a newly clicked unit; select it
    DeselectAllUnits();
    AddSelectedUnit(clickedUnit);
  }

  private void ControlClickSelection(Unit clickedUnit)
  {
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
      DeselectAllUnits();
      AddSelectedUnit(clickedUnit);
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
    // Inform each unit of deselection first
    foreach (Unit unit in selectedUnits)
    {
      unit.OnDeselect();
    }

    // Clear selected list
    selectedUnits.Clear(); ;
  }

  private void DeselectUnit(Unit unit)
  {
    // Inform unit of deselection
    unit.OnDeselect();

    // Remove from selected list
    selectedUnits.Remove(unit);
  }

  private void AddSelectedUnit(Unit unit)
  {
    // Inform unit of selection
    unit.OnSelect();

    // Add to selected list
    selectedUnits.Add(unit);
  }

  private bool IsUnitSelected(Unit unit)
  {
    return selectedUnits.Contains(unit);
  }

  public List<Unit> GetSelectedUnits()
  {
    return selectedUnits;
  }
}


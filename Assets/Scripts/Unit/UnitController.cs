using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitController : MonoBehaviour
{
  [SerializeField] private LayerMask selectablesLayerMask;

  private void Update()
  {
    // Listen for movement commands on a right click
    if (Input.GetMouseButtonDown(1))
    {
      // Only bother if there are selected units to move
      List<Unit> selectedUnits = UnitSelectionManager.Instance.GetSelectedUnits();
      if (selectedUnits.Count == 0)
      {
        // Nothing selected to move; stop
        return;
      }

      // Was right click location a valid move target?
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      bool didHit = Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, selectablesLayerMask);
      if (!didHit || raycastHit.transform.GetComponent<Unit>() != null)
      {
        // Hit a unit; not a valid move target
        return;
      }

      // Otherwise, hit a vaild move target -  issue move command to selected units
      foreach (Unit unit in selectedUnits)
      {
        unit.MoveTo(raycastHit.point);
      }

      // Show the movement indicator visual
      MovementIndicator.Instance.AppearAt(raycastHit.point);
    }
  }
}


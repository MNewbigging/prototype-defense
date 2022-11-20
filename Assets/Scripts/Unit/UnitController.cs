using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class UnitController : MonoBehaviour
{
  [SerializeField] private LayerMask selectablesLayerMask;
  [SerializeField] private int groundLayerMask;

  private void Update()
  {
    // Listen for right click commands
    if (Input.GetMouseButtonDown(1))
    {
      // Only bother if there are selected units to command
      List<Unit> selectedUnits = UnitSelectionManager.Instance.GetSelectedUnits();
      if (selectedUnits.Count == 0)
      {
        return;
      }

      // Raycast into scene from the mouse
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      bool didHit = Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue);
      if (!didHit)
      {
        // Nothing at all was hit; stop!
        return;
      }

      // Determine if right-clicked an enemy unit
      if (raycastHit.transform.TryGetComponent<Unit>(out Unit clickedUnit) &&
          LevelUnitManager.Instance.IsEnemyToUnits(selectedUnits, clickedUnit))
      {
        // Treat as an attack command
        Debug.Log("Attack move yet to be implemented");
        return;
      }

      // Move towards the click location - the pathfinding will get as close as it can
      foreach (Unit unit in selectedUnits)
      {
        IssueMoveCommand(unit, raycastHit.point);
      }
    }
  }

  private void IssueMoveCommand(Unit unit, Vector3 targetPosition)
  {
    // First get the seeker a* component for pathfinding
    Seeker seeker = unit.GetComponent<Seeker>();

    // Calculate the path to target
    seeker.StartPath(unit.transform.position, targetPosition);
  }
}


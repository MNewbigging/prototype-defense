using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
  This class performs actions on units following user input.
*/
public class UnitActionManager : MonoBehaviour
{
  [SerializeField] private LayerMask walkableGroundLayerMask;

  private void Update()
  {
    // Listen for a right click
    if (Input.GetMouseButtonDown(1))
    {
      // Did right click occur on a vaild movement target in the scene?
      Vector3? clickPos = GetClickedPosition();
      if (clickPos == null)
      {
        // Not a valid move target; stop
        Debug.Log("Not a valid move target");
        return;
      }
      else
      {
        Debug.Log("Right clicked on valid move target at " + clickPos);
      }
    }
  }

  private Vector3? GetClickedPosition()
  {
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    bool didHit = Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, walkableGroundLayerMask);

    // So long as the thing hit was not a unit
    if (didHit && raycastHit.transform.GetComponent<Unit>() == null)
    {
      return raycastHit.point;
    }

    return null;
  }
}

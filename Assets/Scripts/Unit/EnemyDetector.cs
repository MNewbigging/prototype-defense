using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
  Stores enemies in range, accessible via editor reference in Unit.
*/
public class EnemyDetector : MonoBehaviour
{
  public event EventHandler<Transform> OnEnemyExitRange;

  [SerializeField] private Unit attachedUnit;
  private List<Transform> enemiesInRange = new List<Transform>();


  private void OnTriggerEnter(Collider other)
  {
    // Ensure this was a unit
    if (!other.transform.TryGetComponent<Unit>(out Unit unit))
    {
      return;
    }

    // An enemy unit is one not on this unit's team
    if (unit.GetTeam() == attachedUnit.GetTeam())
    {
      return;
    }

    // Found a unit on another team - it's an enemy!
    enemiesInRange.Add(other.transform);
  }

  private void OnTriggerExit(Collider other)
  {
    // If other was in the list of in-range enemies
    if (enemiesInRange.Contains(other.transform))
    {
      // It has just left this unit's range, so remove it from the list
      enemiesInRange.Remove(other.transform);

      // And notify
      OnEnemyExitRange(this, other.transform);
    }
  }


  public bool EnemiesAreInRange()
  {
    return enemiesInRange.Count > 0;
  }

  public List<Transform> GetEnemiesInRange()
  {
    return enemiesInRange;
  }
}

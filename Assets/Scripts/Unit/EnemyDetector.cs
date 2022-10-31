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
  [SerializeField] private float detectionRange = 2f;
  private List<Transform> enemiesInRange = new List<Transform>();




  public List<Transform> GetEnemiesInRange()
  {
    // Spherecast to detect any enemies in range
    RaycastHit[] hits = Physics.SphereCastAll(transform.position, detectionRange, transform.forward, detectionRange);

    List<Transform> enemies = new List<Transform>();

    foreach (RaycastHit hit in hits)
    {
      // Ensure this was a unit
      if (!hit.transform.TryGetComponent<Unit>(out Unit hitUnit))
      {
        continue;
      }

      // Ensure this was a different unit than the one doing the detecting
      if (hitUnit == attachedUnit)
      {
        continue;
      }

      // Also ensure this unit is an enemy
      if (LevelUnitManager.Instance.UnitsAreEnemies(hitUnit, attachedUnit))
      {
        // Can add it to list of potential enemies
        enemies.Add(hitUnit.transform);
      }
    }

    return enemies;
  }
}

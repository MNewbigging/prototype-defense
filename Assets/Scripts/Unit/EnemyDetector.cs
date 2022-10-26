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

  private List<Transform> enemiesInRange = new List<Transform>();

  private void OnTriggerEnter(Collider other)
  {
    // Ensure this was a valid enemy
    if (!other.transform.TryGetComponent<Enemy>(out Enemy enemy))
    {
      return;
    }

    enemiesInRange.Add(other.transform);
  }

  private void OnTriggerExit(Collider other)
  {
    // Ensure this was a valid enemy
    if (!other.transform.TryGetComponent<Enemy>(out Enemy enemy))
    {
      return;
    }

    enemiesInRange.Remove(other.transform);
    OnEnemyExitRange(this, other.transform);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
  Stores enemies in range, accessible via editor reference in Unit.
*/
public class EnemyDetector : MonoBehaviour
{
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

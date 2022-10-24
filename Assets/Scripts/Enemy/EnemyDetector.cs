using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
  [SerializeField] private Unit unit;

  private List<Transform> detectedEnemies = new List<Transform>();
  private Transform targetEnemy;

  private void OnTriggerEnter(Collider other)
  {
    Debug.Log("Detected " + other);
    // Ensure this was a valid enemy
    bool isEnemy = other.transform.TryGetComponent<Enemy>(out Enemy enemy);
    if (!isEnemy)
    {
      return;
    }
    Debug.Log("Valid enemy!");

    // Add to list of enemies in range
    detectedEnemies.Add(other.transform);

    // If there is no current target enemy, use this one
    if (targetEnemy == null)
    {
      targetEnemy = other.transform;
    }
    else if (Vector3.Distance(transform.position, other.transform.position) < Vector3.Distance(transform.position, targetEnemy.position))
    {
      // Otherwise, if this enemy is closer, target that
      targetEnemy = other.transform;
    }
  }

  private void OnTriggerExit(Collider other)
  {
    // No longer in range; remove from list
    detectedEnemies.Remove(other.transform);

    // Stop targeting 
    if (targetEnemy == other.transform)
    {
      targetEnemy = null;
    }
  }

  private void Update()
  {
    // If there's no target to look at, stop
    if (targetEnemy == null)
    {
      return;
    }

    // Otherwise, turn to face target enemy
    unit.transform.LookAt(targetEnemy, Vector3.up);
  }
}

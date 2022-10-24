using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetClosestEnemy : MonoBehaviour
{
  [SerializeField] private Unit unit;
  [SerializeField] private Animator unitAnimator;

  private List<Transform> detectedEnemies = new List<Transform>();
  private Transform targetEnemy;

  private void OnTriggerEnter(Collider other)
  {
    // Ensure this was a valid enemy
    bool isEnemy = other.transform.TryGetComponent<Enemy>(out Enemy enemy);
    if (!isEnemy)
    {
      return;
    }

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
      unitAnimator.SetBool("IsFiring", false);
    }
  }

  private void Update()
  {
    // If there's no target to look at, stop
    if (targetEnemy == null)
    {
      return;
    }

    // Rotate to face target
    Vector3 lookDirection = (targetEnemy.position - unit.transform.position).normalized;
    unit.transform.forward = Vector3.Lerp(transform.forward, lookDirection, unit.rotateSpeed * Time.deltaTime);


    // Fire at it!
    unitAnimator.SetBool("IsFiring", true);
  }
}

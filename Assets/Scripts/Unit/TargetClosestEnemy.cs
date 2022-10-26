using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetClosestEnemy : TargetingBehaviour
{
  public override Transform GetTargetEnemy(List<Transform> enemiesInRange)
  {
    float closestDistance = float.MaxValue;
    Transform closestEnemy = enemiesInRange[0];

    foreach (Transform enemy in enemiesInRange)
    {
      float distance = Vector3.Distance(transform.position, enemy.position);
      if (distance < closestDistance)
      {
        closestDistance = distance;
        closestEnemy = enemy;
      }
    }

    return closestEnemy;
  }
}

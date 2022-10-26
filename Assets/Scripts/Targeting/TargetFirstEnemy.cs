using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFirstEnemy : TargetingBehaviour
{
  public override Transform GetTargetEnemy(List<Transform> enemiesInRange)
  {
    return enemiesInRange[0];
  }
}

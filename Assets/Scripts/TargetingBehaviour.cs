using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  
  Base class for targeting behaviours.
  This should only return an enemy to target from those in range.
*/
public abstract class TargetingBehaviour : MonoBehaviour
{
  public abstract Transform GetTargetEnemy(List<Transform> enemiesInRange);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
  Responsible for spawning units for a given level, and keeping track of unit teams.
*/
public class LevelUnitManager : MonoBehaviour
{
  public static LevelUnitManager Instance { get; private set; }

  private void Awake()
  {
    // Singleton setup
    if (Instance != null)
    {
      Debug.LogError("Found more than one LevelUnitManager " + transform);
      Destroy(gameObject);
      return;
    }
    Instance = this;
  }

  public bool WasFriendlyFire(int fromTeam, int toTeam)
  {
    return fromTeam == toTeam;
  }
}

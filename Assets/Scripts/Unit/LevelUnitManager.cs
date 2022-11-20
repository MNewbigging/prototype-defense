using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
  Responsible for spawning units for a given level, and keeping track of unit teams.
*/
public class LevelUnitManager : MonoBehaviour {
  public static LevelUnitManager Instance { get; private set; }

  [SerializeField] private Transform enemySpawnPoint;
  [SerializeField] private GameObject enemyUnitPrefab;
  private float nextSpawnTimer = 1f;
  private int remainingEnemies = 1;

  private void Awake() {
    // Singleton setup
    if (Instance != null) {
      Debug.LogError("Found more than one LevelUnitManager " + transform);
      Destroy(gameObject);
      return;
    }
    Instance = this;
  }

  private void FixedUpdate() {
    HandleEnemySpawning();
  }

  private void HandleEnemySpawning() {
    // If there are no more enemies to spawn, stop
    if (remainingEnemies <= 0) {
      return;
    }



    nextSpawnTimer -= Time.deltaTime;

    if (nextSpawnTimer <= 0f) {
      SpawnEnemy();
    }
  }

  private void SpawnEnemy() {
    // Create a new unit
    Instantiate(enemyUnitPrefab, enemySpawnPoint.position, Quaternion.identity);

    // Send it into the scene

    // One less enemy remains to be spawned
    remainingEnemies--;
  }

  public bool WasFriendlyFire(int fromTeam, int toTeam) {
    return fromTeam == toTeam;
  }

  public bool UnitsAreEnemies(Unit first, Unit second) {
    return first.GetTeam() != second.GetTeam();
  }

  public bool IsEnemyToUnits(List<Unit> units, Unit potentialEnemy) {
    foreach (Unit unit in units) {
      if (UnitsAreEnemies(unit, potentialEnemy)) {
        return true;
      }
    }

    return false;
  }
}

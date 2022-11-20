using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombatState : UnitStateBase {
  [SerializeField] private float detectionRange = 5f;
  [SerializeField] private float aimRotateSpeed = 10f;

  private Unit unit;

  private TargetingBehaviour targetingBehaviour;
  private Transform targetEnemy;

  private Gun gun;

  private void Awake() {
    unit = GetComponent<Unit>();
    targetingBehaviour = GetComponent<TargetClosestEnemy>();
    gun = GetComponent<Gun>();
  }

  private void HandleCombat() {
    // First, see if there are any enemies in range of this unit
    // TODO - might want to throttle this check since it can become expensive
    List<Transform> enemiesInRange = GetEnemiesInRange();
    if (enemiesInRange.Count == 0) {
      return;
    }

    // Pick a target according to current targeting behaviour
    targetEnemy = targetingBehaviour.GetTargetEnemy(enemiesInRange);

    // Face target
    Vector3 lookDir = (targetEnemy.position - transform.position).normalized;
    transform.forward = Vector3.Lerp(transform.forward, lookDir, aimRotateSpeed * Time.deltaTime);

    // Shoot it 
    // TODO - only shoot when actually facing the enemy
    gun.FireAt(targetEnemy.transform);
  }

  private List<Transform> GetEnemiesInRange() {
    // Spherecast to detect any enemies in range
    RaycastHit[] hits = Physics.SphereCastAll(transform.position, detectionRange, transform.forward, detectionRange);

    List<Transform> enemies = new List<Transform>();

    foreach (RaycastHit hit in hits) {
      // Ensure this was a unit
      if (!hit.transform.TryGetComponent<Unit>(out Unit hitUnit)) {
        continue;
      }

      // Ensure this was a different unit than the one doing the detecting
      if (this == hitUnit) {
        continue;
      }

      // Also ensure this unit is an enemy
      if (LevelUnitManager.Instance.UnitsAreEnemies(unit, hitUnit)) {
        // Can add it to list of potential enemies
        enemies.Add(hitUnit.transform);
      }
    }

    return enemies;
  }

  public override UnitStateName GetName() {
    return UnitStateName.COMBAT;
  }

  public override void OnUpdate() {
    base.OnUpdate();

    HandleCombat();
  }
}

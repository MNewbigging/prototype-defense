using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombatState : UnitStateBase {
  [SerializeField] private float detectionRange = 5f;
  [SerializeField] private float aimRotateSpeed = 10f;
  [SerializeField] private LayerMask unitsLayerMask;
  [SerializeField] private LayerMask obstaclesLayerMask;
  [SerializeField] private float viewRadius = 4f;
  [SerializeField][Range(0, 360)] private float viewAngle = 45f;
  [SerializeField] private Transform eyeHeight;
  private Vector3 searchTarget;
  private Vector3 castFrom = new Vector3();

  private TargetingBehaviour targetingBehaviour;
  private Transform targetEnemy;

  private Gun gun;

  private void Awake() {
    targetingBehaviour = GetComponent<TargetClosestEnemy>();
    gun = GetComponent<Gun>();
  }

  private void HandleCombat() {
    // First, see if there are any enemies in range of this unit
    // TODO - might want to throttle this check since it can become expensive
    List<Transform> enemiesInRange = FindEnemiesInView();
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

  private List<Transform> FindEnemiesInView() {
    // Get all targets in viewing range
    Collider[] targetsInRange = Physics.OverlapSphere(transform.position, viewRadius, unitsLayerMask);

    // Filter out by enemy units
    List<Unit> enemyUnitsInRange = GetEnemyUnitsFromColliders(targetsInRange);
    if (enemyUnitsInRange.Count == 0) {
      return null;
    }

    List<Transform> enemyUnitsInView = new List<Transform>();
    foreach (Unit targetUnit in enemyUnitsInRange) {
      // Get direction to target in range, test if within viewing angle
      Vector3 dirToTarget = (targetUnit.transform.position - transform.position).normalized;
      if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) {
        float distance = Vector3.Distance(transform.position, targetUnit.transform.position);
        // Ensure it can be 'seen' by raycasting from eye-height
        castFrom = transform.position;
        castFrom.y = eyeHeight.position.y;
        // As long as no obstacle was hit on the way to the target
        if (!Physics.Raycast(castFrom, dirToTarget, distance, obstaclesLayerMask)) {
          // This is an enemy in view
          enemyUnitsInView.Add(targetUnit.transform);
        }
      }
    }

    return enemyUnitsInView;
  }

  private List<Unit> GetEnemyUnitsFromColliders(Collider[] colliders) {
    List<Unit> units = new List<Unit>();

    foreach (Collider collider in colliders) {
      if (collider.gameObject.TryGetComponent<Unit>(out Unit colliderUnit)) {
        // See if this was an enemy unit
        if (LevelUnitManager.Instance.UnitsAreEnemies(unit, colliderUnit)) {
          units.Add(colliderUnit);
        }
      }
    }

    return units;
  }

  public override UnitStateName GetName() {
    return UnitStateName.COMBAT;
  }

  public override void OnUpdate() {
    base.OnUpdate();

    HandleCombat();
  }
}

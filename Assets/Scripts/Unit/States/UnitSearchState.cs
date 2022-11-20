using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSearchState : UnitStateBase {

  [SerializeField] private LayerMask unitsLayerMask;
  [SerializeField] private LayerMask obstaclesLayerMask;
  [SerializeField] private float viewRadius = 4f;
  [SerializeField][Range(0, 360)] private float viewAngle = 45f;
  [SerializeField] private Transform eyeHeight;
  private Vector3 searchTarget;
  private Vector3 castFrom = new Vector3();


  public override UnitStateName GetName() {
    return UnitStateName.SEARCH;
  }

  public override void OnEnter() {
    base.OnEnter();

    // Find a new search target location within the world bounds
    // For now, set this to the player's position
  }

  private void HandleSearch() {
    // Get all targets in viewing range
    Collider[] targetsInView = Physics.OverlapSphere(transform.position, viewRadius, unitsLayerMask);

    // Filter out by enemy units
    List<Unit> enemyUnits = GetEnemyUnitsFromColliders(targetsInView);
    if (enemyUnits.Count == 0) {
      return;
    }


    foreach (Unit targetUnit in enemyUnits) {
      // Get direction to target in range, test if within viewing angle
      Vector3 dirToTarget = (targetUnit.transform.position - transform.position).normalized;
      if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) {
        float distance = Vector3.Distance(transform.position, targetUnit.transform.position);
        // Ensure it can be 'seen' by raycasting from eye-height
        castFrom = transform.position;
        castFrom.y = eyeHeight.position.y;
        // As long as no obstacle was hit on the way to the target
        if (!Physics.Raycast(castFrom, dirToTarget, distance, obstaclesLayerMask)) {
          // Search is over!
        }
      }
    }
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

  private Vector3 DirFromAngle(float angleInDegress) {
    return new Vector3(Mathf.Sin(angleInDegress * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegress * Mathf.Deg2Rad));
  }
}

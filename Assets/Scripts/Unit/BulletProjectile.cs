using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour {
  [SerializeField] private TrailRenderer trailRenderer;
  [SerializeField] private Transform bulletHitVfxPrefab;

  private Vector3 targetDirection;
  private Vector3 lastPosition;

  // Ref to the unit that fired the gun which created this bullet
  private Unit firingUnit;
  private int bulletDamage;

  public void Setup(Unit unit, int bulletDamage, Vector3 targetDirection) {
    this.firingUnit = unit;
    this.bulletDamage = bulletDamage;
    this.targetDirection = targetDirection;

    // Set last position to the current position at start of flight
    lastPosition = transform.position;
  }

  private void FixedUpdate() {
    // Move in target direction
    float moveSpeed = 200f;
    transform.position += targetDirection * moveSpeed * Time.deltaTime;

    // Raycast from last position to current position to see if anything was hit
    float distanceTravelled = Vector3.Distance(transform.position, lastPosition);
    if (Physics.Raycast(lastPosition, targetDirection, out RaycastHit raycastHit, distanceTravelled)) {
      // Hit something - move to place hit
      transform.position = raycastHit.point;

      // Check if the bullet hit a unit
      if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit)) {
        // Check with level unit manager if this was friendly fire or not
        if (!LevelUnitManager.Instance.WasFriendlyFire(firingUnit.GetTeam(), unit.GetTeam())) {
          // Should damage other unit
          unit.TakeDamage(bulletDamage);
        }
      }

      // Unparent trail renderer so it fades away and destroys itself when done
      trailRenderer.transform.parent = null;

      // Create the bullet hit vfx
      Instantiate(bulletHitVfxPrefab, transform.position, Quaternion.identity);

      // Destroy the bullet
      Destroy(gameObject);
    }

    // This is now the last position for next tick
    lastPosition = transform.position;
  }
}

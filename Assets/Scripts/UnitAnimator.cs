using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitAnimator : MonoBehaviour
{
  [SerializeField] private Animator animator;
  [SerializeField] private Transform bulletProjectilePrefab;
  [SerializeField] private Transform shootPointTransform;

  private void Awake()
  {
    if (TryGetComponent<Unit>(out Unit unit))
    {
      unit.OnStartMoving += Unit_OnStartMoving;
      unit.OnStopMoving += Unit_OnStopMoving;
      unit.OnShoot += Unit_OnShoot;
    }
  }

  private void Unit_OnStartMoving(object sender, EventArgs e)
  {
    animator.SetBool("IsWalking", true);
  }

  private void Unit_OnStopMoving(object sender, EventArgs e)
  {
    animator.SetBool("IsWalking", false);
  }

  private void Unit_OnShoot(object sender, Transform enemy)
  {
    animator.SetTrigger("Shoot");

    // Create the bullet, get its script and set its target position
    Transform bulletTransform = Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
    BulletProjectile bulletProjectile = bulletTransform.GetComponent<BulletProjectile>();

    // Keep bullet target level with origin for now - can introduce spread later
    Vector3 targetPosition = enemy.transform.position;
    targetPosition.y = shootPointTransform.position.y;

    bulletProjectile.SetTarget(targetPosition);
  }
}

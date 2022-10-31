using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gun : MonoBehaviour
{
  public event EventHandler OnShoot;
  public event EventHandler OnReload;

  [SerializeField] private Transform bulletProjectilePrefab;
  [SerializeField] private Transform shootPointTransform;

  [SerializeField] private int rpm = 60;
  [SerializeField] private int magSize = 10;
  [SerializeField] private int bulletDamage = 30;

  private float rps = 1;
  private float bulletTimer = 0f;
  private int bulletsInMag;
  private bool reloading = false;

  private int team;

  private void Awake()
  {
    // Work out rounds per second
    rps = 60f / rpm;

    // Starts fully loaded
    bulletsInMag = this.magSize;
  }

  public void SetTeam(int team)
  {
    this.team = team;
  }

  public void FireAt(Transform enemyTransform)
  {
    // Can't fire if reloading
    if (reloading)
    {
      return;
    }

    // Increment bullet timer
    bulletTimer += Time.deltaTime;

    // If the timer has reached the rps
    if (bulletTimer >= rps)
    {
      // Fire this bullet
      FireBullet(enemyTransform);

      // Was that the last bullet in the mag?
      if (bulletsInMag == 0)
      {
        // Then reload
        Reload();
      }

      // Restart the bullet timer for next bullet
      bulletTimer = 0f;
    }
  }

  private void FireBullet(Transform target)
  {
    // Create the bullet, get its script 
    Transform bulletTransform = Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
    BulletProjectile bulletProjectile = bulletTransform.GetComponent<BulletProjectile>();

    // Keep bullet target level with origin for now - can introduce spread later
    Vector3 targetPosition = target.position;
    targetPosition.y = shootPointTransform.position.y;
    Vector3 targetDirection = (targetPosition - shootPointTransform.position).normalized;

    // Inform bullet which team this gun fires for, its damage and direction
    bulletProjectile.Setup(team, bulletDamage, targetDirection);

    // One less bullet in the mag
    bulletsInMag--;

    // Inform others that this gun has fired a shot
    OnShoot?.Invoke(this, EventArgs.Empty);
  }

  private void Reload()
  {
    this.reloading = true;

    // Inform others
    OnReload?.Invoke(this, EventArgs.Empty);
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Unit : MonoBehaviour
{
  // Unit action events
  public event EventHandler OnStartMoving;
  public event EventHandler OnStopMoving;

  // General
  [SerializeField] private MeshRenderer unitSelectedVisual;
  [SerializeField] private int team = 1;
  private HealthSystem healthSystem;

  // Movement
  private Vector3 targetMovePosition;
  private float moveSpeed = 4f;
  private bool moving = false;
  private float rotateSpeed = 10f;

  // Enemy
  private TargetingBehaviour targetingBehaviour;
  private Transform targetEnemy;
  private float detectionRange = 5f;

  // Weapon
  private Gun gun;

  private void Awake()
  {
    // Default target move position is position this unit starts in
    targetMovePosition = transform.position;

    // Turn off selected visual by default
    unitSelectedVisual.enabled = false;

    // Target closest enemy by default
    targetingBehaviour = GetComponent<TargetClosestEnemy>();

    // Get the attached gun
    gun = GetComponent<Gun>();

    // Get attached health system
    healthSystem = GetComponent<HealthSystem>();
  }

  private void Update()
  {
    HandleMovement();
    HandleCombat();
  }

  private void HandleMovement()
  {
    // Check if we should be moving
    if (moving)
    {
      // Has unit reached target yet?
      float stoppingDistance = 0.05f;
      if (Vector3.Distance(transform.position, targetMovePosition) > stoppingDistance)
      {
        // Not yet; continue moving
        MoveToTarget();
      }
      else
      {
        // Within acceptable stopping distance of target; stop moving now
        ReachedTarget();
      }
    }
  }

  private void MoveToTarget()
  {
    // Move towards target
    Vector3 moveDirection = (targetMovePosition - transform.position).normalized;
    transform.position += moveDirection * this.moveSpeed * Time.deltaTime;

    // Rotate to face target smoothly - so long as there isn't an enemy to face
    if (targetEnemy == null)
    {
      transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
    }
  }

  private void ReachedTarget()
  {
    moving = false;
    OnStopMoving?.Invoke(this, EventArgs.Empty);
  }

  private void HandleCombat()
  {
    // If there is no current target enemy,
    if (targetEnemy == null)
    {
      // Get all enemies in range 
      List<Transform> enemiesInRange = GetEnemiesInRange();

      // If no enemies, nothing to do!
      if (enemiesInRange.Count == 0)
      {
        return;
      }

      // Find one from enemies in range, according to current targeting behaviour
      targetEnemy = targetingBehaviour.GetTargetEnemy(enemiesInRange);

      // Listen for when that enemy dies
      targetEnemy.GetComponent<HealthSystem>().OnDie += EnemyHealthSystem_OnDie;
    }

    // Face enemy
    Vector3 lookDir = (targetEnemy.position - transform.position).normalized;
    transform.forward = Vector3.Lerp(transform.forward, lookDir, rotateSpeed * Time.deltaTime);

    // Shoot it 
    // TODO - only shoot when actually facing the enemy
    gun.FireAt(targetEnemy.transform);
  }

  private List<Transform> GetEnemiesInRange()
  {
    // Spherecast to detect any enemies in range
    RaycastHit[] hits = Physics.SphereCastAll(transform.position, detectionRange, transform.forward, detectionRange);

    List<Transform> enemies = new List<Transform>();

    foreach (RaycastHit hit in hits)
    {
      // Ensure this was a unit
      if (!hit.transform.TryGetComponent<Unit>(out Unit hitUnit))
      {
        continue;
      }

      // Ensure this was a different unit than the one doing the detecting
      if (this == hitUnit)
      {
        continue;
      }

      // Also ensure this unit is an enemy
      if (LevelUnitManager.Instance.UnitsAreEnemies(this, hitUnit))
      {
        // Can add it to list of potential enemies
        enemies.Add(hitUnit.transform);
      }
    }

    return enemies;
  }

  private void EnemyHealthSystem_OnDie(object sender, Transform enemy)
  {
    RemoveTargetEnemy(enemy);
  }

  private void RemoveTargetEnemy(Transform enemy)
  {
    // If that is our target, can remove this enemy as a target
    if (targetEnemy == enemy)
    {
      targetEnemy = null;
    }

    // Stop listening to that event now
    enemy.GetComponent<HealthSystem>().OnDie -= EnemyHealthSystem_OnDie;
  }

  public int GetTeam()
  {
    return team;
  }

  public void SetTeam(int team)
  {
    this.team = team;
  }

  public void OnSelect()
  {
    unitSelectedVisual.enabled = true;
  }

  public void OnDeselect()
  {
    unitSelectedVisual.enabled = false;
  }

  public void MoveTo(Vector3 moveTarget)
  {
    // Start moving
    targetMovePosition = moveTarget;
    moving = true;
    OnStartMoving?.Invoke(this, EventArgs.Empty);
  }

  public void TakeDamage(int damageAmount) => healthSystem.TakeDamage(damageAmount);
}

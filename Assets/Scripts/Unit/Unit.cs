using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pathfinding;

public class Unit : MonoBehaviour
{
  // Unit action events
  public event EventHandler OnStartMoving;
  public event EventHandler OnStopMoving;

  // General
  [SerializeField] private MeshRenderer unitSelectedVisual;
  [SerializeField] private int team = 1;
  private HealthSystem healthSystem;

  // Enemy
  private TargetingBehaviour targetingBehaviour;
  private Transform targetEnemy;
  private float detectionRange = 5f;
  private float rotateSpeed = 10f;

  // Movement
  private RichAI agent;
  private bool moving = false;

  // Weapon
  private Gun gun;

  private void Awake()
  {
    // Turn off selected visual by default
    unitSelectedVisual.enabled = false;

    // Target closest enemy by default
    targetingBehaviour = GetComponent<TargetClosestEnemy>();

    // Get the attached gun
    gun = GetComponent<Gun>();

    // Get attached health system
    healthSystem = GetComponent<HealthSystem>();

    agent = GetComponent<RichAI>();
  }

  private void FixedUpdate()
  {
    HandleMovement();
    HandleCombat();
  }

  private void HandleMovement()
  {
    if (!moving)
    {
      return;
    }

    if (agent.reachedEndOfPath && !agent.pathPending)
    {
      StopMoving();
    }
  }

  private void HandleCombat()
  {
    // First, see if there are any enemies in range of this unit
    // TODO - might want to throttle this check since it can become expensive
    List<Transform> enemiesInRange = GetEnemiesInRange();
    if (enemiesInRange.Count == 0)
    {
      return;
    }

    // Pick a target according to current targeting behaviour
    targetEnemy = targetingBehaviour.GetTargetEnemy(enemiesInRange);

    // Face target
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

  public int GetTeam()
  {
    return team;
  }

  public void OnSelect()
  {
    unitSelectedVisual.enabled = true;
  }

  public void OnDeselect()
  {
    unitSelectedVisual.enabled = false;
  }

  public void TakeDamage(int damageAmount) => healthSystem.TakeDamage(damageAmount);

  public void StartMoving()
  {
    moving = true;
    OnStartMoving?.Invoke(this, EventArgs.Empty);
  }

  public void StopMoving()
  {
    if (!moving)
    {
      return;
    }

    moving = false;
    OnStopMoving?.Invoke(this, EventArgs.Empty);
  }
}

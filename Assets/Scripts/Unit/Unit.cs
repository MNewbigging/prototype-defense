using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Unit : MonoBehaviour
{
  public event EventHandler OnStartMoving;
  public event EventHandler OnStopMoving;
  public event EventHandler<Transform> OnShoot;

  [SerializeField] private MeshRenderer unitSelectedVisual;

  // Movement
  private Vector3 targetMovePosition;
  private float moveSpeed = 4f;
  private bool moving = false;
  private float rotateSpeed = 10f;

  // Enemy
  [SerializeField] private EnemyDetector enemyDetector;
  private TargetingBehaviour targetingBehaviour;
  private Transform targetEnemy;

  private void Awake()
  {
    // Default target move position is position this unit starts in
    targetMovePosition = transform.position;

    // Turn off selected visual by default
    unitSelectedVisual.enabled = false;

    // Target closest enemy by default
    targetingBehaviour = GetComponent<TargetClosestEnemy>();

    // Listen for when enemies leave range
    enemyDetector.OnEnemyExitRange += EnemyDetector_OnEnemyExitRange;
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
    // If there are no enemies in range, stop
    if (!enemyDetector.EnemiesAreInRange())
    {
      return;
    }

    // If there is no current target enemy, find one from enemies in range
    if (targetEnemy == null)
    {
      targetEnemy = targetingBehaviour.GetTargetEnemy(enemyDetector.GetEnemiesInRange());
    }

    // Face enemy
    Vector3 lookDir = (targetEnemy.position - transform.position).normalized;
    transform.forward = Vector3.Lerp(transform.forward, lookDir, rotateSpeed * Time.deltaTime);

    // Shoot it 
    // TODO - need to implement a Gun class with rate of fire so this doesn't get called every frame
    OnShoot(this, targetEnemy);
  }

  private void EnemyDetector_OnEnemyExitRange(object sender, Transform enemy)
  {
    // If the enemy that left our range was being targeted, clear the target
    if (targetEnemy == enemy)
    {
      targetEnemy = null;
    }
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
}

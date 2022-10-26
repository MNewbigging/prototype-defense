using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unit : MonoBehaviour
{
  [SerializeField] private Animator unitAnimator;
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
  }

  private void Update()
  {
    HandleMovement();

    // Check if we should be firing at enemies

    // If currently targeting an enemy, shoot at it
    if (targetEnemy)
    {
      // Face enemy
      Vector3 lookDir = (targetEnemy.position - transform.position).normalized;
      transform.forward = Vector3.Lerp(transform.forward, lookDir, rotateSpeed * Time.deltaTime);

      // Shoot it
      unitAnimator.SetBool("IsFiring", true);
    }
    else
    {
      // Otherwise, check if we should be targeting an enemy
      if (!enemyDetector.EnemiesAreInRange())
      {
        // No enemies to target
        return;
      }

      // Select a new target enemy according to current targeting behaviour
      targetEnemy = targetingBehaviour.GetTargetEnemy(enemyDetector.GetEnemiesInRange());
    }
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

    // Play walking animation
    unitAnimator.SetBool("IsWalking", true);
  }

  private void ReachedTarget()
  {
    // No longer moving
    unitAnimator.SetBool("IsWalking", false);
    moving = false;
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
    targetMovePosition = moveTarget;
    moving = true;
  }

  public void OnDetectEnemy(Transform enemy)
  {
    Debug.Log("Detected an enemey");
  }
}

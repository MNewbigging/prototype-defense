using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unit : MonoBehaviour
{
  public float rotateSpeed = 10f;

  [SerializeField] private Animator unitAnimator;
  [SerializeField] private MeshRenderer unitSelectedVisual;
  private Vector3 targetPosition;
  private float moveSpeed = 4f;
  private bool moving = false;

  private void Awake()
  {
    // Default target position is position this unit starts in
    targetPosition = transform.position;

    // Turn off select visual by default
    unitSelectedVisual.enabled = false;
  }

  private void Update()
  {
    // Check if we should be moving
    if (moving)
    {
      // Has unit reached target yet?
      float stoppingDistance = 0.05f;
      if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
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
    Vector3 moveDirection = (targetPosition - transform.position).normalized;
    transform.position += moveDirection * this.moveSpeed * Time.deltaTime;

    // Rotate to face target smoothly
    transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);

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
    targetPosition = moveTarget;
    moving = true;
  }

  public void OnDetectEnemy(Transform enemy)
  {
    Debug.Log("Detected an enemey");
  }
}

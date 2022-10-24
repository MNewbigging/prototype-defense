using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  private Vector3 startPosition = new Vector3(-12, 0, -4);
  private Vector3 targetPosition = new Vector3(-12, 0, 4);

  private void Update()
  {
    // Reached target yet?
    float stoppingDistance = 0.1f;
    if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
    {
      // Not yet; keep moving
      MoveToTarget();
    }
    else
    {
      // Reached target
      ReachedTarget();
    }
  }

  private void MoveToTarget()
  {
    float moveSpeed = 2f;
    transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
  }

  private void ReachedTarget()
  {
    // Teleport back to start
    transform.position = startPosition;
  }
}

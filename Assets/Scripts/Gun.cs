using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
  [SerializeField] private int rpm = 60;

  private float rps = 1;
  private float bulletTimer = 0f;


  private void Awake()
  {
    // Work out rounds per second
    Debug.Log("rpm is " + rpm);
    rps = 60f / rpm;
    Debug.Log("rps is " + rps);
  }

  public bool TryShoot()
  {
    // Increment bullet timer
    bulletTimer += Time.deltaTime;

    // If the timer has reached the rps
    if (bulletTimer >= rps)
    {
      // Restart the bullet timer
      bulletTimer = 0f;

      // Can shoot
      return true;
    }

    // Cannot shoot yet
    return false;
  }
}

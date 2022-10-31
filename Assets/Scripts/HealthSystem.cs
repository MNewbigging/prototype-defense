using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
  public event EventHandler<Transform> OnDie;

  [SerializeField] private int health = 100;

  public void TakeDamage(int damageAmount)
  {
    health -= damageAmount;

    if (health <= 0)
    {
      health = 0;

      OnDie?.Invoke(this, transform);
    }
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
  public event EventHandler<Transform> OnDie;
  public event EventHandler OnDamage;

  [SerializeField] private int health = 100;
  private int maxHealth;

  private void Awake()
  {
    maxHealth = health;
  }

  public void TakeDamage(int damageAmount)
  {
    health -= damageAmount;

    OnDamage?.Invoke(this, EventArgs.Empty);

    if (health <= 0)
    {
      health = 0;

      OnDie?.Invoke(this, transform);
    }
  }

  public float GetHealthNormalised()
  {
    return (float)health / maxHealth;
  }
}

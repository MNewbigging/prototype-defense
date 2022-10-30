using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitAnimator : MonoBehaviour
{
  [SerializeField] private Animator animator;

  private void Awake()
  {
    if (TryGetComponent<Unit>(out Unit unit))
    {
      unit.OnStartMoving += Unit_OnStartMoving;
      unit.OnStopMoving += Unit_OnStopMoving;
    }

    if (TryGetComponent<Gun>(out Gun gun))
    {
      gun.OnShoot += Gun_OnShoot;
      gun.OnReload += Gun_OnReload;
    }
  }

  private void Unit_OnStartMoving(object sender, EventArgs e)
  {
    animator.SetBool("IsWalking", true);
  }

  private void Unit_OnStopMoving(object sender, EventArgs e)
  {
    animator.SetBool("IsWalking", false);
  }

  private void Gun_OnReload(object sender, EventArgs e)
  {
    animator.SetTrigger("Reload");
  }

  private void Gun_OnShoot(object sender, EventArgs e)
  {
    animator.SetTrigger("Shoot");
  }
}

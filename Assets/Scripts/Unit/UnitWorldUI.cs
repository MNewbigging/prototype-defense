using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UnitWorldUI : MonoBehaviour
{
  [SerializeField] private Image healthBarImage;
  [SerializeField] private HealthSystem healthSystem;

  private void Start()
  {
    healthSystem.OnDamage += HealthSystem_OnDamage;
  }

  private void HealthSystem_OnDamage(object sender, EventArgs e)
  {
    UpdateHealthBar();
  }

  private void UpdateHealthBar()
  {
    healthBarImage.fillAmount = healthSystem.GetHealthNormalised();
  }
}

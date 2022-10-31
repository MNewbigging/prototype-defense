using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitRagdollSpawner : MonoBehaviour
{
  [SerializeField] private Transform ragdollPrefab;

  private HealthSystem healthSystem;

  private void Awake()
  {
    healthSystem = GetComponent<HealthSystem>();

    healthSystem.OnDie += HealthSystem_OnDie;
  }

  private void HealthSystem_OnDie(object sender, Transform unit)
  {
    Instantiate(ragdollPrefab, transform.position, transform.rotation);
    Destroy(gameObject);
  }
}

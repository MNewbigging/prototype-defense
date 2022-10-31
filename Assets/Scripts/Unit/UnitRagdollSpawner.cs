using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitRagdollSpawner : MonoBehaviour
{
  [SerializeField] private Transform ragdollPrefab;
  [SerializeField] private Transform originalRootBone;

  private HealthSystem healthSystem;

  private void Awake()
  {
    healthSystem = GetComponent<HealthSystem>();
  }

  private void Start()
  {
    healthSystem.OnDie += HealthSystem_OnDie;
  }

  private void HealthSystem_OnDie(object sender, Transform unit)
  {
    Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);
    UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
    unitRagdoll.Setup(originalRootBone);

    Destroy(gameObject);
  }
}

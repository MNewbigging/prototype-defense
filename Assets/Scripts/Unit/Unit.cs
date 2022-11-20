using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pathfinding;

public class Unit : MonoBehaviour {
  // Unit action events
  public event EventHandler OnStartMoving;
  public event EventHandler OnStopMoving;

  // General
  [SerializeField] private MeshRenderer unitSelectedVisual;
  [SerializeField] private int team = 1;
  private HealthSystem healthSystem;


  // Movement
  private RichAI agent;
  private bool moving = false;


  private void Awake() {
    // Turn off selected visual by default
    unitSelectedVisual.enabled = false;

    healthSystem = GetComponent<HealthSystem>();
    agent = GetComponent<RichAI>();
  }

  private void FixedUpdate() {
    HandleMovement();
  }

  private void HandleMovement() {
    if (!moving) {
      return;
    }

    if (agent.reachedEndOfPath && !agent.pathPending) {
      StopMoving();
    }
  }

  public int GetTeam() {
    return team;
  }

  public void SetTeam(int team) {
    this.team = team;
  }

  public void OnSelect() {
    unitSelectedVisual.enabled = true;
  }

  public void OnDeselect() {
    unitSelectedVisual.enabled = false;
  }

  public void TakeDamage(int damageAmount) => healthSystem.TakeDamage(damageAmount);

  public void StartMoving() {
    moving = true;
    OnStartMoving?.Invoke(this, EventArgs.Empty);
  }

  public void StopMoving() {
    if (!moving) {
      return;
    }

    moving = false;
    OnStopMoving?.Invoke(this, EventArgs.Empty);
  }
}

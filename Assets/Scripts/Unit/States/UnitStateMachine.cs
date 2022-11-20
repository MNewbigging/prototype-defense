using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum UnitStateName {
  SEARCH,
  COMBAT
}

public abstract class UnitStateBase : MonoBehaviour {
  public abstract UnitStateName GetName();

  public virtual void OnEnter() { }
  public virtual void OnUpdate() { }
  public virtual void OnExit() { }

  public event EventHandler<UnitStateName> ChangeToState;

  protected Unit unit;

  private void Awake() {
    unit = GetComponent<Unit>();
  }
}

public class UnitStateMachine : MonoBehaviour {
  [SerializeField] private UnitStateName defaultState;

  private Dictionary<UnitStateName, UnitStateBase> states = new Dictionary<UnitStateName, UnitStateBase>();
  private UnitStateBase currentState;

  private void Start() {
    // Get the attached state scripts
    foreach (UnitStateBase unitStateBase in GetComponents<UnitStateBase>()) {
      // Setup callbacks to their change events
      unitStateBase.ChangeToState += OnChangeToState;

      // Store them in the dictionary by their names
      states.Add(unitStateBase.GetName(), unitStateBase);
    }

    // Start off on the default state
    ChangeState(defaultState);
  }

  private void OnChangeToState(object sender, UnitStateName name) {
    ChangeState(name);
  }

  private void ChangeState(UnitStateName name) {
    // Exit from the current state, if any
    if (currentState != null) {
      currentState.OnExit();
    }

    // Get the state from the map
    if (!states.TryGetValue(name, out UnitStateBase nextState)) {
      return;
    }

    // Assign the new state
    currentState = nextState;

    // Enter the new state
    currentState.OnEnter();
  }

  private void FixedUpdate() {
    currentState.OnUpdate();
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Responsible for reacting to mouse input
public class UnitController : MonoBehaviour
{

}


/*
    Input behaviour:

    - may want multiple units to be selected at once and give them commands
    - units may have common and unique commands


    Therefore, need a higher order class that:
    - holds a reference to all selected units
    - gathers minimum common actions available to all selected units

    It, or something else, also needs to:
    - manage the (de)selection of units; click, drag box, hotkey/cycle, bound group key


    So, the high-level concepts are:

    - Input: separate class(es) to listen for and respond to specific inputs

    - Selection: whether a unit is selected or not, which units are selected

    - Actions: behaviours resulting form user input that are undertaken by units
    -- A unit needs a list of actions it can do: at any time, at the current time/state

*/
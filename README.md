# State Machine for Unity

*Generic Finite State Machine that can be used for controlling enemies or other game elements that have some form of simple AI.*

## Concept

When you have a game object, usually an enemy, that goes through some states you can get cleaner code when each state is defined in it's own class (instead of a long switch statement).

Limitations:
- Each state knows what other states to go to
- There can only be one active state at one time

Try to keep functionality in the object to which the state applies.

## Overview

The FSM consists of the following files:
- **StateMachine.cs**; The base class for the state machine. Derive your actual state machine from this class.
- **State.cs**: The base class for the state. Derive your actual state implementation from this class.
- **IState.cs**; this is the interface for the state (internal use).
- **IStateMachine.cs**; this is the interface for the StateMachine (internal use).

## Usage / Example

To make use of the state machine you have to do the following:
- Create an enum with all states
- Create a class derived from StateMachine
- Create a class for each state, derived from State
- In your created StateMachine class, link the enums and the states together.

## Example: State Machine implementation


```C#
public class EnemyStateMachine: StateMachine<EnemyStates>
{
    /// <summary>
    /// Reference to your enemy game object.
    /// This is not required but usually handy to access functionality.
    /// </summary>
    [HideInInspector]
    public Enemy Enemy;

    /// <summary>
    /// Enum for enemy states.
    /// These will be linked to the actual State.
    /// Enums are used in the states to switch to an other state.
    /// </summary>
    [HideInInspector]
    public enum EnemyStates
    {
        Stop,
        Idle,
        ScanForTargets,
        Attack
    }

    void Start()
    {
        InitializeStates(Enemy);

        // Set the starting state.
        State = EnemyStates.Idle;
    }

    /// <summary>
    /// Link the states enum to the actual component where the state code is implemented.
    /// </summary>
    private void InitializeStates(Enemy enemy)
    {
        AddState(EnemyStates.Stop, gameObject.AddComponent<StopState>(), enemy);
        AddState(EnemyStates.Idle, gameObject.AddComponent<IdleState>(), enemy);
        AddState(EnemyStates.ScanForTargets, gameObject.AddComponent<ScanForTargetsState>(), enemy);
         AddState(EnemyStates.Attack, gameObject.AddComponent<AttackState>(), enemy);
    }

    private void AddState(EnemyStates eState, enemyState state, enemy enemy)
    {
        state.enemy = enemy;
        AddState(eState, state);
    }
}
```

## Example: State implementation

Create an abstract class for all states to derive from.

Switch to an other state by calling: `StateMachine.State = YourStateEnum.SomeState` 

```C#
public abstract class EnemyState : State<EnemyState>
{
    public Enemy Enemy;
}
```

Create the state code.
The `State` base object has the following methods:

- OnEnter: code that is executed when entering this state.
- OnExit: code that is executed when leaving the state.
- Update: called by the StateMachine in the Update method.

```C#
public class ScanForTargetsState : EnemyState
{
    /// <summary>
    /// State enter.
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();

        scanTime = 0;
        InvokeRepeating("ScanForTargets", scanDelay, scanDelay);
    }

    /// <summary>
    /// State exit.
    /// </summary>
    public override void OnExit()
    {
        CancelInvoke("ScanForTargets");

        base.OnExit();
    }

    /// <summary>
    /// Scan for targets to attack
    /// </summary>
    private void ScanForTargets()
    {
        if (Enemy.CanSeePlayer())
        {
            // Switch to the attack state.
            StateMachine.State = EnemyStates.Attack;
        }
        else
        {
            if (ScanTimeHasPassed())
            {
                // Nothing in seen after some time. Go to idle state.
                StateMachine.State = SentinelStates.Idle;
            }
        }
    }

    /// <summary>
    /// Check if the time has passed when nothing is found in the scan area.
    /// </summary>
    private bool ScanTimeHasPassed()
    {
        // Code to check for some timeout.
    }

    private void OnDestroy()
    {
        CancelInvoke("ScanForTargets");
    }
}
```
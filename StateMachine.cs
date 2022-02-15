using System;
using System.Collections.Generic;
using UnityEngine;

namespace PurpleFox.AI
{
    public abstract class StateMachine<TEnum>: MonoBehaviour, IStateMachine<TEnum>
    {
        private IState currentState;
        private TEnum state;

        protected Dictionary<TEnum, IState> States = new Dictionary<TEnum, IState>();

        public bool Enabled { get; set; }

        [HideInInspector]
        public TEnum State
        {
            set
            {
                state = value;
                currentState?.OnExit();
                currentState = States[value];
                currentState.OnEnter();
            }
            get
            {
                return state;
            }
        }

        public void Update()
        {
            currentState?.Update();
        }

        public void AddState(TEnum eState, State<TEnum> state)
        {
            state.StateMachine = this;
            States.Add(eState, state);
        }
    }
}

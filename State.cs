using UnityEngine;

namespace PurpleFox.AI
{
    public abstract class State<TEnum> : MonoBehaviour, IState
    {
        [HideInInspector]
        public TEnum t;

        [HideInInspector]
        public IStateMachine<TEnum> StateMachine;

        public virtual void OnEnter() { }

        public virtual void OnExit() { }

        public virtual void Update() { }
    }
}

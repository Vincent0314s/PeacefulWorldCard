using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIState
{
    public class EnemyStateMachine<T>
    {
        public State<T> currentState { get; private set; }
        public T owner;

        public EnemyStateMachine(T _owner) {
            owner = _owner;
            currentState = null;
        }

        public void ChangeState(State<T> _state) {
            if (currentState != null) {
                currentState.StateExit(owner);
            }
            currentState = _state;
            currentState.StateEnter(owner);
        }

        public void Update() {
            if (currentState != null) {
                currentState.StateUpdate(owner);
                //if (!currentState.Equals(RunforwardState.i) && !currentState.Equals(RunBackwardState.i)) {
                //    currentState.StateUpdate(owner);
                //}
            }
        }

        //public void FixedUpdate() {
        //    if (currentState != null) {
        //        if (currentState.Equals(RunforwardState.i) || currentState.Equals(RunBackwardState.i))
        //        {
        //            currentState.StateUpdate(owner);
        //        }
        //    }
        //}
    }

    [System.Serializable]
    public abstract class State<T> {
        public abstract void StateEnter(T _owner);
        public abstract void StateUpdate(T _owner);
        public abstract void StateExit(T _owner);
    }
}


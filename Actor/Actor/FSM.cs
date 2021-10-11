using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSM
{
    abstract class FSM_State<T>
    {
        protected List<FSM_Transformation<T>> transformations;
        public int ToStateCounts { get { return transformations == null ? 0 : transformations.Count; } }
        public FSM_Transformation<T> this[int index] { get { return transformations[index]; } }
        
        public void AddTransformation(FSM_Transformation<T> transformation)
        {
            if (transformations == null)
                transformations = new List<FSM_Transformation<T>>();
            transformations.Add(transformation);
        }

        public abstract void Enter(T owner);
        public abstract void Update(T owner);
        public abstract void Exit(T owner);
    }

    abstract class FSM_Transformation<T>
    {
        protected FSM_State<T> toState;
        public FSM_State<T> ToState { get { return toState; } }

        public FSM_Transformation(FSM_State<T> toState)
        {
            this.toState = toState;
        }

        public abstract bool CheckTransform(T owner);
    }

    class FSM_Manager<T>
    {
        protected T owner;
        protected FSM_State<T> curState;
        public FSM_Manager(T owner,FSM_State<T> originalState)
        {
            this.owner = owner;
            curState = originalState;
            curState.Enter(owner);
        }

        public void Update()
        {
            if (curState != null)
            {
                curState.Update(owner);
                for (int i = 0; i < curState.ToStateCounts; i++)
                {
                    if (curState[i].CheckTransform(owner))
                    {
                        curState.Exit(owner);
                        curState = curState[i].ToState;
                        curState.Enter(owner);
                        break;
                    }
                }
            }
        }
    }
}

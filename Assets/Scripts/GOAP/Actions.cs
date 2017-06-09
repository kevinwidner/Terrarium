using System;

using UnityEngine;

namespace GOAP
{
    public interface IAction
    {
        string Name { get; }
        bool IsPreconditionsMet();
    }

    public abstract class Action : IAction
    {
        protected Memory m_memory;

        public abstract string Name { get; }
        public abstract string PreCondition{ get; }
        public abstract string Effect{ get; }
        public abstract int Cost { get; }
        public abstract bool IsPreconditionsMet();
        public abstract bool TickAction();

        public Action(Memory memory)
        {
            m_memory = memory;
        }
    }

    public class Action_MoveTo : Action
    {
        public override string Name { get { return "MoveToTarget"; } }
        public override string PreCondition{ get { return "TargetExists"; } }
        public override string Effect{ get { return "TargetReached"; } }
        public override int Cost { get { return 10; } } //TODO: Calculate this.

        public Action_MoveTo(Memory memory) : base(memory){}

        public override bool IsPreconditionsMet()
        {
            return m_memory.TargetPosition != Vector3.zero;
        }

        public override bool TickAction()
        {
            //Do things.
            return false;
        }
    }
}


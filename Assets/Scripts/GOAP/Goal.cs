using System;

namespace GOAP
{
    public struct Goal
    {
        private string m_name;
        private GoalTarget m_target;
        private string m_desiredEffect;
        private int m_priority;

        public Goal(string name, GoalTarget target, string desiredEffect, int priority)
        {
            m_name = name;
            m_target = target;
            m_desiredEffect = desiredEffect;
            m_priority = priority;
        }

        public string Name { get { return m_name; } }

        public GoalTarget Target{ get { return m_target; } }

        public string DesiredEffect{ get { return m_desiredEffect; } }

        public int Priority { get { return m_priority; } }
    }


    public class GoalTarget
    {
        private string m_valueType;
        private object m_value;

        public GoalTarget(object value, string valueType)
        {
            m_value = value;
            m_valueType = valueType;
        }

        public string ValueType { get { return m_valueType; } }
        public object Value { get { return m_value; } }
    }
}


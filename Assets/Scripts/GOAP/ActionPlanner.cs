using System;
using System.Collections.Generic;

using UnityEngine;

namespace GOAP
{
    public class ActionPlanner : MonoBehaviour
    {
        public ActionPlanner()
        {
        }

        public ActionPlan Plan(Goal goal, Action[] actions)
        {
            // Iterate through passed in Actions to determine which path will lead to the passed in Goal.
            ActionPlan blah = new ActionPlan(goal.DesiredEffect, actions);
            return blah;
        }
    }

    public class ActionPlan
    {
        private List<Action> m_plan;
        private Action[] m_actions;
        private int m_totalCost;
        private bool m_valid = false;

        public List<Action> Plan { get { return m_plan; } }
        public int TotalCost { get { return m_totalCost; } }
        public bool Valid { get { return m_valid; } }

        public ActionPlan(string desiredEffect, Action[] actions)
        {
            m_actions = actions;
            m_plan = new List<Action>();

            Action[] goalActions = Array.FindAll(actions, a => a.Effect == desiredEffect);

            int minTotalCost = int.MaxValue;
            ActionPlan bestGoalActionPlan = null;
            foreach (Action goalAction in goalActions)
            {
                ActionPlan goalActionPlan = new ActionPlan(goalAction, actions);

                if (goalActionPlan.Valid && goalActionPlan.TotalCost < minTotalCost)
                {
                    bestGoalActionPlan = goalActionPlan;
                    minTotalCost = goalActionPlan.TotalCost;
                }
            }

            if (bestGoalActionPlan != null)
            {
                AddActionPlan(bestGoalActionPlan);
                m_valid = true;
            }
        }

        private ActionPlan(Action action, Action[] actions)
        {
            m_actions = actions;
            m_plan = new List<Action>();
            BuildOutPlan(action);
        }

        private void BuildOutPlan(Action action)
        {
            AddAction(action);

            if (action.IsPreconditionsMet())
            {
                m_valid = true;
            }
            else
            {
                Action[] preconditionActions = Array.FindAll<Action>(m_actions, a => a.Effect == action.PreCondition);

                int minTotalCost = int.MaxValue;
                ActionPlan bestActionPlan = null;

                foreach(Action preconditionAction in preconditionActions)
                {
                    ActionPlan proposedActionPlan = new ActionPlan(preconditionAction, m_actions);
                    if (proposedActionPlan.Valid && (proposedActionPlan.TotalCost <= minTotalCost))
                    {
                        bestActionPlan = proposedActionPlan;
                        minTotalCost = bestActionPlan.TotalCost;
                    }
                }

                if (bestActionPlan != null)
                {
                    AddActionPlan(bestActionPlan);
                    m_valid = true;
                }
            }
        }

        private void AddAction(Action action)
        {
            m_plan.Insert(0, action);
            m_totalCost += action.Cost;
        }

        private void AddActionPlan(ActionPlan plan)
        {
            m_plan.InsertRange(0, plan.m_plan);
            m_totalCost += plan.TotalCost;
        }
    }
}


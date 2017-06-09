using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GOAP
{
    public class GOAPTester : MonoBehaviour
    {
        List<Action> m_actions;
        List<Goal> m_goals;
        Goal goal = new Goal("Goal1", null, "Goal1_Happy", 100);

        Dictionary<String, String> m_memory;
        ActionPlanner m_actionPlanner;

        public GOAPTester()
        {
        }


        void Start () 
        {
            m_memory = new Dictionary<string, string>();
            m_actionPlanner = new ActionPlanner();

            goal = new Goal("Goal1", null, "Goal1_Happy", 100);

            m_goals = new List<Goal>();

            //               Action : Name : PreCondition : Effect : Cost
            m_actions = new List<Action>();
//            m_actions.Add(new Action("A1", "A1_Happy", "Goal1_Happy", 2));
//            m_actions.Add(new Action("A2", "A2_Happy", "Goal1_Happy", 2));
//            m_actions.Add(new Action("A3", "", "A1_Happy", 3));
//            m_actions.Add(new Action("A4", "A4_Happy", "A1_Happy", 2));
//            m_actions.Add(new Action("A5", "", "A2_Happy", 8));
//            m_actions.Add(new Action("A6", "A6_Happy", "A2_Happy", 1));
//            m_actions.Add(new Action("A7", "", "A4_Happy", 1));
//            m_actions.Add(new Action("A8", "", "A4_Happy", 4));
//            m_actions.Add(new Action("A9", "", "A6_Happy", 4));

        }

        void Update()
        {

//            ActionPlan myPlan = m_actionPlanner.Plan(goal, m_actions.ToArray());
//
//            foreach (Action action in myPlan.Plan)
//            {
//                Debug.Log(string.Format("Name: {0};  PreCondition: {1};   End-Effect: {2};    Cost: {3}", action.Name, action.PreCondition, action.Effect, action.Cost.ToString()));
//            }
//
//            Debug.Log(string.Format("BestPlan: {0} for a total cost of: {1}", myPlan.Plan[myPlan.Plan.Count - 1].Name, myPlan.TotalCost.ToString()));
        }
    }
}


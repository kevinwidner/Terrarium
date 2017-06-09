using System;
using System.Collections.Generic;

using UnityEngine;

namespace GOAP
{
    public class Memory
    {
        // Long-term Memory
        public float Health = 100;
        public float Speed = 10;
        public float RotationSpeed = 3.0f;
        public float Damage = 20;
        public float AttackCooldown = 3.0f;
        public float AttackRadius = 7.0f;
        public float MaxAngleForAttack = 20.0f;

        // Working Memory
        public bool Selected = false;
        public List<spider> VisibleInsects = new List<spider>();
        public spider TargetInsect;
        public Vector3 TargetPosition;
        public bool ReachedDestination;
        public spider AttackingInsect;
        public bool AttackReady = true;

        public Memory()
        {
        }
    }
}


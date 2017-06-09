using System;

namespace GOAP
{
    public class WorldContext
    {
        public bool isAlive = true;

        public const string CREATURE_IN_VIEW = "CREATUREINVIEW";
        public const string FOOD_IN_VIEW = "FOODINVIEW";
        public const string TARGET_ALIVE = "TARGETALIVE";
        public const string CREATURE_IN_DANGER_ZONE = "CREATUREINDANGERZONE";

        public WorldContext()
        {
        }
    }
}


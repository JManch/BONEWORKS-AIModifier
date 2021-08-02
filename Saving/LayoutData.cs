using UnityEngine;
using AIModifier.AI;

namespace AIModifier.Saving
{
    public class LayoutData
    {
        public string sceneName { get; set; }
        public AILayoutData[] ai { get; set; } 
    }

    public class AILayoutData
    {
        public string poolName { get; set; }
        public Vector3 position { get; set; }
        public Quaternion rotation { get; set; }
        public AIData aiData { get; set; }
        public bool selected { get; set; }
        public bool selectedTarget { get; set; }
    }

}

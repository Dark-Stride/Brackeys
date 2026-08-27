using UnityEngine;
using System.Collections.Generic;

namespace Scripts.Systems.Aggro_System
{
    public enum Faction
    {
        PlayerSide, // Player and Brother
        EnemySide   // Hostile mobs
    }

    public class AggroTarget : MonoBehaviour
    {
        [SerializeField] private Faction faction = Faction.EnemySide;
        [SerializeField] private float baseThreatPriority = 1f;

        public Faction EntityFaction => faction;
        public float BasePriority => baseThreatPriority;

        // Global list of active targets in the scene for fast AI lookups
        public static List<AggroTarget> ActiveTargets { get; private set; } = new();

        private void OnEnable()
        {
            if (!ActiveTargets.Contains(this))
            {
                ActiveTargets.Add(this);
            }
        }

        private void OnDisable()
        {
            ActiveTargets.Remove(this);
        }
    }
}

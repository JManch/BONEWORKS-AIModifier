using UnityEngine;
using StressLevelZero.AI;
using PuppetMasta;
using MelonLoader;
using System.Collections.Generic;
using AIModifier.AI;

namespace AIModifier.Utilities
{
    class Debugging
    {
        public static void DebugLocalAIBrains()
        {
            AIBrain[] aiBrains = GameObject.FindObjectsOfType<AIBrain>();
            foreach (AIBrain aiBrain in aiBrains)
            {
                MelonLogger.Msg(aiBrain.gameObject.name);
                MelonLogger.Msg("cur_hp is " + aiBrain.behaviour.health.cur_hp);
                MelonLogger.Msg("maxAppendageHp is " + aiBrain.behaviour.health.maxAppendageHp);
                MelonLogger.Msg("maxHitPoints is " + aiBrain.behaviour.health.maxHitPoints);
                MelonLogger.Msg("cur_arm_lf is " + aiBrain.behaviour.health.cur_arm_lf);
                MelonLogger.Msg("cur_arm_rt is " + aiBrain.behaviour.health.cur_arm_rt);
                MelonLogger.Msg("cur_leg_lf is " + aiBrain.behaviour.health.cur_leg_lf);
                MelonLogger.Msg("cur_leg_rt is " + aiBrain.behaviour.health.cur_leg_rt);
            }
        }

        public static void DebugAIData()
        {
            AIBrain[] aiBrains = GameObject.FindObjectsOfType<AIBrain>();
            BehaviourCrablet[] behaviourCrablets = GameObject.FindObjectsOfType<BehaviourCrablet>();

            MelonLogger.Msg(behaviourCrablets.Length);

            List<AIData> aiDatas = new List<AIData>();

            foreach (AIBrain aiBrain in aiBrains)
            {
                aiDatas.Add(AIDataManager.GenerateAIData(aiBrain));
            }

            XMLDataManager.SaveXMLData(aiDatas, @"\Mods\AIDataDebug.xml");
        }
    }
}

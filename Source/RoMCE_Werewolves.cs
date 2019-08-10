using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using Verse;
using Werewolf;

namespace RoMCE_Werewolves
{
    [StaticConstructorOnStartup]
    static class RoMCE_Werewolves
    {
        static RoMCE_Werewolves()
        {
            var harmony = HarmonyInstance.Create("deathrat.werewolves.patches");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.Message("RoMCE_Werewolves initialized.");
        }
    }

    [HarmonyPatch, HarmonyAfter(("rimworld.jecrell.werewolves"))]
    static class Patch_SpawnWolves
    {
        static MethodInfo TargetMethod()
        {
            return AccessTools.Method(typeof(CompWerewolf), "SpawnWolves");
        }

        private static IEnumerable<CodeInstruction> Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> instructions)
        {
            var list = instructions.ToList();
            var timberIndex = list.FirstIndexOf(c => c.opcode == OpCodes.Ldstr && c.operand.Equals("WolfTimber"));
            list[timberIndex].operand = "Wolf_Timber";
            var arcticIndex = list.FirstIndexOf(c => c.opcode == OpCodes.Ldstr && c.operand.Equals("WolfArctic"));
            list[arcticIndex].operand = "Wolf_Arctic";
            return list;
        }
    }
}
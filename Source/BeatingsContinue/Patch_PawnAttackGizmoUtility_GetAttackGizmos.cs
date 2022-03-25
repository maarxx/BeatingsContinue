using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace BeatingsContinue;

[HarmonyPatch(typeof(PawnAttackGizmoUtility))]
[HarmonyPatch("GetAttackGizmos")]
[HarmonyPatch(new[] { typeof(Pawn) })]
internal class Patch_PawnAttackGizmoUtility_GetAttackGizmos
{
    private static void Postfix(ref Pawn pawn, ref IEnumerable<Gizmo> __result)
    {
        //Log.Message("Hello from Harmony in Patch_PawnAttackGizmoUtility_GetAttackGizmos");
        __result = __result.ToList().AddItem(UnarmedAttack.GetGizmo(pawn));
        __result = __result.ToList().AddItem(BeatAttack.GetGizmo(pawn));
    }
}
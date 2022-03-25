using System.Reflection;
using HarmonyLib;
using Verse;

namespace BeatingsContinue;

[StaticConstructorOnStartup]
internal class Main
{
    static Main()
    {
        //Log.Message("Hello from Harmony in scope: com.github.harmony.rimworld.maarx.beatingscontinue");
        var harmony = new Harmony("com.github.harmony.rimworld.maarx.beatingscontinue");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
}

/*
[HarmonyPatch(typeof(Pawn))]
[HarmonyPatch("GetGizmos")]
class Patch_Pawn_GetGizmos
{
    static void Postfix(ref Pawn pawn, ref IEnumerable<Gizmo> __result)
    {
        __result = (__result.ToList()).AddItem(SuppressGizmo.GetGizmo(pawn));
    }
}
*/
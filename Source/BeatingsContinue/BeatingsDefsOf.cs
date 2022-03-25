using Verse;

namespace BeatingsContinue;

internal class BeatingsDefsOf
{
    public static DesignationDef designationDef => DefDatabase<DesignationDef>.GetNamed("Suppress");
    public static JobDef jobDefBeat => DefDatabase<JobDef>.GetNamed("BeatAttack");
    public static JobDef jobDefUnarmed => DefDatabase<JobDef>.GetNamed("UnarmedAttack");
}
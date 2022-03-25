using System.Linq;
using Verse;

namespace BeatingsContinue;

internal class BeatDecider
{
    public static bool shouldStopBeating(Pawn beater, Pawn beatee)
    {
        return shouldStopBeating(beater) || shouldStopBeating(beatee);
    }

    public static bool shouldStopBeating(Pawn pawn)
    {
        if (pawn.IsColonistPlayerControlled)
        {
            return hasEndangeredPart(pawn);
        }

        if (!pawn.IsPrisonerOfColony)
        {
            return true;
        }

        var recruitMode = pawn.guest.interactionMode.defName;
        if (recruitMode == "AttemptRecruit")
        {
            return hasEndangeredPart(pawn);
        }

        if (recruitMode == "ReduceResistance" || recruitMode == "NoInteraction")
        {
            return hasEndangeredPart(pawn, true);
        }

        return true;
    }

    private static bool hasEndangeredPart(Pawn p, bool onlyNecessary = false)
    {
        foreach (var bpr in p.health.hediffSet.GetInjuredParts().ToList().ListFullCopy())
        {
            if (!(p.health.hediffSet.GetPartHealth(bpr) < 11))
            {
                continue;
            }

            foreach (var h in p.health.hediffSet.hediffs.ListFullCopy())
            {
                if (h.Part != bpr || h.IsPermanent())
                {
                    continue;
                }

                if (onlyNecessary)
                {
                    if (p.health.WouldDieAfterAddingHediff(DefDatabase<HediffDef>.GetNamed("Bruise"), bpr, 11))
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
        }

        return false;
    }
}
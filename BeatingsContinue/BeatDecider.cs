using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BeatingsContinue
{
    class BeatDecider
    {
        public static bool shouldStopBeating(Pawn beater, Pawn beatee)
        {
            return hasEndangeredPart(beater) || hasEndangeredPart(beatee);
        }

        private static bool hasEndangeredPart(Pawn p)
        {
            foreach (BodyPartRecord bpr in p.health.hediffSet.GetInjuredParts())
            {
                if (p.health.hediffSet.GetPartHealth(bpr) < 10)
                {
                    foreach (Hediff h in p.health.hediffSet.hediffs)
                    {
                        if (h.Part == bpr && !h.IsPermanent())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}

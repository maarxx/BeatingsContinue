using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace BeatingsContinue
{
    class JobDriver_BeatAttack : JobDriver_AttackMelee
    {
        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_General.DoAtomic(delegate
            {
                Pawn pawn = job.targetA.Thing as Pawn;
                if (pawn != null && pawn.Downed && base.pawn.mindState.duty != null && base.pawn.mindState.duty.attackDownedIfStarving && base.pawn.Starving())
                {
                    job.killIncappedTarget = true;
                }
            });
            yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
            yield return Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, delegate
            {
                Thing thing = job.GetTarget(TargetIndex.A).Thing;
                Pawn p;
                if (job.reactingToMeleeThreat && (p = (thing as Pawn)) != null && !p.Awake())
                {
                    EndJobWith(JobCondition.InterruptForced);
                }
                InteractionUtility.TryGetRandomVerbForSocialFight(base.pawn, out Verb verb);
                if (shouldStopAttacking())
                {
                    EndJobWith(JobCondition.Succeeded);
                }
                else
                {
                    pawn.meleeVerbs.TryMeleeAttack(thing, verb);
                }
            }).FailOnDespawnedOrNull(TargetIndex.A);
        }

        protected bool shouldStopAttacking()
        {
            return hasEndangeredPart(base.pawn) || hasEndangeredPart(base.TargetA.Thing as Pawn);
        }

        private bool hasEndangeredPart(Pawn p)
        {
            foreach (BodyPartRecord bpr in p.health.hediffSet.GetInjuredParts())
            {
                if (p.health.hediffSet.GetPartHealth(bpr) < 10)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

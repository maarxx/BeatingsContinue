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
                if (true)
                {
                    job.killIncappedTarget = true;
                }
            });
            yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
            yield return Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, delegate
            {
                Thing thing = job.GetTarget(TargetIndex.A).Thing;
                if (BeatDecider.shouldStopBeating(base.pawn, base.TargetA.Thing as Pawn))
                {
                    EndJobWith(JobCondition.Succeeded);
                }
                InteractionUtility.TryGetRandomVerbForSocialFight(base.pawn, out Verb verb);
                pawn.meleeVerbs.TryMeleeAttack(thing, verb);

            }).FailOnDespawnedOrNull(TargetIndex.A);
        }
    }
}

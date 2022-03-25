using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace BeatingsContinue;

internal class JobDriver_BeatAttack : JobDriver_AttackMelee
{
    protected override IEnumerable<Toil> MakeNewToils()
    {
        yield return Toils_General.DoAtomic(delegate
        {
            var pawn = job.targetA.Thing as Pawn;
            //if (pawn != null && pawn.Downed && base.pawn.mindState.duty != null && base.pawn.mindState.duty.attackDownedIfStarving && base.pawn.Starving())
            //{
            job.killIncappedTarget = true;
            //}
        });
        yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
        yield return Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, delegate
        {
            var thing = job.GetTarget(TargetIndex.A).Thing;
            Pawn p;
            //if (job.reactingToMeleeThreat && (p = (thing as Pawn)) != null && !p.Awake())
            //{
            //    EndJobWith(JobCondition.InterruptForced);
            //}
            InteractionUtility.TryGetRandomVerbForSocialFight(pawn, out var verb);
            if (BeatDecider.shouldStopBeating(pawn, TargetA.Thing as Pawn))
            {
                EndJobWith(JobCondition.Succeeded);
            }
            else
            {
                pawn.meleeVerbs.TryMeleeAttack(thing, verb);
            }
        }).FailOnDespawnedOrNull(TargetIndex.A);
    }
}
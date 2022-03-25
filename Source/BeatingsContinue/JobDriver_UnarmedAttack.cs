using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace BeatingsContinue;

internal class JobDriver_UnarmedAttack : JobDriver_AttackMelee
{
    protected override IEnumerable<Toil> MakeNewToils()
    {
        yield return Toils_General.DoAtomic(delegate
        {
            if (job.targetA.Thing is Pawn { Downed: true } && pawn.mindState.duty is
                {
                    attackDownedIfStarving: true
                } && pawn.Starving())
            {
                job.killIncappedTarget = true;
            }
        });
        yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
        yield return Toils_Combat.FollowAndMeleeAttack(TargetIndex.A, delegate
        {
            var thing = job.GetTarget(TargetIndex.A).Thing;
            Pawn p;
            if (job.reactingToMeleeThreat && (p = thing as Pawn) != null && !p.Awake())
            {
                EndJobWith(JobCondition.InterruptForced);
            }

            InteractionUtility.TryGetRandomVerbForSocialFight(pawn, out var verb);
            pawn.meleeVerbs.TryMeleeAttack(thing, verb);
        }).FailOnDespawnedOrNull(TargetIndex.A);
    }
}
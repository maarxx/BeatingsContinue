using System;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace BeatingsContinue;

internal class BeatAttack
{
    private static string defaultLabel => "Beat";
    private static string defaultDesc => "Beatdown!";
    private static JobDef jobDef => BeatingsDefsOf.jobDefBeat;

    public static Gizmo GetGizmo(Pawn pawn)
    {
        var command_Target = new Command_Target
        {
            defaultLabel = defaultLabel,
            //command_Target.defaultLabel = "CommandMeleeAttack".Translate();
            defaultDesc = defaultDesc,
            //command_Target.defaultDesc = "CommandMeleeAttackDesc".Translate();
            targetingParams = TargetingParameters.ForAttackAny(),
            //command_Target.hotKey = KeyBindingDefOf.Misc2;
            icon = TexCommand.AttackMelee
        };
        if (GetAttackAction(pawn, LocalTargetInfo.Invalid, out var failStr) == null)
        {
            command_Target.Disable(failStr.CapitalizeFirst() + ".");
        }

        command_Target.action = delegate(LocalTargetInfo target)
        {
            foreach (var item in Find.Selector.SelectedObjects.Where(x =>
                         x is Pawn { IsColonistPlayerControlled: true }).Cast<Pawn>())
            {
                var unarmedAttackAction = GetAttackAction(item, target, out var failStr2);
                if (unarmedAttackAction != null)
                {
                    unarmedAttackAction();
                }
                else if (!failStr2.NullOrEmpty())
                {
                    Messages.Message(failStr2, target.Thing, MessageTypeDefOf.RejectInput, false);
                }
            }
        };
        return command_Target;
    }

    public static Action GetAttackAction(Pawn pawn, LocalTargetInfo target, out string failStr)
    {
        failStr = "";
        if (!pawn.IsColonistPlayerControlled)
        {
            failStr = "CannotOrderNonControlledLower".Translate();
        }
        else if (target.IsValid && !pawn.CanReach(target, PathEndMode.Touch, Danger.Deadly))
        {
            failStr = "NoPath".Translate();
        }
        else if (pawn.WorkTagIsDisabled(WorkTags.Violent))
        {
            failStr = "IsIncapableOfViolenceLower".Translate(pawn.LabelShort, pawn);
        }
        else if (pawn.meleeVerbs.TryGetMeleeVerb(target.Thing) == null)
        {
            failStr = "Incapable".Translate();
        }
        else if (pawn == target.Thing)
        {
            failStr = "CannotAttackSelf".Translate();
        }
        else
        {
            Pawn target2;
            if ((target2 = target.Thing as Pawn) == null ||
                !pawn.InSameExtraFaction(target2, ExtraFactionType.HomeFaction) &&
                !pawn.InSameExtraFaction(target2, ExtraFactionType.MiniFaction))
            {
                return delegate
                {
                    if (BeatDecider.shouldStopBeating(pawn, target2) ||
                        !InteractionUtility.TryGetRandomVerbForSocialFight(pawn, out _))
                    {
                        return;
                    }

                    var job = JobMaker.MakeJob(jobDef, target.Thing as Pawn);
                    //job.maxNumMeleeAttacks = 1;
                    job.killIncappedTarget = true;
                    var unused = target.Thing as Pawn;
                    //job.verbToUse = verb;
                    pawn.jobs.TryTakeOrderedJob(job);
                    //Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, target);
                    //pawn.jobs.TryTakeOrderedJob(job);
                };
            }

            failStr = "CannotAttackSameFactionMember".Translate();
        }

        failStr = failStr.CapitalizeFirst();
        return null;
    }
}
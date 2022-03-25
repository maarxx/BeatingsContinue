using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace BeatingsContinue;

public class Designator_Suppress : Designator
{
    public Designator_Suppress()
    {
        defaultLabel = "Suppress";
        defaultDesc = "Beat prisoners as much as possible, without permanent damage.";
        icon = ContentFinder<Texture2D>.Get("UI/Designators/Tame");
        soundDragSustain = SoundDefOf.Designate_DragStandard;
        soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
        useMouseIcon = true;
        soundSucceeded = SoundDefOf.Designate_Claim;
        //hotKey = KeyBindingDefOf.Misc11;
    }

    public override int DraggableDimensions => 2;

    //protected override DesignationDef Designation => DesignationDefOf.Strip;
    protected override DesignationDef Designation => BeatingsDefsOf.designationDef;

    public override AcceptanceReport CanDesignateCell(IntVec3 c)
    {
        if (!c.InBounds(Map))
        {
            return false;
        }

        if (!SuppressablesInCell(c).Any())
        {
            return "Must Designate Prisoners";
        }

        return true;
    }

    public override void DesignateSingleCell(IntVec3 c)
    {
        foreach (var item in SuppressablesInCell(c))
        {
            DesignateThing(item);
        }
    }

    public override AcceptanceReport CanDesignateThing(Thing t)
    {
        if (Map.designationManager.DesignationOn(t, Designation) != null)
        {
            return false;
        }

        if (t is Pawn { IsPrisoner: true })
        {
            return true;
        }

        return false;
    }

    public override void DesignateThing(Thing t)
    {
        Map.designationManager.AddDesignation(new Designation(t, Designation));
        //StrippableUtility.CheckSendStrippingImpactsGoodwillMessage(t);
    }

    private IEnumerable<Thing> SuppressablesInCell(IntVec3 c)
    {
        if (c.Fogged(Map))
        {
            yield break;
        }

        var thingList = c.GetThingList(Map);
        foreach (var suppressablesInCell in thingList)
        {
            if (CanDesignateThing(suppressablesInCell).Accepted)
            {
                yield return suppressablesInCell;
            }
        }
    }
}
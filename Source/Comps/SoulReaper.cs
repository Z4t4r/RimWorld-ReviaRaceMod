﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace ReviaRace.Comps
{
    public class SoulReaper : ThingComp
    {
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            var pawn = parent as Pawn;
            
            if (pawn != null && GetSoulReapTier() == -1)
            {
                // This should have at least tier 1 of soul reap.
                // Add it now.
                var rng = new Random();
                AddSoulReapTier(rng.Next(1,5));
            }
        }

        private Hediff SoulReapHediff => (parent as Pawn)?.health.hediffSet.hediffs
                                         .FirstOrDefault(hediff => hediff.def.defName.Contains("ReviaRaceSoulreapTier"));

        internal int GetSoulReapTier()
        {
            if (SoulReapHediff != null)
            {
                string tierAsString = SoulReapHediff.def.defName.Replace("ReviaRaceSoulreapTier", "");
                return int.TryParse(tierAsString, out int tier) ? tier : -1;
            }
            else
            {
                return -1;
            }
        }

        internal void AddSoulReapTier(int tier)
        {
            var pawn = parent as Pawn;
            if (tier < 0 || tier > 9 || 
                pawn == null)
            {
                return;
            }
           
            HediffDef toAdd = HediffDef.Named($"ReviaRaceSoulreapTier{tier}");
            pawn.health.AddHediff(toAdd);
        }

        internal void RemoveSoulReapHediffs()
        {
            var pawn = parent as Pawn;
            if (pawn == null)
            {
                return;
            }

            var hediffsToRemove = pawn.health.hediffSet.hediffs.Where(hediff => hediff.def.defName.StartsWith("ReviaRaceSoulreapTier")).ToList();
            foreach (var hediff in hediffsToRemove)
            {
                pawn.health.RemoveHediff(hediff);
            }
        }
    }
}

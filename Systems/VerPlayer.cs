using Microsoft.Xna.Framework;
using Newtonsoft.Json.Bson;
using ReLogic.Utilities;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.Net;
using vermage.Buffs;
using vermage.Items.Abstracts;
using vermage.Projectiles.Rapiers;
using vermage.Systems.Events;
using vermage.Systems.Utilities;

namespace vermage.Systems
{
    public class VerPlayer : ModPlayer
    {
        public float BlackMana = 0;
        public float WhiteMana = 0;
        public int MaxMana = 3;
        public int GetCombinedMana()
        {
            return (int)Math.Min(Math.Floor(BlackMana), Math.Floor(WhiteMana));
        }
        public void AddMana(ManaColor Type, float amount)
        {
            switch (Type)
            {
                case ManaColor.Black:
                    if (amount > 0) BlackMana += ManaGainRate.ApplyTo(amount);
                    else BlackMana += amount; break;
                case ManaColor.White:
                    if (amount > 0) WhiteMana += ManaGainRate.ApplyTo(amount);
                    else WhiteMana += amount; break;
                default:
                    if (amount > 0) BlackMana += ManaGainRate.ApplyTo(amount);
                    else BlackMana += amount;
                    if (amount > 0) WhiteMana += ManaGainRate.ApplyTo(amount);
                    else WhiteMana += amount; break;
            }
        }

        
        public string Slot1;
        public string Slot2;
        public int SelectedSlot = 1;
        public bool IsCasting;
        public bool WasCasting;
        public void CycleSlots()
        {
            if (SelectedSlot == 1) SelectedSlot = 2;
            else SelectedSlot = 1;
        }
        public SpellData? GetCurrentSpell()
        {
            if (SelectedSlot == 1)
            {
                if (GetSpellInSlot(1).HasValue) return GetSpellInSlot(1).Value;
                else if (GetSpellInSlot(2).HasValue) return GetSpellInSlot(2).Value;
                else return null;
            }
            else
            {
                if (GetSpellInSlot(2).HasValue) return GetSpellInSlot(2).Value;
                else if (GetSpellInSlot(1).HasValue) return GetSpellInSlot(1).Value;
                else return null;
            }
        }
        public SpellData? GetSpellInSlot(int Slot)
        {
            Slot = Utils.Clamp(Math.Abs(Slot), 1, 2);

            if (Slot == 1)
            {
                if (string.IsNullOrEmpty(Slot1)) return null;
                else if (vermage.Spells.TryGetValue(Slot1, out SpellData value)) return value;
                else return null;
            }
            else
            {
                if (string.IsNullOrEmpty(Slot2)) return null;
                else if (vermage.Spells.TryGetValue(Slot2, out SpellData value)) return value;
                else return null;
            }
        }


        public StatModifier ManaGainRate = new(1f, 1f);
        public StatModifier CastingSpeed = new(1f, Main.frameRate);

        public static ModKeybind ToggleSpellbook;
        public static ModKeybind SwapSpells;

        private SlotId? CastingSFXSlot;

        public List<IVerEvent> Events = new();

        public Vector2? FocusPosition;
        public RapierData? Rapier { get
            {
                if (Player.HeldItem.ModItem is BaseRapier)
                {
                    return (Player.HeldItem.ModItem as BaseRapier).RapierData;
                }
                return null;
            }
        }
        public Behavior RapierBehavior = Behavior.Idle;
        public int BehaviorFrames = 0;
        public int CastingTimer = 0;
        public Dictionary<string, DateTime> LastRapierUsage = new();
        public bool LungeTechnique = false;

        public override void PreUpdate()
        {
            if (BlackMana > MaxMana) BlackMana = MaxMana;
            if (WhiteMana > MaxMana) WhiteMana = MaxMana;
            if (BlackMana < 0) BlackMana = 0;
            if (WhiteMana < 0) WhiteMana = 0;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (SwapSpells.JustPressed)
            {

            }

            if (IsCasting && Main.mouseRight)
            {
                var spellData = GetCurrentSpell();
                if (spellData.HasValue)
                {
                    HandleCasting();
                }
            }
            else if (IsCasting && !Main.mouseRight)
            {
                IsCasting = false;
                CastingTimer = 0;
                RapierBehavior = Behavior.Idle;
                BehaviorFrames = 0;
            }

        }
        public void HandleCasting()
        {
            if (CastingTimer == 0)
            {
                SoundEngine.PlaySound(new SoundStyle("vermage/Assets/Sounds/Latch"));
            }
            else if (CastingTimer > BehaviorFrames)
            {
                IsCasting = false;
                RapierBehavior = Behavior.Idle;
                BehaviorFrames = 0;
                CastingTimer = 0;
            }
            CastingTimer++;
        }

        public override void ResetEffects()
        {
            if (!Rapier.HasValue)
            {
                IsCasting = false;
                RapierBehavior = Behavior.Idle;
                CastingTimer = 0;
                BehaviorFrames = 0;
            }
            else if (Rapier.HasValue)
            {
                if (RapierBehavior == Behavior.Casting || IsCasting)
                {
                    if ((DateTime.Now - LastRapierUsage[Rapier.Value.FullName]).TotalSeconds > 0.5)
                    {
                        IsCasting = false;
                        RapierBehavior = Behavior.Idle;
                        CastingTimer = 0;
                        BehaviorFrames = 0;
                    }
                }
            }
            
            LungeTechnique = false;
            MaxMana = 3;
            ManaGainRate = new(1, 1);
            CastingSpeed = new(1, 1);

        }

        public void ProcessOnCast(SpellData spell)
        {
            foreach (OnCastSpell e in Events.Where(x=> x is OnCastSpell))
            {
                e.OnCast(Player, spell);
            }
        }
        public void ProcessOnHitWithSpell(Projectile projectile, NPC target, NPC.HitInfo hitInfo, int damage)
        {
            foreach (OnHitWithSpell e in Events.Where(x => x is OnHitWithSpell))
            {
                e.OnHitNPC(projectile, target, hitInfo, damage);
            }
        }
        public void ProcessOnHitWithSpell(Projectile projectile, Player target, Player.HurtInfo hitInfo, int damage)
        {
            foreach (OnHitWithSpell e in Events.Where(x => x is OnHitWithSpell))
            {
                e.OnHitPlayer(projectile, target, hitInfo, damage);
            }
        }
        public void ProcessOnHitWithRapier(Projectile projectile, NPC target, NPC.HitInfo hitInfo, int damage)
        {
            foreach (OnHitWithRapier e in Events.Where(x=>x is OnHitWithRapier))
            {
                e.OnHitNPC(projectile, target, hitInfo, damage);
            }
        }
        public void ProcessOnHitWithRapier(Projectile projectile, Player target, Player.HurtInfo hitInfo, int damage)
        {
            foreach (OnHitWithRapier e in Events.Where(x => x is OnHitWithRapier))
            {
                e.OnHitPlayer(projectile, target, hitInfo, damage);
            }
        }
    }

    public enum ManaColor
    {
        Black = 0,
        White = 1,
        Red = 2
    }
}

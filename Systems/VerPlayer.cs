using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
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
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Net;
using vermage.Buffs;
using vermage.Items.Abstracts;
using vermage.Items.Materia;
using vermage.Projectiles.Rapiers;
using vermage.Systems.Events;
using vermage.Systems.Handlers;
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
            if (amount > 0 && Type == ManaColor.Red && RefundRedMana()) return;

            switch (Type)
            {
                case ManaColor.Black:
                    BlackMana += amount > 0 ? BlackManaGainRate.ApplyTo(amount) : amount;
                    WhiteMana += amount > 0 ? WhiteManaGainRate.ApplyTo(Math.Min(amount, ManaConversionRate.ApplyTo(amount))) : 0;
                    break;  
                case ManaColor.White:
                    WhiteMana += amount > 0 ? WhiteManaGainRate.ApplyTo(amount) : amount;
                    BlackMana += amount > 0 ? BlackManaGainRate.ApplyTo(Math.Min(amount, ManaConversionRate.ApplyTo(amount))) : 0;
                    break;
                default:
                    WhiteMana += amount > 0 ? WhiteManaGainRate.ApplyTo(amount) : amount;
                    BlackMana += amount > 0 ? BlackManaGainRate.ApplyTo(amount) : amount;
                    break;
            }
        }
        public bool RefundRedMana()
        {
            return Main.rand.NextFloat(0f, 1f) <= manaRefundChance;
        }

        public Dictionary<string, bool> UnlockedSpells = new();


        public string Slot1;
        public string Slot2;
        public int SelectedSlot = 1;
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
        public bool HasSpellUnloked(string ID)
        {
            if (UnlockedSpells.TryGetValue(ID, out bool value)) return value;
            else return false;
        }
        public void UnlockSpell(string id)
        {
            if (UnlockedSpells.ContainsKey(id))
            {
                UnlockedSpells[id] = true;
            }
        }


        public StatModifier BlackManaGainRate = new(1f, 1f);
        public StatModifier WhiteManaGainRate = new(1f, 1f);
        public StatModifier ManaConversionRate = new(0f, 1f);
        public StatModifier CastingSpeed = new(1f, Main.frameRate);
        public StatModifier BuffDuration = new(1f, 1f);
        public float manaRefundChance = 0f;
        public float doubleFinisherchance = 0f;

        public static ModKeybind ToggleSpellbook;
        public static ModKeybind SwapSpells;

        private SlotId? CastingSFXSlot;

        public List<IVerEvent> Events = new();

        public Vector2? FocusPosition;
        public RapierData? RapierData => Rapier?.RapierData ?? null;
        public BaseRapier Rapier
        {
            get
            {
                if (Player.HeldItem.ModItem is BaseRapier)
                {
                    return (Player.HeldItem.ModItem as BaseRapier);
                }
                return null;
            }
        }
        public Behavior RapierBehavior = Behavior.Idle;
        public SpellData? CastingSpell = null;
        public int CastingTimer = 0;
        public DateTime? LastRapierUsage = null;
        public bool LungeTechnique = false;
        
        public List<MateriaColor> MaterialColors = new();
        public bool UnlockedMateriaSlot2;
        public bool UnlockedMateriaSlot3;

        public Vector2 CursorPosition;

        public override void LoadData(TagCompound tag)
        {
            foreach(var sp in vermage.Spells)
            {
                if (tag.ContainsKey($"{sp.Key}/unlock")) UnlockedSpells.Add(sp.Key, tag.GetBool($"{sp.Key}/unlock"));
                else UnlockedSpells.Add(sp.Key, false);
            }

            if (tag.ContainsKey($"{Mod.Name}/Slot1")) Slot1 = tag.GetString($"{Mod.Name}/Slot1");
            if (tag.ContainsKey($"{Mod.Name}/Slot2")) Slot2 = tag.GetString($"{Mod.Name}/Slot2");
            if (tag.ContainsKey($"{Mod.Name}/SelectedSlot")) SelectedSlot = tag.GetAsInt($"{Mod.Name}/SelectedSlot");
            if (tag.ContainsKey($"{Mod.Name}/UnlockedMateriaSlot2")) UnlockedMateriaSlot2 = tag.GetBool($"{Mod.Name}/UnlockedMateriaSlot2");
            if (tag.ContainsKey($"{Mod.Name}/UnlockedMateriaSlot3")) UnlockedMateriaSlot3 = tag.GetBool($"{Mod.Name}/UnlockedMateriaSlot3");
        }
        public override void SaveData(TagCompound tag)
        {
            foreach (var sp in UnlockedSpells)
            {
                tag.Add($"{sp.Key}/unlock", sp.Value);
            }
            tag.Add($"{Mod.Name}/Slot1", Slot1);
            tag.Add($"{Mod.Name}/Slot2", Slot2);
            tag.Add($"{Mod.Name}/SelectedSlot", SelectedSlot);
            tag.Add($"{Mod.Name}/UnlockedMateriaSlot2", UnlockedMateriaSlot2);
            tag.Add($"{Mod.Name}/UnlockedMateriaSlot3", UnlockedMateriaSlot3);
        }

        public override void PreUpdate()
        {
            if (BlackMana > MaxMana) BlackMana = MaxMana;
            if (WhiteMana > MaxMana) WhiteMana = MaxMana;
            if (BlackMana < 0) BlackMana = 0;
            if (WhiteMana < 0) WhiteMana = 0;

            if (!Player.ItemTimeIsZero && Rapier != null)
            {
                LastRapierUsage = DateTime.Now;
            }
        }
        
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (SwapSpells.JustPressed)
            {
                CycleSlots();
            }

            if (RapierData != null && GetCurrentSpell().HasValue)
            {
                HandleCasting();
            }

            CursorPosition = Main.MouseWorld;

            if (Main.netMode == NetmodeID.MultiplayerClient) vermage.Instance.ShareCursorData(CursorPosition, Player.whoAmI);
        }
        public void HandleCasting()
        {
            if (Player.ItemTimeIsZero && !CastingSpell.HasValue && GetCurrentSpell().HasValue && RapierBehavior == Behavior.Idle && MouseRightCurrent()) // If MouseRight was JUST pressed and the player is not casting
            {
                if (Player.CheckMana(GetCurrentSpell().Value.GetManaCost(Player, Rapier)))
                {
                    WasCasting = true;
                    RapierBehavior = Behavior.Casting;
                    LastRapierUsage = DateTime.Now;
                    CastingSpell = GetCurrentSpell();
                    CastingSFXSlot = SoundEngine.PlaySound(new SoundStyle("vermage/Assets/Sounds/Latch"), Player.Center);
                }
                Player.SetItemTime(10);
            }
            else if (CastingSpell.HasValue && MouseRightCurrent()) // If the player is casting && mouseRight is currently being held
            {
                int castingTime = CastingSpell.Value.GetCastingFrames(Player);
                // If the timer is equal or higher than the casting frames. 
                // Then cast the spell.
                if (CastingTimer >= castingTime) 
                {
                    if (Player.CheckMana(GetCurrentSpell().Value.GetManaCost(Player, Rapier), true)) 
                    {
                        CastSpell(Rapier, CastingSpell.Value);
                        Player.SetItemTime(10); // IMPORTANT! This adds a 10-frame cooldown between repeated castings of spells.
                        RapierBehavior = Behavior.Idle;
                        CastingTimer = 0;
                        CastingSpell = null;
                    }
                }
                else
                {
                    // IMPORTANT! This adds a 10-frame cooldown between your last casting tick and your next spell being casted.
                    // If the user was not holding down RMB this frame, they should still have 9 frames of cooldown left 
                    // Which will prevent them from casting by mashing RMB.
                    Player.SetItemTime(10); 
                    LastRapierUsage = DateTime.Now;
                    CastingTimer++;
                }
            }
            else // If the player is neither casting, nor holding mouse right, reset casting state.
            {
                if (CastingSFXSlot.HasValue) // If there is a tracked casting sound effect 
                {
                    if (SoundEngine.TryGetActiveSound(CastingSFXSlot.Value, out ActiveSound sound)) // And we can pull the ActiveSound out of that tracked sould
                    {
                        sound.Stop(); // Then stop the sound effect.
                    }
                }
                CastingSpell = null;
                CastingTimer = 0;
            }
        }

        public override void ResetEffects()
        {
            if (RapierData == null)
            {
                RapierBehavior = Behavior.Idle;
                CastingTimer = 0;
                CastingSpell = null;
                LastRapierUsage = null;
            }
            else
            {
                if (LastRapierUsage.HasValue)
                {
                    // If the user is current casting a spell
                    // AND it's been 1/2 a second since the LastRapierUsage has been update
                    // Then clear all spell data and return to idle behavior
                    if ((RapierBehavior == Behavior.Casting || CastingSpell.HasValue) && (DateTime.Now - LastRapierUsage.Value).TotalSeconds > 0.5f)
                    {
                        CastingSpell = null;
                        RapierBehavior = Behavior.Idle;
                        CastingTimer = 0;
                        LastRapierUsage = null;
                    }

                    // If the user is in Swing or Thrust behavior
                    // AND it's been 8/10ths of a second since the LastRapierUsage has been updated
                    // Then end the combo and return to idle
                    if ((RapierBehavior == Behavior.Swing || RapierBehavior == Behavior.Thrust) && (DateTime.Now - LastRapierUsage.Value).TotalSeconds > 0.8f)
                    {
                        RapierBehavior = Behavior.Idle;
                        LastRapierUsage = null;
                    }
                }
                else
                {
                    RapierBehavior = Behavior.Idle;
                    CastingTimer = 0;
                    CastingSpell = null;
                    LastRapierUsage = null;
                }
            }
            
            LungeTechnique = false;
            MaxMana = 3;
            BlackManaGainRate = new(1f, 1f);
            WhiteManaGainRate = new(1f, 1f);
            CastingSpeed = new(1f, Main.frameRate);
            ManaConversionRate = new(0f, 1f);
            Events = new();
            MaterialColors = new();
            manaRefundChance = 0f;
            doubleFinisherchance = 0f;
            BuffDuration = new(1f, 1f);
        }



        private bool MouseRightJustPressed() => Main.mouseRight && Main.mouseRightRelease;
        private bool MouseRightCurrent() => Main.mouseRight && !Main.mouseRightRelease;
        public void CastSpell(BaseRapier rapier, SpellData spell)
        {
            int damage = (int)Player.GetTotalDamage<VermilionDamageClass>().ApplyTo(rapier.Item.damage);
            float knockback = Player.GetTotalKnockback<VermilionDamageClass>().ApplyTo(rapier.Item.knockBack);
            Vector2 direction = Player.DirectionTo(Main.MouseWorld);
            direction.Normalize();

            Projectile.NewProjectileDirect(Player.GetSource_ItemUse(rapier.Item), FocusPosition ?? Player.Center, direction * spell.Velocity, spell.ProjectileType, damage, knockback, Player.whoAmI);
            ProcessOnCast(spell);
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

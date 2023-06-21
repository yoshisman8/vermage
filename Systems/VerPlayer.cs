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
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.ModLoader;
using vermage.Items.Foci;
using vermage.Items.Rapiers;

namespace vermage.Systems
{
    public class VerPlayer : ModPlayer
    {
        public float BlackMana = 0;
        public float WhiteMana = 0;

        public float BaseManaGain = 0.25f;

        private DateTime? LastManaGain;

        public int MaxMana = 3;

        public float FociFramesLeft = 0;
        public int AttackFramesLeft = 0;
        public (int Total, int Left) CastFrames = (0,0);
        public int ComboState = 0;

        public bool IsMageStance = false;
        public bool IsCasting = false;

        public int? Focus = null;
        public int? Rapier = null;

        public Vector2? FocusPos;

        public static ModKeybind ActionKey;
        public static ModKeybind QuickFociToggle;

        private SlotId? CastingSlot;

        public int GetCombinedMana()
        {
            return (int)Math.Min(Math.Floor(BlackMana), Math.Floor(WhiteMana));
        }
        public void RemoveAllFoci()
        {
            
            foreach (var focus in vermage.Instance.GetContent<BaseFoci>())
            {
                if (Player.HasBuff(focus.Item.buffType))
                {
                    int index = Player.FindBuffIndex(focus.Item.buffType);
                    Player.DelBuff(index);
                }
            }
        }

        public override void PreUpdate()
        {
            if (BlackMana > MaxMana) BlackMana = MaxMana;
            if (WhiteMana > MaxMana) WhiteMana = MaxMana;
            if (BlackMana < 0) BlackMana = 0;
            if (WhiteMana < 0) WhiteMana = 0;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (ActionKey.JustPressed)
            {
                if (Focus.HasValue)
                {
                    BaseFoci foci = ModContent.GetModItem(Focus.Value) as BaseFoci;

                    if (GetCombinedMana() >= foci.ActivationCost)
                    {
                        BlackMana -= foci.ActivationCost;
                        WhiteMana -= foci.ActivationCost;
                        foci.Activate(Player);
                    }
                }
            }
            if (QuickFociToggle.JustPressed)
            {
                CycleFoci();
            }

            HandleCasting();
        }
        public void HandleCasting()
        {
            // Check if the player has a rapier and is in mage stance
            if (Rapier.HasValue && IsMageStance && Main.mouseLeft)
            {
                // If Mouse 1 was just pressed and there is no CastFrame data
                if (Main.mouseLeft && CastFrames == (0, 0))
                {
                    
                    if (Rapier.HasValue)
                    {
                        BaseRapier r = ModContent.GetModItem(Rapier.Value) as BaseRapier;
                        var frames = r.CastTime;

                        if (frames > 0)
                        {
                            if (SoundEngine.TryGetActiveSound(CastingSlot ?? SlotId.Invalid, out ActiveSound result))
                            {
                                result.Stop();
                            }
                            CastingSlot = SoundEngine.PlaySound(new SoundStyle("vermage/Assets/Sounds/Latch"), Player.position);
                            CastFrames = (frames, frames);
                            IsCasting = true;
                        }
                    }
                    
                }
                // If Mouse 1 is being held & there's cast frames left to go
                else if (Main.mouseLeft && !Main.mouseLeftRelease && CastFrames.Left > 0)
                {
                    CastFrames.Left--; // Lower casting frames by 1
                    IsCasting = true; // Keep IsCasting to true
                }
                else if (Main.mouseLeft && !Main.mouseLeftRelease && CastFrames.Left == 0)
                {
                    IsCasting = true;
                    Player.itemTime = 0;
                    Player.itemAnimation = 0;
                    CastFrames.Left--;
                }
                else if (Main.mouseLeft && !Main.mouseLeftRelease && CastFrames.Left < 1)
                {
                    CastFrames = (0, 0);
                    IsCasting = false;
                    if (SoundEngine.TryGetActiveSound(CastingSlot ?? SlotId.Invalid, out ActiveSound result))
                    {
                        result.Stop();
                    }
                }
                else
                {
                    CastFrames = (0, 0);
                    IsCasting = false;
                    if (SoundEngine.TryGetActiveSound(CastingSlot ?? SlotId.Invalid, out ActiveSound result))
                    {
                        result.Stop();
                    }
                }
            }
            else
            {
                CastFrames = (0, 0);
                IsCasting = false;
                if (SoundEngine.TryGetActiveSound(CastingSlot ?? SlotId.Invalid, out ActiveSound result))
                {
                    result.Stop();
                }
            }
        }
        public void CycleFoci()
        {
            List<BaseFoci> Foci = new List<BaseFoci>();

            for (int i = Main.InventoryItemSlotsStart; i < 9; i++)
            {
                if (Player.inventory[i].ModItem is BaseFoci foci)
                {
                    Foci.Add(foci);
                }
            }
            
            if (Foci.Count == 0) return;

            if (Focus.HasValue)
            {
                if (Foci.Count == 1) return;

                int index = Foci.FindIndex(x => x.Type == Focus.Value);

                if (index >= Foci.Count - 1)
                {
                    var f = Foci.First();
                    //CombatText.NewText(Player.Hitbox, ItemRarity.GetColor(f.Item.rare), f.DisplayName.GetDefault());
                    f.Toggle(Player);
                }
                else
                {
                    var f = Foci[index + 1];
                    //CombatText.NewText(Player.Hitbox, ItemRarity.GetColor(f.Item.rare), f.DisplayName.GetDefault());
                    f.Toggle(Player);
                }
            }
            else
            {
                var f = Foci.First();
                //CombatText.NewText(Player.Hitbox, ItemRarity.GetColor(f.Item.rare), f.DisplayName.GetDefault());
                f.Toggle(Player);
            }
        }
        public void AddMana(ManaColor Type, float amount)
        {
            if (FociFramesLeft > 0) return;

            switch(Type)
            {
                case ManaColor.Black:
                    BlackMana += amount; break;
                case ManaColor.White:
                    WhiteMana += amount; break;
                default:
                    BlackMana += amount;
                    WhiteMana += amount;
                    break;
            }
            LastManaGain = DateTime.Now;
        }
        public void HandleManaDecline()
        {
            if (LastManaGain.HasValue)
            {
                if((DateTime.Now - LastManaGain.Value).Seconds >= 5)
                {
                    AddMana(ManaColor.Both, 0.01f);
                }
            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (AttackFramesLeft > 0) AttackFramesLeft--;
            if (FociFramesLeft > 0) FociFramesLeft--;

            if (AttackFramesLeft < 0) AttackFramesLeft = 0;
            if (FociFramesLeft < 0) FociFramesLeft = 0;

            HandleManaDecline();
        }
        public override void ResetEffects()
        {
            Rapier = null;
            if (!Focus.HasValue)
            {
                FocusPos = null;
            }
            MaxMana = 3;
            BaseManaGain = 0.25f;
        }
    }

    public enum ManaColor
    {
        Black = 0,
        White = 1,
        Both = 2
    }
}

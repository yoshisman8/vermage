using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace vermage.Systems.ModSupport
{
    public static class KaiokenSupport
    {
        public static Mod KaiokenMod;
        public static KPlayer_Data FetchKaiokenData(Player player)
        {
            var ptr = (IntPtr)KaiokenMod.Call(true, player.whoAmI);
            var data = Marshal.PtrToStructure<KPlayer_Data>(ptr);
            return data;
        }
        public static void SendKaiokenData(Player player, KPlayer_Data data)
        {
            KaiokenMod.Call(false, player.whoAmI, data);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct KPlayer_Data
    {
        private float _mastery = float.Epsilon;
        private double _strain = double.Epsilon;
        private bool _formed = false;

        public KPlayer_Data()
        {
        }

        public float Mastery
        {
            readonly get => _mastery;
            internal set => _mastery = value;
        }

        public double Strain
        {
            readonly get => _strain;
            internal set => _strain = value;
        }

        public bool Formed
        {
            readonly get => _formed;
            internal set => _formed = value;
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using vermage.Systems;

namespace vermage
{
    public partial class vermage
    {
        internal enum MessageType : byte
        {
            SendCursordata,
            ReceiveCursorData
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType msgType = (MessageType)reader.ReadByte();

            switch (msgType)
            {
                case MessageType.ReceiveCursorData:
                    float x = reader.ReadSingle();
                    float y = reader.ReadSingle();
                    int player = reader.ReadInt32();
                    ReceiveCursorData(x, y, player, whoAmI);
                    break;
                case MessageType.SendCursordata:
                    float x2 = reader.ReadSingle();
                    float y2 = reader.ReadSingle();
                    SendCursorData(x2, y2, whoAmI);
                    break;
                default:
                    Logger.WarnFormat("ExampleMod: Unknown Message type: {0}", msgType);
                    break;
            }
        }
        public void ShareCursorData(Vector2 data, int whoAmI)
        {
            ModPacket packet = Instance.GetPacket();

            packet.Write(data.X);
            packet.Write(data.Y);

            packet.Send(256);
        }
        internal void ReceiveCursorData(float x, float y, int player, int fromWho)
        {
            // Only as an emergency defense, really.
            if (Main.netMode == NetmodeID.Server) return;
            if (fromWho == player) return; 

            Player pl = Main.player[player];
            pl.GetModPlayer<VerPlayer>().CursorPosition = new(x, y);
        }

        internal void SendCursorData(float x, float y, int player)
        {
            Player pl = Main.player[player];
            pl.GetModPlayer<VerPlayer>().CursorPosition = new(x, y);

            ModPacket packet = GetPacket();

            packet.Write((byte)MessageType.ReceiveCursorData);
            packet.Write(x);
            packet.Write(y);
            packet.Write(player);

            packet.Send(-1, player);
        }
    }
}

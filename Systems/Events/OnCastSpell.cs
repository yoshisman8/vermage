using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using vermage.Items.Abstracts;
using vermage.Systems.Utilities;

namespace vermage.Systems.Events
{
    public class OnCastSpell : IVerEvent
    {
        public string SourceID { get; set; }
        public Action<Player, SpellData> OnCast { get; set; }
        public OnCastSpell(string sourceID, Action<Player, SpellData> onCast)
        {
            SourceID = sourceID;
            OnCast = onCast;
        }
    }
}

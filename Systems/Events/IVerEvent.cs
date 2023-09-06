using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace vermage.Systems.Events
{
    public interface IVerEvent
    {
        public string SourceID { get; set; }
    }
}

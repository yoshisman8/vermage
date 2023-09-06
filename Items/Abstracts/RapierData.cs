using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vermage.Items.Abstracts
{
    public struct RapierData
    {
        public string FullName { get; set; }
        public int ItemType { get; set; }
        public int UseTime { get; set; }
        public int Damage { get; set; }
        public float Knockback { get; set; }
        public int RapierProjectile { get; set; }
        public int FocusProjectile { get; set; }
        public int GuardProjectile { get; set; }
    }
}

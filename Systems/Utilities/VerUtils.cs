using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace vermage.Systems.Utilities
{
    public static class VerUtils
    {

        /// <summary>
        /// Returns the closest NPC from a given location and within a search distance.
        /// </summary>
        /// <param name="Location">Starting point of the search.</param>
        /// <param name="maxDetectDistance">Distance in Pixels.</param>
        /// <returns>Closest NPC or Null if none is found.</returns>
        public static NPC FindClosestNPC(Vector2 Location, float maxDetectDistance)
        {
            NPC closestNPC = null;

            // Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            // Loop through all NPCs(max always 200)
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC target = Main.npc[k];
                // Check if NPC able to be targeted. It means that NPC is
                // 1. active (alive)
                // 2. chaseable (e.g. not a cultist archer)
                // 3. max life bigger than 5 (e.g. not a critter)
                // 4. can take damage (e.g. moonlord core after all it's parts are downed)
                // 5. hostile (!friendly)
                // 6. not immortal (e.g. not a target dummy)
                if (target.CanBeChasedBy())
                {
                    // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Location);

                    // Check if it is within the radius
                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closestNPC = target;
                    }
                }
            }

            return closestNPC;
        }

        /// <summary>
        /// Returns all nearby NPCs from a given location and within a search distance.
        /// </summary>
        /// <param name="Location">Starting point of the search.</param>
        /// <param name="maxDetectDistance">Distance in Pixels.</param>
        /// <returns>List of NPCs within the given radius.</returns>
        public static NPC[] FindAllNearbyNPCs(Vector2 Location, float maxDetectDistance)
        {
            List<NPC> closestNPC = new();

            // Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            // Loop through all NPCs(max always 200)
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC target = Main.npc[k];
                // Check if NPC able to be targeted. It means that NPC is
                // 1. active (alive)
                // 2. chaseable (e.g. not a cultist archer)
                // 3. max life bigger than 5 (e.g. not a critter)
                // 4. can take damage (e.g. moonlord core after all it's parts are downed)
                // 5. hostile (!friendly)
                // 6. not immortal (e.g. not a target dummy)
                if (target.CanBeChasedBy())
                {
                    // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Location);

                    // Check if it is within the radius
                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closestNPC.Add(target);
                    }
                }
            }

            return closestNPC.ToArray();
        }

        /// <summary>
        /// Returns all nearby Players from a given location and within a search distance.
        /// </summary>
        /// <param name="Player">Starting point of the search.</param>
        /// <param name="maxDetectDistance">Distance in Pixels.</param>
        /// <returns>List of NPCs within the given radius.</returns>
        public static Player[] FindAllNearbyPlayers(Player Player, float maxDetectDistance, bool ExcludeCenter = true)
        {
            List<Player> closestPlayer = new();

            // Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            // Loop through all NPCs(max always 200)
            for (int k = 0; k < Main.maxPlayers; k++)
            {
                Player target = Main.player[k];

                // Checks if the player is active and not dead
                if (target.active && !target.dead)
                {
                    if (target.whoAmI != Player.whoAmI || !ExcludeCenter)
                    {
                        // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                        float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Player.Center);

                        // Check if it is within the radius
                        if (sqrDistanceToTarget < sqrMaxDetectDistance)
                        {
                            sqrMaxDetectDistance = sqrDistanceToTarget;
                            closestPlayer.Add(target);
                        }
                    }
                }
            }

            return closestPlayer.ToArray();
        }

        public static class Easings
        {
            public static float Linear(float t) => t;

            public static float InQuad(float t) => t * t;
            public static float OutQuad(float t) => 1 - InQuad(1 - t);
            public static float InOutQuad(float t)
            {
                if (t < 0.5) return InQuad(t * 2) / 2;
                return 1 - InQuad((1 - t) * 2) / 2;
            }

            public static float InCubic(float t) => t * t * t;
            public static float OutCubic(float t) => 1 - InCubic(1 - t);
            public static float InOutCubic(float t)
            {
                if (t < 0.5) return InCubic(t * 2) / 2;
                return 1 - InCubic((1 - t) * 2) / 2;
            }

            public static float InQuart(float t) => t * t * t * t;
            public static float OutQuart(float t) => 1 - InQuart(1 - t);
            public static float InOutQuart(float t)
            {
                if (t < 0.5) return InQuart(t * 2) / 2;
                return 1 - InQuart((1 - t) * 2) / 2;
            }

            public static float InQuint(float t) => t * t * t * t * t;
            public static float OutQuint(float t) => 1 - InQuint(1 - t);
            public static float InOutQuint(float t)
            {
                if (t < 0.5) return InQuint(t * 2) / 2;
                return 1 - InQuint((1 - t) * 2) / 2;
            }

            public static float InSine(float t) => (float)-Math.Cos(t * Math.PI / 2);
            public static float OutSine(float t) => (float)Math.Sin(t * Math.PI / 2);
            public static float InOutSine(float t) => (float)(Math.Cos(t * Math.PI) - 1) / -2;

            public static float InExpo(float t) => (float)Math.Pow(2, 10 * (t - 1));
            public static float OutExpo(float t) => 1 - InExpo(1 - t);
            public static float InOutExpo(float t)
            {
                if (t < 0.5) return InExpo(t * 2) / 2;
                return 1 - InExpo((1 - t) * 2) / 2;
            }

            public static float InCirc(float t) => -((float)Math.Sqrt(1 - t * t) - 1);
            public static float OutCirc(float t) => 1 - InCirc(1 - t);
            public static float InOutCirc(float t)
            {
                if (t < 0.5) return InCirc(t * 2) / 2;
                return 1 - InCirc((1 - t) * 2) / 2;
            }

            public static float InElastic(float t) => 1 - OutElastic(1 - t);
            public static float OutElastic(float t)
            {
                float p = 0.3f;
                return (float)Math.Pow(2, -10 * t) * (float)Math.Sin((t - p / 4) * (2 * Math.PI) / p) + 1;
            }
            public static float InOutElastic(float t)
            {
                if (t < 0.5) return InElastic(t * 2) / 2;
                return 1 - InElastic((1 - t) * 2) / 2;
            }

            public static float InBack(float t)
            {
                float s = 1.70158f;
                return t * t * ((s + 1) * t - s);
            }
            public static float OutBack(float t) => 1 - InBack(1 - t);
            public static float InOutBack(float t)
            {
                if (t < 0.5) return InBack(t * 2) / 2;
                return 1 - InBack((1 - t) * 2) / 2;
            }

            public static float InBounce(float t) => 1 - OutBounce(1 - t);
            public static float OutBounce(float t)
            {
                float div = 2.75f;
                float mult = 7.5625f;

                if (t < 1 / div)
                {
                    return mult * t * t;
                }
                else if (t < 2 / div)
                {
                    t -= 1.5f / div;
                    return mult * t * t + 0.75f;
                }
                else if (t < 2.5 / div)
                {
                    t -= 2.25f / div;
                    return mult * t * t + 0.9375f;
                }
                else
                {
                    t -= 2.625f / div;
                    return mult * t * t + 0.984375f;
                }
            }
            public static float InOutBounce(float t)
            {
                if (t < 0.5) return InBounce(t * 2) / 2;
                return 1 - InBounce((1 - t) * 2) / 2;
            }
        }

    }
}

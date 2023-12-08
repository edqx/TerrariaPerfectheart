using Microsoft.Xna.Framework;
using PerfectheartMod.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace PerfectheartMod.Projectiles
{
	public class AngelicWrath : SpawnBeam
	{
        public override string Texture => "PerfectheartMod/Projectiles/SpawnBeam";

		public override void SetDefaults() {
            base.SetDefaults();

            Projectile.hostile = false;
			Projectile.friendly = false;
		}

		public override void AI() {
			if (!NPC.AnyNPCs(ModContent.NPCType<PerfectheartBoss>())) {
				Projectile.Kill();
				return;
			}

			if (++Projectile.frameCounter >= 5) {
				Projectile.frameCounter = 0;
                if (++Projectile.frame > 9) {
                    Projectile.frame = 3;
                }
			}
		}
	}
}
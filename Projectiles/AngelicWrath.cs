using Microsoft.Xna.Framework;
using Terraria;

namespace PerfectheartMod.Projectiles
{
	public class AngelicWrath : SpawnBeam
	{
        public override string Texture => "PerfectheartMod/Projectiles/SpawnBeam";

		public override void SetDefaults() {
            base.SetDefaults();

            Projectile.hostile = true;
			Projectile.friendly = false;
		}

		public override void AI() {
			if (++Projectile.frameCounter >= 5) {
				Projectile.frameCounter = 0;
                if (++Projectile.frame > 9) {
                    Projectile.frame = 3;
                }
			}
		}
	}
}
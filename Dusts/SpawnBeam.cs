using Terraria;
using Terraria.ModLoader;

namespace PerfectheartMod.Dusts
{
	public class SpawnBeam : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.noGravity = true;
            dust.scale = 1f;
            dust.frame = new Microsoft.Xna.Framework.Rectangle(0, 0, 106, 142);
		}

		public override bool Update(Dust dust) {
            if (Main.GameUpdateCount % 5 == 0) {
                if (dust.frame.Y < 1846 - 142) {
                    dust.frame = new Microsoft.Xna.Framework.Rectangle(0, dust.frame.Y + 142, 106, 142);
                } else {
                    dust.alpha = 254;
                }
            }
			return false;
		}
    }
}
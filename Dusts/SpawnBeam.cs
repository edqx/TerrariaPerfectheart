using Terraria;
using Terraria.ModLoader;

namespace PerfectheartMod.Dusts
{
	public class SpawnBeam : ModDust
	{
		public override void OnSpawn(Dust dust) {
			dust.noGravity = true;
            dust.frame = new Microsoft.Xna.Framework.Rectangle(0, 0, 47, 57);
		}

		public override bool Update(Dust dust) {
            if (dust.frame.Y < 811 - 57) {
                dust.frame = new Microsoft.Xna.Framework.Rectangle(0, dust.frame.Bottom + 57, 47, 57);
            } else {
                dust.frame = new Microsoft.Xna.Framework.Rectangle(0, 0, 47, 57);
            }
			return false;
		}
    }
}
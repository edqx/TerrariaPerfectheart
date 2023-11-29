using PerfectheartMod.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace PerfectheartMod.Dusts
{
	public class SpawnBeamSpawner : SpawnBeam
	{
        public override string Texture => "PerfectheartMod/Dusts/SpawnBeam";
        
		public override bool Update(Dust dust) {
            if (Main.GameUpdateCount % 5 == 0 && dust.frame.Y >= 11 * 142 && dust.frame.Y < 12 * 142) {
                Filters.Scene.Activate("PerfectheartSpawnFlash");
            } else if (dust.frame.Y >= 12 * 142 && dust.alpha == 254) {
                if ((int)dust.customData > 80) {
                    dust.customData = (int)dust.customData + 1;
                    Filters.Scene["PerfectheartSpawnFlash"].GetShader().UseProgress(1f - ((1f - (int)dust.customData / 100f) / ((100f - 80f) / 50f))); // idfk
                } else if ((int)dust.customData > 50) {
                    dust.customData = (int)dust.customData + 1;
                    Filters.Scene["PerfectheartSpawnFlash"].GetShader().UseProgress(0.5f);
                } else {
                    dust.customData = (int)dust.customData + 10;
                    Filters.Scene["PerfectheartSpawnFlash"].GetShader().UseProgress((int)dust.customData / 100f);
                }
                if ((int)dust.customData >= 100) {
                    dust.active = false;
                } else if ((int)dust.customData == 50) {
                    NPC.SpawnBoss((int)dust.position.X, (int)dust.position.Y, ModContent.NPCType<PerfectheartBoss>(), Main.myPlayer);
                }
                return false;
            }

            base.Update(dust);
            if (dust.alpha == 254) {
                dust.customData = 0;
            }
            return false;
		}
    }
}
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace perfectheart.NPCs
{
    public class PerfectheartBoss : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Entity.type] = 8;
        }

        public override void SetDefaults()
        {
            Entity.aiStyle = -1;
            Entity.lifeMax = 100000;
            Entity.damage = 400;
            Entity.defense = 50;
            Entity.knockBackResist = 0f;
            Entity.width = 75;
            Entity.height = 80;
            Entity.noGravity = true;
            Entity.boss = true;
            Entity.noTileCollide = true;
            Entity.HitSound = SoundID.Shimmer1;
        }

        public override void AI()
        {
            Entity.position = Main.player[Entity.target].position + new Microsoft.Xna.Framework.Vector2(250f, -50f);
        }
    }
}

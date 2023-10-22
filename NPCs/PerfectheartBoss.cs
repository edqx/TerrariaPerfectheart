using System;
using System.Drawing;
using Microsoft.Xna.Framework;
using perfectheart;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PerfectheartMod.NPCs
{
    public class PerfectheartBoss : ModNPC
    {
        public int NumFightAttempts = 0;
        public float ascendVelocity = 0f;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Entity.type] = 8;
        }

        public override void SetDefaults()
        {
            Entity.aiStyle = -1;
            Entity.lifeMax = 100000;
            Entity.damage = 0;
            Entity.defense = 50;
            Entity.knockBackResist = 0f;
            Entity.width = 75;
            Entity.height = 80;
            Entity.noGravity = true;
            Entity.noTileCollide = true;
            Entity.dontTakeDamage = true;
            Entity.friendly = true;
            Entity.HitSound = SoundID.NPCHit5;
            Entity.despawnEncouraged = false;

            NPCID.Sets.NoTownNPCHappiness[Entity.type] = true;

            Music = -1;
        }

        public override bool CanChat()
        {
            return NumFightAttempts < 3;
        }

        public override string GetChat()
        {
            NumFightAttempts = 1;
            return GetChatForFightAttempt();
        }

        public string GetChatForFightAttempt() {
            switch (NumFightAttempts)
            {
            case 1:
                return Language.GetTextValue("Mods.PerfectheartMod.Dialogue.FightAttempt1");
            case 2:
                return Language.GetTextValue("Mods.PerfectheartMod.Dialogue.FightAttempt2");
            case 3:
                return Language.GetTextValue("Mods.PerfectheartMod.Dialogue.FightAttempt3");
            }
            return "";
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Fight";
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                if (NumFightAttempts < 3)
                {
                    NumFightAttempts++;
                    Main.npcChatText = GetChatForFightAttempt();
                }
                else
                {
                    Main.CloseNPCChatOrSign();
					if (Main.netMode == NetmodeID.Server)
                    {
                        ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.PerfectheartMod.Dialogue.FightBegin"), Microsoft.Xna.Framework.Color.Pink);
					}
					else if (Main.netMode == NetmodeID.SinglePlayer)
                    {
						Main.NewText(Language.GetTextValue("Mods.PerfectheartMod.Dialogue.FightBegin"), Microsoft.Xna.Framework.Color.Pink);
					}
                    PerfectheartBossSystem.BossStage = FightStage.PhaseOne;
                    Entity.boss = true;
                }
            }
        }

        public override void OnKill()
        {
            PerfectheartBossSystem.BossStage = FightStage.Nil;
        }

        public override void AI()
        {
			if (Entity.target < 0 || Entity.target == 255 || Main.player[Entity.target].dead || !Main.player[Entity.target].active)
            {
				Entity.TargetClosest();
			}

            if (PerfectheartBossSystem.BossStage == FightStage.FightStarting) {
                Entity.position = Entity.position - new Vector2(0f, ascendVelocity);
                ascendVelocity += 0.4f;
                if (ascendVelocity > 16f) {
                    ascendVelocity = 16f;
                }
                if (Entity.position.Y < 0) {
                    PerfectheartBossSystem.BossStage = FightStage.PhaseOne;
                }
                return;
            }

            // Entity.position = Main.player[Entity.target].position + new Microsoft.Xna.Framework.Vector2(250f, -50f);
        }
    }
}

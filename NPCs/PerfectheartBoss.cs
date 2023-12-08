using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using perfectheart;
using PerfectheartMod.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;

namespace PerfectheartMod.NPCs
{
    public class PerfectheartBoss : ModNPC
    {
        public int NumFightAttempts = 0;
        public float ascendVelocity = 0f;

        public float startX = 0f;
        public bool isAngelicWrathActive = false;
        public Dictionary<int, long> playersOutsideArena = new Dictionary<int, long>();

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
            return NumFightAttempts <= 3;
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
                    PerfectheartBossSystem.BossStage = FightStage.FightStarting;
                    Entity.boss = true;
                }
            }
        }

        public override void OnKill()
        {
            PerfectheartBossSystem.BossStage = FightStage.Nil;
            foreach (Projectile proj in Main.projectile) {
                if (proj.type == ModContent.ProjectileType<AngelicWrath>()) {
                    proj.Kill();
                }
            }
        }

        bool CallAngelicWrath() {
            if (isAngelicWrathActive) return false;

            isAngelicWrathActive = true;
            float k = 0;
            startX = Entity.position.X;
            while (k < Main.maxTilesY)
            {
                Projectile.NewProjectileDirect(
                    NPC.GetSource_FromThis(),
                    new Vector2(startX, 0f) + new Vector2(-100, k).ToWorldCoordinates(),
                    Vector2.Zero,
                    ModContent.ProjectileType<AngelicWrath>(),
                    0,
                    0f,
                    -1,
                    k
                );
                Projectile.NewProjectileDirect(
                    NPC.GetSource_FromThis(),
                    new Vector2(startX, 0f) + new Vector2(100, k).ToWorldCoordinates(),
                    Vector2.Zero,
                    ModContent.ProjectileType<AngelicWrath>(),
                    0,
                    0f,
                    -1,
                    k
                );
                k += 6f;
            }
            return true;
        }

        public override void AI()
         {
			if (Entity.target < 0 || Entity.target == 255 || Main.player[Entity.target].dead || !Main.player[Entity.target].active)
            {
				Entity.TargetClosest();
			}

            if (isAngelicWrathActive) {
                for (int i = 0; i < Main.player.Length; i++) {
                    Player player = Main.player[i];
                    if (player != null && player.active && !player.dead) {
                        if (player.position.X <= startX - 100 * 16 || player.position.X >= startX + 100 * 16) {
                            long timeMs = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                            if (playersOutsideArena.TryGetValue(i, out long existingTime)) {
                                if (timeMs - existingTime >= 3000) {
                                    player.KillMe(PlayerDeathReason.ByNPC(Entity.whoAmI), 999999999, 0, false);
                                }
                                return;
                            }
                            playersOutsideArena[i] = timeMs;
                        } else {
                            playersOutsideArena.Remove(i);
                        }
                    }
                }
            }

            if (PerfectheartBossSystem.BossStage == FightStage.FightStarting) {
                Entity.position = Entity.position - new Vector2(0f, ascendVelocity);
                Entity.despawnEncouraged = false;
                ascendVelocity += 0.4f;
                if (ascendVelocity > 32f) {
                    ascendVelocity = 32f;
                }
                if (Entity.position.Y < 0) {
                    PerfectheartBossSystem.BossStage = FightStage.PhaseOne;
                    CallAngelicWrath();
                }
                return;
            }
            if (PerfectheartBossSystem.BossStage == FightStage.PhaseOne) {
                Entity.position = Main.player[Entity.target].position + new Vector2(250f, -50f);
            }
        }
    }
}

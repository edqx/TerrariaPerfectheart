using System;
using System.Collections.Generic;
using PerfectheartMod.Items;
using PerfectheartMod.NPCs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace PerfectheartMod.Tiles {
    public class LargeTestTubeTile : ModTile {
        protected int multiversalTranslocatorModuleType;
        protected Dictionary<(int, int), long> frameTimers = new();

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Width = 5;
            TileObjectData.newTile.Height = 9;
            TileObjectData.newTile.CoordinateHeights = new int[]{ 16, 16, 16, 16, 16, 16, 16, 16, 16 };
            TileObjectData.newTile.Origin = new Point16(2, 8);
            TileObjectData.addTile(Type);

            AddMapEntry(new Microsoft.Xna.Framework.Color(255, 192, 203), Language.GetText("Mods.PerfectheartMod.Map.LargeTestTube"));

            multiversalTranslocatorModuleType = ModContent.ItemType<MultiversalTranslocatorModule>();
        }

        public override void MouseOver(int i, int j)
        {
            Tile tile = Main.tile[i, j];

            if (tile.TileFrameX >= 90 && tile.TileFrameX <= 178) {
                Main.LocalPlayer.cursorItemIconEnabled = true;
                Main.LocalPlayer.cursorItemIconID = -1;
                Main.LocalPlayer.cursorItemIconText = "Activate";
                return;
            }

            if (Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].type == multiversalTranslocatorModuleType)
            {
                Main.LocalPlayer.noThrow = 2;
                Main.LocalPlayer.cursorItemIconEnabled = true;
                
                int style = TileObjectData.GetTileStyle(Main.tile[i, j]);
                Main.LocalPlayer.cursorItemIconID = TileLoader.GetItemDropFromTypeAndStyle(multiversalTranslocatorModuleType, style);
            }
        }

        public void SetAllTileFrameX(int i, int j, int frameX, int frameY) {
            Tile tile = Main.tile[i, j];
            int topX = i - tile.TileFrameX % 90 / 18;
            int topY = j - tile.TileFrameY % 160 / 18;

            for (int x = topX; x < topX + 5; x++)
            {
                for (int y = topY; y < topY + 9; y++)
                {
                    Main.tile[x, y].TileFrameX = (short)(frameX + ((x - topX) * 18));
                    Main.tile[x, y].TileFrameY = (short)(frameY + ((y - topY) * 18));
                }
            }
        }

        public long GetLastFrameTimer(int x, int y) {
            if (frameTimers.TryGetValue((x, y), out long timer)) return timer;

            long currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            frameTimers[(x, y)] = currentTime;
            return currentTime;
        }

        public void SetLastFrameTimer(int x, int y) {
            frameTimers[(x, y)] = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public override bool RightClick(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX >= 90 && tile.TileFrameX <= 178)
            {
                int topX = i - tile.TileFrameX % 90 / 18;
                int topY = j - tile.TileFrameY % 160 / 18;

                for (int x = topX; x < topX + 5; x++)
                {
                    for (int y = topY; y < topY + 9; y++)
                    {
                        SetLastFrameTimer(x, y);
                    }
                }
                SetAllTileFrameX(i, j, 270, 0);
                return true;
            }

            if (Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].type == multiversalTranslocatorModuleType)
            {
                Main.LocalPlayer.ConsumeItem(multiversalTranslocatorModuleType);
                SetAllTileFrameX(i, j, 90, 0);
                return true;
            }
            return false;
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            Tile tile = Main.tile[i, j];
            int topX = i - tile.TileFrameX % 90 / 18;
            int topY = j - tile.TileFrameY % 160 / 18;
            long frameMs = DateTimeOffset.Now.ToUnixTimeMilliseconds() - GetLastFrameTimer(i, j);
            if (tile.TileFrameX >= 90 && tile.TileFrameX <= 178)
            {
                if (frameMs > 1000 / 2) {
                    if (tile.TileFrameY >= 162)
                    {
                        tile.TileFrameY -= 162;
                    }
                    else
                    {
                        tile.TileFrameY += 162;
                    }
                    SetLastFrameTimer(i, j);
                }
            }
            else if (tile.TileFrameX >= 270 && tile.TileFrameX <= 358)
            {
                if (frameMs > 1000 * 2) {
                    if (tile.TileFrameY >= 324 && tile.TileFrameY <= 484 && i == topX && j == topY) {
                        Microsoft.Xna.Framework.Vector2 pt = new Point16(topX + 3, topY + 4).ToWorldCoordinates();
                        NPC.SpawnBoss((int)pt.X, (int)pt.Y, ModContent.NPCType<PerfectheartBoss>(), Main.myPlayer);
                    }
                    if (tile.TileFrameY < 486)
                    {
                        tile.TileFrameY += 162;
                    }
                    SetLastFrameTimer(i, j);
                }
            }
        }
    }
}
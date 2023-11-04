using System.Numerics;
using Microsoft.Xna.Framework.Graphics;
using PerfectheartMod.Items;
using PerfectheartMod.NPCs;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace PerfectheartMod.Tiles {
    public class LargeTestTubeTile : ModTile {
        protected int multiversalTranslocatorModuleType;
        protected int currentTick;

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Width = 5;
            TileObjectData.newTile.Height = 9;
            TileObjectData.newTile.CoordinateHeights = new int[]{ 16, 16, 16, 16, 16, 16, 16, 16, 16 };
            TileObjectData.newTile.Origin = new Terraria.DataStructures.Point16(2, 8);
            TileObjectData.addTile(Type);

            AddMapEntry(new Microsoft.Xna.Framework.Color(255, 192, 203), Language.GetText("Mods.PerfectheartMod.Map.LargeTestTube"));

            multiversalTranslocatorModuleType = ModContent.ItemType<MultiversalTranslocatorModule>();
        }

        public override void MouseOver(int i, int j)
        {
            Tile tile = Main.tile[i, j];

            if (tile.TileFrameX >= 90 && tile.TileFrameX <= 178) {
                Main.LocalPlayer.cursorItemIconEnabled = true;
                Main.LocalPlayer.cursorItemIconID = 0;
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

        public override bool RightClick(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            if (tile.TileFrameX >= 90 && tile.TileFrameX <= 178) {
                int topX = i - tile.TileFrameX % 90 / 18;
                int topY = j - tile.TileFrameY % 160 / 18;
                Microsoft.Xna.Framework.Vector2 pt = new Point16(topX + 3, topY + 4).ToWorldCoordinates();
                NPC.SpawnBoss((int)pt.X, (int)pt.Y, ModContent.NPCType<PerfectheartBoss>(), Main.myPlayer);
                return true;
            }

            if (Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].type == multiversalTranslocatorModuleType)
            {
                Main.LocalPlayer.ConsumeItem(multiversalTranslocatorModuleType);
                int topX = i - tile.TileFrameX % 90 / 18;
                int topY = j - tile.TileFrameY % 160 / 18;

                for (int x = topX; x < topX + 5; x++)
                {
                    for (int y = topY; y < topY + 9; y++)
                    {
                        Main.tile[x, y].TileFrameX = (short)(90 + ((x - topX) * 18));
                        Main.tile[x, y].TileFrameY = (short)(0 + ((y - topY) * 18));
                    }
                }
                return true;
            }
            return false;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            currentTick++;

            if (currentTick > 8) {
                currentTick = 0;
            }
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            Tile tile = Main.tile[i, j];
            if (currentTick == 0)
            {
                if (tile.TileFrameX >= 90 && tile.TileFrameX <= 178)
                {
                    if (tile.TileFrameY >= 162)
                    {
                        tile.TileFrameY -= 162;
                    } else {
                        tile.TileFrameY += 162;
                    }
                }
            }
        }
    }
}
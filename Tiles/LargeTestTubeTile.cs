using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace PerfectheartMod.Tiles {
    public class LargeTestTubeTile : ModTile {
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
        }
    }
}
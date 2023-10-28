﻿using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PerfectheartMod.Items
{
	class LargeTestTube : ModItem
	{
		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LargeTestTubeTile>());
			Item.width = 10;
			Item.height = 24;
			Item.value = 500;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.DirtBlock, 5)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
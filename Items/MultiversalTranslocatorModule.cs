using System;
using PerfectheartMod.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PerfectheartMod.Items
{
	public class MultiversalTranslocatorModule : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 40;
			Item.value = 10000;
			Item.rare = ItemRarityID.Purple;
		}

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe()
				.AddIngredient(ItemID.DirtBlock, 10)
				.AddTile(TileID.WorkBenches)
				.Register();
		}

        public override bool? UseItem(Player player)
        {
			NPC.SpawnBoss((int)player.position.X + 250, (int)player.position.Y - 40, ModContent.NPCType<PerfectheartBoss>(), Array.FindIndex(Main.player, x => x == player));
            return true;
        }
    }
}
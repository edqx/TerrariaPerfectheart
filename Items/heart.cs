using System;
using perfectheart.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace perfectheart.Items
{
	public class heart : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
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
			NPC.SpawnBoss((int)player.position.X, (int)player.position.Y - 250, ModContent.NPCType<PerfectheartBoss>(), Array.FindIndex(Main.player, x => x == player));
            return true;
        }
    }
}
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Armors.Jungle
{
	[AutoloadEquip(EquipType.Legs)]
	public class AlchemistJungleLegs : OrchidModAlchemistEquipable
	{
		public override void SafeSetDefaults()
		{
			item.width = 22;
			item.height = 18;
			item.value = Item.sellPrice(0, 0, 60, 0);
			item.rare = 3;
			item.defense = 7;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lily Leggings");
			Tooltip.SetDefault("10% increased chemical damage");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.alchemistDamage += 0.1f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Content.Items.Materials.JungleLilyBloomed>(), 1);
			recipe.AddIngredient(ItemID.Vine, 2);
			recipe.AddIngredient(ItemID.JungleSpores, 3);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}

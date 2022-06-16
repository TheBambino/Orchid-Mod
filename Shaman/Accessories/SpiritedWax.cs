using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Accessories
{
	public class SpiritedWax : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 55, 0);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Waxy Tear");
			Tooltip.SetDefault("Your shamanic water bonds will increase your shamanic critical strike chance by 10%"
							 + "\nYour shamanic critical strikes will recover you some health"
							 + "\nYour shamanic earth bonds will cover you in honey"
							 + "\nYou have a chance to release harmful bees when under the effect of shamanic earth bonds");

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanHoney = true;
			modPlayer.shamanWaterHoney = true;
			if (modPlayer.shamanWaterTimer > 0)
			{
				modPlayer.shamanCrit += 10;
			}
		}
		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(null, "SpiritedWater", 1);
			recipe.AddIngredient(null, "WaxyVial", 1);
			recipe.AddTile(114);
			recipe.Register();
			recipe.Register();
		}
	}
}

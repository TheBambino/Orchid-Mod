using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.SunPriest
{
	[AutoloadEquip(EquipType.Legs)]
	public class SunPriestPants : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 14;
			Item.value = Item.sellPrice(0, 4, 50, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 15;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sun Priest Pants");
			Tooltip.SetDefault("6% increased shamanic damage"
							 + "\n5% increased movement speed");
		}

		public override void UpdateEquip(Player player)
		{
			OrchidShaman modPlayer = player.GetModPlayer<OrchidShaman>();
			player.moveSpeed += 0.05f;
			player.GetDamage<ShamanDamageClass>() += 0.06f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(null, "LihzahrdSilk", 4);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 18);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}

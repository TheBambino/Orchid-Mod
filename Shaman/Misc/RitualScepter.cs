using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Misc
{
	public class RitualScepter : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 34;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ritual Scepter");
			Tooltip.SetDefault("Can be upgraded into various shamanic weapons");
		}
	}
}

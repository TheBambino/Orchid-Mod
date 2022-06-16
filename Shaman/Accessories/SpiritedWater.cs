using Terraria;

namespace OrchidMod.Shaman.Accessories
{
	public class SpiritedWater : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 35, 0);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ondine Tear");
			Tooltip.SetDefault("Your shamanic water bonds will increase your shamanic critical strike chance by 10%");

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (modPlayer.shamanWaterTimer > 0)
			{
				modPlayer.shamanCrit += 10;
			}
		}
	}
}

using Terraria;

namespace OrchidMod.Gambler.Decks
{
	public class DeckJungle : GamblerDeck
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jungle Gambler Deck");
			Tooltip.SetDefault("Allows the use of gambler abilities"
							+ "\n'+15 leafiness '");
		}

		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 20, 0);
		}
	}
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Armors.Abyss
{
	[AutoloadEquip(EquipType.Body)]
	public class AbyssalChestplate : OrchidModShamanEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 34;
			Item.height = 22;
			Item.value = 0;
			Item.rare = ItemRarityID.Red;
			Item.defense = 32;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Abyssal Chestplate");
			Tooltip.SetDefault("9% increased shamanic damage and critical strike chance");
		}

		public override void UpdateEquip(Player player)
		{
			player.GetCritChance<ShamanDamageClass>() += 9;
			player.GetDamage<ShamanDamageClass>() += 0.09f;
			Lighting.AddLight(player.position, 0.15f, 0.15f, 0.8f);
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowLokis = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LunarBar, 16);
			recipe.AddIngredient(ModContent.ItemType<Misc.AbyssFragment>(), 20);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			spriteBatch.DrawSimpleItemGlowmaskInWorld(Item, Color.White, rotation, scale);
		}
	}
}

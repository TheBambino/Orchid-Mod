using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class EyeCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 85;
			Item.knockBack = 3f;
			Item.useAnimation = 30;
			Item.useTime = 30;

			this.cardRequirement = 4;
			this.cardSets = GamblerCardSets.Boss;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Guardian of the Night");
			Tooltip.SetDefault("Summons a servant of cthulhu, dashing at your cursor"
							+ "\nThe servant will only attack if your cursor is far enough from its location");
		}

		public override void GamblerShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback, bool dummy = false)
		{
			int projType = ProjectileType<Gambler.Projectiles.EyeCardProj>();
			bool found = false;
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == projType && proj.owner == player.whoAmI)
				{
					found = true;
					break;
				}
			}
			if (!found)
			{
				DummyProjectile(Projectile.NewProjectile(source, position, velocity, projType, damage, knockback, player.whoAmI), dummy);
				SoundEngine.PlaySound(SoundID.Item1);
			}
			else
			{
				SoundEngine.PlaySound(SoundID.Item7);
			}
		}
	}
}

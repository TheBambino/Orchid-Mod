using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.PlayerDrawLayers;
using OrchidMod.Utilities;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Weapons.Hardmode
{
	public class ShroomiteScepter : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 103;
			Item.width = 44;
			Item.height = 44;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.knockBack = 1.15f;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.UseSound = SoundID.Item45;
			Item.autoReuse = false;
			Item.shootSpeed = 15f;
			Item.shoot = ModContent.ProjectileType<Projectiles.ShroomiteScepterProj>();
			this.empowermentType = 4;
			this.energy = 35;
		}

		public override void SafeSetStaticDefaults()
		{
			HeldItemLayer.RegisterDrawMethod(Type, OrchidUtils.DrawSimpleItemGlowmaskOnPlayer);

			DisplayName.SetDefault("Bloom Shroom");
			Tooltip.SetDefault("Summons a protective shroom, harming nearby enemies"
				+ "\nHaving 3 or more active shamanic bonds weakens hit targets"
				+ "\nHaving 5 active shamanic bonds increases nearby players health regeneration"
				+ "\n[c/FF0000:Currently does not work as intended, and will be fixed soon]");
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidShaman modPlayer = new OrchidShaman();
			int projectileType = ModContent.ProjectileType<Projectiles.ShroomiteScepterProj>();
			int nbBonds = modPlayer.GetNbShamanicBonds();

			if (player.ownedProjectileCounts[projectileType] > 0)
			{
				var oldProjs = Array.FindAll(Main.projectile, i => i.active && i.type == projectileType && i.owner == player.whoAmI);
				foreach (var elem in oldProjs) elem?.Kill();
			}

			var projectile = CreateNewProjectile(Item, player, source, projectileType, damage, knockback);
			projectile.ai[1] = nbBonds;
			projectile.netUpdate = true;

			return false;
		}

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			Color color = new Color(0.3f, 0.35f, 0.9f) * 0.25f;
			Lighting.AddLight(Item.Center, color.R / 255f, color.G / 255f, color.B / 255f);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			spriteBatch.DrawSimpleItemGlowmaskInWorld(Item, Color.White, rotation, scale);
		}

		private Projectile CreateNewProjectile(Item item, Player player, EntitySource_ItemUse_WithAmmo source, int type, int damage, float knockback)
		{
			Point point = new Point((int)((float)Main.mouseX + Main.screenPosition.X) / 16, (int)((float)Main.mouseY + Main.screenPosition.Y) / 16);
			if (player.gravDir == -1f) point.Y = (int)(Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY) / 16;
			while (point.Y < Main.maxTilesY - 10 && Main.tile[point.X, point.Y] != null && !WorldGen.SolidTile2(point.X, point.Y) && Main.tile[point.X - 1, point.Y] != null && !WorldGen.SolidTile2(point.X - 1, point.Y) && Main.tile[point.X + 1, point.Y] != null && !WorldGen.SolidTile2(point.X + 1, point.Y))
			{
				point.Y++;
			}

			Vector2 position = new Vector2(Main.mouseX + Main.screenPosition.X, point.Y * 16 - 22);
			Vector2 velocity = new Vector2(0f, 15f);
			var projectile = this.NewShamanProjectile(player, source, position, velocity, type, damage, knockback);
			return Main.projectile[projectile];
		}
	}
}

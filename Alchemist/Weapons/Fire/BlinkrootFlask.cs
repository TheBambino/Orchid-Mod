using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Fire
{
	public class BlinkrootFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 9;
			Item.width = 30;
			Item.height = 30;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.FIRE;
			this.rightClickDust = 57;
			this.colorR = 35;
			this.colorG = 54;
			this.colorB = 20;
			this.secondaryDamage = 16;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blinkroot Extract");
			Tooltip.SetDefault("Releases fire spores, the less other extracts used, the more"
							+ "\nOnly one set of spores can exist at once"
							+ "\nSpores deals 10% increased damage against fire-coated enemies");
		}

		public override void KillSecond(int timeLeft, Player player, OrchidAlchemist modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int nb = 2 + Main.rand.Next(2);
			for (int i = 0; i < nb; i++)
			{
				Vector2 vel = (new Vector2(0f, (float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(180)));
				int spawnProj = ProjectileType<Alchemist.Projectiles.Fire.FireSporeProjAlt>();
				SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, vel, spawnProj, 0, 0f, projectile.owner);
			}
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active == true && proj.type == ProjectileType<Alchemist.Projectiles.Fire.FireSporeProj>() && proj.owner == projectile.owner && proj.localAI[1] != 1f)
				{
					proj.Kill();
				}
			}

			nb = alchProj.nbElements + alchProj.nbElementsNoExtract;
			nb += player.HasBuff(BuffType<Alchemist.Buffs.MushroomHeal>()) ? Main.rand.Next(3) : 0;
			for (int i = 0; i < nb; i++)
			{
				Vector2 vel = (new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(180)));
				int dmg = GetSecondaryDamage(player, alchProj.nbElements);
				if (alchProj.natureFlask.type == ItemType<Nature.MushroomFlask>()) dmg += 5;
				SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, vel, ProjectileType<Alchemist.Projectiles.Fire.FireSporeProj>(), dmg, 0f, projectile.owner);
			}
		}

		public override void AddVariousEffects(Player player, OrchidAlchemist modPlayer, AlchemistProj alchProj, Projectile proj, OrchidModGlobalItem globalItem)
		{
			alchProj.nbElementsNoExtract--;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(null, "EmptyFlask", 1);
			recipe.AddIngredient(ItemID.Blinkroot, 3);
			recipe.AddIngredient(ItemID.Cobweb, 10);
			recipe.Register();
		}
	}
}

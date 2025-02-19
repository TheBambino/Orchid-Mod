using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium
{
	public class PatchWerkScepterProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 120;
			Projectile.scale = 1f;
			AIType = ProjectileID.Bullet;
			Projectile.alpha = 196;
			this.projectileTrail = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Festering Bolt");
		}

		public override void AI()
		{
			Projectile.rotation += 0.2f;
			Projectile.velocity.Y += 0.2f;
			Projectile.velocity.X *= 0.99f;

			int dust = 5;
			if (Main.rand.Next(4) == 0)
			{
				switch (Main.rand.Next(2))
				{
					case 0:
						dust = 258;
						break;
					case 1:
						dust = 60;
						break;
					default:
						break;
				}
			}

			int DustID = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dust, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 125, default(Color), 1.25f);
			Main.dust[DustID].noGravity = true;
			Main.dust[DustID].velocity = -Projectile.velocity / 2;
			Main.dust[DustID].scale *= 1.5f;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 4; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 258);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = -Projectile.velocity / 2;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			if (modPlayer.GetNbShamanicBonds() > 1)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, Mod.Find<ModProjectile>("PatchWerkScepterProjAlt").Type, Projectile.damage, 0f, 0, 0f, 0f);
			}
		}
	}
}
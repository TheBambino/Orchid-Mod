using Microsoft.Xna.Framework;
using Terraria;

namespace OrchidMod.Shaman.Projectiles
{
	public class AbyssShardS : OrchidModShamanProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Abyss Lightning");
		}
		public override void SafeSetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 70;
			Projectile.penetrate = 15;
			Projectile.extraUpdates = 5;
			Projectile.alpha = 255;
		}

		public override void AI()
		{
			for (int index1 = 0; index1 < 10; ++index1)
			{
				if (index1 % 2 == 0)
				{
					float x = Projectile.Center.X - Projectile.velocity.X / 10f * (float)index1;
					float y = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)index1;
					int index2 = Dust.NewDust(new Vector2(x, y), 1, 1, 172, 0.0f, 0.0f, 0, new Color(), 1f);
					Main.dust[index2].alpha = Projectile.alpha;
					Main.dust[index2].position.X = x;
					Main.dust[index2].position.Y = y;
					Main.dust[index2].scale = (float)Main.rand.Next(70, 110) * 0.013f;
					Main.dust[index2].velocity *= 0.0f;
					Main.dust[index2].noGravity = true;
				}
			}
		}
	}
}
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;

namespace OrchidMod.Alchemist.Projectiles.Reactive
{
	public class OilBubble : AlchemistProjReactive
	{
		private int animDirection;

		public override void SafeSetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 600;
			Projectile.scale = 1f;
			Projectile.alpha = 64;
			this.spawnTimeLeft = 600;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sap Bubble");
		}

		public override void OnSpawn(IEntitySource source)
		{
			animDirection = (Main.rand.NextBool(2) ? 1 : -1);
		}

		public override void SafeAI()
		{
			Projectile.velocity.Y *= 0.95f;
			Projectile.velocity.X *= 0.99f;
			Projectile.rotation += (0.05f * (0.2f - Math.Abs(Projectile.rotation)) + 0.001f) * animDirection;
			if (Math.Abs(Projectile.rotation) >= 0.2f)
			{
				Projectile.rotation = 0.2f * animDirection;
				animDirection *= -1;
			}

			if (Main.rand.NextBool(20))
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.ScourgeOfTheCorruptor);
				Main.dust[dust].velocity *= 0.1f;
				Main.dust[dust].scale *= 1f;
			}
		}

		public override void Despawn()
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.ScourgeOfTheCorruptor);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].scale *= 1f;
			}
		}

		public override void SafeKill(int timeLeft, Player player, OrchidAlchemist modPlayer)
		{
			SoundEngine.PlaySound(SoundID.Item85, Projectile.Center);
			int dmg = Projectile.damage;
			OrchidModProjectile.spawnDustCircle(Projectile.Center, 184, 200, 15, false, 1.5f, 1f, 5f);
			OrchidModProjectile.spawnDustCircle(Projectile.Center, 184, 175, 30, false, 1.5f, 1f, 5f);

			Vector2 move = Vector2.Zero;
			float distance = 500f;
			for (int k = 0; k < 200; k++)
			{
				if (Main.npc[k].active && !Main.npc[k].friendly)
				{
					Vector2 newMove = Main.npc[k].Center - Projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo < distance)
					{
						OrchidModAlchemistNPC modTarget = Main.npc[k].GetGlobalNPC<OrchidModAlchemistNPC>();
						modTarget.alchemistWater = 60 * 10;
					}
				}
			}
		}
	}
}
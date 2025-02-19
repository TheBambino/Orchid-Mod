using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class SnowCardProj : OrchidModGamblerProjectile
	{
		//private Texture2D texture;
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pinecone");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 24;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 10;
			Projectile.alpha = 255;
		}
		
		public override void OnSpawn(IEntitySource source)
		{
			int dustType = 31;
			Vector2 pos = new Vector2(Projectile.position.X, Projectile.position.Y);
			Main.dust[Dust.NewDust(pos, Projectile.width, Projectile.height, dustType)].velocity *= 0.25f;
		}

		public override void SafeAI()
		{
			this.checkMouseDrag();
			
			if (Projectile.ai[1] == 2f) {
				Projectile.rotation += Projectile.velocity.Length() / 30f * (Projectile.velocity.X > 0 ? 1f : -1f);
			}
		}
		
		public void checkMouseDrag() {
			Projectile proj = Main.projectile[(int)Projectile.ai[0]];
			
			if (proj.type != ProjectileType<Gambler.Projectiles.SnowCardBase>() || proj.active == false && Projectile.ai[1] != 2f) {
				Projectile.Kill();
			}
			
			if (Projectile.ai[1] == 0f) {
				proj.ai[0] ++;
				Projectile.timeLeft ++;
				if (Projectile.velocity.X > 0f) {
					Projectile.localAI[1] = Projectile.velocity.X;
					Projectile.velocity.X = 0f;
				}
				
				Projectile.position = proj.Center - new Vector2(Projectile.width, Projectile.height - 20) * 0.5f;
				
				if (Main.mouseLeft && Main.mouseLeftRelease) {
					Vector2 newMove = Main.MouseWorld - Projectile.Center;
					float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
					if (distanceTo < 25f) {
						Projectile.ai[1] = 1f;
						Projectile.netUpdate = true;
						Projectile.localAI[0] = Main.myPlayer;
					}
				}
			}
			
			if (Projectile.ai[1] == 1f) {
				proj.ai[0] ++;
				Projectile.timeLeft ++;
				if ((int)Projectile.localAI[0] == Main.myPlayer) {
					if (Main.mouseLeft) {
						Vector2 newMove = Main.MouseWorld - proj.Center;
						float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
						float range = 40f;
						if (distanceTo > range) {
							newMove.Normalize();
							Projectile.position = proj.Center + newMove * range - new Vector2(Projectile.width, Projectile.height) * 0.5f;
						} else {
							Projectile.position = Main.MouseWorld - new Vector2(Projectile.width, Projectile.height) * 0.5f;
						}
					} else {
						Vector2 newMove = proj.Center - Projectile.Center;
						newMove.Normalize();
						newMove *= Projectile.localAI[1];
						Projectile.velocity = newMove;
						Projectile.ai[1] = 2f;
						Projectile.tileCollide = true;
						Projectile.friendly = true;
						Projectile.netUpdate = true;
						Projectile.alpha = 0;
						SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
					}
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			int dustType = 31;
			Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType)].velocity *= 0.25f;
			int projType = ProjectileType<Gambler.Projectiles.SnowCardProjAlt>();
			bool dummy = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
			DummyProjectile(Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, projType, Projectile.damage, Projectile.knockBack, Projectile.owner), dummy);
		}
	}
}
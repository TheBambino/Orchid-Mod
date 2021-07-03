﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Gambler.Projectiles
{
    public class EmbersCardProj: OrchidModGamblerProjectile
    {
		bool started = false;
		int count = 0;
		
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Embers");
        } 
		
        public override void SafeSetDefaults()
        {
            projectile.width = 6;
            projectile.height = 6;
            projectile.friendly = true;
            projectile.aiStyle = 0;
			projectile.alpha = 126;
			projectile.timeLeft = 180;
			ProjectileID.Sets.Homing[projectile.type] = true;
			this.gamblingChipChance = 5;
        }
		
		public override void SafeAI()
        {
			this.count++;
			projectile.rotation += 0.1f;
			
			if (projectile.wet) {
				projectile.Kill();
			}
			
			if (projectile.timeLeft > 120) {
				projectile.velocity.Y += 0.01f;
				projectile.velocity.X *= 0.95f;
			}
			
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].noLight = true;
			
			if (started == false) {
				if (count == 30) started = true;
			}
			if (started == true) {
				projectile.ai[1]++;
				if (projectile.ai[1] == 10)
				{	
					projectile.ai[1] = 0;
					projectile.netUpdate = true;
					switch (Main.rand.Next(4)) {	
						case 0:
						projectile.velocity.Y =  1;
						projectile.velocity.X =  1;
						break;
						case 1:
						projectile.velocity.Y =  -1;
						projectile.velocity.X =  -1;
						break;
						case 2:
						projectile.velocity.Y =  -1;
						projectile.velocity.X =  1;
						break;
						case 3:
						projectile.velocity.Y =  1;
						projectile.velocity.X =  -1;
						break;
					}
				}
				
				// for (int index1 = 0; index1 < 1; ++index1)
				// {	
					// projectile.velocity = projectile.velocity * 0.75f;		
				// }
				
				if (projectile.alpha > 70)
				{
					projectile.alpha -= 15;
					if (projectile.alpha < 70)
					{
						projectile.alpha = 70;
					}
				}
				
				if (projectile.localAI[0] == 0f)
				{
					AdjustMagnitude(ref projectile.velocity);
					projectile.localAI[0] = 1f;
				}
				
				Vector2 move = Vector2.Zero;
				float distance = 150f;
				bool target = false;
				bool dummy = projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
				for (int k = 0; k < 200; k++)
				{
					if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && ((!dummy && Main.npc[k].type != NPCID.TargetDummy) || (dummy && Main.npc[k].type == NPCID.TargetDummy)) && projectile.timeLeft < 240)
					{
						Vector2 newMove = Main.npc[k].Center - projectile.Center;
						float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
						if (distanceTo < distance)
						{
							move = newMove;
							distance = distanceTo;
							target = true;
						}
					}
				}
				
				if (target)
				{
					AdjustMagnitude(ref move);
					projectile.velocity = (5 * projectile.velocity + move) / 1f;
					AdjustMagnitude(ref projectile.velocity);
				}
			}
        }
		private void AdjustMagnitude(ref Vector2 vector)
		{
			float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
			if (magnitude > 6f)
			{
				vector *= 6f / magnitude;
			}
		}
		
		public override void Kill(int timeLeft)
        {
            for(int i=0; i<3; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
				Main.dust[dust].velocity *= 3f;
            }
		}
		
		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer) {
			target.AddBuff(BuffID.OnFire, modPlayer.gamblerElementalLens ? 60 * 5 : 60 * 1);
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common;
using OrchidMod.Common.Graphics;
using OrchidMod.Common.Graphics.Primitives;
using OrchidMod.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles
{
	public class WyvernMorayProj : OrchidModShamanProjectile, IDrawOnDifferentLayers
	{
		public bool Improved { get => Projectile.ai[1] == 1; set => Projectile.ai[1] = value.ToInt(); }

		private bool _death = false;
		private float _deathProgress = 1f;
		private PrimitiveStrip _trail;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wyvern Spit");

			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 2;
			Projectile.friendly = true;
			Projectile.timeLeft = 150;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
		}

		private readonly Color[] _effectColors = new Color[] { new Color(113, 187, 162), new Color(40, 116, 255) };

		public Color GetCurrentColor() => _effectColors[Improved.ToInt()] * _deathProgress;

		public override void OnSpawn(IEntitySource source)
		{
			_trail = new PrimitiveStrip
			(
				width: progress => 20 * (1 - progress * 0.35f),
				color: progress => GetCurrentColor() * (1 - progress),
				effect: new IPrimitiveEffect.Custom("WyvernMoray", (e) => e["Time"].SetValue(-Main.GlobalTimeWrappedHourly)),
				headTip: null,
				tailTip: null
			);
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (Improved) damage += damage;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, this.GetCurrentColor().ToVector3() * 0.25f);

			if (_death) this.DeathUpdate();
			else OrchidModProjectile.resetIFrames(Projectile);
		}

		public void DeathUpdate()
		{
			Projectile.velocity = Vector2.Zero;
			Projectile.timeLeft = 2;

			if (_deathProgress == 1f)
			{
				SoundEngine.PlaySound(SoundID.Item34, Projectile.position);

				Projectile.friendly = false;
				Projectile.tileCollide = false;

				var proj = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<WyvernMorayProjLingering>(), (int)(Projectile.damage * 0.6f), 0.0f, Projectile.owner, 0.0f, 0.0f)];

				if (proj.ModProjectile is WyvernMorayProjLingering hehe)
				{
					hehe.effectColor = GetCurrentColor();
					hehe.Improved = this.Improved;
					proj.netUpdate = true;
				}
			}

			_deathProgress -= 0.085f;

			if (_deathProgress <= 0f) Projectile.Kill();
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity = oldVelocity;

			_death = true;
			return false;
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			_death = true;
		}

		void IDrawOnDifferentLayers.DrawOnDifferentLayers(DrawSystem system)
		{
			_trail.UpdatePointsAsSimpleTrail(currentPosition: Projectile.Center, maxPoints: 25, maxLength: 16 * 7);
			system.AddToAlphaBlend(layer: DrawLayers.Tiles, data: _trail);

			// ...

			Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
			Color color = GetCurrentColor();
			Texture2D texture = OrchidAssets.GetExtraTexture(11).Value;
			DefaultDrawData data;

			for (int k = 1; k < Projectile.oldPos.Length; k++)
			{
				float progress = ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Vector2 drawPosTrail = Projectile.oldPos[k] - Main.screenPosition + Projectile.Size * 0.5f + new Vector2(0f, Projectile.gfxOffY);

				data = new DefaultDrawData(texture, drawPosTrail, null, color * progress, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale * 0.4f * progress, SpriteEffects.None);
				system.AddToAdditive(DrawLayers.Tiles, data);
			}

			data = new DefaultDrawData(texture, drawPos, null, color, Projectile.velocity.ToRotation() + MathHelper.PiOver2, texture.Size() * 0.5f, Projectile.scale * 0.6f, SpriteEffects.None);
			system.AddToAdditive(DrawLayers.Tiles, data);

			texture = OrchidAssets.GetExtraTexture(13).Value;

			data = new DefaultDrawData(texture, drawPos, null, color * 0.4f, Main.GlobalTimeWrappedHourly, texture.Size() * 0.5f, Projectile.scale * 0.75f, SpriteEffects.None);
			system.AddToAdditive(DrawLayers.Tiles, data);

			data = new DefaultDrawData(texture, drawPos, null, color * 0.8f, Main.GlobalTimeWrappedHourly * 5f, texture.Size() * 0.5f, Projectile.scale * 0.3f, SpriteEffects.None);
			system.AddToAdditive(DrawLayers.Tiles, data);

			texture = OrchidAssets.GetExtraTexture(8).Value;

			data = new DefaultDrawData(texture, drawPos, null, color * _deathProgress, Projectile.velocity.ToRotation() + MathHelper.PiOver2, texture.Size() * 0.5f, Projectile.scale * 0.4f, SpriteEffects.None);
			system.AddToAdditive(DrawLayers.Tiles, data);

			texture = OrchidAssets.GetExtraTexture(3).Value;

			data = new DefaultDrawData(texture, drawPos + Vector2.Normalize(Projectile.velocity) * 8f, null, color * MathHelper.SmoothStep(0, 1, Projectile.velocity.Length() * 0.1f), Projectile.velocity.ToRotation() + MathHelper.PiOver2, texture.Size() * 0.5f, Projectile.scale * 0.4f, SpriteEffects.None);
			system.AddToAdditive(DrawLayers.Tiles, data);

			if (_death)
			{
				texture = OrchidAssets.GetExtraTexture(9).Value;
				float progress = 1 - (float)Math.Pow(MathHelper.Lerp(0, 1, _deathProgress), 3);
				color *= progress;
				Vector2 origin = texture.Size() * 0.5f;
				float scale = Projectile.scale * progress;

				data = new DefaultDrawData(texture, drawPos, null, color * 0.6f, 0f, origin, scale, SpriteEffects.None);
				system.AddToAdditive(DrawLayers.Tiles, data);

				data = new DefaultDrawData(texture, drawPos, null, color, 0f, origin, scale * 1.6f, SpriteEffects.None);
				system.AddToAdditive(DrawLayers.Tiles, data);
			}
		}
	}
}
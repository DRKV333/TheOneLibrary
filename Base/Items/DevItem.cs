﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace TheOneLibrary.Base.Items
{
	public class ThePreciousOne : BaseItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Precious One");
			Tooltip.SetDefault("Dev-only item, use at your own risk");
		}

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 20;
			item.accessory = true;
			item.expert = true;
			item.rare = 12;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.name.Contains("Itorius") || player.name == "TMLChar")
			{
				player.AddBuff(BuffID.Spelunker, 1);
				player.statLife = player.statLifeMax2;
				player.statMana = player.statManaMax2;
				player.endurance += 1;
				player.statDefense += 100;
				player.rangedDamage *= 50;
				player.meleeDamage *= 50;
				player.magicDamage *= 50;
				player.minionDamage *= 50;
				player.maxMinions += 49;
				player.wallSpeed *= 100;
				for (int i = -10; i <= 10; i++)
				{
					for (int j = -10; j <= 10; j++)
					{
						Lighting.AddLight(player.Center - new Vector2(i * 16, j * 16), Vector3.One);
					}
				}
			}
			else
			{
				player.AddBuff(BuffID.Electrified, 36000);
				player.AddBuff(BuffID.Venom, 36000);
				player.AddBuff(BuffID.Blackout, 36000);
			}
		}
	}
}
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TheOneLibrary.Base.Items;
using TheOneLibrary.Layer.Layer;

namespace TheOneLibrary.Layer.Items
{
	public class LayerTool : BaseItem
	{
		public override bool CloneNewInstances => true;

		public override ModItem Clone(Item item)
		{
			LayerTool clone = (LayerTool)base.Clone(item);
			clone.mode = mode;
			return clone;
		}

		public enum Mode
		{
			Place,
			Cut,
			Modify,
			Info
		}

		public Mode mode;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Layer Tool");
			Tooltip.SetDefault("Allows you to modify a layer");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.rare = 6;
			item.useStyle = 1;
			item.useTime = 20;
			item.useAnimation = 20;
			item.noMelee = true;
			item.autoReuse = true;
			item.useTurn = true;
		}

		public const string InfoColor = "00B19C";

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "CurrentMode", $"Current mode: [c/00d948:{mode}]"));
			if (Main.keyState.GetPressedKeys().Any(x => x == Keys.RightShift))
			{
				tooltips.Add(new TooltipLine(mod, "ModeInfo0", $"[c/{InfoColor}:{Mode.Place}]: Allows you to place components"));
				tooltips.Add(new TooltipLine(mod, "ModeInfo1", $"[c/{InfoColor}:{Mode.Cut}]: Allows you to remove components"));
				tooltips.Add(new TooltipLine(mod, "ModeInfo2", $"[c/{InfoColor}:{Mode.Info}]: Allows you to get info about the layer"));
			}
			else tooltips.Add(new TooltipLine(mod, "ShiftInfo", "Press [c/f2ff82:RSHIFT] to display additional info"));
		}

		public bool Cut(Point16 mouse, Player player)
		{
			CustomDictionary<ILayerElement> elements = LayerManager.ActiveLayer?.GetElements();

			if (elements != null && elements.ContainsKey(mouse))
			{
				LayerManager.ActiveLayer.Remove(mouse, player);

				return true;
			}

			return false;
		}

		public bool Place(Point16 mouse, Player player)
		{
			CustomDictionary<ILayerElement> elements = LayerManager.ActiveLayer?.GetElements();

			if (elements != null && !elements.ContainsKey(mouse))
			{
				LayerManager.ActiveLayer.Place(mouse, player);

				return true;
			}

			return false;
		}

		public bool Modify(Point16 mouse)
		{
			CustomDictionary<ILayerElement> elements = LayerManager.ActiveLayer?.GetElements();

			if (elements != null && elements.ContainsKey(mouse))
			{
				LayerManager.ActiveLayer.Modify(mouse);

				return true;
			}

			return false;
		}

		public bool Info(Point16 mouse)
		{
			CustomDictionary<ILayerElement> elements = LayerManager.ActiveLayer?.GetElements();

			if (elements != null && elements.ContainsKey(mouse))
			{
				LayerManager.ActiveLayer.Info(mouse);

				return true;
			}

			return false;
		}

		public override bool UseItem(Player player)
		{
			Point16 mouse = Utility.Utility.MouseToWorldPoint();

			if (Vector2.Distance(mouse.ToVector2() * 16, player.Center) <= 320f)
			{
				switch (mode)
				{
					case Mode.Cut:
						return Cut(mouse, player);
					case Mode.Place:
						return Place(mouse, player);
					case Mode.Modify:
						return Modify(mouse);
					case Mode.Info:
						return Info(mouse);
				}
			}

			return false;
		}

		public override TagCompound Save() => new TagCompound
		{
			["Mode"] = (int)mode
		};

		public override void Load(TagCompound tag)
		{
			mode = (Mode)tag.GetInt("Mode");
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write((int)mode);
		}

		public override void NetRecieve(BinaryReader reader)
		{
			mode = (Mode)reader.ReadInt32();
		}
	}
}
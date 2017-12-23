using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ObjectData;

namespace TheOneLibrary.Utility
{
	public static partial class Utility
	{
		public static Vector2 MouseToWorldVector() => new Vector2((int) (Main.MouseWorld.X / 16), (int) (Main.MouseWorld.Y / 16));

		public static Point16 MouseToWorldPoint() => new Point16((int) (Main.MouseWorld.X / 16), (int) (Main.MouseWorld.Y / 16));

		public static bool AnyTowerLiving => NPC.TowerActiveNebula || NPC.TowerActiveSolar || NPC.TowerActiveStardust || NPC.TowerActiveVortex;

		public static Rectangle GetRectangle(this Item item) => Main.itemAnimations[item.type] != null ? Main.itemAnimations[item.type].GetFrame(Main.itemTexture[item.type]) : Main.itemTexture[item.type].Frame(1, 1, 0, 0);

		public static bool TopLeft(this Tile tile)
		{
			int style = 0;
			int alt = 0;
			TileObjectData.GetTileInfo(tile, ref style, ref alt);
			TileObjectData data = TileObjectData.GetTileData(tile.type, style, alt);

			if (data != null) return tile.frameX % (data.Width * 18) == 0 && tile.frameY % (data.Height * 18) == 0;

			return true;
		}

		#region Tracing

		public static List<Point16> CheckNeighbours(int width = 1, int height = 1)
		{
			List<Point16> list = new List<Point16>();

			for (int i = 0; i < height; i++)
			{
				list.Add(new Point16(-1, i));
				list.Add(new Point16(width, i));
			}
			for (int i = 0; i < width; i++)
			{
				list.Add(new Point16(i, -1));
				list.Add(new Point16(i, height));
			}

			return list;
		}

		public static IEnumerable<Point16> AdjencentTiles(Point16 point, Func<Tile, bool> selector)
		{
			List<Point16> points = new List<Point16>();

			foreach (Point16 add in CheckNeighbours())
			{
				Point16 check = point + add;
				Tile tile = Main.tile[check.X, check.Y];

				if (selector.Invoke(tile)) points.Add(check);
			}
			return points;
		}

		public static void Trace(Point16 start, Func<Tile, bool> selector, Action<Tile> OnExplore = null)
		{
			HashSet<Point16> explored = new HashSet<Point16>();
			explored.Add(start);
			Queue<Point16> toExplore = new Queue<Point16>();
			foreach (Point16 point in AdjencentTiles(start, selector)) toExplore.Enqueue(point);

			while (toExplore.Count > 0)
			{
				Point16 explore = toExplore.Dequeue();
				if (!explored.Contains(explore))
				{
					explored.Add(explore);
					OnExplore?.Invoke(Main.tile[explore.X, explore.Y]);

					foreach (Point16 point in AdjencentTiles(explore, selector)) toExplore.Enqueue(point);
				}
			}
		}

		#endregion
	}
}
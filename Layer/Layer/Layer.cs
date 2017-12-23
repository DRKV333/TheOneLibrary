using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace TheOneLibrary.Layer.Layer
{
	public interface ILayerElement
	{
		void Draw(SpriteBatch spriteBatch, Vector2 position);

		ElementInfo GetInfo();
	}

	public interface ILayer
	{
		TagCompound Save();

		void Load(TagCompound tag);

		/// <summary>
		///     Called in ModWorld.PostDrawTiles
		/// </summary>
		void Draw();

		/// <summary>
		///     Called in ModWorld.PostDrawTiles after Draw is called
		/// </summary>
		void DrawPreview();

		/// <summary>
		///     Called in ModWorld.PreUpdate
		/// </summary>
		void Update();

		/// <summary>
		///     Used for placing elements, automatically checks if element is not present in the layer
		/// </summary>
		/// <param name="mouse">Current mouse position (world coords)</param>
		void Place(Point16 mouse, Player player);

		/// <summary>
		///     Used for removing elements, automatically checks if element is present in the layer
		/// </summary>
		/// <param name="mouse">Current mouse position (world coords)</param>
		void Remove(Point16 mouse, Player player);

		/// <summary>
		///     Used for placing elements, automatically checks if element is present in the layer
		///     Leave empty if you don't want to use it
		/// </summary>
		/// <param name="mouse">Current mouse position (world coords)</param>
		void Modify(Point16 mouse);

		/// <summary>
		///     Used for getting info, automatically checks if element is present in the layer
		/// </summary>
		/// <param name="mouse">Current mouse position (world coords)</param>
		void Info(Point16 mouse);

		CustomDictionary<ILayerElement> GetElements();

		LayerInfo GetInfo();
	}
}
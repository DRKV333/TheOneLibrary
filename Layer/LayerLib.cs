using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TheOneLibrary.Layer.Layer;

namespace TheOneLibrary.Layer
{
	public class LayerLibWorld : ModWorld
	{
		public override TagCompound Save()
		{
			LayerManager.layers.Clear();

			return null;
		}

		public override void PreUpdate()
		{
			foreach (KeyValuePair<string, ILayer> layer in LayerManager.layers) layer.Value.Update();
		}

		public override void PostDrawTiles()
		{
            try
            {
                RasterizerState rasterizer = Main.gameMenu || Main.LocalPlayer.gravDir == 1.0 ? RasterizerState.CullCounterClockwise : RasterizerState.CullClockwise;
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

                foreach (KeyValuePair<string, ILayer> layer in LayerManager.layers) layer.Value.Draw();

                if (LayerManager.ActiveLayer != null && Vector2.Distance(Main.LocalPlayer.Center, Utility.Utility.MouseToWorldVector() * 16) <= 320f &&
                    LayerManager.ActiveLayer.GetInfo().DrawPreview) LayerManager.ActiveLayer.DrawPreview();

                Main.spriteBatch.End();
            }
            catch (System.Exception ex)
            {
                ErrorLogger.Log(ex);
            }
		}
	}

	public class LayerManager
	{
		public static Dictionary<string, ILayer> layers = new Dictionary<string, ILayer>();
		public static Dictionary<string, Texture2D> icons = new Dictionary<string, Texture2D>();
		public static Dictionary<string, string> names = new Dictionary<string, string>();

		public static ILayer ActiveLayer;

		public static void RegisterLayer(ILayer layer)
		{
			string TypeName = layer.GetType().FullName;
			layers[TypeName] = layer;

			LayerInfo info = layer.GetInfo();
			names[TypeName] = info.Name;
			icons[TypeName] = ModLoader.GetTexture(info.Texture);
		}
	}
}
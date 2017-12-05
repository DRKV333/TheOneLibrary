using Terraria.GameContent.UI.Elements;
using TheOneLibrary.Base.UI;
using TheOneLibrary.UI.Elements;

namespace TheOneLibrary.Layer
{
	public class LayerDisplayUI : BaseUI
	{
		public UIText textLayer = new UIText("");
		public UIButton imageLayer = new UIButton(null);

		public override void OnInitialize()
		{
			panelMain.Width.Pixels = 200;
			panelMain.Height.Pixels = 56;
			panelMain.HAlign = 1;
			panelMain.VAlign = 1;
			panelMain.SetPadding(0);
			panelMain.BackgroundColor = panelColor;
			Append(panelMain);

			textLayer.VAlign = 0.5f;
			textLayer.Left.Pixels = 56;
			panelMain.Append(textLayer);

			imageLayer.Left.Pixels = 8;
			imageLayer.Top.Pixels = 8;
			imageLayer.Height.Pixels = 40;
			imageLayer.Width.Pixels = 40;
			panelMain.Append(imageLayer);
		}

		public void ChangeLayer()
		{
			string TypeName = LayerManager.ActiveLayer.GetType().FullName;
			textLayer.SetText(LayerManager.names[TypeName]);
			textLayer.Recalculate();
			imageLayer.texture = LayerManager.icons[TypeName];
		}
	}
}
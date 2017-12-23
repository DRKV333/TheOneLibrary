using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.UI;
using Terraria.UI.Chat;
using TheOneLibrary.Base;
using TheOneLibrary.Base.Items;
using TheOneLibrary.Base.UI;

namespace TheOneLibrary.Utility
{
	public static partial class Utility
	{
		public static string MouseText;

		public static readonly Regex colorGetText = new Regex(@"(?<=\[c\/\w{6}:)[^]]*(?=\])");
		public static readonly Regex colorGetTag = new Regex(@"\[c\/\w{6}:[^]]*\]");

		public static readonly Color COLOR_PLATINUM = new Color(220, 220, 198);
		public static readonly Color COLOR_GOLD = new Color(224, 201, 92);
		public static readonly Color COLOR_SILVER = new Color(181, 192, 193);
		public static readonly Color COLOR_COPPER = new Color(246, 138, 96);
		public static readonly Color COLOR_NOCOIN = new Color(120, 120, 120);

		public const float opacityActive = 1f;
		public const float opacityInactive = 0.6f;

		public const BindingFlags defaultFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

		public enum Facing
		{
			Left,
			Right,
			Up,
			Down
		}

		#region Tile Entity
		public static Point16 TileEntityTopLeft(int i, int j)
		{
			if (i <= Main.maxTilesX && j <= Main.maxTilesY)
			{
				Tile tile = Main.tile[i, j];

				int fX = 0;
				int fY = 0;

				if (tile != null)
				{
					TileObjectData data = TileObjectData.GetTileData(tile.type, 0);
					if (data != null)
					{
						fX = tile.frameX % (18 * data.Width) / 18;
						fY = tile.frameY % (18 * data.Height) / 18;
					}
				}

				return new Point16(i - fX, j - fY);
			}
			return Point16.Zero;
		}

		public static int GetID<T>(this Mod mod, int i, int j) where T : ModTileEntity
		{
			Point16 topLeft = TileEntityTopLeft(i, j);
			return mod.GetTileEntity<T>().Find(topLeft.X, topLeft.Y);
		}

		public static int GetID<T>(this Mod mod, Point16 point) where T : ModTileEntity => GetID<T>(mod, point.X, point.Y);

		public static void DrawInfo<T>(this Mod mod, int i, int j) where T : BaseTE
		{
			int ID = mod.GetID<T>(i, j);
			if (ID == -1) return;

			((BaseTE) TileEntity.ByID[ID]).drawInfo = true;
		}

		public static TileObjectDirection GetDirection(int i, int j, int type)
		{
			Tile tile = Main.tile[i, j];
			int style = 0;
			int alt = 0;
			TileObjectData.GetTileInfo(tile, ref style, ref alt);
			return TileObjectData.GetTileData(type, style, alt).Direction;
		}

		#endregion

		#region UI
		public static void DrawPanel(this SpriteBatch spriteBatch, Rectangle dimensions, Texture2D texture, Color color)
		{
			Point point = new Point(dimensions.X, dimensions.Y);
			Point point2 = new Point(point.X + dimensions.Width - 12, point.Y + dimensions.Height - 12);
			int width = point2.X - point.X - 12;
			int height = point2.Y - point.Y - 12;
			spriteBatch.Draw(texture, new Rectangle(point.X, point.Y, 12, 12), new Rectangle(0, 0, 12, 12), color);
			spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y, 12, 12), new Rectangle(16, 0, 12, 12), color);
			spriteBatch.Draw(texture, new Rectangle(point.X, point2.Y, 12, 12), new Rectangle(0, 16, 12, 12), color);
			spriteBatch.Draw(texture, new Rectangle(point2.X, point2.Y, 12, 12), new Rectangle(16, 16, 12, 12), color);
			spriteBatch.Draw(texture, new Rectangle(point.X + 12, point.Y, width, 12), new Rectangle(12, 0, 4, 12), color);
			spriteBatch.Draw(texture, new Rectangle(point.X + 12, point2.Y, width, 12), new Rectangle(12, 16, 4, 12), color);
			spriteBatch.Draw(texture, new Rectangle(point.X, point.Y + 12, 12, height), new Rectangle(0, 12, 12, 4), color);
			spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y + 12, 12, height), new Rectangle(16, 12, 12, 4), color);
			spriteBatch.Draw(texture, new Rectangle(point.X + 12, point.Y + 12, width, height), new Rectangle(12, 12, 4, 4), color);
		}

		public static void DrawPanel(this SpriteBatch spriteBatch, CalculatedStyle dimensions, Texture2D texture, Color color) => spriteBatch.DrawPanel(dimensions.ToRectangle(), texture, color);

		public static void Draw(this ICollection<GUI> UI)
		{
			List<GUI> uis = UI.ToList();
			for (int i = 0; i < uis.Count; i++)
			{
				if (uis[i].key.visible)
				{
					uis[i].value.Update(Main._drawInterfaceGameTime);
					uis[i].key.Draw(Main.spriteBatch);
				}
			}
		}

		public static void DrawMouseText(object text) => MouseText = text.ToString();

		public static Color FromInt(this int value) => value == 0 ? Color.White : new Color((value >> 16) & 0xff, (value >> 8) & 0xff, (value >> 0) & 0xff);

		public static bool IsKeyDown(this Keys key) => Main.keyState.IsKeyDown(key) && !Main.oldKeyState.IsKeyDown(key);

		public static bool IsKeyDown(this int key) => IsKeyDown((Keys) key);

		public static void HandleUIFar(this ModTileEntity tileEntity, int distance = 320)
		{
			Dictionary<ModTileEntity, GUI> UIs = tileEntity.mod.GetTEUIs();

			if (UIs != null)
			{
				Tile tile = Main.tile[tileEntity.Position.X, tileEntity.Position.Y];

				int offsetX = 0;
				int offsetY = 0;

				if (tile != null)
				{
					TileObjectData data = TileObjectData.GetTileData(tile.type, 0);
					if (data != null)
					{
						offsetX = data.Width * 8;
						offsetY = data.Height * 8;
					}
				}

				Vector2 center = new Vector2(tileEntity.Position.X * 16 + offsetX, tileEntity.Position.Y * 16 + offsetY);
				if (Vector2.Distance(Main.LocalPlayer.Center, center) >= distance && UIs.ContainsKey(tileEntity)) UIs.Remove(tileEntity);
			}
		}

		internal static Dictionary<ModTileEntity, GUI> GetTEUIs(this Mod mod)
		{
			List<FieldInfo> fields = mod.GetType().GetFields().ToList();
			FieldInfo teUI = fields.FirstOrDefault(x => x.HasAttribute<UIAttribute>() && ((UIAttribute) x.GetCustomAttributes().First(y => y.GetType() == typeof(UIAttribute))).Name == "TileEntity");
			return teUI != null && teUI.FieldType == typeof(Dictionary<ModTileEntity, GUI>) ? (Dictionary<ModTileEntity, GUI>) teUI.GetValue(mod) : null;
		}

		public static void HandleUI<T>(this Mod mod, int ID) where T : BaseUI
		{
			Dictionary<ModTileEntity, GUI> UIs = mod.GetTEUIs();

			if (UIs != null && ID >= 0)
			{
				ModTileEntity tileEntity = (ModTileEntity) TileEntity.ByID[ID];
				if (!UIs.ContainsKey(tileEntity)) tileEntity.OpenUI<T>();
				else mod.CloseUI(ID);
			}
		}

		public static void OpenUI<T>(this ModTileEntity tileEntity) where T : BaseUI
		{
			Dictionary<ModTileEntity, GUI> UIs = tileEntity.mod.GetTEUIs();

			if (UIs != null)
			{
				BaseUI ui = Activator.CreateInstance<T>();
				((ITileEntityUI) ui).SetTileEntity(tileEntity);
				UserInterface userInterface = new UserInterface();
				ui.Activate();
				ui.visible = true;
				ui.Load();
				userInterface.SetState(ui);
				UIs.Add(tileEntity, new GUI(ui, userInterface));
			}
		}

		public static void CloseUI(this Mod mod, int ID)
		{
			Dictionary<ModTileEntity, GUI> UIs = mod.GetTEUIs();

			if (UIs != null && ID >= 0)
			{
				ModTileEntity tileEntity = (ModTileEntity) TileEntity.ByID[ID];
				if (UIs.ContainsKey(tileEntity)) UIs.Remove(tileEntity);
			}
		}

		public static Vector2 RequiredSlotArea(int columns, int rows, int slotSize = 40, int padding = 8) => new Vector2(columns * slotSize + (columns - 1) * padding, rows * slotSize + (rows - 1) * padding);

		public static void Center(this UIElement element)
		{
			element.HAlign = 0.5f;
			element.VAlign = 0.5f;
		}

		public static LocalizedText TextFromTranslation(this ModTranslation translation)
		{
			Type type = typeof(LocalizedText);
			return (LocalizedText) type.Assembly.CreateInstance(type.FullName, false, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] {translation.Key, translation.GetTranslation(Language.ActiveCulture.LegacyId)}, null, null);
		}

		public static void EnableScissor(this SpriteBatch spriteBatch)
		{
			RasterizerState state = new RasterizerState {ScissorTestEnable = true};

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, null, state, null, Main.UIScaleMatrix);
		}

		public static void DisableScissor(this SpriteBatch spriteBatch)
		{
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, null, null, null, Main.UIScaleMatrix);
		}

		public static void SetupForShader(this SpriteBatch spriteBatch, Effect shader)
		{
			RasterizerState state = new RasterizerState {ScissorTestEnable = true};

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, state, shader, Main.UIScaleMatrix);
		}

		#endregion

		#region Inventory
		public static bool HasItem(this Player player, int type, int stack)
		{
			int count = player.inventory.Where(t => type == t.type).Sum(t => t.stack);
			return count >= stack;
		}

		public static bool HasItems(this Player player, List<Item> items) => items.All(t => player.HasItem(t.type, t.stack));

		public static void ConsumeItem(this Player player, int type, int stack)
		{
			for (int i = 0; i < player.inventory.Length; i++)
			{
				if (player.inventory[i].stack >= stack && player.inventory[i].type == type)
				{
					player.inventory[i].stack -= stack;
					if (player.inventory[i].stack <= 0) player.inventory[i].SetDefaults(0);
				}
			}
		}

		public static bool ConsumeItems(this Player player, List<Item> items)
		{
			if (player.HasItems(items))
			{
				for (int i = 0; i < items.Count; i++) player.ConsumeItem(items[i].type, items[i].stack);
				return true;
			}
			return false;
		}

		public static void SpawnItems(this Player player, List<Item> items)
		{
			for (int i = 0; i < items.Count; i++) Item.NewItem(player.position, player.Size, items[i].type, items[i].stack, noGrabDelay: true);
		}

		public static bool HasSpace(List<Item> items, Item item) => items.FindAll(x => x.type == item.type).Select(x => x.maxStack - x.stack).Sum(x => x) >= item.stack || items.Any(t => t.IsAir);

		public static void InsertItem(List<Item> from, List<Item> to) => from.ForEach(x => InsertItem(x, to));

		public static void InsertItem(Item item, List<Item> to)
		{
			for (int i = 0; i < to.Count; i++)
			{
				if (to[i].type == item.type)
				{
					int count = Math.Min(item.stack, to[i].maxStack - to[i].stack);
					item.stack -= count;
					if (item.stack <= 0) item.TurnToAir();
					to[i].stack += count;
				}
			}

			while (!item.IsAir && to.Any(x => x.IsAir))
			{
				Item next = to.FirstOrDefault(x => x.IsAir);
				if (next != null)
				{
					next.SetDefaults(item.type);
					int count = Math.Min(item.maxStack, item.stack);
					item.stack -= count;
					next.stack = count;
				}
				if (item.stack <= 0) item.TurnToAir();
			}
		}

		public static int GetItemValue(int type)
		{
			Item item = new Item();
			item.SetDefaults(type);
			return item.value;
		}

		public static IEnumerable<int> Armor => Main.LocalPlayer.armor.Where((x, i) => i > 0 && i < 3).Select(x => x.type);

		public static IEnumerable<int> Accessory => Main.LocalPlayer.armor.Where((x, i) => i >= 3 && i < 8 + Main.LocalPlayer.extraAccessorySlots).Select(x => x.type);

		public static List<Item> ArmorItems => Main.LocalPlayer.armor.Where((x, i) => i > 0 && i < 3).ToList();

		public static List<Item> AccessoryItems => Main.LocalPlayer.armor.Where((x, i) => i >= 3 && i < 8 + Main.LocalPlayer.extraAccessorySlots).ToList();

		public static bool HasArmor(int type) => Armor.Contains(type);

		public static bool HasAccessory(int type) => Accessory.Contains(type);

		public static Item HeldItem => Main.mouseItem.IsAir ? Main.LocalPlayer.HeldItem : Main.mouseItem;

		public static bool HasWrench => HeldItem.modItem is Wrench;

		public static bool IsCoin(this Item item) => item.type == ItemID.CopperCoin || item.type == ItemID.SilverCoin || item.type == ItemID.GoldCoin || item.type == ItemID.PlatinumCoin;

		#endregion

		#region Reflection

		public static T GetField<T>(this Type type, string name, object obj = null, BindingFlags flags = defaultFlags) => (T) type.GetField(name, flags)?.GetValue(obj);

		public static void SetField(this Type type, string name, object value, object obj = null, BindingFlags flags = defaultFlags) => type.GetField(name, flags)?.SetValue(obj, value);

		public static T InvokeMethod<T>(this Type type, string name, object[] args, object obj = null, BindingFlags flags = defaultFlags) => (T) type.GetMethod(name, flags)?.Invoke(obj, args);

		public static bool HasAttribute<T>(this FieldInfo field) => field.GetCustomAttributes().Any(x => x.GetType() == typeof(T));

		public static bool HasAttribute<T>(this PropertyInfo field) => field.GetCustomAttributes().Any(x => x.GetType() == typeof(T));

		#endregion

		#region Color

		public static Color DoubleLerp(Color c1, Color c2, Color c3, float step) => step < .5f ? Color.Lerp(c1, c2, step * 2f) : Color.Lerp(c2, c3, (step - .5f) * 2f);

		public static string RGBToHex(Color color) => color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");

		#endregion

		#region Collections

		public static bool ContainsAll<T>(this List<T> subset, List<T> superset) => superset.Except(subset).Any();

		public static bool IsEqual<T>(this List<T> list1, List<T> list2) => list1.All(list2.Contains) && list1.Count == list2.Count;

		#endregion

		#region Math

		public static string ToSI(this double d, IFormatProvider format = null)
		{
			char[] incPrefixes = {'k', 'M', 'G', 'T', 'P', 'E', 'Z', 'Y'};
			char[] decPrefixes = {'m', '\u03bc', 'n', 'p', 'f', 'a', 'z', 'y'};

			if (Math.Abs(d) > 0.0)
			{
				int degree = (int) Math.Floor(Math.Log10(Math.Abs(d)) / 3);
				double scaled = d * Math.Pow(1000, -degree);

				char? prefix = null;
				switch (Math.Sign(degree))
				{
					case 1:
						prefix = incPrefixes[degree - 1];
						break;
					case -1:
						prefix = decPrefixes[-degree - 1];
						break;
				}

				return scaled.ToString(format) + prefix;
			}
			return d.ToString(CultureInfo.InvariantCulture);
		}

		public static string ToSI(this float d, IFormatProvider format = null) => ToSI((double) d, format);

		public static string ToSI(this long l, IFormatProvider format = null) => ToSI((double) l, format);

		public static Point16 Min(this Point16 point, Point16 compareTo) => new Point16(point.X > compareTo.X ? compareTo.X : point.X, point.Y > compareTo.Y ? compareTo.Y : point.Y);

		public static long Min(long val1, long val2, long val3) => Math.Min(val1, Math.Min(val2, val3));

		public static int Clamp(int value, int min, int max)
		{
			if (value < min) value = min;
			if (value > max) value = max;
			return value;
		}

		public static long Clamp(long value, long min, long max)
		{
			if (value < min) value = min;
			if (value > max) value = max;
			return value;
		}

		#endregion

		#region Other

		public static bool PointInTriangle(Point point, Point t0, Point t1, Point t2)
		{
			var s = t0.Y * t2.X - t0.X * t2.Y + (t2.Y - t0.Y) * point.X + (t0.X - t2.X) * point.Y;
			var t = t0.X * t1.Y - t0.Y * t1.X + (t0.Y - t1.Y) * point.X + (t1.X - t0.X) * point.Y;

			if (s < 0 != t < 0) return false;

			var A = -t1.Y * t2.X + t0.Y * (t2.X - t1.X) + t0.X * (t1.Y - t2.Y) + t1.X * t2.Y;
			if (A < 0.0)
			{
				s = -s;
				t = -t;
				A = -A;
			}
			return s > 0 && t > 0 && s + t <= A;
		}

		public static bool Contains(this Rectangle rectangle, Point16 point) => rectangle.Contains(point.X, point.Y);

		public static bool Contains(this Rectangle rectangle, Vector2 vector) => rectangle.Contains((int) vector.X, (int) vector.Y);

		public static string GetHotkeyValue(string hotkey)
		{
			Dictionary<string, ModHotKey> hotkeys = typeof(ModLoader).GetField<Dictionary<string, ModHotKey>>("modHotKeys");
			return hotkeys != null && hotkeys.ContainsKey(hotkey) ? hotkeys[hotkey].GetAssignedKeys().First() : string.Empty;
		}

		public static string ReplaceTagWithText(Match m) => colorGetText.Match(colorGetTag.Match(m.Value).Value).Value;

		public static string ExtractText(string withTag) => colorGetTag.Replace(withTag, ReplaceTagWithText);

		public static void Log(this Mod mod, object message, bool timestamp = true) => ErrorLogger.Log($"[{mod.DisplayName}" + (timestamp ? $" | {DateTime.Now}" : "") + $"]{message}");

		public static void DrawOutline(this SpriteBatch spriteBatch, Point16 start, Point16 end, Color color, float lineSize, bool addZero = false)
		{
			float width = Math.Abs(start.X - end.X) * 16 + 16;
			float height = Math.Abs(start.Y - end.Y) * 16 + 16;

			Vector2 position = -Main.screenPosition + start.Min(end).ToVector2() * 16;

			if (addZero)
			{
				Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
				if (Main.drawToScreen) zero = Vector2.Zero;
				position += zero;
			}

			spriteBatch.Draw(Main.magicPixel, position, null, color, 0f, Vector2.Zero, new Vector2(width, lineSize / 1000f), SpriteEffects.None, 0f);
			spriteBatch.Draw(Main.magicPixel, position, null, color, 0f, Vector2.Zero, new Vector2(lineSize, height / 1000f), SpriteEffects.None, 0f);

			spriteBatch.Draw(Main.magicPixel, position + new Vector2(0, height - lineSize), null, color, 0f, Vector2.Zero, new Vector2(width, lineSize / 1000f), SpriteEffects.None, 0f);
			spriteBatch.Draw(Main.magicPixel, position + new Vector2(width - lineSize, 0), null, color, 0f, Vector2.Zero, new Vector2(lineSize, height / 1000f), SpriteEffects.None, 0f);
		}

		public static string Subscript(this int number)
		{
			var intList = number.ToString().Select(digit => int.Parse(digit.ToString()));
			return intList.Aggregate("", (current, i) => current + ("\\u832" + i));
		}

		public static void UnloadNullableTypes(this Mod mod)
		{
			foreach (Type type in mod.Code.GetTypes())
			{
				foreach (FieldInfo info in type.GetFields(defaultFlags))
				{
					if (!type.IsValueType && info.HasAttribute<NullAttribute>())
					{
						info.SetValue(null, null);
						ErrorLogger.Log(type.FullName + ": " + info.Name);
					}
				}

				foreach (PropertyInfo info in type.GetProperties(defaultFlags))
				{
					if (!type.IsValueType && info.HasAttribute<NullAttribute>())
					{
						info.SetValue(null, null);
						ErrorLogger.Log(type.FullName + ": " + info.Name);
					}
				}
			}
		}

		#endregion

		public static ModHotKey Register(this Mod mod, string name, Keys key) => ModLoader.RegisterHotKey(mod, name, key.ToString());

		public static void DrawMouseText()
		{
			Main.LocalPlayer.showItemIcon = false;
			Main.ItemIconCacheUpdate(0);
			Main.mouseText = true;

			PlayerInput.SetZoom_UI();
			int hackedScreenWidth = Main.screenWidth;
			int hackedScreenHeight = Main.screenHeight;
			int hackedMouseX = Main.mouseX;
			int hackedMouseY = Main.mouseY;
			PlayerInput.SetZoom_UI();
			PlayerInput.SetZoom_Test();

			if (MouseText == null) return;

			int num = Main.mouseX + 10;
			int num2 = Main.mouseY + 10;
			if (hackedMouseX != -1 && hackedMouseY != -1)
			{
				num = hackedMouseX + 10;
				num2 = hackedMouseY + 10;
			}
			if (Main.ThickMouse)
			{
				num += 6;
				num2 += 6;
			}

			Vector2 vector = Main.fontMouseText.MeasureString(MouseText);
			if (hackedScreenHeight != -1 && hackedScreenWidth != -1)
			{
				if (num + vector.X + 4f > hackedScreenWidth) num = (int) (hackedScreenWidth - vector.X - 4f);
				if (num2 + vector.Y + 4f > hackedScreenHeight) num2 = (int) (hackedScreenHeight - vector.Y - 4f);
			}
			else
			{
				if (num + vector.X + 4f > Main.screenWidth) num = (int) (Main.screenWidth - vector.X - 4f);
				if (num2 + vector.Y + 4f > Main.screenHeight) num2 = (int) (Main.screenHeight - vector.Y - 4f);
			}

			Color baseColor = new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor);

			ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, MouseText, new Vector2(num, num2), baseColor, 0f, Vector2.Zero, Vector2.One);

			MouseText = null;
		}

		public static void ModifyText(this TooltipLine line, string text)
		{
			line.text = text;
		}

		public static Vector2 Measure(this string text, DynamicSpriteFont font = null)
		{
			if (font == null) font = Main.fontMouseText;
			return font.MeasureString(text);
		}
	}
}
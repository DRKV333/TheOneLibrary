using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheOneLibrary.Base
{
	public static class NetUtility
	{
		public static void HandlePacket(BinaryReader reader, int sender)
		{
			MessageType type = (MessageType)reader.ReadByte();
			if (type == MessageType.ClientSendTEUpdate) ReceiveClientSendTEUpdate(reader, sender);
		}

		public static void SendUpdate(this ModTileEntity tileEntity)
		{
			if (Main.netMode != NetmodeID.Server) return;
			NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, tileEntity.ID, tileEntity.Position.X, tileEntity.Position.Y);
		}

		public static void ClientSendTEUpdate(this Mod mod, int ID)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				ModPacket packet = mod.GetPacket();
				packet.Write((byte)MessageType.ClientSendTEUpdate);
				packet.Write(ID);
				TileEntity.Write(packet, TileEntity.ByID[ID], true);
				packet.Send();
			}
		}
		
		public static void ReceiveClientSendTEUpdate(BinaryReader reader, int sender)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				int id = reader.ReadInt32();
				TileEntity ent = TileEntity.Read(reader, true);
				ent.ID = id;
				TileEntity.ByID[id] = ent;
				TileEntity.ByPosition[ent.Position] = ent;
				NetMessage.SendData(MessageID.TileEntitySharing, -1, sender, null, id, ent.Position.X, ent.Position.Y);
			}
		}

		public static void SendChatMessage(object message, Color? color = null)
		{
			if (Main.netMode == NetmodeID.SinglePlayer) Main.NewText(message, color ?? Color.White);
			else NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(message.ToString()), color ?? Color.White);
		}

		public static void SyncItem(Item item)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				Player player = Main.player[item.owner];
				List<Item> joined = player.inventory.Concat(player.armor).Concat(player.dye).Concat(player.miscEquips).Concat(player.miscDyes).Concat(player.bank.item).Concat(player.bank2.item).Concat(new[] { player.trashItem }).Concat(player.bank3.item).ToList();
				int index = joined.FindIndex(x => x == item);
				if (index < 0) return;
				
				NetMessage.SendData(MessageID.SyncEquipment, number: item.owner, number2: index);
			}
		}
	}

	internal enum MessageType : byte
	{
		ClientSendTEUpdate
	}
}
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheOneLibrary.Base
{
	public static class NetHelper
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

		public static void SyncEntity(byte id, int index)
		{
			if (Main.netMode != NetmodeID.Server) return;
			switch (id)
			{
				case MessageID.SyncItem:
					NetMessage.SendData(MessageID.SyncItem, number: index);
					return;
				case MessageID.SyncNPC:
					NetMessage.SendData(MessageID.SyncNPC, number: index);
					return;
				case MessageID.SyncProjectile:
					NetMessage.SendData(MessageID.SyncProjectile, number: index);
					return;
				default:
					return;
			}
		}
	}

	enum MessageType : byte
	{
		ClientSendTEUpdate
	}
}
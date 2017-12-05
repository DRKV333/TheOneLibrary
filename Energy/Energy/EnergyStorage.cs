using System;
using TheOneLibrary.Utility;

namespace TheOneLibrary.Energy.Energy
{
	public class EnergyStorage : IEnergyStorage
	{
		// Config
		public const string EnergyUnit = "J";
		public const string EnergyUnitPerSecond = "W";
		// ---

		internal long energy;
		internal long capacity;
		internal long maxReceive;
		internal long maxExtract;

		public EnergyStorage(long capacity)
		{
			this.capacity = capacity;
			maxReceive = capacity;
			maxExtract = capacity;
		}

		public EnergyStorage(long capacity, long maxTransfer)
		{
			this.capacity = capacity;
			maxReceive = maxTransfer;
			maxExtract = maxTransfer;
		}

		public EnergyStorage(long capacity, long maxReceive, long maxExtract)
		{
			this.capacity = capacity;
			this.maxReceive = maxReceive;
			this.maxExtract = maxExtract;
		}

		public EnergyStorage SetCapacity(long capacity)
		{
			this.capacity = capacity;
			if (energy > capacity) energy = capacity;

			return this;
		}

		public EnergyStorage AddCapacity(long capacity)
		{
			this.capacity += capacity;
			if (energy > capacity) energy = capacity;

			return this;
		}

		public EnergyStorage SetMaxTransfer(long maxTransfer)
		{
			SetMaxReceive(maxTransfer);
			SetMaxExtract(maxTransfer);
			return this;
		}

		public EnergyStorage SetMaxReceive(long maxReceive)
		{
			this.maxReceive = maxReceive;
			return this;
		}

		public EnergyStorage SetMaxExtract(long maxExtract)
		{
			this.maxExtract = maxExtract;
			return this;
		}

		public long GetMaxReceive() => maxReceive;

		public long GetMaxExtract() => maxExtract;

		public void ModifyEnergyStored(long energy)
		{
			this.energy += energy;

			if (this.energy > capacity) this.energy = capacity;
			else if (this.energy < 0) this.energy = 0;
		}

		public long ReceiveEnergy(long maxReceive)
		{
			long energyReceived = Math.Min(capacity - energy, Math.Min(this.maxReceive, maxReceive));
			energy += energyReceived;

			return energyReceived;
		}

		public long ExtractEnergy(long maxExtract)
		{
			long energyExtracted = Math.Min(energy, Math.Min(this.maxExtract, maxExtract));
			energy -= energyExtracted;

			return energyExtracted;
		}

		public long GetEnergy() => energy;

		public long GetCapacity() => capacity;

		public EnergyStorage GetEnergyStorage() => this;

		public override string ToString() => energy.ToSI() + "/" + capacity.ToSI() + EnergyUnit;
	}
}
namespace KRnD.Source
{
	// This class stores all types of upgrades a part can have.
	public class PartUpgrades
	{
		public int batteryCharge;
		public int efficiencyMult;
		public int converterEfficiency;
		public int crashTolerance;
		public int dryMass;
		public int fuelCapacity;
		public int fuelFlow;
		public int generatorEfficiency;
		public int ispAtm;
		public int ispVac;
		public int maxTemperature;
		public int parachuteStrength;
		public int torqueStrength;
		public int antennaPower;
		public int packetSize;
		public int dataStorage;
		public int resourceHarvester;
		public int maxEnergyTransfer;

		public ConfigNode CreateConfigNode(string name)
		{
			var node = new ConfigNode(name);
			if (packetSize > 0) node.AddValue(StringConstants.PACKET_SIZE, packetSize.ToString());
			if (antennaPower > 0) node.AddValue(StringConstants.ANTENNA_POWER, antennaPower.ToString());
			if (dataStorage > 0) node.AddValue(StringConstants.DATA_STORAGE, dataStorage.ToString());
			if (ispVac > 0) node.AddValue(StringConstants.ISP_VAC, ispVac.ToString());
			if (ispAtm > 0) node.AddValue(StringConstants.ISP_ATM, ispAtm.ToString());
			if (dryMass > 0) node.AddValue(StringConstants.DRY_MASS, dryMass.ToString());
			if (fuelFlow > 0) node.AddValue(StringConstants.FUEL_FLOW, fuelFlow.ToString());
			if (torqueStrength > 0) node.AddValue(StringConstants.TORQUE, torqueStrength.ToString());
			if (efficiencyMult > 0) node.AddValue(StringConstants.CHARGE_RATE, efficiencyMult.ToString());
			if (crashTolerance > 0) node.AddValue(StringConstants.CRASH_TOLERANCE, crashTolerance.ToString());
			if (batteryCharge > 0) node.AddValue(StringConstants.BATTERY_CHARGE, batteryCharge.ToString());
			if (generatorEfficiency > 0) node.AddValue(StringConstants.GENERATOR_EFFICIENCY, generatorEfficiency.ToString());
			if (converterEfficiency > 0) node.AddValue(StringConstants.CONVERTER_EFFICIENCY, converterEfficiency.ToString());
			if (parachuteStrength > 0) node.AddValue(StringConstants.PARACHUTE_STRENGTH, parachuteStrength.ToString());
			if (maxTemperature > 0) node.AddValue(StringConstants.MAX_TEMPERATURE, maxTemperature.ToString());
			if (fuelCapacity > 0) node.AddValue(StringConstants.FUEL_CAPACITY, fuelCapacity.ToString());
			if (resourceHarvester > 0) node.AddValue(StringConstants.RESOURCE_HARVESTER, resourceHarvester.ToString());
			if (maxEnergyTransfer > 0) node.AddValue(StringConstants.ENERGY_TRANSFER, maxEnergyTransfer.ToString());
			return node;
		}

		public static PartUpgrades CreateFromConfigNode(ConfigNode node)
		{
			var upgrade = new PartUpgrades();
			if (node.HasValue(StringConstants.PACKET_SIZE)) upgrade.packetSize = int.Parse(node.GetValue(StringConstants.PACKET_SIZE));
			if (node.HasValue(StringConstants.ANTENNA_POWER)) upgrade.antennaPower = int.Parse(node.GetValue(StringConstants.ANTENNA_POWER));
			if (node.HasValue(StringConstants.DATA_STORAGE)) upgrade.dataStorage = int.Parse(node.GetValue(StringConstants.DATA_STORAGE));
			if (node.HasValue(StringConstants.ISP_VAC)) upgrade.ispVac = int.Parse(node.GetValue(StringConstants.ISP_VAC));
			if (node.HasValue(StringConstants.ISP_ATM)) upgrade.ispAtm = int.Parse(node.GetValue(StringConstants.ISP_ATM));
			if (node.HasValue(StringConstants.DRY_MASS)) upgrade.dryMass = int.Parse(node.GetValue(StringConstants.DRY_MASS));
			if (node.HasValue(StringConstants.FUEL_FLOW)) upgrade.fuelFlow = int.Parse(node.GetValue(StringConstants.FUEL_FLOW));
			if (node.HasValue(StringConstants.TORQUE)) upgrade.torqueStrength = int.Parse(node.GetValue(StringConstants.TORQUE));
			if (node.HasValue(StringConstants.CHARGE_RATE)) upgrade.efficiencyMult = int.Parse(node.GetValue(StringConstants.CHARGE_RATE));
			if (node.HasValue(StringConstants.CRASH_TOLERANCE)) upgrade.crashTolerance = int.Parse(node.GetValue(StringConstants.CRASH_TOLERANCE));
			if (node.HasValue(StringConstants.BATTERY_CHARGE)) upgrade.batteryCharge = int.Parse(node.GetValue(StringConstants.BATTERY_CHARGE));
			if (node.HasValue(StringConstants.GENERATOR_EFFICIENCY)) upgrade.generatorEfficiency = int.Parse(node.GetValue(StringConstants.GENERATOR_EFFICIENCY));
			if (node.HasValue(StringConstants.CONVERTER_EFFICIENCY)) upgrade.converterEfficiency = int.Parse(node.GetValue(StringConstants.CONVERTER_EFFICIENCY));
			if (node.HasValue(StringConstants.PARACHUTE_STRENGTH)) upgrade.parachuteStrength = int.Parse(node.GetValue(StringConstants.PARACHUTE_STRENGTH));
			if (node.HasValue(StringConstants.MAX_TEMPERATURE)) upgrade.maxTemperature = int.Parse(node.GetValue(StringConstants.MAX_TEMPERATURE));
			if (node.HasValue(StringConstants.FUEL_CAPACITY)) upgrade.fuelCapacity = int.Parse(node.GetValue(StringConstants.FUEL_CAPACITY));
			if (node.HasValue(StringConstants.RESOURCE_HARVESTER)) upgrade.resourceHarvester = int.Parse(node.GetValue(StringConstants.RESOURCE_HARVESTER));
			if (node.HasValue(StringConstants.ENERGY_TRANSFER)) upgrade.maxEnergyTransfer = int.Parse(node.GetValue(StringConstants.ENERGY_TRANSFER));
			return upgrade;
		}

		public PartUpgrades Clone()
		{
			return (PartUpgrades) MemberwiseClone();
		}
	}
}

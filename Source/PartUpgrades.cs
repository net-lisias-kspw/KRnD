namespace KRnD.Source
{
	// This class stores all types of upgrades a part can have.
	public class PartUpgrades
	{
		//public const string ISP_VAC = "ispVac";
		//public const string ISP_ATM = "ispAtm";
		//public const string DRY_MASS = "dryMass";
		//public const string FUEL_FLOW = "fuelFlow";
		//public const string TORQUE = "torque";
		//public const string CHARGE_RATE = "chargeRate";
		//public const string CRASH_TOLERANCE = "crashTolerance";
		//public const string BATTERY_CHARGE = "batteryCharge";
		//public const string GENERATOR_EFFICIENCY = "generatorEfficiency";
		//public const string CONVERTER_EFFICIENCY = "converterEfficiency";
		//public const string PARACHUTE_STRENGTH = "parachuteStrength";
		//public const string MAX_TEMPERATURE = "maxTemperature";
		//public const string FUEL_CAPACITY = "fuelCapacity";
		public int batteryCharge;
		public int chargeRate;
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

#if false
		public override string ToString()
		{
			return "KRnDUpgrade(" +
				   Constants.ISP_VAC + ":" + ispVac + "," +
				   Constants.ISP_ATM + ":" + ispAtm + "," +
				   Constants.DRY_MASS + ":" + dryMass + "," +
				   Constants.FUEL_FLOW + ":" + fuelFlow + "," +
				   Constants.TORQUE + ":" + torque + "," +
				   Constants.CHARGE_RATE + ":" + chargeRate + "," +
				   Constants.CRASH_TOLERANCE + ":" + crashTolerance + "," +
				   Constants.BATTERY_CHARGE + ":" + batteryCharge + "," +
				   Constants.GENERATOR_EFFICIENCY + ":" + generatorEfficiency + "," +
				   Constants.CONVERTER_EFFICIENCY + ":" + converterEfficiency + "," +
				   Constants.PARACHUTE_STRENGTH + ":" + parachuteStrength + "," +
				   Constants.MAX_TEMPERATURE + ":" + maxTemperature + "," +
				   Constants.FUEL_CAPACITY + ":" + fuelCapacity +
				   ")";
		}
#endif

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
			if (chargeRate > 0) node.AddValue(StringConstants.CHARGE_RATE, chargeRate.ToString());
			if (crashTolerance > 0) node.AddValue(StringConstants.CRASH_TOLERANCE, crashTolerance.ToString());
			if (batteryCharge > 0) node.AddValue(StringConstants.BATTERY_CHARGE, batteryCharge.ToString());
			if (generatorEfficiency > 0) node.AddValue(StringConstants.GENERATOR_EFFICIENCY, generatorEfficiency.ToString());
			if (converterEfficiency > 0) node.AddValue(StringConstants.CONVERTER_EFFICIENCY, converterEfficiency.ToString());
			if (parachuteStrength > 0) node.AddValue(StringConstants.PARACHUTE_STRENGTH, parachuteStrength.ToString());
			if (maxTemperature > 0) node.AddValue(StringConstants.MAX_TEMPERATURE, maxTemperature.ToString());
			if (fuelCapacity > 0) node.AddValue(StringConstants.FUEL_CAPACITY, fuelCapacity.ToString());
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
			if (node.HasValue(StringConstants.CHARGE_RATE)) upgrade.chargeRate = int.Parse(node.GetValue(StringConstants.CHARGE_RATE));
			if (node.HasValue(StringConstants.CRASH_TOLERANCE)) upgrade.crashTolerance = int.Parse(node.GetValue(StringConstants.CRASH_TOLERANCE));
			if (node.HasValue(StringConstants.BATTERY_CHARGE)) upgrade.batteryCharge = int.Parse(node.GetValue(StringConstants.BATTERY_CHARGE));
			if (node.HasValue(StringConstants.GENERATOR_EFFICIENCY)) upgrade.generatorEfficiency = int.Parse(node.GetValue(StringConstants.GENERATOR_EFFICIENCY));
			if (node.HasValue(StringConstants.CONVERTER_EFFICIENCY)) upgrade.converterEfficiency = int.Parse(node.GetValue(StringConstants.CONVERTER_EFFICIENCY));
			if (node.HasValue(StringConstants.PARACHUTE_STRENGTH)) upgrade.parachuteStrength = int.Parse(node.GetValue(StringConstants.PARACHUTE_STRENGTH));
			if (node.HasValue(StringConstants.MAX_TEMPERATURE)) upgrade.maxTemperature = int.Parse(node.GetValue(StringConstants.MAX_TEMPERATURE));
			if (node.HasValue(StringConstants.FUEL_CAPACITY)) upgrade.fuelCapacity = int.Parse(node.GetValue(StringConstants.FUEL_CAPACITY));
			return upgrade;
		}

		public PartUpgrades Clone()
		{
			var copy = new PartUpgrades
			{
				packetSize = packetSize,
				antennaPower = antennaPower,
				dataStorage = dataStorage,
				ispVac = ispVac,
				ispAtm = ispAtm,
				dryMass = dryMass,
				fuelFlow = fuelFlow,
				torqueStrength = torqueStrength,
				chargeRate = chargeRate,
				crashTolerance = crashTolerance,
				batteryCharge = batteryCharge,
				generatorEfficiency = generatorEfficiency,
				converterEfficiency = converterEfficiency,
				parachuteStrength = parachuteStrength,
				maxTemperature = maxTemperature,
				fuelCapacity = fuelCapacity
			};
			return copy;
		}
	}
}

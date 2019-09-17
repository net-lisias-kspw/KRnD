using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
		public int torque;

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
			if (ispVac > 0) node.AddValue(Constants.ISP_VAC, ispVac.ToString());
			if (ispAtm > 0) node.AddValue(Constants.ISP_ATM, ispAtm.ToString());
			if (dryMass > 0) node.AddValue(Constants.DRY_MASS, dryMass.ToString());
			if (fuelFlow > 0) node.AddValue(Constants.FUEL_FLOW, fuelFlow.ToString());
			if (torque > 0) node.AddValue(Constants.TORQUE, torque.ToString());
			if (chargeRate > 0) node.AddValue(Constants.CHARGE_RATE, chargeRate.ToString());
			if (crashTolerance > 0) node.AddValue(Constants.CRASH_TOLERANCE, crashTolerance.ToString());
			if (batteryCharge > 0) node.AddValue(Constants.BATTERY_CHARGE, batteryCharge.ToString());
			if (generatorEfficiency > 0) node.AddValue(Constants.GENERATOR_EFFICIENCY, generatorEfficiency.ToString());
			if (converterEfficiency > 0) node.AddValue(Constants.CONVERTER_EFFICIENCY, converterEfficiency.ToString());
			if (parachuteStrength > 0) node.AddValue(Constants.PARACHUTE_STRENGTH, parachuteStrength.ToString());
			if (maxTemperature > 0) node.AddValue(Constants.MAX_TEMPERATURE, maxTemperature.ToString());
			if (fuelCapacity > 0) node.AddValue(Constants.FUEL_CAPACITY, fuelCapacity.ToString());
			return node;
		}

		public static PartUpgrades CreateFromConfigNode(ConfigNode node)
		{
			var upgrade = new PartUpgrades();
			if (node.HasValue(Constants.ISP_VAC)) upgrade.ispVac = int.Parse(node.GetValue(Constants.ISP_VAC));
			if (node.HasValue(Constants.ISP_ATM)) upgrade.ispAtm = int.Parse(node.GetValue(Constants.ISP_ATM));
			if (node.HasValue(Constants.DRY_MASS)) upgrade.dryMass = int.Parse(node.GetValue(Constants.DRY_MASS));
			if (node.HasValue(Constants.FUEL_FLOW)) upgrade.fuelFlow = int.Parse(node.GetValue(Constants.FUEL_FLOW));
			if (node.HasValue(Constants.TORQUE)) upgrade.torque = int.Parse(node.GetValue(Constants.TORQUE));
			if (node.HasValue(Constants.CHARGE_RATE)) upgrade.chargeRate = int.Parse(node.GetValue(Constants.CHARGE_RATE));
			if (node.HasValue(Constants.CRASH_TOLERANCE)) upgrade.crashTolerance = int.Parse(node.GetValue(Constants.CRASH_TOLERANCE));
			if (node.HasValue(Constants.BATTERY_CHARGE)) upgrade.batteryCharge = int.Parse(node.GetValue(Constants.BATTERY_CHARGE));
			if (node.HasValue(Constants.GENERATOR_EFFICIENCY)) upgrade.generatorEfficiency = int.Parse(node.GetValue(Constants.GENERATOR_EFFICIENCY));
			if (node.HasValue(Constants.CONVERTER_EFFICIENCY)) upgrade.converterEfficiency = int.Parse(node.GetValue(Constants.CONVERTER_EFFICIENCY));
			if (node.HasValue(Constants.PARACHUTE_STRENGTH)) upgrade.parachuteStrength = int.Parse(node.GetValue(Constants.PARACHUTE_STRENGTH));
			if (node.HasValue(Constants.MAX_TEMPERATURE)) upgrade.maxTemperature = int.Parse(node.GetValue(Constants.MAX_TEMPERATURE));
			if (node.HasValue(Constants.FUEL_CAPACITY)) upgrade.fuelCapacity = int.Parse(node.GetValue(Constants.FUEL_CAPACITY));
			return upgrade;
		}

		public PartUpgrades Clone()
		{
			var copy = new PartUpgrades();
			copy.ispVac = ispVac;
			copy.ispAtm = ispAtm;
			copy.dryMass = dryMass;
			copy.fuelFlow = fuelFlow;
			copy.torque = torque;
			copy.chargeRate = chargeRate;
			copy.crashTolerance = crashTolerance;
			copy.batteryCharge = batteryCharge;
			copy.generatorEfficiency = generatorEfficiency;
			copy.converterEfficiency = converterEfficiency;
			copy.parachuteStrength = parachuteStrength;
			copy.maxTemperature = maxTemperature;
			copy.fuelCapacity = fuelCapacity;
			return copy;
		}
	}
}

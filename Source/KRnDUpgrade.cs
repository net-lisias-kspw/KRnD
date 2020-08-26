using System;

namespace KRnD
{
    // This class stores all types of upgrades a part can have.
    public class KRnDUpgrade
    {
        public int ispVac = 0;
        public int ispAtm = 0;
        public int dryMass = 0;
        public int fuelFlow = 0;
        public int torque = 0;
        public int chargeRate = 0;
        public int crashTolerance = 0;
        public int batteryCharge = 0;
        public int generatorEfficiency = 0;
        public int converterEfficiency = 0;
        public int parachuteStrength = 0;
        public int maxTemperature = 0;
        public int fuelCapacity = 0;
        //public int antPower = 0; // WIP
        public int harvester = 0;
        public int radiatorEfficiency = 0;

        public const String ISP_VAC = "ispVac";
        public const String ISP_ATM = "ispAtm";
        public const String DRY_MASS = "dryMass";
        public const String FUEL_FLOW = "fuelFlow";
        public const String TORQUE = "torque";
        public const String CHARGE_RATE = "chargeRate";
        public const String CRASH_TOLERANCE = "crashTolerance";
        public const String BATTERY_CHARGE = "batteryCharge";
        public const String GENERATOR_EFFICIENCY = "generatorEfficiency";
        public const String CONVERTER_EFFICIENCY = "converterEfficiency";
        public const String PARACHUTE_STRENGTH = "parachuteStrength";
        public const String MAX_TEMPERATURE = "maxTemperature";
        public const String FUEL_CAPACITY = "fuelCapacity";
        //public const String ANTENNA_POWER = "antPower"; // WIP
        public const String DRILL_EFFICIENCY = "harvester";
        public const String RADIATOR_EFFICIENCY = "radiatorEfficiency";

        public override string ToString()
        {
            return "KRnDUpgrade(" +
                ISP_VAC + ":" + this.ispVac.ToString() + "," +
                ISP_ATM + ":" + this.ispAtm.ToString() + "," +
                DRY_MASS + ":" + this.dryMass.ToString() + "," +
                FUEL_FLOW + ":" + this.fuelFlow.ToString() + "," +
                TORQUE + ":" + this.torque.ToString() + "," +
                CHARGE_RATE + ":" + this.chargeRate.ToString() + "," +
                CRASH_TOLERANCE + ":" + this.crashTolerance.ToString() + "," +
                BATTERY_CHARGE + ":" + this.batteryCharge.ToString() + "," +
                GENERATOR_EFFICIENCY + ":" + this.generatorEfficiency.ToString() + "," +
                CONVERTER_EFFICIENCY + ":" + this.converterEfficiency.ToString() + "," +
                PARACHUTE_STRENGTH + ":" + this.parachuteStrength.ToString() + "," +
                MAX_TEMPERATURE + ":" + this.maxTemperature.ToString() + "," +
                FUEL_CAPACITY + ":" + this.fuelCapacity.ToString() + "," +
                //ANTENNA_POWER + ":" + this.antPower.ToString() + "," + // WIP
                DRILL_EFFICIENCY + ":" + this.harvester.ToString() + "," +
                RADIATOR_EFFICIENCY + ":" + this.radiatorEfficiency +
                ")";
        }

        public ConfigNode createConfigNode(string name)
        {
            ConfigNode node = new ConfigNode(name);
            if (this.ispVac > 0) node.AddValue(ISP_VAC, this.ispVac.ToString());
            if (this.ispAtm > 0) node.AddValue(ISP_ATM, this.ispAtm.ToString());
            if (this.dryMass > 0) node.AddValue(DRY_MASS, this.dryMass.ToString());
            if (this.fuelFlow > 0) node.AddValue(FUEL_FLOW, this.fuelFlow.ToString());
            if (this.torque > 0) node.AddValue(TORQUE, this.torque.ToString());
            if (this.chargeRate > 0) node.AddValue(CHARGE_RATE, this.chargeRate.ToString());
            if (this.crashTolerance > 0) node.AddValue(CRASH_TOLERANCE, this.crashTolerance.ToString());
            if (this.batteryCharge > 0) node.AddValue(BATTERY_CHARGE, this.batteryCharge.ToString());
            if (this.generatorEfficiency > 0) node.AddValue(GENERATOR_EFFICIENCY, this.generatorEfficiency.ToString());
            if (this.converterEfficiency > 0) node.AddValue(CONVERTER_EFFICIENCY, this.converterEfficiency.ToString());
            if (this.parachuteStrength > 0) node.AddValue(PARACHUTE_STRENGTH, this.parachuteStrength.ToString());
            if (this.maxTemperature > 0) node.AddValue(MAX_TEMPERATURE, this.maxTemperature.ToString());
            if (this.fuelCapacity > 0) node.AddValue(FUEL_CAPACITY, this.fuelCapacity.ToString());
            //if (this.antPower > 0) node.AddValue(ANTENNA_POWER, this.antPower.ToString()); // WIP
            if (this.harvester > 0) node.AddValue(DRILL_EFFICIENCY, this.harvester.ToString());
            if (this.radiatorEfficiency > 0) node.AddValue(RADIATOR_EFFICIENCY, this.radiatorEfficiency.ToString());
            return node;
        }

        public static KRnDUpgrade createFromConfigNode(ConfigNode node)
        {
            KRnDUpgrade upgrade = new KRnDUpgrade();
            if (node.HasValue(ISP_VAC)) upgrade.ispVac = Int32.Parse(node.GetValue(ISP_VAC));
            if (node.HasValue(ISP_ATM)) upgrade.ispAtm = Int32.Parse(node.GetValue(ISP_ATM));
            if (node.HasValue(DRY_MASS)) upgrade.dryMass = Int32.Parse(node.GetValue(DRY_MASS));
            if (node.HasValue(FUEL_FLOW)) upgrade.fuelFlow = Int32.Parse(node.GetValue(FUEL_FLOW));
            if (node.HasValue(TORQUE)) upgrade.torque = Int32.Parse(node.GetValue(TORQUE));
            if (node.HasValue(CHARGE_RATE)) upgrade.chargeRate = Int32.Parse(node.GetValue(CHARGE_RATE));
            if (node.HasValue(CRASH_TOLERANCE)) upgrade.crashTolerance = Int32.Parse(node.GetValue(CRASH_TOLERANCE));
            if (node.HasValue(BATTERY_CHARGE)) upgrade.batteryCharge = Int32.Parse(node.GetValue(BATTERY_CHARGE));
            if (node.HasValue(GENERATOR_EFFICIENCY)) upgrade.generatorEfficiency = Int32.Parse(node.GetValue(GENERATOR_EFFICIENCY));
            if (node.HasValue(CONVERTER_EFFICIENCY)) upgrade.converterEfficiency = Int32.Parse(node.GetValue(CONVERTER_EFFICIENCY));
            if (node.HasValue(PARACHUTE_STRENGTH)) upgrade.parachuteStrength = Int32.Parse(node.GetValue(PARACHUTE_STRENGTH));
            if (node.HasValue(MAX_TEMPERATURE)) upgrade.maxTemperature = Int32.Parse(node.GetValue(MAX_TEMPERATURE));
            if (node.HasValue(FUEL_CAPACITY)) upgrade.fuelCapacity = Int32.Parse(node.GetValue(FUEL_CAPACITY));
            //if (node.HasValue(ANTENNA_POWER)) upgrade.antPower = Int32.Parse(node.GetValue(ANTENNA_POWER)); //WIP
            if (node.HasValue(DRILL_EFFICIENCY)) upgrade.harvester = Int32.Parse(node.GetValue(DRILL_EFFICIENCY));
            if (node.HasValue(RADIATOR_EFFICIENCY)) upgrade.radiatorEfficiency = Int32.Parse(node.GetValue(RADIATOR_EFFICIENCY));
            return upgrade;
        }

        public KRnDUpgrade clone()
        {
            KRnDUpgrade copy = new KRnDUpgrade();
            copy.ispVac = this.ispVac;
            copy.ispAtm = this.ispAtm;
            copy.dryMass = this.dryMass;
            copy.fuelFlow = this.fuelFlow;
            copy.torque = this.torque;
            copy.chargeRate = this.chargeRate;
            copy.crashTolerance = this.crashTolerance;
            copy.batteryCharge = this.batteryCharge;
            copy.generatorEfficiency = this.generatorEfficiency;
            copy.converterEfficiency = this.converterEfficiency;
            copy.parachuteStrength = this.parachuteStrength;
            copy.maxTemperature = this.maxTemperature;
            copy.fuelCapacity = this.fuelCapacity;
            //copy.antPower = this.antPower; // WIP
            copy.harvester = this.harvester;
            copy.radiatorEfficiency = this.radiatorEfficiency;
            return copy;
        }
    }
}
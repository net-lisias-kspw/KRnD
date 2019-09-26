namespace KRnD.Source
{
	[KSPModule("R&D")]
	public class KRnDModule : PartModule
	{
		[KSPField(isPersistant = true)] public int batteryCharge_upgrades = 0;
		[KSPField(isPersistant = true)] public int chargeRate_upgrades = 0;
		[KSPField(isPersistant = true)] public int converterEfficiency_upgrades = 0;
		[KSPField(isPersistant = true)] public int crashTolerance_upgrades = 0;
		[KSPField(isPersistant = true)] public int dryMass_upgrades = 0;
		[KSPField(isPersistant = true)] public int fuelCapacity_upgrades = 0;
		[KSPField(isPersistant = true)] public int fuelFlow_upgrades = 0;
		[KSPField(isPersistant = true)] public int generatorEfficiency_upgrades = 0;
		[KSPField(isPersistant = true)] public int ispAtm_upgrades = 0;
		[KSPField(isPersistant = true)] public int ispVac_upgrades = 0;
		[KSPField(isPersistant = true)] public int maxTemperature_upgrades = 0;
		[KSPField(isPersistant = true)] public int parachuteStrength_upgrades = 0;
		[KSPField(isPersistant = true)] public int torque_upgrades = 0;
		[KSPField(isPersistant = true)] public int packetSize_upgrades = 0;
		[KSPField(isPersistant = true)] public int antennaPower_upgrades = 0;
		[KSPField(isPersistant = true)] public int dataStorage_upgrades = 0;
		[KSPField(isPersistant = true)] public int resourceHarvester_upgrades = 0;
		[KSPField(isPersistant = true)] public int maxEnergyTransfer_upgrades = 0;

		// Flag, which can be set by other mods to apply latest upgrades on load:
		[KSPField(isPersistant = true)] public int upgradeToLatest = 0;

		[KSPField(guiActive = true, guiName = "R&D", guiUnits = "", guiFormat = "", isPersistant = false)]
		public string moduleVersion;

		public static string ToRoman(int number)
		{
			if (number == 0) return "";
			if (number >= 1000) return "M" + ToRoman(number - 1000);
			if (number >= 900) return "CM" + ToRoman(number - 900);
			if (number >= 500) return "D" + ToRoman(number - 500);
			if (number >= 400) return "CD" + ToRoman(number - 400);
			if (number >= 100) return "C" + ToRoman(number - 100);
			if (number >= 90) return "XC" + ToRoman(number - 90);
			if (number >= 50) return "L" + ToRoman(number - 50);
			if (number >= 40) return "XL" + ToRoman(number - 40);
			if (number >= 10) return "X" + ToRoman(number - 10);
			if (number >= 9) return "IX" + ToRoman(number - 9);
			if (number >= 5) return "V" + ToRoman(number - 5);
			if (number >= 4) return "IV" + ToRoman(number - 4);
			if (number >= 1) return "I" + ToRoman(number - 1);
			return number.ToString();
		}

		public string GetVersion()
		{
			var upgrades =
				dryMass_upgrades +
				packetSize_upgrades +
				antennaPower_upgrades +
				dataStorage_upgrades +
				fuelFlow_upgrades +
				ispVac_upgrades +
				ispAtm_upgrades +
				torque_upgrades +
				chargeRate_upgrades +
				crashTolerance_upgrades +
				batteryCharge_upgrades +
				generatorEfficiency_upgrades +
				converterEfficiency_upgrades +
				parachuteStrength_upgrades +
				maxTemperature_upgrades +
				resourceHarvester_upgrades +
				maxEnergyTransfer_upgrades +
				fuelCapacity_upgrades;
			if (upgrades == 0) return "";
			return "Mk " + ToRoman(upgrades + 1); // Mk I is the part without upgrades, Mk II the first upgraded version.
		}

		public override void OnStart(StartState state)
		{
			moduleVersion = GetVersion();
			if (moduleVersion == "")
				Fields[0].guiActive = false;
			else
				Fields[0].guiActive = true;
		}


		public void ApplyUpgrades(PartUpgrades upgrades_to_apply)
		{
			packetSize_upgrades = upgrades_to_apply.packetSize;
			antennaPower_upgrades = upgrades_to_apply.antennaPower;
			dataStorage_upgrades = upgrades_to_apply.dataStorage;
			dryMass_upgrades = upgrades_to_apply.dryMass;
			fuelFlow_upgrades = upgrades_to_apply.fuelFlow;
			ispVac_upgrades = upgrades_to_apply.ispVac;
			ispAtm_upgrades = upgrades_to_apply.ispAtm;
			torque_upgrades = upgrades_to_apply.torqueStrength;
			chargeRate_upgrades = upgrades_to_apply.efficiencyMult;
			crashTolerance_upgrades = upgrades_to_apply.crashTolerance;
			batteryCharge_upgrades = upgrades_to_apply.batteryCharge;
			generatorEfficiency_upgrades = upgrades_to_apply.generatorEfficiency;
			converterEfficiency_upgrades = upgrades_to_apply.converterEfficiency;
			parachuteStrength_upgrades = upgrades_to_apply.parachuteStrength;
			maxTemperature_upgrades = upgrades_to_apply.maxTemperature;
			fuelCapacity_upgrades = upgrades_to_apply.fuelCapacity;
			resourceHarvester_upgrades = upgrades_to_apply.resourceHarvester;
			maxEnergyTransfer_upgrades = upgrades_to_apply.maxEnergyTransfer;
		}

		// Returns the upgrade-stats which this module represents.
		public PartUpgrades GetCurrentUpgrades()
		{
			return new PartUpgrades
			{
				packetSize = packetSize_upgrades,
				antennaPower = antennaPower_upgrades,
				dataStorage = dataStorage_upgrades,
				dryMass = dryMass_upgrades,
				fuelFlow = fuelFlow_upgrades,
				ispVac = ispVac_upgrades,
				ispAtm = ispAtm_upgrades,
				torqueStrength = torque_upgrades,
				efficiencyMult = chargeRate_upgrades,
				crashTolerance = crashTolerance_upgrades,
				batteryCharge = batteryCharge_upgrades,
				generatorEfficiency = generatorEfficiency_upgrades,
				converterEfficiency = converterEfficiency_upgrades,
				parachuteStrength = parachuteStrength_upgrades,
				maxTemperature = maxTemperature_upgrades,
				fuelCapacity = fuelCapacity_upgrades,
				resourceHarvester = resourceHarvester_upgrades,
				maxEnergyTransfer = maxEnergyTransfer_upgrades
			};
		}
	}
}
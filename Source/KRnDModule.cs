namespace KRnD.Source
{
	[KSPModule("R&D")]
	public class KRnDModule : PartModule
	{
		[KSPField(isPersistant = false)] public float batteryCharge_costScale = 2f;

		[KSPField(isPersistant = false)] public float batteryCharge_costScaleReference = 500f;

		[KSPField(isPersistant = false)] public float batteryCharge_improvement = 0.2f;

		[KSPField(isPersistant = false)] public float batteryCharge_improvementScale = 1f;

		[KSPField(isPersistant = false)] public int batteryCharge_scienceCost = 10;

		// Battery Charge
		[KSPField(isPersistant = true)] public int batteryCharge_upgrades = 0;

		[KSPField(isPersistant = false)] public float chargeRate_costScale = 2f;

		[KSPField(isPersistant = false)] public float chargeRate_improvement = 0.05f;

		[KSPField(isPersistant = false)] public float chargeRate_improvementScale = 1f;

		[KSPField(isPersistant = false)] public int chargeRate_scienceCost = 10;

		// Charge Rate
		[KSPField(isPersistant = true)] public int chargeRate_upgrades = 0;

		[KSPField(isPersistant = false)] public float converterEfficiency_costScale = 2f;

		[KSPField(isPersistant = false)] public float converterEfficiency_improvement = 0.1f;

		[KSPField(isPersistant = false)] public float converterEfficiency_improvementScale = 1f;

		[KSPField(isPersistant = false)] public int converterEfficiency_scienceCost = 15;

		// Converter Efficiency
		[KSPField(isPersistant = true)] public int converterEfficiency_upgrades = 0;

		[KSPField(isPersistant = false)] public float crashTolerance_costScale = 2f;

		[KSPField(isPersistant = false)] public float crashTolerance_improvement = 0.15f;

		[KSPField(isPersistant = false)] public float crashTolerance_improvementScale = 1f;

		[KSPField(isPersistant = false)] public int crashTolerance_scienceCost = 10;

		// Crash Tolerance
		[KSPField(isPersistant = true)] public int crashTolerance_upgrades = 0;

		[KSPField(isPersistant = false)] public float dryMass_costScale = 2f;

		[KSPField(isPersistant = false)] public float dryMass_costScaleReference = 1f;

		[KSPField(isPersistant = false)] public float dryMass_improvement = -0.1f;

		[KSPField(isPersistant = false)] public float dryMass_improvementScale = 1f;

		[KSPField(isPersistant = false)] public int dryMass_scienceCost = 10;

		// Dry Mass
		[KSPField(isPersistant = true)] public int dryMass_upgrades = 0;

		[KSPField(isPersistant = false)] public float fuelCapacity_costScale = 2f;

		[KSPField(isPersistant = false)] public float fuelCapacity_costScaleReference = 1000f;

		[KSPField(isPersistant = false)] public float fuelCapacity_improvement = 0.05f;

		[KSPField(isPersistant = false)] public float fuelCapacity_improvementScale = 1f;

		[KSPField(isPersistant = false)] public int fuelCapacity_scienceCost = 5;

		// Fuel Capacity
		[KSPField(isPersistant = true)] public int fuelCapacity_upgrades = 0;

		[KSPField(isPersistant = false)] public float fuelFlow_costScale = 2f;

		[KSPField(isPersistant = false)] public float fuelFlow_improvement = 0.1f;

		[KSPField(isPersistant = false)] public float fuelFlow_improvementScale = 1f;

		[KSPField(isPersistant = false)] public int fuelFlow_scienceCost = 10;

		// Fuel Flow
		[KSPField(isPersistant = true)] public int fuelFlow_upgrades = 0;

		[KSPField(isPersistant = false)] public float generatorEfficiency_costScale = 2f;

		[KSPField(isPersistant = false)] public float generatorEfficiency_improvement = 0.1f;

		[KSPField(isPersistant = false)] public float generatorEfficiency_improvementScale = 1f;

		[KSPField(isPersistant = false)] public int generatorEfficiency_scienceCost = 15;

		// Generator Efficiency
		[KSPField(isPersistant = true)] public int generatorEfficiency_upgrades = 0;

		[KSPField(isPersistant = false)] public float ispAtm_costScale = 2f;

		[KSPField(isPersistant = false)] public float ispAtm_improvement = 0.05f;

		[KSPField(isPersistant = false)] public float ispAtm_improvementScale = 1f;

		[KSPField(isPersistant = false)] public int ispAtm_scienceCost = 15;

		// ISP Atm
		[KSPField(isPersistant = true)] public int ispAtm_upgrades = 0;

		[KSPField(isPersistant = false)] public float ispVac_costScale = 2f;

		[KSPField(isPersistant = false)] public float ispVac_improvement = 0.05f;

		[KSPField(isPersistant = false)] public float ispVac_improvementScale = 1f;

		[KSPField(isPersistant = false)] public int ispVac_scienceCost = 15;

		// ISP Vac
		[KSPField(isPersistant = true)] public int ispVac_upgrades = 0;

		[KSPField(isPersistant = false)] public float maxTemperature_costScale = 2f;

		[KSPField(isPersistant = false)] public float maxTemperature_improvement = 0.2f;

		[KSPField(isPersistant = false)] public float maxTemperature_improvementScale = 1f;

		[KSPField(isPersistant = false)] public int maxTemperature_scienceCost = 5;

		// Max Skin Temp
		[KSPField(isPersistant = true)] public int maxTemperature_upgrades = 0;

		// Version of this part (just for show):
		[KSPField(guiActive = true, guiName = "R&D", guiUnits = "", guiFormat = "", isPersistant = false)]
		public string moduleVersion;

		[KSPField(isPersistant = false)] public float parachuteStrength_costScale = 2f;

		[KSPField(isPersistant = false)] public float parachuteStrength_improvement = 0.3f;

		[KSPField(isPersistant = false)] public float parachuteStrength_improvementScale = 1f;

		[KSPField(isPersistant = false)] public int parachuteStrength_scienceCost = 10;

		// Parachute Strength
		[KSPField(isPersistant = true)] public int parachuteStrength_upgrades = 0;

		[KSPField(isPersistant = false)] public float torque_costScale = 2f;

		[KSPField(isPersistant = false)] public float torque_improvement = 0.25f;

		[KSPField(isPersistant = false)] public float torque_improvementScale = 1f;

		[KSPField(isPersistant = false)] public int torque_scienceCost = 5;

		// Torque
		[KSPField(isPersistant = true)] public int torque_upgrades = 0;

		// Flag, which can be set by other mods to apply latest upgrades on load:
		[KSPField(isPersistant = true)] public int upgradeToLatest = 0;

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

		public string getVersion()
		{
			var upgrades =
				dryMass_upgrades +
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
				fuelCapacity_upgrades;
			if (upgrades == 0) return "";
			return "Mk " + ToRoman(upgrades + 1); // Mk I is the part without upgrades, Mk II the first upgraded version.
		}

		public override void OnStart(StartState state)
		{
			moduleVersion = getVersion();
			if (moduleVersion == "")
				Fields[0].guiActive = false;
			else
				Fields[0].guiActive = true;
		}

		// Returns the upgrade-stats which this module represents.
		public KRnDUpgrade getCurrentUpgrades()
		{
			var upgrades = new KRnDUpgrade();
			upgrades.dryMass = dryMass_upgrades;
			upgrades.fuelFlow = fuelFlow_upgrades;
			upgrades.ispVac = ispVac_upgrades;
			upgrades.ispAtm = ispAtm_upgrades;
			upgrades.torque = torque_upgrades;
			upgrades.chargeRate = chargeRate_upgrades;
			upgrades.crashTolerance = crashTolerance_upgrades;
			upgrades.batteryCharge = batteryCharge_upgrades;
			upgrades.generatorEfficiency = generatorEfficiency_upgrades;
			upgrades.converterEfficiency = converterEfficiency_upgrades;
			upgrades.parachuteStrength = parachuteStrength_upgrades;
			upgrades.maxTemperature = maxTemperature_upgrades;
			upgrades.fuelCapacity = fuelCapacity_upgrades;
			return upgrades;
		}
	}
}
namespace KRnD.Source
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary> The constants this mod uses that are tightly coupled to the saved-game file. Changing these will
	/// 		  break existing games.</summary>
	public static class StringConstants
	{
		#region Upgrade Section Identifier Strings

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> The battery EC storage capacity.</summary>
		public static string BATTERY_CHARGE = "batteryCharge";


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> The EC production rate of the part. Generally reactors, RTGs, and solar-panels have this.</summary>
		public static string CHARGE_RATE = "chargeRate";


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> The converter efficiency when transforming one resource into another. ISRU parts use this mostly.</summary>
		public static string CONVERTER_EFFICIENCY = "converterEfficiency";


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> The crash tolerance -- the sturdiness of the part with respect to impact damage. Typically, only
		/// 		  landing-legs are upgraded.</summary>
		public static string CRASH_TOLERANCE = "crashTolerance";


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> The dry mass of the part -- part mass less any resources it may contain.</summary>
		public static string DRY_MASS = "dryMass";


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> The fuel (resource) capacity of the part. Typically, this is fuel, but also includes Enriched
		/// 		  Uranium, Ore, etc.</summary>
		public static string FUEL_CAPACITY = "fuelCapacity";


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> The fuel flow which actually translates directly to thrust (by way of the ISP factor).</summary>
		public static string FUEL_FLOW = "fuelFlow";


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> The generator efficiency which mostly applies to fuel-cells.</summary>
		public static string GENERATOR_EFFICIENCY = "generatorEfficiency";


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> The engine ISP in atmosphere.</summary>
		public static string ISP_ATM = "ispAtm";


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> The engine ISP in vacuum.</summary>
		public static string ISP_VAC = "ispVac";


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> The maximum part temperature it can take before destruction. Directly relates to atmosphere
		/// 		  reentry survivability.</summary>
		public static string MAX_TEMPERATURE = "maxTemperature";


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> The parachute strength controls the maximum speed the parachute can deploy at without being
		/// 		  instantly destroyed.</summary>
		public static string PARACHUTE_STRENGTH = "parachuteStrength";


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> The torque is the reaction-wheel strength.</summary>
		public static string TORQUE = "torque";
		#endregion


		#region Upgrade Equation Constant Identifiers

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> The science cost for upgrading the part one level. The cost increases geometrically for
		/// 		  subsequent upgrades. The rate of increase is controlled by the "costRate" constant.</summary>
		public static string SCIENCE_COST = "scienceCost";


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> The cost divisor is used to scale the science cost of an upgrade with the magnitude of the
		/// 		  original stat being upgraded. In some cases, it doesn't matter what the original stat value
		/// 		  was -- the cost is always the same. In such cases, the costDivisor should be set to 0. In
		/// 		  other cases, such as with fuel-tanks, the science cost is greatly affected by the magnitude
		/// 		  of the original stat. In such cases, the value will be greater than zero. The original stat
		/// 		  is divided by the costDivisor and the science cost is multiplied by this value. For example,
		/// 		  the costDivisor for fuel capacity is 500. This means a small capacity tank, like one that
		/// 		  holds 50 units, costs 1/10th the science points that a 500 unit fuel tank does per upgrade
		/// 		  level.</summary>
		public static string COST_DIVISOR = "costDivisor";


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> The improvement stat specifies how much the part stat improves (either increases or decreases)
		/// 		  per upgrade level. There is a hard limit that a part stat can never go below 10% or above
		/// 		  400% of the original part stat. For example, a value of -0.2f means the part stat gets
		/// 		  smaller by 20% per upgrade level. A value of 0.1f means the part stat gets larger by 10% per
		/// 		  upgrade level. The actual upgrade amount is subject to diminishing returns if the
		/// 		  "improvementRate" constant is less than 1.</summary>
		public static string IMPROVEMENT = "improvement";
		#endregion


		#region Common Equation Constant Identifiers

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> The cost rate specifies the rate that the science-cost increases per upgrade level. A value of 2,
		/// 		  for example, would mean the cost doubles for each upgrade level.</summary>
		public static string COST_RATE = "costRate";


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> The improvement rate is the rate that the part-stat upgrade diminishes per level. For example, a
		/// 		  value of 0.9f means each upgrade level only grants 90% of the nominal upgrade amount per
		/// 		  level per upgrade.</summary>
		public static string IMPROVEMENT_RATE = "improvementRate";
		#endregion


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Pathname of the directory that contains this mod's files relative to the main game directory.</summary>
		public static string MOD_DIRECTORY = "KRnD/";

		public static string CLOSE_ICON = "Icons/close";


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Filename of the configuration file that initializes the upgrade behavior. Most balance changes
		/// 		  can be controlled by editing this file.</summary>
		public static string CONFIG_FILENAME = "Config/Config.cfg";


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> The application icon that appears in the button bar.</summary>
		public static string APP_ICON = "Icons/R&D_icon";


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Filename of the blacklist file that specifies the mods that directly conflict with this mod.</summary>
		public static string BLACKLIST_FILENAME = "Config/blacklist.cfg";
	}
}

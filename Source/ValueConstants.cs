using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace KRnD.Source
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary> This class holds essentially all the game constants that control the balance and behavior of this
	/// 		  mod. The values in this class are initialized from the "Config.cfg" file.</summary>
	[KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
	public class ValueConstants : MonoBehaviour
	{
		public static Dictionary<string, UpgradeConstants> upgradeDatabase = new Dictionary<string, UpgradeConstants>();


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> The improvement rate is the rate that the part-stat upgrade diminishes per level. For example, a
		/// 		  value of 0.9f means each upgrade level only grants 90% of the nominal upgrade amount per
		/// 		  level per upgrade.</summary>
		public static float improvementRate = 0.9f;


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> The cost rate specifies the rate that the science-cost increases per upgrade level. A value of 2,
		/// 		  for example, would mean the cost doubles for each upgrade level.</summary>
		public static float costRate = 2.0f;

		public static float minFactor = 0.1f;

		public static float maxFactor = 4.0f;


		public static void Initialize()
		{
			try {
				// Default upgrade control data used only if no config file found. Every modifiable aspect needs to be listed here since
				// the dictionary is populated by this step as a side effect.
				upgradeDatabase = new Dictionary<string, UpgradeConstants>
				{
					[StringConstants.BATTERY_CHARGE] = new UpgradeConstants(StringConstants.BATTERY_CHARGE, 500, 0.2f, 10),
					[StringConstants.CHARGE_RATE] = new UpgradeConstants(StringConstants.CHARGE_RATE, 0, 0.05f, 10),
					[StringConstants.CONVERTER_EFFICIENCY] = new UpgradeConstants(StringConstants.CONVERTER_EFFICIENCY, 0, 0.1f, 15),
					[StringConstants.CRASH_TOLERANCE] = new UpgradeConstants(StringConstants.CRASH_TOLERANCE, 0, 0.15f, 10),
					[StringConstants.DRY_MASS] = new UpgradeConstants(StringConstants.DRY_MASS, 1, -0.1f, 10),
					[StringConstants.FUEL_CAPACITY] = new UpgradeConstants(StringConstants.FUEL_CAPACITY, 1000, 0.05f, 5),
					[StringConstants.FUEL_FLOW] = new UpgradeConstants(StringConstants.FUEL_FLOW, 0, 0.1f, 10),
					[StringConstants.GENERATOR_EFFICIENCY] = new UpgradeConstants(StringConstants.GENERATOR_EFFICIENCY, 0, 0.1f, 15),
					[StringConstants.ISP_ATM] = new UpgradeConstants(StringConstants.ISP_ATM, 0, 0.05f, 15),
					[StringConstants.ISP_VAC] = new UpgradeConstants(StringConstants.ISP_VAC, 0, 0.05f, 15),
					[StringConstants.MAX_TEMPERATURE] = new UpgradeConstants(StringConstants.MAX_TEMPERATURE, 1200, 0.2f, 5),
					[StringConstants.PARACHUTE_STRENGTH] = new UpgradeConstants(StringConstants.PARACHUTE_STRENGTH, 250, 0.3f, 10),
					[StringConstants.TORQUE] = new UpgradeConstants(StringConstants.TORQUE,  0, 0.25f, 5),
					[StringConstants.PACKET_SIZE] = new UpgradeConstants(StringConstants.PACKET_SIZE, 0, 0.10f, 10),
					[StringConstants.ANTENNA_POWER] = new UpgradeConstants(StringConstants.ANTENNA_POWER,  0, 0.10f, 10),
					[StringConstants.DATA_STORAGE] = new UpgradeConstants(StringConstants.DATA_STORAGE,  0, 0.40f, 10),
					[StringConstants.RESOURCE_HARVESTER] = new UpgradeConstants(StringConstants.RESOURCE_HARVESTER,  0, 0.10f, 10),
				};


				// Load in the default values from the config file.

				string filename = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException(), StringConstants.CONFIG_FILENAME);
				if (File.Exists(filename)) {

					// Read config data from config file.
					var settings = ConfigNode.Load(filename);
					LoadConstants(settings);

				} else {

					// Create initial config file since it doesn't exist. This shouldn't be needed.
					var node = new ConfigNode();
					SaveConstants(node);
					node.Save(filename);
				}
			} catch (Exception e) {
				Debug.LogError("[KRnD] Initialize(): " + e);
			}
		}

		public static UpgradeConstants GetData(string key)
		{
			try { 
				return upgradeDatabase[key];
			} catch (Exception e) {
				Debug.LogError("[KRnD] GetData(): " + e);
				return null;
			}
		}


		static void SaveConstants(ConfigNode node)
		{
			node.SetValue(StringConstants.COST_RATE, costRate, true);
			node.SetValue(StringConstants.IMPROVEMENT_RATE, improvementRate, true);
			foreach (var data in upgradeDatabase) {
				data.Value.SaveUpgradeData(node);
			}
		}

		static void LoadConstants(ConfigNode node)
		{
			node.TryGetValue(StringConstants.COST_RATE, ref costRate);
			node.TryGetValue(StringConstants.IMPROVEMENT_RATE, ref improvementRate);
			foreach (var data in upgradeDatabase) {
				data.Value.LoadUpgradeData(node);
			}
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Executes the save action which saves all the current settings to the save-game file.</summary>
		///
		/// <param name="node"> The saved node.</param>
		public static void OnSave(ConfigNode node)
		{
			SaveConstants(node);
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Executes the load action which loads the settings from the saved-game file. </summary>
		/// <param name="node"> The node. </param>
		public static void OnLoad(ConfigNode node)
		{
			LoadConstants(node);
		}


	}
}

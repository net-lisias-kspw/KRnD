using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;

namespace KRnD.Source
{
	[KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
	public class KRnDSettings : MonoBehaviour
	{
		public Dictionary<string, UpgradeData> upgradeDatabase = new Dictionary<string, UpgradeData>();
		//public List<string> upgradeTypes = new List<string>();


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Reference to the singleton instance of this class object. </summary>
		/// <value> The instance. This may be null. </value>
		[CanBeNull]
		public static KRnDSettings Instance { get; private set; }

		public static float improvementRate = 0.9f;
		public static float costRate = 2.0f;


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Awakes this object.</summary>
		[UsedImplicitly]
		private void Awake()
		{
			if (Instance != null && Instance != this) {
				Destroy(this);
				return;
			}

			if (Instance == null) {
				Instance = this;
				DontDestroyOnLoad(this);

				// Default upgrade control data used only if no config file found. Every modifiable aspect needs to be listed here since
				// the dictionary is populated by this step as a side effect.
				upgradeDatabase = new Dictionary<string, UpgradeData>
				{
					[Constants.BATTERY_CHARGE] = new UpgradeData(Constants.BATTERY_CHARGE, "EC storage capacity for batteries", 500, 0.2f, 10),
					[Constants.CHARGE_RATE] = new UpgradeData(Constants.CHARGE_RATE, "The EC rate generated from solar panels", 0, 0.05f, 10),
					[Constants.CONVERTER_EFFICIENCY] = new UpgradeData(Constants.CONVERTER_EFFICIENCY, "ISRU and Fuel Cell conversion efficiency", 0, 0.1f, 15),
					[Constants.CRASH_TOLERANCE] = new UpgradeData(Constants.CRASH_TOLERANCE, "Crash-tolerance for landing legs", 0, 0.15f, 10),
					[Constants.DRY_MASS] = new UpgradeData(Constants.DRY_MASS, "Dry mass (applies to every kind of part)", 1, -0.1f, 10),
					[Constants.FUEL_CAPACITY] = new UpgradeData(Constants.FUEL_CAPACITY, "Contained resource capacity", 1000, 0.05f, 5),
					[Constants.FUEL_FLOW] = new UpgradeData(Constants.FUEL_FLOW, "Rate that fuel is consumed -- increases thrust", 0, 0.1f, 10),
					[Constants.GENERATOR_EFFICIENCY] = new UpgradeData(Constants.GENERATOR_EFFICIENCY, "Power output efficiency from reactors", 0, 0.1f, 15),
					[Constants.ISP_ATM] = new UpgradeData(Constants.ISP_ATM, "I.S.P. in atmosphere", 0, 0.05f, 15),
					[Constants.ISP_VAC] = new UpgradeData(Constants.ISP_VAC, "I.S.P. in vacuum", 0, 0.05f, 15),
					[Constants.MAX_TEMPERATURE] = new UpgradeData(Constants.MAX_TEMPERATURE, "Maximum part temperature limit", 1200, 0.2f, 5),
					[Constants.PARACHUTE_STRENGTH] = new UpgradeData(Constants.PARACHUTE_STRENGTH, "Parachute strength -- affects max speed when deploying", 250, 0.3f, 10),
					[Constants.TORQUE] = new UpgradeData(Constants.TORQUE, "Reaction-wheel torque power", 0, 0.25f, 5)
				};


				// Load in the default values from the config file.

				string filename = KSPUtil.ApplicationRootPath + Constants.CONFIG_FILENAME;
				if (File.Exists(filename)) {

					// Read config data from config file.
					var settings = ConfigNode.Load(filename);
					LoadFromNode(settings);

				} else {

					// Create initial config file since it doesn't exist. This shouldn't be needed.
					var node = new ConfigNode();
					SaveToNode(node);
					node.Save(filename);
				}
			}
		}


		void SaveToNode(ConfigNode node)
		{
			node.SetValue(Constants.COST_RATE, costRate, "Science cost per level increases at this rate for every upgrade level", true);
			node.SetValue(Constants.IMPROVEMENT_RATE, improvementRate, "Part stat improvement per level diminishes by this factor for every upgrade level", true);
			foreach (var data in upgradeDatabase) {
				data.Value.SaveToNode(node);
			}
		}

		void LoadFromNode(ConfigNode node)
		{
			node.TryGetValue(Constants.COST_RATE, ref costRate);
			node.TryGetValue(Constants.IMPROVEMENT_RATE, ref improvementRate);
			foreach (var data in upgradeDatabase) {
				data.Value.LoadFromNode(node);
			}
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Executes the save action which saves all the current settings to the save-game file.</summary>
		///
		/// <param name="node"> The saved node.</param>
		public void OnSave(ConfigNode node)
		{
			SaveToNode(node);
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Executes the load action which loads the settings from the saved-game file. </summary>
		/// <param name="node"> The node. </param>
		public void OnLoad(ConfigNode node)
		{
			LoadFromNode(node);
		}


	}
}

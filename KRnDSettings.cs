using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using static KRnD.Constants;


namespace KRnD
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

				// Default upgrade control data used only if no config file found.
				upgradeDatabase = new Dictionary<string, UpgradeData>
				{
					[BATTERY_CHARGE] = new UpgradeData(BATTERY_CHARGE, 2, 500, 0.2f, 1, 10),
					[CHARGE_RATE] = new UpgradeData(CHARGE_RATE, 2, 1, 0.05f, 1, 10),
					[CONVERTER_EFFICIENCY] = new UpgradeData(CONVERTER_EFFICIENCY, 2, 1, 0.1f, 1, 15),
					[CRASH_TOLERANCE] = new UpgradeData(CRASH_TOLERANCE, 2, 1, 0.15f, 1, 10),
					[DRY_MASS] = new UpgradeData(DRY_MASS, 2, 1, -0.1f, 1, 10),
					[FUEL_CAPACITY] = new UpgradeData(FUEL_CAPACITY, 2, 1000, 0.05f, 1, 5),
					[FUEL_FLOW] = new UpgradeData(FUEL_FLOW, 2, 1, 0.1f, 1, 10),
					[GENERATOR_EFFICIENCY] = new UpgradeData(GENERATOR_EFFICIENCY, 2, 1, 0.1f, 1, 15),
					[ISP_ATM] = new UpgradeData(ISP_ATM, 2, 1, 0.05f, 1, 15),
					[ISP_VAC] = new UpgradeData(ISP_VAC, 2, 1, 0.05f, 1, 15),
					[MAX_TEMPERATURE] = new UpgradeData(MAX_TEMPERATURE, 2, 1, 0.2f, 1, 5),
					[PARACHUTE_STRENGTH] = new UpgradeData(PARACHUTE_STRENGTH, 2, 1, 0.3f, 1, 10),
					[TORQUE] = new UpgradeData(TORQUE, 2, 1, 0.25f, 1, 5)
				};


				// Load in the default values from the config file.

				string filename = KSPUtil.ApplicationRootPath + CONFIG_FILENAME;
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
			foreach (var data in upgradeDatabase) {
				data.Value.SaveToNode(node);
			}
		}

		void LoadFromNode(ConfigNode node)
		{
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

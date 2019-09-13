using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace KRnD
{
	[KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
	internal class KRnDSettings : MonoBehaviour
	{
		// Battery Charge
		public float batteryCharge_costScale = 2f;
		public float batteryCharge_costScaleReference = 500f;
		public float batteryCharge_improvement = 0.2f;
		public float batteryCharge_improvementScale = 1f;
		public int batteryCharge_scienceCost = 10;

		// Charge Rate
		public float chargeRate_costScale = 2f;
		public float chargeRate_improvement = 0.05f;
		public float chargeRate_improvementScale = 1f;
		public int chargeRate_scienceCost = 10;

		// Converter Efficiency
		public float converterEfficiency_costScale = 2f;
		public float converterEfficiency_improvement = 0.1f;
		public float converterEfficiency_improvementScale = 1f;
		public int converterEfficiency_scienceCost = 15;

		// Crash Tolerance
		public float crashTolerance_costScale = 2f;
		public float crashTolerance_improvement = 0.15f;
		public float crashTolerance_improvementScale = 1f;
		public int crashTolerance_scienceCost = 10;

		// Dry Mass
		public float dryMass_costScale = 2f;
		public float dryMass_costScaleReference = 1f;
		public float dryMass_improvement = -0.1f;
		public float dryMass_improvementScale = 1f;
		public int dryMass_scienceCost = 10;

		// Fuel Capacity
		public float fuelCapacity_costScale = 2f;
		public float fuelCapacity_costScaleReference = 1000f;
		public float fuelCapacity_improvement = 0.05f;
		public float fuelCapacity_improvementScale = 1f;
		public int fuelCapacity_scienceCost = 5;

		// Fuel Flow
		public float fuelFlow_costScale = 2f;
		public float fuelFlow_improvement = 0.1f;
		public float fuelFlow_improvementScale = 1f;
		public int fuelFlow_scienceCost = 10;

		// Generator Efficiency
		public float generatorEfficiency_costScale = 2f;
		public float generatorEfficiency_improvement = 0.1f;
		public float generatorEfficiency_improvementScale = 1f;
		public int generatorEfficiency_scienceCost = 15;

		// ISP Atm
		public float ispAtm_costScale = 2f;
		public float ispAtm_improvement = 0.05f;
		public float ispAtm_improvementScale = 1f;
		public int ispAtm_scienceCost = 15;

		// ISP Vac
		public float ispVac_costScale = 2f;
		public float ispVac_improvement = 0.05f;
		public float ispVac_improvementScale = 1f;
		public int ispVac_scienceCost = 15;

		// Max Skin Temp
		public float maxTemperature_costScale = 2f;
		public float maxTemperature_improvement = 0.2f;
		public float maxTemperature_improvementScale = 1f;
		public int maxTemperature_scienceCost = 5;

		// Parachute Strength
		public float parachuteStrength_costScale = 2f;
		public float parachuteStrength_improvement = 0.3f;
		public float parachuteStrength_improvementScale = 1f;
		public int parachuteStrength_scienceCost = 10;

		// Torque
		public float torque_costScale = 2f;
		public float torque_improvement = 0.25f;
		public float torque_improvementScale = 1f;
		public int torque_scienceCost = 5;

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
			}
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Executes the save action which saves all the current settings to the save-game file. </summary>
		/// <param name="savedNode"> The saved node. </param>
		public void OnSave(ConfigNode savedNode)
		{
			//savedNode.SetValue("EmergencyFundMultiple", bigProjectMultiple, true);
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Executes the load action which loads the settings from the saved-game file. </summary>
		/// <param name="node"> The node. </param>
		public void OnLoad(ConfigNode node)
		{
			//masterSwitch = true;

			//node.TryGetValue("EmergencyFundMultiple", ref bigProjectMultiple);
		}


	}
}

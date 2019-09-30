using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ExtraplanetaryLaunchpads;
using JetBrains.Annotations;
using KSP.UI.Screens;
using UnityEngine;

// For "ApplicationLauncherButton"

namespace KRnD.Source
{
	[KSPAddon(KSPAddon.Startup.EditorAny, false)]
	public class KRnDUI : MonoBehaviour
	{
		// TODO: The Application-Button shows up during the flight scene ...
		//private static ApplicationLauncherButton _launcherButton;
		public static Rect windowPosition = new Rect(300, 60, 450, 400 + 80);
		private static readonly GUIStyle _windowStyle = new GUIStyle(HighLogic.Skin.window) {fixedWidth = 500f, fixedHeight = 300f + 80};
		private static readonly GUIStyle _labelStyle = new GUIStyle(HighLogic.Skin.label);
		private static readonly GUIStyle _labelStyleSmall = new GUIStyle(HighLogic.Skin.label) {fontSize = 10};
		private static readonly GUIStyle _buttonStyle = new GUIStyle(HighLogic.Skin.button);
		private static readonly GUIStyle _scrollStyle = new GUIStyle(HighLogic.Skin.scrollView);
		private static Vector2 _scrollPos = Vector2.zero;
		//private static Texture2D texture;
		public static bool showGui;

		private static Texture2D _closeIcon;

		// The part that was last selected in the editor:
		public static Part selectedPart;

		private int _selectedUpgradeOption;

		[UsedImplicitly]
		private void Awake()
		{
			//if (texture == null) {
			//texture = new Texture2D(36, 36, TextureFormat.RGBA32, false);
			//var textureFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException(), Constants.APP_ICON);
			//texture.LoadImage(File.ReadAllBytes(textureFile));
			//}

			//_closeIcon = GameDatabase.Instance.GetTexture("KRnD/Icons/close", false);



			// Add event-handlers to create and destroy our button:
			//GameEvents.onGUIApplicationLauncherReady.Remove(ReadyEvent);
			//GameEvents.onGUIApplicationLauncherReady.Add(ReadyEvent);
			//GameEvents.onGUIApplicationLauncherDestroyed.Remove(DestroyEvent);
			//GameEvents.onGUIApplicationLauncherDestroyed.Add(DestroyEvent);
		}

		[UsedImplicitly]
		private void OnDestroy()
		{
			//GameEvents.onGUIApplicationLauncherReady.Remove(ReadyEvent);
			//GameEvents.onGUIApplicationLauncherDestroyed.Remove(DestroyEvent);
		}

		// Fires when a scene is ready so we can install our button.
//		public void ReadyEvent()
//		{
//			if (!ApplicationLauncher.Ready || _launcherButton != null) return;
//			var visible_in_scenes = ApplicationLauncher.AppScenes.SPH | ApplicationLauncher.AppScenes.VAB;
//			var texture_file = StringConstants.MOD_DIRECTORY + StringConstants.APP_ICON;
//			_launcherButton = ApplicationLauncher.Instance.AddModApplication(GuiToggle, GuiToggle, null, null, null, null, visible_in_scenes, GameDatabase.Instance.GetTexture(texture_file, false));
//		}

		// Fires when a scene is unloaded and we should destroy our button:
//		public void DestroyEvent()
//		{
//			if (_launcherButton == null) return;
//			ApplicationLauncher.Instance.RemoveModApplication(_launcherButton);
//			_launcherButton = null;
//			selectedPart = null;
//			showGui = false;
//		}

//		private void GuiToggle()
//		{
//			showGui = !showGui;
//		}


		[UsedImplicitly]
		public void OnGUI()
		{
			if (!showGui) return;

			if (_closeIcon == null) _closeIcon = GameDatabase.Instance.GetTexture(StringConstants.MOD_DIRECTORY + StringConstants.CLOSE_ICON, false);

			GUI.depth = 0;
			windowPosition = GUILayout.Window(100, windowPosition, OnWindow, "", _windowStyle);
			const int icon_size = 28;
			if (GUI.Button(new Rect(windowPosition.xMax - (icon_size + 2), windowPosition.yMin + 2, icon_size, icon_size), _closeIcon, GUI.skin.button)) {
				showGui = false;
			}
		}


		// Returns the info-text of the given part with the given upgrades to be displayed in the GUI-comparison.
		private string BuildPartInfoString(Part part, PartUpgrades upgrades_to_apply = null)
		{
			var info = "";
			PartUpgrades original_upgrades = null;
			try {
				var rnd_module = PartStats.GetKRnDModule(part);
				if (rnd_module == null || (original_upgrades = rnd_module.GetCurrentUpgrades()) == null) return info;

				// Upgrade the part to get the correct info, we revert it back to its previous values in the finally block below:
				KRnD.UpdatePart(part, upgrades_to_apply);
				var engine_modules = PartStats.GetModuleEnginesList(part);
				var rcs_module = PartStats.GetModuleRCS(part);
				var reaction_wheel_module = PartStats.GetModuleReactionWheel(part);
				var solar_panel_module = PartStats.GetModuleDeployableSolarPanel(part);
				var landing_leg_module = PartStats.GetModuleWheelBase(part);
				var electric_charge_resource = PartStats.GetElectricCharge(part);
				var generator_module = PartStats.GetModuleGenerator(part);
				var fission_generator = PartStats.GetFissionGenerator(part);
				var converter_modules = PartStats.GetModuleResourceConverterList(part);
				var parachute_module = PartStats.GetModuleParachute(part);
				var fairing_module = PartStats.GetModuleProceduralFairing(part);
				var fuel_resources = PartStats.GetFuelResources(part);
				var antenna_module = PartStats.GetModuleDataTransmitter(part);
				var science_module = PartStats.GetModuleScienceLab(part);
				var harvester_module = PartStats.GetModuleResourceHarvester(part);
				var radiator_module = PartStats.GetModuleActiveRadiator(part);
				var el_converter = PartStats.GetModuleElConverter(part);

				// Basic stats:
				info = "<color=#FFFFFF><b>Dry Mass:</b> " + part.mass.ToString("0.#### t") + "\n";

				info += "<b>Max Temp.:</b> " + part.maxTemp.ToString("0.#") + "/" + part.skinMaxTemp.ToString("0.#") + " °K\n";

				if (landing_leg_module != null) info += "<b>Crash Tolerance:</b> " + part.crashTolerance.ToString("0.#### m/s") + "\n";

				if (electric_charge_resource != null) info += "<b>Electric Charge:</b> " + electric_charge_resource.maxAmount + "\n";

				// Fuels:
				if (fuel_resources != null)
					foreach (var fuel_resource in fuel_resources) {
						// Reformat resource-names like "ElectricCharge" to "Electric Charge":
						var fuel_name = fuel_resource.resourceName;
						fuel_name = Regex.Replace(fuel_name, @"([a-z])([A-Z])", "$1 $2");
						info += "<b>" + fuel_name + ":</b> " + fuel_resource.maxAmount + "\n";
					}

				// Module stats:
				info += "\n";
				if (engine_modules != null)
					foreach (var engine_module in engine_modules) {
						info += "<color=#99FF00><b>Engine";
						if (engine_modules.Count > 1) info += " (" + engine_module.engineID + ")";
						info += ":</b></color>\n" + engine_module.GetInfo();
						if (engine_modules.Count > 1) info += "\n";
					}

				if (rcs_module) info += "<color=#99FF00><b>RCS:</b></color>\n" + rcs_module.GetInfo();
				if (reaction_wheel_module) info += "<color=#99FF00><b>Reaction Wheel:</b></color>\n" + reaction_wheel_module.GetInfo();
				if (solar_panel_module) info += "<color=#99FF00><b>Solar Panel:</b></color>\n" + KRnD.GetSolarPanelInfo(solar_panel_module);
				if (generator_module) info += "<color=#99FF00><b>Generator:</b></color>\n" + generator_module.GetInfo();
				if (fission_generator) info += "<color=#99FF00><b>Fission-Generator:</b></color>\n" + fission_generator.GetInfo();
				if (converter_modules != null)
					foreach (var converter_module in converter_modules)
						info += "<color=#99FF00><b>Converter " + converter_module.ConverterName + ":</b></color>\n" + converter_module.GetInfo() + "\n";
				if (parachute_module) info += "<color=#99FF00><b>Parachute:</b></color>\n" + parachute_module.GetInfo();
				if (fairing_module) info += "<color=#99FF00><b>Fairing:</b></color>\n" + fairing_module.GetInfo();
				if (antenna_module) info += "<color=#99FF00><b>Antenna:</b></color>\n" + antenna_module.GetInfo();
				if (science_module) info += "<color=#99FF00><b>Science Lab:</b></color>\n" + science_module.GetInfo();
				if (harvester_module) info += "<color=#99FF00><b>Harvester:</b></color>\n" + harvester_module.GetInfo();
				if (radiator_module) info += "<color=#99FF00><b>Radiator:</b></color>\n" + radiator_module.GetInfo();
				if (el_converter) info += "<color=#99FF00><b>EL Converter:</b></color>\n" + el_converter.GetInfo();


				info += "</color>";
			} catch (Exception e) {
				Debug.LogError("[KRnDGUI] getPartInfo(): " + e);
			} finally {
				try {
					if (original_upgrades != null) KRnD.UpdatePart(part, original_upgrades);
				} catch (Exception e) {
					Debug.LogError("[KRnDGUI] getPartInfo() restore of part failed: " + e);
				}
			}

			return info;
		}

		// Highlights differences between the two given texts, assuming they contain the same number of words.
		private string HighlightChanges(string original_text, string new_text, string color = "00FF00")
		{
			var highlighted_text = "";
			try {
				// Split as whitespaces and tags, we only need normal words and numbers:
				var set1 = Regex.Split(original_text, @"([\s<>])");
				var set2 = Regex.Split(new_text, @"([\s<>])");
				for (var i = 0; i < set2.Length; i++) {
					var old_word = "";
					if (i < set1.Length) old_word = set1[i];
					var new_word = set2[i];

					if (old_word != new_word) new_word = "<color=#" + color + "><b>" + new_word + "</b></color>";
					highlighted_text += new_word;
				}
			} catch (Exception e) {
				Debug.LogError("[KRnDGUI] highlightChanges(): " + e);
			}

			if (highlighted_text == "") return new_text;
			return highlighted_text;
		}

		private void OnWindow(int window_id)
		{
			try {
				GUILayout.BeginVertical();

				// Get all modules of the selected part:
				var part_title = "";
				Part part = null;
				KRnDModule rnd_module = null;
				List<ModuleEngines> engine_modules = null;
				ModuleRCS rcs_module = null;
				ModuleReactionWheel reaction_wheel_module = null;
				ModuleDeployableSolarPanel solar_panel_module = null;
				ModuleWheelBase landing_leg_module = null;
				PartResource electric_charge_resource = null;
				ModuleGenerator generator_module = null;
				PartModule fission_generator = null;
				List<ModuleResourceConverter> converter_modules = null;
				ModuleParachute parachute_module = null;
				ModuleDataTransmitter antenna_module = null;
				ModuleScienceLab science_lab = null;
				List<PartResource> fuel_resources = null;
				ModuleResourceHarvester harvester_module = null;
				ModuleActiveRadiator radiator_module = null;
				ELConverter el_converter = null;


				if (selectedPart != null) {
					foreach (var a_part in PartLoader.LoadedPartsList)
						if (a_part.partPrefab.name == selectedPart.name) {
							part = a_part.partPrefab;
							part_title = a_part.title;
							break;
						}

					if (part) {
						antenna_module = PartStats.GetModuleDataTransmitter(part);
						science_lab = PartStats.GetModuleScienceLab(part);
						rnd_module = PartStats.GetKRnDModule(part);
						engine_modules = PartStats.GetModuleEnginesList(part);
						rcs_module = PartStats.GetModuleRCS(part);
						reaction_wheel_module = PartStats.GetModuleReactionWheel(part);
						solar_panel_module = PartStats.GetModuleDeployableSolarPanel(part);
						landing_leg_module = PartStats.GetModuleWheelBase(part);
						electric_charge_resource = PartStats.GetElectricCharge(part);
						generator_module = PartStats.GetModuleGenerator(part);
						fission_generator = PartStats.GetFissionGenerator(part);
						converter_modules = PartStats.GetModuleResourceConverterList(part);
						parachute_module = PartStats.GetModuleParachute(part);
						fuel_resources = PartStats.GetFuelResources(part);
						harvester_module = PartStats.GetModuleResourceHarvester(part);
						radiator_module = PartStats.GetModuleActiveRadiator(part);
						el_converter = PartStats.GetModuleElConverter(part);
					}
				}

				if (!part) {
					// No part selected:
					GUILayout.BeginArea(new Rect(10, 5, _windowStyle.fixedWidth, 20));
					GUILayout.Label("<b>Kerbal R&D: Select a part to improve</b>", _labelStyle);
					GUILayout.EndArea();
					GUILayout.EndVertical();
					GUI.DragWindow();
					return;
				}

				if (!rnd_module) {
					// Invalid part selected:
					GUILayout.BeginArea(new Rect(10, 5, _windowStyle.fixedWidth, 20));
					GUILayout.Label("<b>Kerbal R&D: Select a different part to improve</b>", _labelStyle);
					GUILayout.EndArea();
					GUILayout.EndVertical();
					GUI.DragWindow();
					return;
				}

				// Get stats of the current version of the selected part:
				if (!KRnD.upgrades.TryGetValue(part.name, out var current_upgrade)) current_upgrade = new PartUpgrades();
				var current_info = BuildPartInfoString(part, current_upgrade);

				// Create a copy of the part-stats which we can use to mock an upgrade further below:
				var next_upgrade = current_upgrade.Clone();

				// Title:
				GUILayout.BeginArea(new Rect(10, 5, _windowStyle.fixedWidth, 20));
				var version = rnd_module.GetVersion();
				if (version != "") version = " - " + version;
				GUILayout.Label("<b>" + part_title + version + "</b>", _labelStyle);
				GUILayout.EndArea();

				// List with upgrade-options:
				float options_width = 100;
				var options_height = _windowStyle.fixedHeight - 30 - 30 - 20;
				GUILayout.BeginArea(new Rect(10, 30 + 20, options_width, options_height));


				GUILayout.BeginVertical();

				var options = new List<string> {"Dry Mass", "Max Temp"};
				if (engine_modules != null || rcs_module) {
					options.Add("ISP Vac");
					options.Add("ISP Atm");
					options.Add("Fuel Flow");
				}

				if (antenna_module != null) options.Add("Antenna Power");
				if (antenna_module != null && antenna_module.antennaType != AntennaType.INTERNAL) options.Add("Packet Size");
				if (science_lab != null) options.Add("Data Storage");

				if (reaction_wheel_module != null) options.Add("Torque");
				if (solar_panel_module != null) options.Add("Charge Rate");
				if (landing_leg_module != null) options.Add("Crash Tolerance");
				if (electric_charge_resource != null) options.Add("Battery");
				//if (fuel_resources != null) options.Add("Fuel Pressure");
				if (generator_module || fission_generator) options.Add("Generator");
				if (converter_modules != null) options.Add("Converter");
				if (parachute_module) options.Add("Parachute");
				if (harvester_module) options.Add("Harvester");
				if (radiator_module) options.Add("Radiator");
				if (el_converter) options.Add("EL Converter");

				if (_selectedUpgradeOption >= options.Count) _selectedUpgradeOption = 0;
				_selectedUpgradeOption = GUILayout.SelectionGrid(_selectedUpgradeOption, options.ToArray(), 1, _buttonStyle);

				GUILayout.EndVertical();

				GUILayout.EndArea();

				var selected_upgrade_option = options.ToArray()[_selectedUpgradeOption];
				int current_upgrade_level;
				int next_upgrade_level;
				int science_cost;
				float current_improvement_factor;
				float next_improvement_factor;
				UpgradeConstants u_constants;

				if (!KRnD.originalStats.TryGetValue(part.name, out var original_stats)) throw new Exception("no original-stats for part '" + part.name + "'");

				//Func<PartUpgrades, int> improve_function;
				if (selected_upgrade_option == "ISP Vac") {
					//improve_function = KRnD.ImproveIspVac;
					current_upgrade_level = current_upgrade.ispVac;
					next_upgrade_level = ++next_upgrade.ispVac;
					u_constants = ValueConstants.GetData(StringConstants.ISP_VAC);
					current_improvement_factor = u_constants.CalculateImprovementFactor(current_upgrade.ispVac);
					next_improvement_factor = u_constants.CalculateImprovementFactor(next_upgrade.ispVac);
					science_cost = u_constants.CalculateScienceCost(0, next_upgrade.ispVac);
				} else if (selected_upgrade_option == "ISP Atm") {
					//improve_function = KRnD.ImproveIspAtm;
					current_upgrade_level = current_upgrade.ispAtm;
					next_upgrade_level = ++next_upgrade.ispAtm;
					u_constants = ValueConstants.GetData(StringConstants.ISP_ATM);
					current_improvement_factor = u_constants.CalculateImprovementFactor(current_upgrade.ispAtm);
					next_improvement_factor = u_constants.CalculateImprovementFactor(next_upgrade.ispAtm);
					science_cost = u_constants.CalculateScienceCost(0, next_upgrade.ispAtm);
				} else if (selected_upgrade_option == "Fuel Flow") {
					//improve_function = KRnD.ImproveFuelFlow;
					current_upgrade_level = current_upgrade.fuelFlow;
					next_upgrade_level = ++next_upgrade.fuelFlow;
					u_constants = ValueConstants.GetData(StringConstants.FUEL_FLOW);
					current_improvement_factor = u_constants.CalculateImprovementFactor(current_upgrade.fuelFlow);
					next_improvement_factor = u_constants.CalculateImprovementFactor(next_upgrade.fuelFlow);
					science_cost = u_constants.CalculateScienceCost(0, next_upgrade.fuelFlow);
				} else if (selected_upgrade_option == "Dry Mass") {
					//improve_function = KRnD.ImproveDryMass;
					current_upgrade_level = current_upgrade.dryMass;
					next_upgrade_level = ++next_upgrade.dryMass;
					u_constants = ValueConstants.GetData(StringConstants.DRY_MASS);
					current_improvement_factor = u_constants.CalculateImprovementFactor(current_upgrade.dryMass);
					next_improvement_factor = u_constants.CalculateImprovementFactor(next_upgrade.dryMass);
					science_cost = u_constants.CalculateScienceCost(original_stats.dryMass, next_upgrade.dryMass);
				} else if (selected_upgrade_option == "Torque") {
					//improve_function = KRnD.ImproveTorque;
					current_upgrade_level = current_upgrade.torqueStrength;
					next_upgrade_level = ++next_upgrade.torqueStrength;
					u_constants = ValueConstants.GetData(StringConstants.TORQUE);
					current_improvement_factor = u_constants.CalculateImprovementFactor(current_upgrade.torqueStrength);
					next_improvement_factor = u_constants.CalculateImprovementFactor(next_upgrade.torqueStrength);
					science_cost = u_constants.CalculateScienceCost(original_stats.torqueStrength, next_upgrade.torqueStrength);
				} else if (selected_upgrade_option == "Antenna Power") {
					//improve_function = KRnD.ImproveAntennaPower;
					current_upgrade_level = current_upgrade.antennaPower;
					next_upgrade_level = ++next_upgrade.antennaPower;
					u_constants = ValueConstants.GetData(StringConstants.ANTENNA_POWER);
					current_improvement_factor = u_constants.CalculateImprovementFactor(current_upgrade.antennaPower);
					next_improvement_factor = u_constants.CalculateImprovementFactor(next_upgrade.antennaPower);
					science_cost = u_constants.CalculateScienceCost((float)original_stats.antennaPower, next_upgrade.antennaPower);
				} else if (selected_upgrade_option == "Packet Size") {
					//improve_function = KRnD.ImprovePacketSize;
					current_upgrade_level = current_upgrade.packetSize;
					next_upgrade_level = ++next_upgrade.packetSize;
					u_constants = ValueConstants.GetData(StringConstants.PACKET_SIZE);
					current_improvement_factor = u_constants.CalculateImprovementFactor(current_upgrade.packetSize);
					next_improvement_factor = u_constants.CalculateImprovementFactor(next_upgrade.packetSize);
					science_cost = u_constants.CalculateScienceCost(original_stats.packetSize, next_upgrade.packetSize);
				} else if (selected_upgrade_option == "Data Storage") {
					//improve_function = KRnD.ImproveDataStorage;
					current_upgrade_level = current_upgrade.dataStorage;
					next_upgrade_level = ++next_upgrade.dataStorage;
					u_constants = ValueConstants.GetData(StringConstants.DATA_STORAGE);
					current_improvement_factor = u_constants.CalculateImprovementFactor(current_upgrade.dataStorage);
					next_improvement_factor = u_constants.CalculateImprovementFactor(next_upgrade.dataStorage);
					science_cost = u_constants.CalculateScienceCost(original_stats.dataStorage, next_upgrade.dataStorage);
				} else if (selected_upgrade_option == "Harvester") {
					//improve_function = KRnD.ImproveResourceHarvester;
					current_upgrade_level = current_upgrade.resourceHarvester;
					next_upgrade_level = ++next_upgrade.resourceHarvester;
					u_constants = ValueConstants.GetData(StringConstants.RESOURCE_HARVESTER);
					current_improvement_factor = u_constants.CalculateImprovementFactor(current_upgrade.resourceHarvester);
					next_improvement_factor = u_constants.CalculateImprovementFactor(next_upgrade.resourceHarvester);
					science_cost = u_constants.CalculateScienceCost(original_stats.resourceHarvester, next_upgrade.resourceHarvester);
				} else if (selected_upgrade_option == "Radiator") {
					//improve_function = KRnD.ImproveActiveRadiator;
					current_upgrade_level = current_upgrade.maxEnergyTransfer;
					next_upgrade_level = ++next_upgrade.maxEnergyTransfer;
					u_constants = ValueConstants.GetData(StringConstants.ENERGY_TRANSFER);
					current_improvement_factor = u_constants.CalculateImprovementFactor(current_upgrade.maxEnergyTransfer);
					next_improvement_factor = u_constants.CalculateImprovementFactor(next_upgrade.maxEnergyTransfer);
					science_cost = u_constants.CalculateScienceCost((float)original_stats.maxEnergyTransfer, next_upgrade.maxEnergyTransfer);
				} else if (selected_upgrade_option == "Charge Rate") {
					//improve_function = KRnD.ImproveChargeRate;
					current_upgrade_level = current_upgrade.efficiencyMult;
					next_upgrade_level = ++next_upgrade.efficiencyMult;
					u_constants = ValueConstants.GetData(StringConstants.CHARGE_RATE);
					current_improvement_factor = u_constants.CalculateImprovementFactor(current_upgrade.efficiencyMult);
					next_improvement_factor = u_constants.CalculateImprovementFactor(next_upgrade.efficiencyMult);
					science_cost = u_constants.CalculateScienceCost(original_stats.efficiencyMult, next_upgrade.efficiencyMult);
				} else if (selected_upgrade_option == "Crash Tolerance") {
					//improve_function = KRnD.ImproveCrashTolerance;
					current_upgrade_level = current_upgrade.crashTolerance;
					next_upgrade_level = ++next_upgrade.crashTolerance;
					u_constants = ValueConstants.GetData(StringConstants.CRASH_TOLERANCE);
					current_improvement_factor = u_constants.CalculateImprovementFactor(current_upgrade.crashTolerance);
					next_improvement_factor = u_constants.CalculateImprovementFactor(next_upgrade.crashTolerance);
					science_cost = u_constants.CalculateScienceCost(original_stats.crashTolerance, next_upgrade.crashTolerance);
				} else if (selected_upgrade_option == "Battery") {
					//improve_function = KRnD.ImproveBatteryCharge;
					current_upgrade_level = current_upgrade.batteryCharge;
					next_upgrade_level = ++next_upgrade.batteryCharge;
					u_constants = ValueConstants.GetData(StringConstants.BATTERY_CHARGE);
					current_improvement_factor = u_constants.CalculateImprovementFactor(current_upgrade.batteryCharge);
					next_improvement_factor = u_constants.CalculateImprovementFactor(next_upgrade.batteryCharge);
					science_cost = u_constants.CalculateScienceCost((float)original_stats.batteryCharge, next_upgrade.batteryCharge);
				} else if (selected_upgrade_option == "Fuel Pressure") {
					//improve_function = KRnD.ImproveFuelCapacity;
					current_upgrade_level = current_upgrade.fuelCapacity;
					next_upgrade_level = ++next_upgrade.fuelCapacity;
					u_constants = ValueConstants.GetData(StringConstants.FUEL_CAPACITY);
					current_improvement_factor = u_constants.CalculateImprovementFactor(current_upgrade.fuelCapacity);
					next_improvement_factor = u_constants.CalculateImprovementFactor(next_upgrade.fuelCapacity);
					science_cost = u_constants.CalculateScienceCost((float)original_stats.fuelCapacitiesSum, next_upgrade.fuelCapacity);
				} else if (selected_upgrade_option == "Generator") {
					//improve_function = KRnD.ImproveGeneratorEfficiency;
					current_upgrade_level = current_upgrade.generatorEfficiency;
					next_upgrade_level = ++next_upgrade.generatorEfficiency;
					u_constants = ValueConstants.GetData(StringConstants.GENERATOR_EFFICIENCY);
					current_improvement_factor = u_constants.CalculateImprovementFactor(current_upgrade.generatorEfficiency);
					next_improvement_factor = u_constants.CalculateImprovementFactor(next_upgrade.generatorEfficiency);
					science_cost = u_constants.CalculateScienceCost(0, next_upgrade.generatorEfficiency);
				} else if (selected_upgrade_option == "Converter") {
					//improve_function = KRnD.ImproveConverterEfficiency;
					current_upgrade_level = current_upgrade.converterEfficiency;
					next_upgrade_level = ++next_upgrade.converterEfficiency;
					u_constants = ValueConstants.GetData(StringConstants.CONVERTER_EFFICIENCY);
					current_improvement_factor = u_constants.CalculateImprovementFactor(current_upgrade.converterEfficiency);
					next_improvement_factor = u_constants.CalculateImprovementFactor(next_upgrade.converterEfficiency);
					science_cost = u_constants.CalculateScienceCost(0, next_upgrade.converterEfficiency);
				} else if (selected_upgrade_option == "Parachute") {
					//improve_function = KRnD.ImproveParachuteStrength;
					current_upgrade_level = current_upgrade.parachuteStrength;
					next_upgrade_level = ++next_upgrade.parachuteStrength;
					u_constants = ValueConstants.GetData(StringConstants.PARACHUTE_STRENGTH);
					current_improvement_factor = u_constants.CalculateImprovementFactor(current_upgrade.parachuteStrength);
					next_improvement_factor = u_constants.CalculateImprovementFactor(next_upgrade.parachuteStrength);
					science_cost = u_constants.CalculateScienceCost((float)original_stats.chuteMaxTemp, next_upgrade.parachuteStrength);
				} else if (selected_upgrade_option == "Max Temp") {
					//improve_function = KRnD.ImproveMaxTemperature;
					current_upgrade_level = current_upgrade.maxTemperature;
					next_upgrade_level = ++next_upgrade.maxTemperature;
					u_constants = ValueConstants.GetData(StringConstants.MAX_TEMPERATURE);
					current_improvement_factor = u_constants.CalculateImprovementFactor(current_upgrade.maxTemperature);
					next_improvement_factor = u_constants.CalculateImprovementFactor(next_upgrade.maxTemperature);
					science_cost = u_constants.CalculateScienceCost((float)original_stats.intMaxTemp, next_upgrade.maxTemperature);
				} else if (selected_upgrade_option == "EL Converter") {
					//improve_function = KRnD.ImproveMaxTemperature;
					current_upgrade_level = current_upgrade.elConverter;
					next_upgrade_level = ++next_upgrade.elConverter;
					u_constants = ValueConstants.GetData(StringConstants.EL_CONVERTER);
					current_improvement_factor = u_constants.CalculateImprovementFactor(current_upgrade.elConverter);
					next_improvement_factor = u_constants.CalculateImprovementFactor(next_upgrade.elConverter);
					science_cost = u_constants.CalculateScienceCost((float)original_stats.ELConverter, next_upgrade.elConverter);
				} else {
					throw new Exception("unexpected option '" + selected_upgrade_option + "'");
				}

				var new_info = BuildPartInfoString(part, next_upgrade); // Calculate part-info if the selected stat was upgraded.
				new_info = HighlightChanges(current_info, new_info);

				// Current stats:
				GUILayout.BeginArea(new Rect(10 + options_width + 10, 30, _windowStyle.fixedWidth, 20));
				GUILayout.Label("<color=#FFFFFF><b>Current:</b> " + current_upgrade_level + " (" + current_improvement_factor.ToString("+0.##%;-0.##%;-") + ")</color>", _labelStyle);
				GUILayout.EndArea();

				var area_width = (_windowStyle.fixedWidth - 20 - options_width) / 2;
				var area_height = options_height;
				GUILayout.BeginArea(new Rect(10 + options_width, 30 + 20, area_width, area_height));
				_scrollPos = GUILayout.BeginScrollView(_scrollPos, _scrollStyle, GUILayout.Width(area_width), GUILayout.Height(area_height));

				GUILayout.Label(current_info, _labelStyleSmall);
				GUILayout.EndScrollView();
				GUILayout.EndArea();

				// Next stats:
				GUILayout.BeginArea(new Rect(10 + options_width + area_width + 10, 30, _windowStyle.fixedWidth, 20));
				GUILayout.Label("<color=#FFFFFF><b>Next upgrade:</b> " + next_upgrade_level + " (" + next_improvement_factor.ToString("+0.##%;-0.##%;-") + ")</color>", _labelStyle);
				GUILayout.EndArea();

				GUILayout.BeginArea(new Rect(10 + options_width + area_width, 30 + 20, area_width, area_height));
				_scrollPos = GUILayout.BeginScrollView(_scrollPos, _scrollStyle, GUILayout.Width(area_width), GUILayout.Height(area_height));
				GUILayout.Label(new_info, _labelStyleSmall);
				GUILayout.EndScrollView();
				GUILayout.EndArea();

				// Bottom-line (display only if the upgrade would have an effect):
				if (Math.Abs(current_improvement_factor - next_improvement_factor) > float.Epsilon) {
					GUILayout.BeginArea(new Rect(10, _windowStyle.fixedHeight - 25, _windowStyle.fixedWidth, 30));
					float current_science = 0;
					if (ResearchAndDevelopment.Instance != null) current_science = ResearchAndDevelopment.Instance.Science;
					var color = "FF0000";
					if (current_science >= science_cost) color = "00FF00";
					GUILayout.Label("<b>Science: <color=#" + color + ">" + science_cost + " / " + Math.Floor(current_science) + "</color></b>", _labelStyle);
					GUILayout.EndArea();
					if (current_science >= science_cost && ResearchAndDevelopment.Instance != null && u_constants != null /*&& improve_function != null*/) {
						GUILayout.BeginArea(new Rect(_windowStyle.fixedWidth - 110, _windowStyle.fixedHeight - 30, 100, 30));
						if (GUILayout.Button("Research", _buttonStyle)) {


							//upgrade_function(part);
							try {
								if (!KRnD.upgrades.TryGetValue(part.name, out var store)) {
									store = new PartUpgrades();
									KRnD.upgrades.Add(part.name, store);
								}

								u_constants.upgradeFunction(store);
								//improve_function(store);
								KRnD.UpdateGlobalParts();
								KRnD.UpdateEditorVessel();
							} catch (Exception e) {
								Debug.LogError("[KRnD] UpgradeIspVac(): " + e);
							}




							ResearchAndDevelopment.Instance.AddScience(-science_cost, TransactionReasons.RnDTechResearch);
						}

						GUILayout.EndArea();
					}
				}

				GUILayout.EndVertical();
				GUI.DragWindow();
			} catch (Exception e) {
				Debug.LogError("[KRnD] GenerateWindow(): " + e);
			}
		}
	}
}
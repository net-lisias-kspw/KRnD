using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using KSP.UI.Screens;
using UnityEngine;

// For "ApplicationLauncherButton"

namespace KRnD.Source
{
	[KSPAddon(KSPAddon.Startup.EditorAny, false)]
	public class UpgradeUI : MonoBehaviour
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

		public static int UpgradeIspVac(Part part)
		{
			try {
				if (!KRnD.upgrades.TryGetValue(part.name, out var store)) {
					store = new PartUpgrades();
					KRnD.upgrades.Add(part.name, store);
				}

				store.ispVac++;
				KRnD.UpdateGlobalParts();
				KRnD.UpdateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeIspVac(): " + e);
			}

			return 0;
		}

		public static int UpgradeIspAtm(Part part)
		{
			try {
				if (!KRnD.upgrades.TryGetValue(part.name, out var store)) {
					store = new PartUpgrades();
					KRnD.upgrades.Add(part.name, store);
				}

				store.ispAtm++;
				KRnD.UpdateGlobalParts();
				KRnD.UpdateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeIspAtm(): " + e);
			}

			return 0;
		}

		public static int UpgradeDryMass(Part part)
		{
			try {
				if (!KRnD.upgrades.TryGetValue(part.name, out var store)) {
					store = new PartUpgrades();
					KRnD.upgrades.Add(part.name, store);
				}

				store.dryMass++;
				KRnD.UpdateGlobalParts();
				KRnD.UpdateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeDryMass(): " + e);
			}

			return 0;
		}

		public static int UpgradeFuelFlow(Part part)
		{
			try {
				if (!KRnD.upgrades.TryGetValue(part.name, out var store)) {
					store = new PartUpgrades();
					KRnD.upgrades.Add(part.name, store);
				}

				store.fuelFlow++;
				KRnD.UpdateGlobalParts();
				KRnD.UpdateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeFuelFlow(): " + e);
			}

			return 0;
		}

		public static int UpgradeTorque(Part part)
		{
			try {
				if (!KRnD.upgrades.TryGetValue(part.name, out var store)) {
					store = new PartUpgrades();
					KRnD.upgrades.Add(part.name, store);
				}

				store.torque++;
				KRnD.UpdateGlobalParts();
				KRnD.UpdateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeTorque(): " + e);
			}

			return 0;
		}

		public static int UpgradeChargeRate(Part part)
		{
			try {
				if (!KRnD.upgrades.TryGetValue(part.name, out var store)) {
					store = new PartUpgrades();
					KRnD.upgrades.Add(part.name, store);
				}

				store.chargeRate++;
				KRnD.UpdateGlobalParts();
				KRnD.UpdateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeChargeRate(): " + e);
			}

			return 0;
		}

		public static int UpgradeCrashTolerance(Part part)
		{
			try {
				if (!KRnD.upgrades.TryGetValue(part.name, out var store)) {
					store = new PartUpgrades();
					KRnD.upgrades.Add(part.name, store);
				}

				store.crashTolerance++;
				KRnD.UpdateGlobalParts();
				KRnD.UpdateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeCrashTolerance(): " + e);
			}

			return 0;
		}

		public static int UpgradeBatteryCharge(Part part)
		{
			try {
				if (!KRnD.upgrades.TryGetValue(part.name, out var store)) {
					store = new PartUpgrades();
					KRnD.upgrades.Add(part.name, store);
				}

				store.batteryCharge++;
				KRnD.UpdateGlobalParts();
				KRnD.UpdateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeBatteryCharge(): " + e);
			}

			return 0;
		}

		public static int UpgradeGeneratorEfficiency(Part part)
		{
			try {
				if (!KRnD.upgrades.TryGetValue(part.name, out var store)) {
					store = new PartUpgrades();
					KRnD.upgrades.Add(part.name, store);
				}

				store.generatorEfficiency++;
				KRnD.UpdateGlobalParts();
				KRnD.UpdateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeGeneratorEfficiency(): " + e);
			}

			return 0;
		}

		public static int UpgradeConverterEfficiency(Part part)
		{
			try {
				if (!KRnD.upgrades.TryGetValue(part.name, out var store)) {
					store = new PartUpgrades();
					KRnD.upgrades.Add(part.name, store);
				}

				store.converterEfficiency++;
				KRnD.UpdateGlobalParts();
				KRnD.UpdateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeConverterEfficiency(): " + e);
			}

			return 0;
		}

		public static int UpgradeParachuteStrength(Part part)
		{
			try {
				if (!KRnD.upgrades.TryGetValue(part.name, out var store)) {
					store = new PartUpgrades();
					KRnD.upgrades.Add(part.name, store);
				}

				store.parachuteStrength++;
				KRnD.UpdateGlobalParts();
				KRnD.UpdateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeParachuteStrength(): " + e);
			}

			return 0;
		}

		public static int UpgradeMaxTemperature(Part part)
		{
			try {
				if (!KRnD.upgrades.TryGetValue(part.name, out var store)) {
					store = new PartUpgrades();
					KRnD.upgrades.Add(part.name, store);
				}

				store.maxTemperature++;
				KRnD.UpdateGlobalParts();
				KRnD.UpdateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeMaxTemperature(): " + e);
			}

			return 0;
		}

		public static int UpgradeFuelCapacity(Part part)
		{
			try {
				if (!KRnD.upgrades.TryGetValue(part.name, out var store)) {
					store = new PartUpgrades();
					KRnD.upgrades.Add(part.name, store);
				}

				store.fuelCapacity++;
				KRnD.UpdateGlobalParts();
				KRnD.UpdateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeFuelCapacity(): " + e);
			}

			return 0;
		}

		// Returns the info-text of the given part with the given upgrades to be displayed in the GUI-comparison.
		private string GetPartInfo(Part part, PartUpgrades upgrades_to_apply = null)
		{
			var info = "";
			PartUpgrades original_upgrades = null;
			try {
				var rnd_module = KRnD.GetKRnDModule(part);
				if (rnd_module == null || (original_upgrades = rnd_module.GetCurrentUpgrades()) == null) return info;

				// Upgrade the part to get the correct info, we revert it back to its previous values in the finally block below:
				KRnD.UpdatePart(part, upgrades_to_apply);
				var engine_modules = KRnD.GetEngineModules(part);
				var rcs_module = KRnD.GetRcsModule(part);
				var reaction_wheel_module = KRnD.GetReactionWheelModule(part);
				var solar_panel_module = KRnD.GetSolarPanelModule(part);
				var landing_leg_module = KRnD.GetLandingLegModule(part);
				var electric_charge_resource = KRnD.GetChargeResource(part);
				var generator_module = KRnD.GetGeneratorModule(part);
				var fission_generator = KRnD.GetFissionGeneratorModule(part);
				var converter_modules = KRnD.GetConverterModules(part);
				var parachute_module = KRnD.GetParachuteModule(part);
				var fairing_module = KRnD.GetFairingModule(part);
				var fuel_resources = KRnD.GetFuelResources(part);

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
				List<PartResource> fuel_resources = null;
				if (selectedPart != null) {
					foreach (var a_part in PartLoader.LoadedPartsList)
						if (a_part.partPrefab.name == selectedPart.name) {
							part = a_part.partPrefab;
							part_title = a_part.title;
							break;
						}

					if (part) {
						rnd_module = KRnD.GetKRnDModule(part);
						engine_modules = KRnD.GetEngineModules(part);
						rcs_module = KRnD.GetRcsModule(part);
						reaction_wheel_module = KRnD.GetReactionWheelModule(part);
						solar_panel_module = KRnD.GetSolarPanelModule(part);
						landing_leg_module = KRnD.GetLandingLegModule(part);
						electric_charge_resource = KRnD.GetChargeResource(part);
						generator_module = KRnD.GetGeneratorModule(part);
						fission_generator = KRnD.GetFissionGeneratorModule(part);
						converter_modules = KRnD.GetConverterModules(part);
						parachute_module = KRnD.GetParachuteModule(part);
						fuel_resources = KRnD.GetFuelResources(part);
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
				var current_info = GetPartInfo(part, current_upgrade);

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

				if (reaction_wheel_module != null) options.Add("Torque");
				if (solar_panel_module != null) options.Add("Charge Rate");
				if (landing_leg_module != null) options.Add("Crash Tolerance");
				if (electric_charge_resource != null) options.Add("Battery");
				if (fuel_resources != null) options.Add("Fuel Pressure");
				if (generator_module || fission_generator) options.Add("Generator");
				if (converter_modules != null) options.Add("Converter");
				if (parachute_module) options.Add("Parachute");
				if (_selectedUpgradeOption >= options.Count) _selectedUpgradeOption = 0;
				_selectedUpgradeOption = GUILayout.SelectionGrid(_selectedUpgradeOption, options.ToArray(), 1, _buttonStyle);

				GUILayout.EndVertical();

				GUILayout.EndArea();

				var selected_upgrade_option = options.ToArray()[_selectedUpgradeOption];
				int current_upgrade_count;
				int next_upgrade_count;
				int science_cost;
				float current_improvement;
				float next_improvement;

				if (!KRnD.originalStats.TryGetValue(part.name, out var original_stats)) throw new Exception("no original-stats for part '" + part.name + "'");

				Func<Part, int> upgrade_function;
				if (selected_upgrade_option == "ISP Vac") {
					upgrade_function = UpgradeIspVac;
					current_upgrade_count = current_upgrade.ispVac;
					next_upgrade_count = ++next_upgrade.ispVac;
					current_improvement = KRnD.CalculateImprovementFactor(rnd_module.ispVac_improvement, rnd_module.ispVac_improvementScale, current_upgrade.ispVac);
					next_improvement = KRnD.CalculateImprovementFactor(rnd_module.ispVac_improvement, rnd_module.ispVac_improvementScale, next_upgrade.ispVac);
					science_cost = KRnD.CalculateScienceCost(rnd_module.ispVac_scienceCost, rnd_module.ispVac_costScale, next_upgrade.ispVac);
				} else if (selected_upgrade_option == "ISP Atm") {
					upgrade_function = UpgradeIspAtm;
					current_upgrade_count = current_upgrade.ispAtm;
					next_upgrade_count = ++next_upgrade.ispAtm;
					current_improvement = KRnD.CalculateImprovementFactor(rnd_module.ispAtm_improvement, rnd_module.ispAtm_improvementScale, current_upgrade.ispAtm);
					next_improvement = KRnD.CalculateImprovementFactor(rnd_module.ispAtm_improvement, rnd_module.ispAtm_improvementScale, next_upgrade.ispAtm);
					science_cost = KRnD.CalculateScienceCost(rnd_module.ispAtm_scienceCost, rnd_module.ispAtm_costScale, next_upgrade.ispAtm);
				} else if (selected_upgrade_option == "Fuel Flow") {
					upgrade_function = UpgradeFuelFlow;
					current_upgrade_count = current_upgrade.fuelFlow;
					next_upgrade_count = ++next_upgrade.fuelFlow;

#if true
					UpgradeData u_data = KRnDSettings.GetData(StringConstants.FUEL_FLOW);
					current_improvement = u_data.CalculateImprovementFactor(current_upgrade.fuelFlow);
					next_improvement = u_data.CalculateImprovementFactor(next_upgrade.fuelFlow);
					science_cost = u_data.CalculateScienceCost(0, next_upgrade.fuelFlow);
#else
					currentImprovement = KRnD.CalculateImprovementFactor(rndModule.fuelFlow_improvement, rndModule.fuelFlow_improvementScale, currentUpgrade.fuelFlow);
					nextImprovement = KRnD.CalculateImprovementFactor(rndModule.fuelFlow_improvement, rndModule.fuelFlow_improvementScale, nextUpgrade.fuelFlow);
					scienceCost = KRnD.CalculateScienceCost(rndModule.fuelFlow_scienceCost, rndModule.fuelFlow_costScale, nextUpgrade.fuelFlow);
#endif


				} else if (selected_upgrade_option == "Dry Mass") {
					upgrade_function = UpgradeDryMass;
					current_upgrade_count = current_upgrade.dryMass;
					next_upgrade_count = ++next_upgrade.dryMass;
					current_improvement = KRnD.CalculateImprovementFactor(rnd_module.dryMass_improvement, rnd_module.dryMass_improvementScale, current_upgrade.dryMass);
					next_improvement = KRnD.CalculateImprovementFactor(rnd_module.dryMass_improvement, rnd_module.dryMass_improvementScale, next_upgrade.dryMass);

					// Scale science cost with original mass:
					//if (!KRnD.originalStats.TryGetValue(part.name, out var original_stats)) throw new Exception("no original-stats for part '" + part.name + "'");
					float scale_reference_factor = 1;
					if (rnd_module.dryMass_costScaleReference > 0) scale_reference_factor = original_stats.dryMass / rnd_module.dryMass_costScaleReference;
					var scaled_cost = (int) Math.Round(rnd_module.dryMass_scienceCost * scale_reference_factor);
					if (scaled_cost < 1) scaled_cost = 1;
					science_cost = KRnD.CalculateScienceCost(scaled_cost, rnd_module.dryMass_costScale, next_upgrade.dryMass);
				} else if (selected_upgrade_option == "Torque") {
					upgrade_function = UpgradeTorque;
					current_upgrade_count = current_upgrade.torque;
					next_upgrade_count = ++next_upgrade.torque;
					current_improvement = KRnD.CalculateImprovementFactor(rnd_module.torque_improvement, rnd_module.torque_improvementScale, current_upgrade.torque);
					next_improvement = KRnD.CalculateImprovementFactor(rnd_module.torque_improvement, rnd_module.torque_improvementScale, next_upgrade.torque);
					science_cost = KRnD.CalculateScienceCost(rnd_module.torque_scienceCost, rnd_module.torque_costScale, next_upgrade.torque);
				} else if (selected_upgrade_option == "Charge Rate") {
					upgrade_function = UpgradeChargeRate;
					current_upgrade_count = current_upgrade.chargeRate;
					next_upgrade_count = ++next_upgrade.chargeRate;
					current_improvement = KRnD.CalculateImprovementFactor(rnd_module.chargeRate_improvement, rnd_module.chargeRate_improvementScale, current_upgrade.chargeRate);
					next_improvement = KRnD.CalculateImprovementFactor(rnd_module.chargeRate_improvement, rnd_module.chargeRate_improvementScale, next_upgrade.chargeRate);
					science_cost = KRnD.CalculateScienceCost(rnd_module.chargeRate_scienceCost, rnd_module.chargeRate_costScale, next_upgrade.chargeRate);
				} else if (selected_upgrade_option == "Crash Tolerance") {
					upgrade_function = UpgradeCrashTolerance;
					current_upgrade_count = current_upgrade.crashTolerance;
					next_upgrade_count = ++next_upgrade.crashTolerance;

#if true
					UpgradeData u_data = KRnDSettings.GetData(StringConstants.CRASH_TOLERANCE);
					current_improvement = u_data.CalculateImprovementFactor(current_upgrade.crashTolerance);
					next_improvement = u_data.CalculateImprovementFactor(next_upgrade.crashTolerance);
					//if (!KRnD.originalStats.TryGetValue(part.name, out var original_stats)) throw new Exception("no original-stats for part '" + part.name + "'");
					science_cost = u_data.CalculateScienceCost(original_stats.crashTolerance, next_upgrade.crashTolerance);
#endif

#if false
					currentImprovement = KRnD.CalculateImprovementFactor(rndModule.crashTolerance_improvement, rndModule.crashTolerance_improvementScale, currentUpgrade.crashTolerance);
					nextImprovement = KRnD.CalculateImprovementFactor(rndModule.crashTolerance_improvement, rndModule.crashTolerance_improvementScale, nextUpgrade.crashTolerance);
					scienceCost = KRnD.CalculateScienceCost(rndModule.crashTolerance_scienceCost, rndModule.crashTolerance_costScale, nextUpgrade.crashTolerance);
#endif

				} else if (selected_upgrade_option == "Battery") {
					upgrade_function = UpgradeBatteryCharge;
					current_upgrade_count = current_upgrade.batteryCharge;
					next_upgrade_count = ++next_upgrade.batteryCharge;
					current_improvement = KRnD.CalculateImprovementFactor(rnd_module.batteryCharge_improvement, rnd_module.batteryCharge_improvementScale, current_upgrade.batteryCharge);
					next_improvement = KRnD.CalculateImprovementFactor(rnd_module.batteryCharge_improvement, rnd_module.batteryCharge_improvementScale, next_upgrade.batteryCharge);

					// Scale science cost with original battery charge:
					//if (!KRnD.originalStats.TryGetValue(part.name, out var original_stats)) throw new Exception("no original-stats for part '" + part.name + "'");
					double scale_reference_factor = 1;
					if (rnd_module.batteryCharge_costScaleReference > 0) scale_reference_factor = original_stats.batteryCharge / rnd_module.batteryCharge_costScaleReference;
					var scaled_cost = (int) Math.Round(rnd_module.batteryCharge_scienceCost * scale_reference_factor);
					if (scaled_cost < 1) scaled_cost = 1;
					science_cost = KRnD.CalculateScienceCost(scaled_cost, rnd_module.batteryCharge_costScale, next_upgrade.batteryCharge);
				} else if (selected_upgrade_option == "Fuel Pressure") {
					upgrade_function = UpgradeFuelCapacity;
					current_upgrade_count = current_upgrade.fuelCapacity;
					next_upgrade_count = ++next_upgrade.fuelCapacity;
					current_improvement = KRnD.CalculateImprovementFactor(rnd_module.fuelCapacity_improvement, rnd_module.fuelCapacity_improvementScale, current_upgrade.fuelCapacity);
					next_improvement = KRnD.CalculateImprovementFactor(rnd_module.fuelCapacity_improvement, rnd_module.fuelCapacity_improvementScale, next_upgrade.fuelCapacity);

					// Scale science cost with original fuel capacity:
					//if (!KRnD.originalStats.TryGetValue(part.name, out var original_stats)) throw new Exception("no original-stats for part '" + part.name + "'");
					double scale_reference_factor = 1;
					if (rnd_module.fuelCapacity_costScaleReference > 0) scale_reference_factor = original_stats.fuelCapacitiesSum / rnd_module.fuelCapacity_costScaleReference;
					var scaled_cost = (int) Math.Round(rnd_module.fuelCapacity_scienceCost * scale_reference_factor);
					if (scaled_cost < 1) scaled_cost = 1;
					science_cost = KRnD.CalculateScienceCost(scaled_cost, rnd_module.fuelCapacity_costScale, next_upgrade.fuelCapacity);
				} else if (selected_upgrade_option == "Generator") {
					upgrade_function = UpgradeGeneratorEfficiency;
					current_upgrade_count = current_upgrade.generatorEfficiency;
					next_upgrade_count = ++next_upgrade.generatorEfficiency;
					current_improvement = KRnD.CalculateImprovementFactor(rnd_module.generatorEfficiency_improvement, rnd_module.generatorEfficiency_improvementScale, current_upgrade.generatorEfficiency);
					next_improvement = KRnD.CalculateImprovementFactor(rnd_module.generatorEfficiency_improvement, rnd_module.generatorEfficiency_improvementScale, next_upgrade.generatorEfficiency);
					science_cost = KRnD.CalculateScienceCost(rnd_module.generatorEfficiency_scienceCost, rnd_module.generatorEfficiency_costScale, next_upgrade.generatorEfficiency);
				} else if (selected_upgrade_option == "Converter") {
					upgrade_function = UpgradeConverterEfficiency;
					current_upgrade_count = current_upgrade.converterEfficiency;
					next_upgrade_count = ++next_upgrade.converterEfficiency;
					current_improvement = KRnD.CalculateImprovementFactor(rnd_module.converterEfficiency_improvement, rnd_module.converterEfficiency_improvementScale, current_upgrade.converterEfficiency);
					next_improvement = KRnD.CalculateImprovementFactor(rnd_module.converterEfficiency_improvement, rnd_module.converterEfficiency_improvementScale, next_upgrade.converterEfficiency);
					science_cost = KRnD.CalculateScienceCost(rnd_module.converterEfficiency_scienceCost, rnd_module.converterEfficiency_costScale, next_upgrade.converterEfficiency);
				} else if (selected_upgrade_option == "Parachute") {
					upgrade_function = UpgradeParachuteStrength;
					current_upgrade_count = current_upgrade.parachuteStrength;
					next_upgrade_count = ++next_upgrade.parachuteStrength;
					current_improvement = KRnD.CalculateImprovementFactor(rnd_module.parachuteStrength_improvement, rnd_module.parachuteStrength_improvementScale, current_upgrade.parachuteStrength);
					next_improvement = KRnD.CalculateImprovementFactor(rnd_module.parachuteStrength_improvement, rnd_module.parachuteStrength_improvementScale, next_upgrade.parachuteStrength);
					science_cost = KRnD.CalculateScienceCost(rnd_module.parachuteStrength_scienceCost, rnd_module.parachuteStrength_costScale, next_upgrade.parachuteStrength);
				} else if (selected_upgrade_option == "Max Temp") {


					upgrade_function = UpgradeMaxTemperature;
					current_upgrade_count = current_upgrade.maxTemperature;
					next_upgrade_count = ++next_upgrade.maxTemperature;

#if true
					UpgradeData u_data = KRnDSettings.GetData(StringConstants.MAX_TEMPERATURE);
					current_improvement = u_data.CalculateImprovementFactor(current_upgrade.maxTemperature);
					next_improvement = u_data.CalculateImprovementFactor(next_upgrade.maxTemperature);


					//if (!KRnD.originalStats.TryGetValue(part.name, out var original_stats)) throw new Exception("no original-stats for part '" + part.name + "'");
					science_cost = u_data.CalculateScienceCost((float)original_stats.skinMaxTemp, next_upgrade.maxTemperature);
#else

					currentImprovement = KRnD.CalculateImprovementFactor(rndModule.maxTemperature_improvement, rndModule.maxTemperature_improvementScale, currentUpgrade.maxTemperature);
					nextImprovement = KRnD.CalculateImprovementFactor(rndModule.maxTemperature_improvement, rndModule.maxTemperature_improvementScale, nextUpgrade.maxTemperature);
					scienceCost = KRnD.CalculateScienceCost(rndModule.maxTemperature_scienceCost, rndModule.maxTemperature_costScale, nextUpgrade.maxTemperature);
#endif

				} else {
					throw new Exception("unexpected option '" + selected_upgrade_option + "'");
				}

				var new_info = GetPartInfo(part, next_upgrade); // Calculate part-info if the selected stat was upgraded.
				new_info = HighlightChanges(current_info, new_info);

				// Current stats:
				GUILayout.BeginArea(new Rect(10 + options_width + 10, 30, _windowStyle.fixedWidth, 20));
				GUILayout.Label("<color=#FFFFFF><b>Current:</b> " + current_upgrade_count + " (" + current_improvement.ToString("+0.##;-0.##;-") + ")</color>", _labelStyle);
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
				GUILayout.Label("<color=#FFFFFF><b>Next upgrade:</b> " + next_upgrade_count + " (" + next_improvement.ToString("+0.##%;-0.##%;-") + ")</color>", _labelStyle);
				GUILayout.EndArea();

				GUILayout.BeginArea(new Rect(10 + options_width + area_width, 30 + 20, area_width, area_height));
				_scrollPos = GUILayout.BeginScrollView(_scrollPos, _scrollStyle, GUILayout.Width(area_width), GUILayout.Height(area_height));
				GUILayout.Label(new_info, _labelStyleSmall);
				GUILayout.EndScrollView();
				GUILayout.EndArea();

				// Bottom-line (display only if the upgrade would have an effect):
				if (current_improvement != next_improvement) {
					GUILayout.BeginArea(new Rect(10, _windowStyle.fixedHeight - 25, _windowStyle.fixedWidth, 30));
					float current_science = 0;
					if (ResearchAndDevelopment.Instance != null) current_science = ResearchAndDevelopment.Instance.Science;
					var color = "FF0000";
					if (current_science >= science_cost) color = "00FF00";
					GUILayout.Label("<b>Science: <color=#" + color + ">" + science_cost + " / " + Math.Floor(current_science) + "</color></b>", _labelStyle);
					GUILayout.EndArea();
					if (current_science >= science_cost && ResearchAndDevelopment.Instance != null && upgrade_function != null) {
						GUILayout.BeginArea(new Rect(_windowStyle.fixedWidth - 110, _windowStyle.fixedHeight - 30, 100, 30));
						if (GUILayout.Button("Research", _buttonStyle)) {
							upgrade_function(part);
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
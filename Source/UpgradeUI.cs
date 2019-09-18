using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using KSP.UI.Screens;
using UnityEngine;

// For "ApplicationLauncherButton"

namespace KRnD.Source
{
	[KSPAddon(KSPAddon.Startup.EditorAny, false)]
	public class UpgradeUI : MonoBehaviour
	{
		// TODO: The Application-Button shows up during the flight scene ...
		private static ApplicationLauncherButton _launcherButton;
		public static Rect windowPosition = new Rect(300, 60, 450, 400 + 80);
		private static readonly GUIStyle _windowStyle = new GUIStyle(HighLogic.Skin.window) {fixedWidth = 500f, fixedHeight = 300f + 80};
		private static readonly GUIStyle _labelStyle = new GUIStyle(HighLogic.Skin.label);
		private static readonly GUIStyle _labelStyleSmall = new GUIStyle(HighLogic.Skin.label) {fontSize = 10};
		private static readonly GUIStyle _buttonStyle = new GUIStyle(HighLogic.Skin.button);
		private static readonly GUIStyle _scrollStyle = new GUIStyle(HighLogic.Skin.scrollView);
		private static Vector2 _scrollPos = Vector2.zero;
		//private static Texture2D texture;
		private static bool _showGui;

		private static Texture2D _closeIcon;

		// The part that was last selected in the editor:
		public static Part selectedPart;

		private int _selectedUpgradeOption;

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
			GameEvents.onGUIApplicationLauncherReady.Add(ReadyEvent);
			//GameEvents.onGUIApplicationLauncherDestroyed.Remove(DestroyEvent);
			GameEvents.onGUIApplicationLauncherDestroyed.Add(DestroyEvent);
		}

		private void OnDestroy()
		{
			GameEvents.onGUIApplicationLauncherReady.Remove(ReadyEvent);
			GameEvents.onGUIApplicationLauncherDestroyed.Remove(DestroyEvent);
		}

		// Fires when a scene is ready so we can install our button.
		public void ReadyEvent()
		{
			if (ApplicationLauncher.Ready && _launcherButton == null) {
				var visible_in_scenes = ApplicationLauncher.AppScenes.SPH | ApplicationLauncher.AppScenes.VAB;
				var texture_file = Constants.MOD_DIRECTORY + Constants.APP_ICON;
				_launcherButton = ApplicationLauncher.Instance.AddModApplication(GuiToggle, GuiToggle, null, null, null, null, visible_in_scenes, GameDatabase.Instance.GetTexture(texture_file, false));
			}
		}

		// Fires when a scene is unloaded and we should destroy our button:
		public void DestroyEvent()
		{
			if (_launcherButton == null) return;
			ApplicationLauncher.Instance.RemoveModApplication(_launcherButton);
			_launcherButton = null;
			selectedPart = null;
			_showGui = false;
		}

		private void GuiToggle()
		{
			_showGui = !_showGui;
		}


		public void OnGUI()
		{
			if (_showGui) {

				if (_closeIcon == null) _closeIcon = GameDatabase.Instance.GetTexture(Constants.MOD_DIRECTORY + Constants.CLOSE_ICON, false);

				GUI.depth = 0;
				windowPosition = GUILayout.Window(100, windowPosition, OnWindow, "", _windowStyle);
				const int icon_size = 28;
				if (GUI.Button(new Rect(windowPosition.xMax - (icon_size + 2), windowPosition.yMin + 2, icon_size, icon_size), _closeIcon, GUI.skin.button)) {
					_showGui = false;
				}
			}
		}

		public static int UpgradeIspVac(Part part)
		{
			try {
				PartUpgrades store;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
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
				PartUpgrades store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
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
				PartUpgrades store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
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
				PartUpgrades store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
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
				PartUpgrades store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
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
				PartUpgrades store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
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
				PartUpgrades store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
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
				PartUpgrades store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
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
				PartUpgrades store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
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
				PartUpgrades store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
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
				PartUpgrades store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
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
				PartUpgrades store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
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
				PartUpgrades store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
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
		private string getPartInfo(Part part, PartUpgrades upgradesToApply = null)
		{
			var info = "";
			PartUpgrades originalUpgrades = null;
			try {
				var rndModule = KRnD.GetKRnDModule(part);
				if (rndModule == null || (originalUpgrades = rndModule.GetCurrentUpgrades()) == null) return info;

				// Upgrade the part to get the correct info, we revert it back to its previous values in the finally block below:
				KRnD.UpdatePart(part, upgradesToApply);
				var engineModules = KRnD.GetEngineModules(part);
				var rcsModule = KRnD.GetRcsModule(part);
				var reactionWheelModule = KRnD.GetReactionWheelModule(part);
				var solarPanelModule = KRnD.GetSolarPanelModule(part);
				var landingLegModule = KRnD.GetLandingLegModule(part);
				var electricChargeResource = KRnD.GetChargeResource(part);
				var generatorModule = KRnD.GetGeneratorModule(part);
				var fissionGenerator = KRnD.GetFissionGeneratorModule(part);
				var converterModules = KRnD.GetConverterModules(part);
				var parachuteModule = KRnD.GetParachuteModule(part);
				var fairingModule = KRnD.GetFairingModule(part);
				var fuelResources = KRnD.GetFuelResources(part);

				// Basic stats:
				info = "<color=#FFFFFF><b>Dry Mass:</b> " + part.mass.ToString("0.#### t") + "\n";

				info += "<b>Max Temp.:</b> " + part.maxTemp.ToString("0.#") + "/" + part.skinMaxTemp.ToString("0.#") + " °K\n";

				if (landingLegModule != null) info += "<b>Crash Tolerance:</b> " + part.crashTolerance.ToString("0.#### m/s") + "\n";

				if (electricChargeResource != null) info += "<b>Electric Charge:</b> " + electricChargeResource.maxAmount + "\n";

				// Fuels:
				if (fuelResources != null)
					foreach (var fuelResource in fuelResources) {
						// Reformat resource-names like "ElectricCharge" to "Electric Charge":
						var fuelName = fuelResource.resourceName;
						fuelName = Regex.Replace(fuelName, @"([a-z])([A-Z])", "$1 $2");
						info += "<b>" + fuelName + ":</b> " + fuelResource.maxAmount + "\n";
					}

				// Module stats:
				info += "\n";
				if (engineModules != null)
					foreach (var engineModule in engineModules) {
						info += "<color=#99FF00><b>Engine";
						if (engineModules.Count > 1) info += " (" + engineModule.engineID + ")";
						info += ":</b></color>\n" + engineModule.GetInfo();
						if (engineModules.Count > 1) info += "\n";
					}

				if (rcsModule) info += "<color=#99FF00><b>RCS:</b></color>\n" + rcsModule.GetInfo();
				if (reactionWheelModule) info += "<color=#99FF00><b>Reaction Wheel:</b></color>\n" + reactionWheelModule.GetInfo();
				if (solarPanelModule) info += "<color=#99FF00><b>Solar Panel:</b></color>\n" + KRnD.GetSolarPanelInfo(solarPanelModule);
				if (generatorModule) info += "<color=#99FF00><b>Generator:</b></color>\n" + generatorModule.GetInfo();
				if (fissionGenerator) info += "<color=#99FF00><b>Fission-Generator:</b></color>\n" + fissionGenerator.GetInfo();
				if (converterModules != null)
					foreach (var converterModule in converterModules)
						info += "<color=#99FF00><b>Converter " + converterModule.ConverterName + ":</b></color>\n" + converterModule.GetInfo() + "\n";
				if (parachuteModule) info += "<color=#99FF00><b>Parachute:</b></color>\n" + parachuteModule.GetInfo();
				if (fairingModule) info += "<color=#99FF00><b>Fairing:</b></color>\n" + fairingModule.GetInfo();
				info += "</color>";
			} catch (Exception e) {
				Debug.LogError("[KRnDGUI] getPartInfo(): " + e);
			} finally {
				try {
					if (originalUpgrades != null) KRnD.UpdatePart(part, originalUpgrades);
				} catch (Exception e) {
					Debug.LogError("[KRnDGUI] getPartInfo() restore of part failed: " + e);
				}
			}

			return info;
		}

		// Highlights differences between the two given texts, assuming they contain the same number of words.
		private string highlightChanges(string originalText, string newText, string color = "00FF00")
		{
			var highlightedText = "";
			try {
				// Split as whitespaces and tags, we only need normal words and numbers:
				var set1 = Regex.Split(originalText, @"([\s<>])");
				var set2 = Regex.Split(newText, @"([\s<>])");
				for (var i = 0; i < set2.Length; i++) {
					var oldWord = "";
					if (i < set1.Length) oldWord = set1[i];
					var newWord = set2[i];

					if (oldWord != newWord) newWord = "<color=#" + color + "><b>" + newWord + "</b></color>";
					highlightedText += newWord;
				}
			} catch (Exception e) {
				Debug.LogError("[KRnDGUI] highlightChanges(): " + e);
			}

			if (highlightedText == "") return newText;
			return highlightedText;
		}

		private void OnWindow(int windowId)
		{
			try {
				GUILayout.BeginVertical();

				// Get all modules of the selected part:
				var partTitle = "";
				Part part = null;
				KRnDModule rndModule = null;
				List<ModuleEngines> engineModules = null;
				ModuleRCS rcsModule = null;
				ModuleReactionWheel reactionWheelModule = null;
				ModuleDeployableSolarPanel solarPanelModule = null;
				ModuleWheelBase landingLegModule = null;
				PartResource electricChargeResource = null;
				ModuleGenerator generatorModule = null;
				PartModule fissionGenerator = null;
				List<ModuleResourceConverter> converterModules = null;
				ModuleParachute parachuteModule = null;
				List<PartResource> fuelResources = null;
				if (selectedPart != null) {
					foreach (var aPart in PartLoader.LoadedPartsList)
						if (aPart.partPrefab.name == selectedPart.name) {
							part = aPart.partPrefab;
							partTitle = aPart.title;
							break;
						}

					if (part) {
						rndModule = KRnD.GetKRnDModule(part);
						engineModules = KRnD.GetEngineModules(part);
						rcsModule = KRnD.GetRcsModule(part);
						reactionWheelModule = KRnD.GetReactionWheelModule(part);
						solarPanelModule = KRnD.GetSolarPanelModule(part);
						landingLegModule = KRnD.GetLandingLegModule(part);
						electricChargeResource = KRnD.GetChargeResource(part);
						generatorModule = KRnD.GetGeneratorModule(part);
						fissionGenerator = KRnD.GetFissionGeneratorModule(part);
						converterModules = KRnD.GetConverterModules(part);
						parachuteModule = KRnD.GetParachuteModule(part);
						fuelResources = KRnD.GetFuelResources(part);
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

				if (!rndModule) {
					// Invalid part selected:
					GUILayout.BeginArea(new Rect(10, 5, _windowStyle.fixedWidth, 20));
					GUILayout.Label("<b>Kerbal R&D: Select a different part to improve</b>", _labelStyle);
					GUILayout.EndArea();
					GUILayout.EndVertical();
					GUI.DragWindow();
					return;
				}

				// Get stats of the current version of the selected part:
				PartUpgrades currentUpgrade;
				if (!KRnD.upgrades.TryGetValue(part.name, out currentUpgrade)) currentUpgrade = new PartUpgrades();
				var currentInfo = getPartInfo(part, currentUpgrade);

				// Create a copy of the part-stats which we can use to mock an upgrade further below:
				var nextUpgrade = currentUpgrade.Clone();

				// Title:
				GUILayout.BeginArea(new Rect(10, 5, _windowStyle.fixedWidth, 20));
				var version = rndModule.GetVersion();
				if (version != "") version = " - " + version;
				GUILayout.Label("<b>" + partTitle + version + "</b>", _labelStyle);
				GUILayout.EndArea();

				// List with upgrade-options:
				float optionsWidth = 100;
				var optionsHeight = _windowStyle.fixedHeight - 30 - 30 - 20;
				GUILayout.BeginArea(new Rect(10, 30 + 20, optionsWidth, optionsHeight));


				GUILayout.BeginVertical();

				var options = new List<string>();
				options.Add("Dry Mass");
				options.Add("Max Temp");
				if (engineModules != null || rcsModule) {
					options.Add("ISP Vac");
					options.Add("ISP Atm");
					options.Add("Fuel Flow");
				}

				if (reactionWheelModule != null) options.Add("Torque");
				if (solarPanelModule != null) options.Add("Charge Rate");
				if (landingLegModule != null) options.Add("Crash Tolerance");
				if (electricChargeResource != null) options.Add("Battery");
				if (fuelResources != null) options.Add("Fuel Pressure");
				if (generatorModule || fissionGenerator) options.Add("Generator");
				if (converterModules != null) options.Add("Converter");
				if (parachuteModule) options.Add("Parachute");
				if (this._selectedUpgradeOption >= options.Count) this._selectedUpgradeOption = 0;
				this._selectedUpgradeOption = GUILayout.SelectionGrid(this._selectedUpgradeOption, options.ToArray(), 1, _buttonStyle);

				GUILayout.EndVertical();

				GUILayout.EndArea();

				var selectedUpgradeOption = options.ToArray()[this._selectedUpgradeOption];
				var currentUpgradeCount = 0;
				var nextUpgradeCount = 0;
				var scienceCost = 0;
				float currentImprovement = 0;
				float nextImprovement = 0;
				Func<Part, int> upgradeFunction = null;
				if (selectedUpgradeOption == "ISP Vac") {
					upgradeFunction = UpgradeIspVac;
					currentUpgradeCount = currentUpgrade.ispVac;
					nextUpgradeCount = ++nextUpgrade.ispVac;
					currentImprovement = KRnD.CalculateImprovementFactor(rndModule.ispVac_improvement, rndModule.ispVac_improvementScale, currentUpgrade.ispVac);
					nextImprovement = KRnD.CalculateImprovementFactor(rndModule.ispVac_improvement, rndModule.ispVac_improvementScale, nextUpgrade.ispVac);
					scienceCost = KRnD.CalculateScienceCost(rndModule.ispVac_scienceCost, rndModule.ispVac_costScale, nextUpgrade.ispVac);
				} else if (selectedUpgradeOption == "ISP Atm") {
					upgradeFunction = UpgradeIspAtm;
					currentUpgradeCount = currentUpgrade.ispAtm;
					nextUpgradeCount = ++nextUpgrade.ispAtm;
					currentImprovement = KRnD.CalculateImprovementFactor(rndModule.ispAtm_improvement, rndModule.ispAtm_improvementScale, currentUpgrade.ispAtm);
					nextImprovement = KRnD.CalculateImprovementFactor(rndModule.ispAtm_improvement, rndModule.ispAtm_improvementScale, nextUpgrade.ispAtm);
					scienceCost = KRnD.CalculateScienceCost(rndModule.ispAtm_scienceCost, rndModule.ispAtm_costScale, nextUpgrade.ispAtm);
				} else if (selectedUpgradeOption == "Fuel Flow") {
					upgradeFunction = UpgradeFuelFlow;
					currentUpgradeCount = currentUpgrade.fuelFlow;
					nextUpgradeCount = ++nextUpgrade.fuelFlow;
					currentImprovement = KRnD.CalculateImprovementFactor(rndModule.fuelFlow_improvement, rndModule.fuelFlow_improvementScale, currentUpgrade.fuelFlow);
					nextImprovement = KRnD.CalculateImprovementFactor(rndModule.fuelFlow_improvement, rndModule.fuelFlow_improvementScale, nextUpgrade.fuelFlow);
					scienceCost = KRnD.CalculateScienceCost(rndModule.fuelFlow_scienceCost, rndModule.fuelFlow_costScale, nextUpgrade.fuelFlow);
				} else if (selectedUpgradeOption == "Dry Mass") {
					upgradeFunction = UpgradeDryMass;
					currentUpgradeCount = currentUpgrade.dryMass;
					nextUpgradeCount = ++nextUpgrade.dryMass;
					currentImprovement = KRnD.CalculateImprovementFactor(rndModule.dryMass_improvement, rndModule.dryMass_improvementScale, currentUpgrade.dryMass);
					nextImprovement = KRnD.CalculateImprovementFactor(rndModule.dryMass_improvement, rndModule.dryMass_improvementScale, nextUpgrade.dryMass);

					// Scale science cost with original mass:
					PartStats originalStats;
					if (!KRnD.originalStats.TryGetValue(part.name, out originalStats)) throw new Exception("no original-stats for part '" + part.name + "'");
					float scaleReferenceFactor = 1;
					if (rndModule.dryMass_costScaleReference > 0) scaleReferenceFactor = originalStats.dryMass / rndModule.dryMass_costScaleReference;
					var scaledCost = (int) Math.Round(rndModule.dryMass_scienceCost * scaleReferenceFactor);
					if (scaledCost < 1) scaledCost = 1;
					scienceCost = KRnD.CalculateScienceCost(scaledCost, rndModule.dryMass_costScale, nextUpgrade.dryMass);
				} else if (selectedUpgradeOption == "Torque") {
					upgradeFunction = UpgradeTorque;
					currentUpgradeCount = currentUpgrade.torque;
					nextUpgradeCount = ++nextUpgrade.torque;
					currentImprovement = KRnD.CalculateImprovementFactor(rndModule.torque_improvement, rndModule.torque_improvementScale, currentUpgrade.torque);
					nextImprovement = KRnD.CalculateImprovementFactor(rndModule.torque_improvement, rndModule.torque_improvementScale, nextUpgrade.torque);
					scienceCost = KRnD.CalculateScienceCost(rndModule.torque_scienceCost, rndModule.torque_costScale, nextUpgrade.torque);
				} else if (selectedUpgradeOption == "Charge Rate") {
					upgradeFunction = UpgradeChargeRate;
					currentUpgradeCount = currentUpgrade.chargeRate;
					nextUpgradeCount = ++nextUpgrade.chargeRate;
					currentImprovement = KRnD.CalculateImprovementFactor(rndModule.chargeRate_improvement, rndModule.chargeRate_improvementScale, currentUpgrade.chargeRate);
					nextImprovement = KRnD.CalculateImprovementFactor(rndModule.chargeRate_improvement, rndModule.chargeRate_improvementScale, nextUpgrade.chargeRate);
					scienceCost = KRnD.CalculateScienceCost(rndModule.chargeRate_scienceCost, rndModule.chargeRate_costScale, nextUpgrade.chargeRate);
				} else if (selectedUpgradeOption == "Crash Tolerance") {
					upgradeFunction = UpgradeCrashTolerance;
					currentUpgradeCount = currentUpgrade.crashTolerance;
					nextUpgradeCount = ++nextUpgrade.crashTolerance;
					currentImprovement = KRnD.CalculateImprovementFactor(rndModule.crashTolerance_improvement, rndModule.crashTolerance_improvementScale, currentUpgrade.crashTolerance);
					nextImprovement = KRnD.CalculateImprovementFactor(rndModule.crashTolerance_improvement, rndModule.crashTolerance_improvementScale, nextUpgrade.crashTolerance);
					scienceCost = KRnD.CalculateScienceCost(rndModule.crashTolerance_scienceCost, rndModule.crashTolerance_costScale, nextUpgrade.crashTolerance);
				} else if (selectedUpgradeOption == "Battery") {
					upgradeFunction = UpgradeBatteryCharge;
					currentUpgradeCount = currentUpgrade.batteryCharge;
					nextUpgradeCount = ++nextUpgrade.batteryCharge;
					currentImprovement = KRnD.CalculateImprovementFactor(rndModule.batteryCharge_improvement, rndModule.batteryCharge_improvementScale, currentUpgrade.batteryCharge);
					nextImprovement = KRnD.CalculateImprovementFactor(rndModule.batteryCharge_improvement, rndModule.batteryCharge_improvementScale, nextUpgrade.batteryCharge);

					// Scale science cost with original battery charge:
					PartStats originalStats;
					if (!KRnD.originalStats.TryGetValue(part.name, out originalStats)) throw new Exception("no origional-stats for part '" + part.name + "'");
					double scaleReferenceFactor = 1;
					if (rndModule.batteryCharge_costScaleReference > 0) scaleReferenceFactor = originalStats.batteryCharge / rndModule.batteryCharge_costScaleReference;
					var scaledCost = (int) Math.Round(rndModule.batteryCharge_scienceCost * scaleReferenceFactor);
					if (scaledCost < 1) scaledCost = 1;
					scienceCost = KRnD.CalculateScienceCost(scaledCost, rndModule.batteryCharge_costScale, nextUpgrade.batteryCharge);
				} else if (selectedUpgradeOption == "Fuel Pressure") {
					upgradeFunction = UpgradeFuelCapacity;
					currentUpgradeCount = currentUpgrade.fuelCapacity;
					nextUpgradeCount = ++nextUpgrade.fuelCapacity;
					currentImprovement = KRnD.CalculateImprovementFactor(rndModule.fuelCapacity_improvement, rndModule.fuelCapacity_improvementScale, currentUpgrade.fuelCapacity);
					nextImprovement = KRnD.CalculateImprovementFactor(rndModule.fuelCapacity_improvement, rndModule.fuelCapacity_improvementScale, nextUpgrade.fuelCapacity);

					// Scale science cost with original fuel capacity:
					PartStats originalStats;
					if (!KRnD.originalStats.TryGetValue(part.name, out originalStats)) throw new Exception("no original-stats for part '" + part.name + "'");
					double scaleReferenceFactor = 1;
					if (rndModule.fuelCapacity_costScaleReference > 0) scaleReferenceFactor = originalStats.fuelCapacitiesSum / rndModule.fuelCapacity_costScaleReference;
					var scaledCost = (int) Math.Round(rndModule.fuelCapacity_scienceCost * scaleReferenceFactor);
					if (scaledCost < 1) scaledCost = 1;
					scienceCost = KRnD.CalculateScienceCost(scaledCost, rndModule.fuelCapacity_costScale, nextUpgrade.fuelCapacity);
				} else if (selectedUpgradeOption == "Generator") {
					upgradeFunction = UpgradeGeneratorEfficiency;
					currentUpgradeCount = currentUpgrade.generatorEfficiency;
					nextUpgradeCount = ++nextUpgrade.generatorEfficiency;
					currentImprovement = KRnD.CalculateImprovementFactor(rndModule.generatorEfficiency_improvement, rndModule.generatorEfficiency_improvementScale, currentUpgrade.generatorEfficiency);
					nextImprovement = KRnD.CalculateImprovementFactor(rndModule.generatorEfficiency_improvement, rndModule.generatorEfficiency_improvementScale, nextUpgrade.generatorEfficiency);
					scienceCost = KRnD.CalculateScienceCost(rndModule.generatorEfficiency_scienceCost, rndModule.generatorEfficiency_costScale, nextUpgrade.generatorEfficiency);
				} else if (selectedUpgradeOption == "Converter") {
					upgradeFunction = UpgradeConverterEfficiency;
					currentUpgradeCount = currentUpgrade.converterEfficiency;
					nextUpgradeCount = ++nextUpgrade.converterEfficiency;
					currentImprovement = KRnD.CalculateImprovementFactor(rndModule.converterEfficiency_improvement, rndModule.converterEfficiency_improvementScale, currentUpgrade.converterEfficiency);
					nextImprovement = KRnD.CalculateImprovementFactor(rndModule.converterEfficiency_improvement, rndModule.converterEfficiency_improvementScale, nextUpgrade.converterEfficiency);
					scienceCost = KRnD.CalculateScienceCost(rndModule.converterEfficiency_scienceCost, rndModule.converterEfficiency_costScale, nextUpgrade.converterEfficiency);
				} else if (selectedUpgradeOption == "Parachute") {
					upgradeFunction = UpgradeParachuteStrength;
					currentUpgradeCount = currentUpgrade.parachuteStrength;
					nextUpgradeCount = ++nextUpgrade.parachuteStrength;
					currentImprovement = KRnD.CalculateImprovementFactor(rndModule.parachuteStrength_improvement, rndModule.parachuteStrength_improvementScale, currentUpgrade.parachuteStrength);
					nextImprovement = KRnD.CalculateImprovementFactor(rndModule.parachuteStrength_improvement, rndModule.parachuteStrength_improvementScale, nextUpgrade.parachuteStrength);
					scienceCost = KRnD.CalculateScienceCost(rndModule.parachuteStrength_scienceCost, rndModule.parachuteStrength_costScale, nextUpgrade.parachuteStrength);
				} else if (selectedUpgradeOption == "Max Temp") {


					upgradeFunction = UpgradeMaxTemperature;
					currentUpgradeCount = currentUpgrade.maxTemperature;
					nextUpgradeCount = ++nextUpgrade.maxTemperature;

#if true
					UpgradeData u_data = KRnDSettings.GetData(Constants.MAX_TEMPERATURE);
					currentImprovement = u_data.CalculateImprovementFactor(currentUpgrade.maxTemperature);
					nextImprovement = u_data.CalculateImprovementFactor(nextUpgrade.maxTemperature);


					if (!KRnD.originalStats.TryGetValue(part.name, out var original_stats)) throw new Exception("no original-stats for part '" + part.name + "'");
					scienceCost = u_data.CalculateScienceCost((float)original_stats.skinMaxTemp, nextUpgrade.maxTemperature);
#else

					currentImprovement = KRnD.CalculateImprovementFactor(rndModule.maxTemperature_improvement, rndModule.maxTemperature_improvementScale, currentUpgrade.maxTemperature);
					nextImprovement = KRnD.CalculateImprovementFactor(rndModule.maxTemperature_improvement, rndModule.maxTemperature_improvementScale, nextUpgrade.maxTemperature);
					scienceCost = KRnD.CalculateScienceCost(rndModule.maxTemperature_scienceCost, rndModule.maxTemperature_costScale, nextUpgrade.maxTemperature);
#endif

				} else {
					throw new Exception("unexpected option '" + selectedUpgradeOption + "'");
				}

				var newInfo = getPartInfo(part, nextUpgrade); // Calculate part-info if the selected stat was upgraded.
				newInfo = highlightChanges(currentInfo, newInfo);

				// Current stats:
				GUILayout.BeginArea(new Rect(10 + optionsWidth + 10, 30, _windowStyle.fixedWidth, 20));
				GUILayout.Label("<color=#FFFFFF><b>Current:</b> " + currentUpgradeCount + " (" + currentImprovement.ToString("+0.##%;-0.##%;-") + ")</color>", _labelStyle);
				GUILayout.EndArea();

				var areaWidth = (_windowStyle.fixedWidth - 20 - optionsWidth) / 2;
				var areaHeight = optionsHeight;
				GUILayout.BeginArea(new Rect(10 + optionsWidth, 30 + 20, areaWidth, areaHeight));
				_scrollPos = GUILayout.BeginScrollView(_scrollPos, _scrollStyle, GUILayout.Width(areaWidth), GUILayout.Height(areaHeight));

				GUILayout.Label(currentInfo, _labelStyleSmall);
				GUILayout.EndScrollView();
				GUILayout.EndArea();

				// Next stats:
				GUILayout.BeginArea(new Rect(10 + optionsWidth + areaWidth + 10, 30, _windowStyle.fixedWidth, 20));
				GUILayout.Label("<color=#FFFFFF><b>Next upgrade:</b> " + nextUpgradeCount + " (" + nextImprovement.ToString("+0.##%;-0.##%;-") + ")</color>", _labelStyle);
				GUILayout.EndArea();

				GUILayout.BeginArea(new Rect(10 + optionsWidth + areaWidth, 30 + 20, areaWidth, areaHeight));
				_scrollPos = GUILayout.BeginScrollView(_scrollPos, _scrollStyle, GUILayout.Width(areaWidth), GUILayout.Height(areaHeight));
				GUILayout.Label(newInfo, _labelStyleSmall);
				GUILayout.EndScrollView();
				GUILayout.EndArea();

				// Bottom-line (display only if the upgrade would have an effect):
				if (currentImprovement != nextImprovement) {
					GUILayout.BeginArea(new Rect(10, _windowStyle.fixedHeight - 25, _windowStyle.fixedWidth, 30));
					float currentScience = 0;
					if (ResearchAndDevelopment.Instance != null) currentScience = ResearchAndDevelopment.Instance.Science;
					var color = "FF0000";
					if (currentScience >= scienceCost) color = "00FF00";
					GUILayout.Label("<b>Science: <color=#" + color + ">" + scienceCost + " / " + Math.Floor(currentScience) + "</color></b>", _labelStyle);
					GUILayout.EndArea();
					if (currentScience >= scienceCost && ResearchAndDevelopment.Instance != null && upgradeFunction != null) {
						GUILayout.BeginArea(new Rect(_windowStyle.fixedWidth - 110, _windowStyle.fixedHeight - 30, 100, 30));
						if (GUILayout.Button("Research", _buttonStyle)) {
							upgradeFunction(part);
							ResearchAndDevelopment.Instance.AddScience(-scienceCost, TransactionReasons.RnDTechResearch);
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
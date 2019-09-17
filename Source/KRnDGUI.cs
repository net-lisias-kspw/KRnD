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
	public class KRnDGUI : MonoBehaviour
	{
		// TODO: The Application-Button shows up during the flight scene ...
		private static ApplicationLauncherButton button;
		public static Rect windowPosition = new Rect(300, 60, 450, 400 + 80);
		private static readonly GUIStyle windowStyle = new GUIStyle(HighLogic.Skin.window) {fixedWidth = 500f, fixedHeight = 300f + 80};
		private static readonly GUIStyle labelStyle = new GUIStyle(HighLogic.Skin.label);
		private static readonly GUIStyle labelStyleSmall = new GUIStyle(HighLogic.Skin.label) {fontSize = 10};
		private static readonly GUIStyle buttonStyle = new GUIStyle(HighLogic.Skin.button);
		private static readonly GUIStyle scrollStyle = new GUIStyle(HighLogic.Skin.scrollView);
		private static Vector2 scrollPos = Vector2.zero;
		private static Texture2D texture;
		private static bool showGui;

		// The part that was last selected in the editor:
		public static Part selectedPart;

		private int selectedUpgradeOption;

		private void Awake()
		{
			if (texture == null) {
				texture = new Texture2D(36, 36, TextureFormat.RGBA32, false);
				var textureFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Constants.APP_ICON);
				texture.LoadImage(File.ReadAllBytes(textureFile));
			}

			// Add event-handlers to create and destroy our button:
			GameEvents.onGUIApplicationLauncherReady.Remove(ReadyEvent);
			GameEvents.onGUIApplicationLauncherReady.Add(ReadyEvent);
			GameEvents.onGUIApplicationLauncherDestroyed.Remove(DestroyEvent);
			GameEvents.onGUIApplicationLauncherDestroyed.Add(DestroyEvent);
		}

		// Fires when a scene is ready so we can install our button.
		public void ReadyEvent()
		{
			if (ApplicationLauncher.Ready && button == null) {
				var visibleScense = ApplicationLauncher.AppScenes.SPH | ApplicationLauncher.AppScenes.VAB;
				button = ApplicationLauncher.Instance.AddModApplication(GuiOn, GuiOff, null, null, null, null, visibleScense, texture);
			}
		}

		// Fires when a scene is unloaded and we should destroy our button:
		public void DestroyEvent()
		{
			if (button == null) return;
			ApplicationLauncher.Instance.RemoveModApplication(button);
			button = null;
			selectedPart = null;
			showGui = false;
		}

		private void GuiOn()
		{
			showGui = true;
		}

		private void GuiOff()
		{
			showGui = false;
		}

		public void OnGUI()
		{
			if (showGui) windowPosition = GUILayout.Window(100, windowPosition, OnWindow, "", windowStyle);
		}

		public static int UpgradeIspVac(Part part)
		{
			try {
				KRnDUpgrade store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
					store = new KRnDUpgrade();
					KRnD.upgrades.Add(part.name, store);
				}

				store.ispVac++;
				KRnD.updateGlobalParts();
				KRnD.updateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeIspVac(): " + e);
			}

			return 0;
		}

		public static int UpgradeIspAtm(Part part)
		{
			try {
				KRnDUpgrade store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
					store = new KRnDUpgrade();
					KRnD.upgrades.Add(part.name, store);
				}

				store.ispAtm++;
				KRnD.updateGlobalParts();
				KRnD.updateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeIspAtm(): " + e);
			}

			return 0;
		}

		public static int UpgradeDryMass(Part part)
		{
			try {
				KRnDUpgrade store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
					store = new KRnDUpgrade();
					KRnD.upgrades.Add(part.name, store);
				}

				store.dryMass++;
				KRnD.updateGlobalParts();
				KRnD.updateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeDryMass(): " + e);
			}

			return 0;
		}

		public static int UpgradeFuelFlow(Part part)
		{
			try {
				KRnDUpgrade store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
					store = new KRnDUpgrade();
					KRnD.upgrades.Add(part.name, store);
				}

				store.fuelFlow++;
				KRnD.updateGlobalParts();
				KRnD.updateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeFuelFlow(): " + e);
			}

			return 0;
		}

		public static int UpgradeTorque(Part part)
		{
			try {
				KRnDUpgrade store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
					store = new KRnDUpgrade();
					KRnD.upgrades.Add(part.name, store);
				}

				store.torque++;
				KRnD.updateGlobalParts();
				KRnD.updateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeTorque(): " + e);
			}

			return 0;
		}

		public static int UpgradeChargeRate(Part part)
		{
			try {
				KRnDUpgrade store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
					store = new KRnDUpgrade();
					KRnD.upgrades.Add(part.name, store);
				}

				store.chargeRate++;
				KRnD.updateGlobalParts();
				KRnD.updateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeChargeRate(): " + e);
			}

			return 0;
		}

		public static int UpgradeCrashTolerance(Part part)
		{
			try {
				KRnDUpgrade store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
					store = new KRnDUpgrade();
					KRnD.upgrades.Add(part.name, store);
				}

				store.crashTolerance++;
				KRnD.updateGlobalParts();
				KRnD.updateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeCrashTolerance(): " + e);
			}

			return 0;
		}

		public static int UpgradeBatteryCharge(Part part)
		{
			try {
				KRnDUpgrade store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
					store = new KRnDUpgrade();
					KRnD.upgrades.Add(part.name, store);
				}

				store.batteryCharge++;
				KRnD.updateGlobalParts();
				KRnD.updateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeBatteryCharge(): " + e);
			}

			return 0;
		}

		public static int UpgradeGeneratorEfficiency(Part part)
		{
			try {
				KRnDUpgrade store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
					store = new KRnDUpgrade();
					KRnD.upgrades.Add(part.name, store);
				}

				store.generatorEfficiency++;
				KRnD.updateGlobalParts();
				KRnD.updateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeGeneratorEfficiency(): " + e);
			}

			return 0;
		}

		public static int UpgradeConverterEfficiency(Part part)
		{
			try {
				KRnDUpgrade store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
					store = new KRnDUpgrade();
					KRnD.upgrades.Add(part.name, store);
				}

				store.converterEfficiency++;
				KRnD.updateGlobalParts();
				KRnD.updateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeConverterEfficiency(): " + e);
			}

			return 0;
		}

		public static int UpgradeParachuteStrength(Part part)
		{
			try {
				KRnDUpgrade store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
					store = new KRnDUpgrade();
					KRnD.upgrades.Add(part.name, store);
				}

				store.parachuteStrength++;
				KRnD.updateGlobalParts();
				KRnD.updateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeParachuteStrength(): " + e);
			}

			return 0;
		}

		public static int UpgradeMaxTemperature(Part part)
		{
			try {
				KRnDUpgrade store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
					store = new KRnDUpgrade();
					KRnD.upgrades.Add(part.name, store);
				}

				store.maxTemperature++;
				KRnD.updateGlobalParts();
				KRnD.updateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeMaxTemperature(): " + e);
			}

			return 0;
		}

		public static int UpgradeFuelCapacity(Part part)
		{
			try {
				KRnDUpgrade store = null;
				if (!KRnD.upgrades.TryGetValue(part.name, out store)) {
					store = new KRnDUpgrade();
					KRnD.upgrades.Add(part.name, store);
				}

				store.fuelCapacity++;
				KRnD.updateGlobalParts();
				KRnD.updateEditorVessel();
			} catch (Exception e) {
				Debug.LogError("[KRnD] UpgradeFuelCapacity(): " + e);
			}

			return 0;
		}

		// Returns the info-text of the given part with the given upgrades to be displayed in the GUI-comparison.
		private string getPartInfo(Part part, KRnDUpgrade upgradesToApply = null)
		{
			var info = "";
			KRnDUpgrade originalUpgrades = null;
			try {
				var rndModule = KRnD.GetKRnDModule(part);
				if (rndModule == null || (originalUpgrades = rndModule.getCurrentUpgrades()) == null) return info;

				// Upgrade the part to get the correct info, we revert it back to its previous values in the finally block below:
				KRnD.updatePart(part, upgradesToApply);
				var engineModules = KRnD.getEngineModules(part);
				var rcsModule = KRnD.getRcsModule(part);
				var reactionWheelModule = KRnD.getReactionWheelModule(part);
				var solarPanelModule = KRnD.getSolarPanelModule(part);
				var landingLegModule = KRnD.getLandingLegModule(part);
				var electricChargeResource = KRnD.getChargeResource(part);
				var generatorModule = KRnD.getGeneratorModule(part);
				var fissionGenerator = KRnD.getFissionGeneratorModule(part);
				var converterModules = KRnD.getConverterModules(part);
				var parachuteModule = KRnD.getParachuteModule(part);
				var fairingModule = KRnD.getFairingModule(part);
				var fuelResources = KRnD.getFuelResources(part);

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
				if (solarPanelModule) info += "<color=#99FF00><b>Solar Panel:</b></color>\n" + KRnD.getSolarPanelInfo(solarPanelModule);
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
					if (originalUpgrades != null) KRnD.updatePart(part, originalUpgrades);
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
						engineModules = KRnD.getEngineModules(part);
						rcsModule = KRnD.getRcsModule(part);
						reactionWheelModule = KRnD.getReactionWheelModule(part);
						solarPanelModule = KRnD.getSolarPanelModule(part);
						landingLegModule = KRnD.getLandingLegModule(part);
						electricChargeResource = KRnD.getChargeResource(part);
						generatorModule = KRnD.getGeneratorModule(part);
						fissionGenerator = KRnD.getFissionGeneratorModule(part);
						converterModules = KRnD.getConverterModules(part);
						parachuteModule = KRnD.getParachuteModule(part);
						fuelResources = KRnD.getFuelResources(part);
					}
				}

				if (!part) {
					// No part selected:
					GUILayout.BeginArea(new Rect(10, 5, windowStyle.fixedWidth, 20));
					GUILayout.Label("<b>Kerbal R&D: Select a part to improve</b>", labelStyle);
					GUILayout.EndArea();
					GUILayout.EndVertical();
					GUI.DragWindow();
					return;
				}

				if (!rndModule) {
					// Invalid part selected:
					GUILayout.BeginArea(new Rect(10, 5, windowStyle.fixedWidth, 20));
					GUILayout.Label("<b>Kerbal R&D: Select a different part to improve</b>", labelStyle);
					GUILayout.EndArea();
					GUILayout.EndVertical();
					GUI.DragWindow();
					return;
				}

				// Get stats of the current version of the selected part:
				KRnDUpgrade currentUpgrade;
				if (!KRnD.upgrades.TryGetValue(part.name, out currentUpgrade)) currentUpgrade = new KRnDUpgrade();
				var currentInfo = getPartInfo(part, currentUpgrade);

				// Create a copy of the part-stats which we can use to mock an upgrade further below:
				var nextUpgrade = currentUpgrade.Clone();

				// Title:
				GUILayout.BeginArea(new Rect(10, 5, windowStyle.fixedWidth, 20));
				var version = rndModule.getVersion();
				if (version != "") version = " - " + version;
				GUILayout.Label("<b>" + partTitle + version + "</b>", labelStyle);
				GUILayout.EndArea();

				// List with upgrade-options:
				float optionsWidth = 100;
				var optionsHeight = windowStyle.fixedHeight - 30 - 30 - 20;
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
				if (this.selectedUpgradeOption >= options.Count) this.selectedUpgradeOption = 0;
				this.selectedUpgradeOption = GUILayout.SelectionGrid(this.selectedUpgradeOption, options.ToArray(), 1, buttonStyle);

				GUILayout.EndVertical();

				GUILayout.EndArea();

				var selectedUpgradeOption = options.ToArray()[this.selectedUpgradeOption];
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
					currentImprovement = KRnD.calculateImprovementFactor(rndModule.ispVac_improvement, rndModule.ispVac_improvementScale, currentUpgrade.ispVac);
					nextImprovement = KRnD.calculateImprovementFactor(rndModule.ispVac_improvement, rndModule.ispVac_improvementScale, nextUpgrade.ispVac);
					scienceCost = KRnD.calculateScienceCost(rndModule.ispVac_scienceCost, rndModule.ispVac_costScale, nextUpgrade.ispVac);
				} else if (selectedUpgradeOption == "ISP Atm") {
					upgradeFunction = UpgradeIspAtm;
					currentUpgradeCount = currentUpgrade.ispAtm;
					nextUpgradeCount = ++nextUpgrade.ispAtm;
					currentImprovement = KRnD.calculateImprovementFactor(rndModule.ispAtm_improvement, rndModule.ispAtm_improvementScale, currentUpgrade.ispAtm);
					nextImprovement = KRnD.calculateImprovementFactor(rndModule.ispAtm_improvement, rndModule.ispAtm_improvementScale, nextUpgrade.ispAtm);
					scienceCost = KRnD.calculateScienceCost(rndModule.ispAtm_scienceCost, rndModule.ispAtm_costScale, nextUpgrade.ispAtm);
				} else if (selectedUpgradeOption == "Fuel Flow") {
					upgradeFunction = UpgradeFuelFlow;
					currentUpgradeCount = currentUpgrade.fuelFlow;
					nextUpgradeCount = ++nextUpgrade.fuelFlow;
					currentImprovement = KRnD.calculateImprovementFactor(rndModule.fuelFlow_improvement, rndModule.fuelFlow_improvementScale, currentUpgrade.fuelFlow);
					nextImprovement = KRnD.calculateImprovementFactor(rndModule.fuelFlow_improvement, rndModule.fuelFlow_improvementScale, nextUpgrade.fuelFlow);
					scienceCost = KRnD.calculateScienceCost(rndModule.fuelFlow_scienceCost, rndModule.fuelFlow_costScale, nextUpgrade.fuelFlow);
				} else if (selectedUpgradeOption == "Dry Mass") {
					upgradeFunction = UpgradeDryMass;
					currentUpgradeCount = currentUpgrade.dryMass;
					nextUpgradeCount = ++nextUpgrade.dryMass;
					currentImprovement = KRnD.calculateImprovementFactor(rndModule.dryMass_improvement, rndModule.dryMass_improvementScale, currentUpgrade.dryMass);
					nextImprovement = KRnD.calculateImprovementFactor(rndModule.dryMass_improvement, rndModule.dryMass_improvementScale, nextUpgrade.dryMass);

					// Scale science cost with original mass:
					PartStats originalStats;
					if (!KRnD.originalStats.TryGetValue(part.name, out originalStats)) throw new Exception("no original-stats for part '" + part.name + "'");
					float scaleReferenceFactor = 1;
					if (rndModule.dryMass_costScaleReference > 0) scaleReferenceFactor = originalStats.mass / rndModule.dryMass_costScaleReference;
					var scaledCost = (int) Math.Round(rndModule.dryMass_scienceCost * scaleReferenceFactor);
					if (scaledCost < 1) scaledCost = 1;
					scienceCost = KRnD.calculateScienceCost(scaledCost, rndModule.dryMass_costScale, nextUpgrade.dryMass);
				} else if (selectedUpgradeOption == "Torque") {
					upgradeFunction = UpgradeTorque;
					currentUpgradeCount = currentUpgrade.torque;
					nextUpgradeCount = ++nextUpgrade.torque;
					currentImprovement = KRnD.calculateImprovementFactor(rndModule.torque_improvement, rndModule.torque_improvementScale, currentUpgrade.torque);
					nextImprovement = KRnD.calculateImprovementFactor(rndModule.torque_improvement, rndModule.torque_improvementScale, nextUpgrade.torque);
					scienceCost = KRnD.calculateScienceCost(rndModule.torque_scienceCost, rndModule.torque_costScale, nextUpgrade.torque);
				} else if (selectedUpgradeOption == "Charge Rate") {
					upgradeFunction = UpgradeChargeRate;
					currentUpgradeCount = currentUpgrade.chargeRate;
					nextUpgradeCount = ++nextUpgrade.chargeRate;
					currentImprovement = KRnD.calculateImprovementFactor(rndModule.chargeRate_improvement, rndModule.chargeRate_improvementScale, currentUpgrade.chargeRate);
					nextImprovement = KRnD.calculateImprovementFactor(rndModule.chargeRate_improvement, rndModule.chargeRate_improvementScale, nextUpgrade.chargeRate);
					scienceCost = KRnD.calculateScienceCost(rndModule.chargeRate_scienceCost, rndModule.chargeRate_costScale, nextUpgrade.chargeRate);
				} else if (selectedUpgradeOption == "Crash Tolerance") {
					upgradeFunction = UpgradeCrashTolerance;
					currentUpgradeCount = currentUpgrade.crashTolerance;
					nextUpgradeCount = ++nextUpgrade.crashTolerance;
					currentImprovement = KRnD.calculateImprovementFactor(rndModule.crashTolerance_improvement, rndModule.crashTolerance_improvementScale, currentUpgrade.crashTolerance);
					nextImprovement = KRnD.calculateImprovementFactor(rndModule.crashTolerance_improvement, rndModule.crashTolerance_improvementScale, nextUpgrade.crashTolerance);
					scienceCost = KRnD.calculateScienceCost(rndModule.crashTolerance_scienceCost, rndModule.crashTolerance_costScale, nextUpgrade.crashTolerance);
				} else if (selectedUpgradeOption == "Battery") {
					upgradeFunction = UpgradeBatteryCharge;
					currentUpgradeCount = currentUpgrade.batteryCharge;
					nextUpgradeCount = ++nextUpgrade.batteryCharge;
					currentImprovement = KRnD.calculateImprovementFactor(rndModule.batteryCharge_improvement, rndModule.batteryCharge_improvementScale, currentUpgrade.batteryCharge);
					nextImprovement = KRnD.calculateImprovementFactor(rndModule.batteryCharge_improvement, rndModule.batteryCharge_improvementScale, nextUpgrade.batteryCharge);

					// Scale science cost with original battery charge:
					PartStats originalStats;
					if (!KRnD.originalStats.TryGetValue(part.name, out originalStats)) throw new Exception("no origional-stats for part '" + part.name + "'");
					double scaleReferenceFactor = 1;
					if (rndModule.batteryCharge_costScaleReference > 0) scaleReferenceFactor = originalStats.batteryCharge / rndModule.batteryCharge_costScaleReference;
					var scaledCost = (int) Math.Round(rndModule.batteryCharge_scienceCost * scaleReferenceFactor);
					if (scaledCost < 1) scaledCost = 1;
					scienceCost = KRnD.calculateScienceCost(scaledCost, rndModule.batteryCharge_costScale, nextUpgrade.batteryCharge);
				} else if (selectedUpgradeOption == "Fuel Pressure") {
					upgradeFunction = UpgradeFuelCapacity;
					currentUpgradeCount = currentUpgrade.fuelCapacity;
					nextUpgradeCount = ++nextUpgrade.fuelCapacity;
					currentImprovement = KRnD.calculateImprovementFactor(rndModule.fuelCapacity_improvement, rndModule.fuelCapacity_improvementScale, currentUpgrade.fuelCapacity);
					nextImprovement = KRnD.calculateImprovementFactor(rndModule.fuelCapacity_improvement, rndModule.fuelCapacity_improvementScale, nextUpgrade.fuelCapacity);

					// Scale science cost with original fuel capacity:
					PartStats originalStats;
					if (!KRnD.originalStats.TryGetValue(part.name, out originalStats)) throw new Exception("no origional-stats for part '" + part.name + "'");
					double scaleReferenceFactor = 1;
					if (rndModule.fuelCapacity_costScaleReference > 0) scaleReferenceFactor = originalStats.fuelCapacitiesSum / rndModule.fuelCapacity_costScaleReference;
					var scaledCost = (int) Math.Round(rndModule.fuelCapacity_scienceCost * scaleReferenceFactor);
					if (scaledCost < 1) scaledCost = 1;
					scienceCost = KRnD.calculateScienceCost(scaledCost, rndModule.fuelCapacity_costScale, nextUpgrade.fuelCapacity);
				} else if (selectedUpgradeOption == "Generator") {
					upgradeFunction = UpgradeGeneratorEfficiency;
					currentUpgradeCount = currentUpgrade.generatorEfficiency;
					nextUpgradeCount = ++nextUpgrade.generatorEfficiency;
					currentImprovement = KRnD.calculateImprovementFactor(rndModule.generatorEfficiency_improvement, rndModule.generatorEfficiency_improvementScale, currentUpgrade.generatorEfficiency);
					nextImprovement = KRnD.calculateImprovementFactor(rndModule.generatorEfficiency_improvement, rndModule.generatorEfficiency_improvementScale, nextUpgrade.generatorEfficiency);
					scienceCost = KRnD.calculateScienceCost(rndModule.generatorEfficiency_scienceCost, rndModule.generatorEfficiency_costScale, nextUpgrade.generatorEfficiency);
				} else if (selectedUpgradeOption == "Converter") {
					upgradeFunction = UpgradeConverterEfficiency;
					currentUpgradeCount = currentUpgrade.converterEfficiency;
					nextUpgradeCount = ++nextUpgrade.converterEfficiency;
					currentImprovement = KRnD.calculateImprovementFactor(rndModule.converterEfficiency_improvement, rndModule.converterEfficiency_improvementScale, currentUpgrade.converterEfficiency);
					nextImprovement = KRnD.calculateImprovementFactor(rndModule.converterEfficiency_improvement, rndModule.converterEfficiency_improvementScale, nextUpgrade.converterEfficiency);
					scienceCost = KRnD.calculateScienceCost(rndModule.converterEfficiency_scienceCost, rndModule.converterEfficiency_costScale, nextUpgrade.converterEfficiency);
				} else if (selectedUpgradeOption == "Parachute") {
					upgradeFunction = UpgradeParachuteStrength;
					currentUpgradeCount = currentUpgrade.parachuteStrength;
					nextUpgradeCount = ++nextUpgrade.parachuteStrength;
					currentImprovement = KRnD.calculateImprovementFactor(rndModule.parachuteStrength_improvement, rndModule.parachuteStrength_improvementScale, currentUpgrade.parachuteStrength);
					nextImprovement = KRnD.calculateImprovementFactor(rndModule.parachuteStrength_improvement, rndModule.parachuteStrength_improvementScale, nextUpgrade.parachuteStrength);
					scienceCost = KRnD.calculateScienceCost(rndModule.parachuteStrength_scienceCost, rndModule.parachuteStrength_costScale, nextUpgrade.parachuteStrength);
				} else if (selectedUpgradeOption == "Max Temp") {
					upgradeFunction = UpgradeMaxTemperature;
					currentUpgradeCount = currentUpgrade.maxTemperature;
					nextUpgradeCount = ++nextUpgrade.maxTemperature;
					currentImprovement = KRnD.calculateImprovementFactor(rndModule.maxTemperature_improvement, rndModule.maxTemperature_improvementScale, currentUpgrade.maxTemperature);
					nextImprovement = KRnD.calculateImprovementFactor(rndModule.maxTemperature_improvement, rndModule.maxTemperature_improvementScale, nextUpgrade.maxTemperature);
					scienceCost = KRnD.calculateScienceCost(rndModule.maxTemperature_scienceCost, rndModule.maxTemperature_costScale, nextUpgrade.maxTemperature);
				} else {
					throw new Exception("unexpected option '" + selectedUpgradeOption + "'");
				}

				var newInfo = getPartInfo(part, nextUpgrade); // Calculate part-info if the selected stat was upgraded.
				newInfo = highlightChanges(currentInfo, newInfo);

				// Current stats:
				GUILayout.BeginArea(new Rect(10 + optionsWidth + 10, 30, windowStyle.fixedWidth, 20));
				GUILayout.Label("<color=#FFFFFF><b>Current:</b> " + currentUpgradeCount + " (" + currentImprovement.ToString("+0.##%;-0.##%;-") + ")</color>", labelStyle);
				GUILayout.EndArea();

				var areaWidth = (windowStyle.fixedWidth - 20 - optionsWidth) / 2;
				var areaHeight = optionsHeight;
				GUILayout.BeginArea(new Rect(10 + optionsWidth, 30 + 20, areaWidth, areaHeight));
				scrollPos = GUILayout.BeginScrollView(scrollPos, scrollStyle, GUILayout.Width(areaWidth), GUILayout.Height(areaHeight));

				GUILayout.Label(currentInfo, labelStyleSmall);
				GUILayout.EndScrollView();
				GUILayout.EndArea();

				// Next stats:
				GUILayout.BeginArea(new Rect(10 + optionsWidth + areaWidth + 10, 30, windowStyle.fixedWidth, 20));
				GUILayout.Label("<color=#FFFFFF><b>Next upgrade:</b> " + nextUpgradeCount + " (" + nextImprovement.ToString("+0.##%;-0.##%;-") + ")</color>", labelStyle);
				GUILayout.EndArea();

				GUILayout.BeginArea(new Rect(10 + optionsWidth + areaWidth, 30 + 20, areaWidth, areaHeight));
				scrollPos = GUILayout.BeginScrollView(scrollPos, scrollStyle, GUILayout.Width(areaWidth), GUILayout.Height(areaHeight));
				GUILayout.Label(newInfo, labelStyleSmall);
				GUILayout.EndScrollView();
				GUILayout.EndArea();

				// Bottom-line (display only if the upgrade would have an effect):
				if (currentImprovement != nextImprovement) {
					GUILayout.BeginArea(new Rect(10, windowStyle.fixedHeight - 25, windowStyle.fixedWidth, 30));
					float currentScience = 0;
					if (ResearchAndDevelopment.Instance != null) currentScience = ResearchAndDevelopment.Instance.Science;
					var color = "FF0000";
					if (currentScience >= scienceCost) color = "00FF00";
					GUILayout.Label("<b>Science: <color=#" + color + ">" + scienceCost + " / " + Math.Floor(currentScience) + "</color></b>", labelStyle);
					GUILayout.EndArea();
					if (currentScience >= scienceCost && ResearchAndDevelopment.Instance != null && upgradeFunction != null) {
						GUILayout.BeginArea(new Rect(windowStyle.fixedWidth - 110, windowStyle.fixedHeight - 30, 100, 30));
						if (GUILayout.Button("Research", buttonStyle)) {
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
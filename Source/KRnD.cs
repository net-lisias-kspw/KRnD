using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace KRnD.Source
{

	[KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
	public class KRnD : MonoBehaviour
	{
		private static bool initialized;
		public static Dictionary<string, PartStats> originalStats;
		public static Dictionary<string, KRnDUpgrade> upgrades = new Dictionary<string, KRnDUpgrade>();
		public static List<string> fuelResources;
		public static List<string> blacklistedParts;

		// Helper for accessing values in third party modules:
		public static double GetGenericModuleValue(PartModule module, string fieldName)
		{
			var type = module.GetType();
			foreach (var info in type.GetFields()) {
				if (info.Name == fieldName) {
					return Convert.ToDouble(info.GetValue(module));
				}
			}

			throw new Exception("field " + fieldName + " not found in module " + module.moduleName);
		}

		// Helper for setting values in third party modules:
		public static void SetGenericModuleValue(PartModule module, string fieldName, double value)
		{
			var type = module.GetType();
			foreach (var info in type.GetFields()) {
				if (info.Name == fieldName) {
					info.SetValue(module, Convert.ChangeType(value, info.FieldType));
					return;
				}
			}

			throw new Exception("field " + fieldName + " not found in module " + module.moduleName);
		}

		// Checks if the given, generic part-module has a field with the given name:
		public static bool HasGenericModuleField(PartModule module, string fieldName)
		{
			var type = module.GetType();
			foreach (var info in type.GetFields()) {
				if (info.Name == fieldName) {
					return true;
				}
			}

			return false;
		}

		public static KRnDModule GetKRnDModule(Part part)
		{
			// If this is a blacklisted part, don't touch it, even if it should have an RnD-Module. We do it like
			// this because using module-manager-magic to prevent RnD from getting installed with other, incompatible
			// modules from other mods depends on the order in which module-manager applies the patches; this way
			// we can avoid these problems. It means though that parts might have the RnD-Module, which isn't used though.
			if (blacklistedParts.Contains(sanatizePartName(part.name))) return null;

			// Check if this part has the RnD-Module and return it:
			foreach (var partModule in part.Modules) {
				if (partModule.moduleName == "KRnDModule") {
					return (KRnDModule) partModule;
				}
			}

			return null;
		}

		// Multi-Mode engines have multiple Engine-Modules which we return as a list.
		public static List<ModuleEngines> getEngineModules(Part part)
		{
			var engines = new List<ModuleEngines>();
			foreach (var partModule in part.Modules) {
				if (partModule.moduleName == "ModuleEngines" || partModule.moduleName == "ModuleEnginesFX") {
					engines.Add((ModuleEngines) partModule);
				}
			}

			if (engines.Count > 0) return engines;
			return null;
		}

		public static ModuleRCS getRcsModule(Part part)
		{
			foreach (var partModule in part.Modules) {
				if (partModule.moduleName == "ModuleRCS" || partModule.moduleName == "ModuleRCSFX") {
					return (ModuleRCS) partModule;
				}
			}

			return null;
		}

		public static ModuleReactionWheel getReactionWheelModule(Part part)
		{
			foreach (var partModule in part.Modules) {
				if (partModule.moduleName == "ModuleReactionWheel") {
					return (ModuleReactionWheel) partModule;
				}
			}

			return null;
		}

		public static ModuleDeployableSolarPanel getSolarPanelModule(Part part)
		{
			foreach (var partModule in part.Modules) {
				if (partModule.moduleName == "ModuleDeployableSolarPanel") {
					return (ModuleDeployableSolarPanel) partModule;
				}
			}

			return null;
		}

		public static ModuleWheelBase getLandingLegModule(Part part)
		{
			ModuleWheelBase wheelBase = null;
			foreach (var partModule in part.Modules) {
				if (partModule.moduleName == "ModuleWheelBase") {
					wheelBase = (ModuleWheelBase) partModule;
					if (wheelBase.wheelType == WheelType.LEG) return wheelBase;
				}
			}

			return null;
		}

		public static PartResource getChargeResource(Part part)
		{
			if (part.Resources == null) return null;
			foreach (var partResource in part.Resources)
				// Engines with an alternator might have a max-amount of 0, skip thoses:
			{
				if (partResource.resourceName == "ElectricCharge" && partResource.maxAmount > 0) {
					return partResource;
				}
			}

			return null;
		}

		public static List<PartResource> getFuelResources(Part part)
		{
			if (part.Resources == null) return null;
			var partFuels = new List<PartResource>();
			foreach (var partResource in part.Resources) {
				if (fuelResources != null && fuelResources.Contains(partResource.resourceName)) {
					partFuels.Add(partResource);
				}
			}

			if (partFuels.Count == 0) return null;
			return partFuels;
		}

		public static ModuleGenerator getGeneratorModule(Part part)
		{
			foreach (var partModule in part.Modules) {
				if (partModule.moduleName == "ModuleGenerator") {
					return (ModuleGenerator) partModule;
				}
			}

			return null;
		}

		public static PartModule getFissionGeneratorModule(Part part)
		{
			foreach (var partModule in part.Modules)
				// We are only interested in "FissionGenerator" with the tunable attribute "PowerGeneration":
			{
				if (partModule.moduleName == "FissionGenerator" && HasGenericModuleField(partModule, "PowerGeneration")) {
					return partModule;
				}
			}

			return null;
		}

		public static List<ModuleResourceConverter> getConverterModules(Part part)
		{
			var converters = new List<ModuleResourceConverter>();
			foreach (var partModule in part.Modules) {
				if (partModule.moduleName == "ModuleResourceConverter") {
					converters.Add((ModuleResourceConverter) partModule);
				}
			}

			if (converters.Count == 0) return null;
			return converters;
		}

		public static ModuleParachute getParachuteModule(Part part)
		{
			foreach (var partModule in part.Modules) {
				if (partModule.moduleName == "ModuleParachute") {
					return (ModuleParachute) partModule;
				}
			}

			return null;
		}

		public static ModuleProceduralFairing getFairingModule(Part part)
		{
			foreach (var partModule in part.Modules) {
				if (partModule.moduleName == "ModuleProceduralFairing") {
					return (ModuleProceduralFairing) partModule;
				}
			}

			return null;
		}

		public static float calculateImprovementFactor(float baseImprovement, float improvementScale, int upgrades)
		{
			float factor = 0;
			if (upgrades < 0) upgrades = 0;
			for (var i = 0; i < upgrades; i++) {
				if (i == 0) {
					factor += baseImprovement;
				} else {
					factor += baseImprovement * (float) Math.Pow(improvementScale, i - 1);
				}
			}

			if (baseImprovement < 0 && factor < -0.9) factor = -0.9f;
			return (float) Math.Round(factor, 4);
		}

		public static int calculateScienceCost(int baseCost, float costScale, int upgrades)
		{
			float cost = 0;
			if (upgrades < 0) upgrades = 0;
			for (var i = 0; i < upgrades; i++) {
				if (i == 0) {
					cost = baseCost;
				} else {
					cost += baseCost * (float) Math.Pow(costScale, i - 1);
				}
			}

			if (cost > 2147483647) return 2147483647; // Cap at signed 32 bit int
			return (int) Math.Round(cost);
		}

		// Since KSP 1.1 the info-text of solar panels is not updated correctly, so we have use this workaround-function
		// to create our own text.
		public static string getSolarPanelInfo(ModuleDeployableSolarPanel solarModule)
		{
			var info = solarModule.GetInfo();
			var chargeRate = solarModule.chargeRate * solarModule.efficiencyMult;
			var chargeString = chargeRate.ToString("0.####/s");
			var prefix = "<b>Electric Charge: </b>";
			return Regex.Replace(info, prefix + "[0-9.]+/[A-Za-z.]+", prefix + chargeString);
		}

		// Updates the global dictionary of available parts with the current set of upgrades (should be
		// executed for example when a new game starts or an existing game is loaded).
		public static int updateGlobalParts()
		{
			var upgradesApplied = 0;
			try {
				if (upgrades == null) throw new Exception("upgrades-dictionary missing");
				foreach (var part in PartLoader.LoadedPartsList) {
					try {
						KRnDUpgrade upgrade;
						if (!upgrades.TryGetValue(part.name, out upgrade)) upgrade = new KRnDUpgrade(); // If there are no upgrades, reset the part.

						// Udate the part to its latest model:
						updatePart(part.partPrefab, true);

						// Rebuild the info-screen:
						var converterModuleNumber = 0; // There might be multiple modules of this type
						var engineModuleNumber = 0; // There might be multiple modules of this type
						foreach (var info in part.moduleInfos) {
							if (info.moduleName.ToLower() == "engine") {
								var engines = getEngineModules(part.partPrefab);
								if (engines != null && engines.Count > 0) {
									var engine = engines[engineModuleNumber];
									info.info = engine.GetInfo();
									info.primaryInfo = engine.GetPrimaryField();
									engineModuleNumber++;
								}
							} else if (info.moduleName.ToLower() == "rcs") {
								var rcs = getRcsModule(part.partPrefab);
								if (rcs) info.info = rcs.GetInfo();
							} else if (info.moduleName.ToLower() == "reaction wheel") {
								var reactionWheel = getReactionWheelModule(part.partPrefab);
								if (reactionWheel) info.info = reactionWheel.GetInfo();
							} else if (info.moduleName.ToLower() == "deployable solar panel") {
								var solarPanel = getSolarPanelModule(part.partPrefab);
								if (solarPanel) info.info = getSolarPanelInfo(solarPanel);
							} else if (info.moduleName.ToLower() == "landing leg") {
								var landingLeg = getLandingLegModule(part.partPrefab);
								if (landingLeg) info.info = landingLeg.GetInfo();
							} else if (info.moduleName.ToLower() == "fission generator") {
								var fissionGenerator = getFissionGeneratorModule(part.partPrefab);
								if (fissionGenerator) info.info = fissionGenerator.GetInfo();
							} else if (info.moduleName.ToLower() == "generator") {
								var generator = getGeneratorModule(part.partPrefab);
								if (generator) info.info = generator.GetInfo();
							} else if (info.moduleName.ToLower() == "resource converter") {
								var converterList = getConverterModules(part.partPrefab);
								if (converterList != null && converterList.Count > 0) {
									var converter = converterList[converterModuleNumber];
									info.info = converter.GetInfo();
									converterModuleNumber++;
								}
							} else if (info.moduleName.ToLower() == "parachute") {
								var parachute = getParachuteModule(part.partPrefab);
								if (parachute) info.info = parachute.GetInfo();
							} else if (info.moduleName.ToLower() == "custom-built fairing") {
								var fairing = getFairingModule(part.partPrefab);
								if (fairing) info.info = fairing.GetInfo();
							}
						}

						var fuelResources = getFuelResources(part.partPrefab);
						var electricCharge = getChargeResource(part.partPrefab);
						// The Resource-Names are not always formated the same way, eg "Electric Charge" vs "ElectricCharge", so we do some reformating.
						foreach (var info in part.resourceInfos) {
							if (electricCharge != null && info.resourceName.Replace(" ", "").ToLower() == electricCharge.resourceName.Replace(" ", "").ToLower()) {
								info.info = electricCharge.GetInfo();
								info.primaryInfo = "<b>" + info.resourceName + ":</b> " + electricCharge.maxAmount;
							} else if (fuelResources != null) {
								foreach (var fuelResource in fuelResources) {
									if (info.resourceName.Replace(" ", "").ToLower() == fuelResource.resourceName.Replace(" ", "").ToLower()) {
										info.info = fuelResource.GetInfo();
										info.primaryInfo = "<b>" + info.resourceName + ":</b> " + fuelResource.maxAmount;
										break;
									}
								}
							}
						}

						upgradesApplied++;
					} catch (Exception e) {
						Debug.LogError("[KRnD] updateGlobalParts(" + part.title + "): " + e);
					}
				}
			} catch (Exception e) {
				Debug.LogError("[KRnD] updateGlobalParts(): " + e);
			}

			return upgradesApplied;
		}

		// Updates all parts in the vessel that is currently active in the editor.
		public static void updateEditorVessel(Part rootPart = null)
		{
			if (rootPart == null) rootPart = EditorLogic.RootPart;
			if (!rootPart) return;
			updatePart(rootPart, true); // Update to the latest model
			foreach (var childPart in rootPart.children) {
				updateEditorVessel(childPart);
			}
		}

		// Updates the given part either to the latest model (updateToLatestModel=TRUE) or to the model defined by its
		// KRnDModule.
		public static void updatePart(Part part, bool updateToLatestModel)
		{
			KRnDUpgrade upgradesToApply;
			if (updateToLatestModel) {
				if (upgrades.TryGetValue(sanatizePartName(part.name), out upgradesToApply)) {
					// Apply upgrades from global list:
					updatePart(part, upgradesToApply);
				} else {
					// No Upgrades found, applay base-stats:
					upgradesToApply = new KRnDUpgrade();
					updatePart(part, upgradesToApply);
				}
			} else {
				// Extract current upgrades of the part and set those stats:
				var rndModule = GetKRnDModule(part);
				if (rndModule != null && (upgradesToApply = rndModule.getCurrentUpgrades()) != null) {
					// Apply upgrades from the RnD-Module:
					updatePart(part, upgradesToApply);
				} else {
					// No Upgrades found, applay base-stats:
					upgradesToApply = new KRnDUpgrade();
					updatePart(part, upgradesToApply);
				}
			}
		}

		// Sometimes the name of the root-part of a vessel is extended by the vessel-name like "Mk1Pod (X-Bird)", this function can be used
		// as wrapper to always return the real name:
		public static string sanatizePartName(string partName)
		{
			return Regex.Replace(partName, @" \(.*\)$", "");
		}

		// Updates the given part with all upgrades provided in "upgradesToApply".
		public static void updatePart(Part part, KRnDUpgrade upgradesToApply)
		{
			try {
				// Find all relevant modules of this part:
				var rndModule = GetKRnDModule(part);
				if (rndModule == null) return;
				if (upgrades == null) throw new Exception("upgrades-dictionary missing");
				if (KRnD.originalStats == null) throw new Exception("original-stats-dictionary missing");

				// Get the part-name ("):
				var partName = sanatizePartName(part.name);

				// Get the original part-stats:
				PartStats originalStats;
				if (!KRnD.originalStats.TryGetValue(partName, out originalStats)) throw new Exception("no original-stats for part '" + partName + "'");

				KRnDUpgrade latestModel;
				if (!upgrades.TryGetValue(partName, out latestModel)) latestModel = null;


				// Dry Mass:
				rndModule.dryMass_upgrades = upgradesToApply.dryMass;
				var dryMassFactor = 1 + calculateImprovementFactor(rndModule.dryMass_improvement, rndModule.dryMass_improvementScale, upgradesToApply.dryMass);
				part.mass = originalStats.mass * dryMassFactor;
				part.prefabMass = part.mass; // New in ksp 1.1, if this is correct is just guesswork however...

				// Dry Mass also improves fairing mass:
				var fairngModule = getFairingModule(part);
				if (fairngModule) fairngModule.UnitAreaMass = originalStats.fairingAreaMass * dryMassFactor;

				// Max Int/Skin Temp:
				rndModule.maxTemperature_upgrades = upgradesToApply.maxTemperature;
				double tempFactor = 1 + calculateImprovementFactor(rndModule.maxTemperature_improvement, rndModule.maxTemperature_improvementScale, upgradesToApply.maxTemperature);
				part.skinMaxTemp = originalStats.skinMaxTemp * tempFactor;
				part.maxTemp = originalStats.intMaxTemp * tempFactor;

				// Fuel Flow:
				var engineModules = getEngineModules(part);
				var rcsModule = getRcsModule(part);
				if (engineModules != null || rcsModule) {
					rndModule.fuelFlow_upgrades = upgradesToApply.fuelFlow;
					for (var i = 0; i < originalStats.maxFuelFlows.Count; i++) {
						var maxFuelFlow = originalStats.maxFuelFlows[i] * (1 + calculateImprovementFactor(rndModule.fuelFlow_improvement, rndModule.fuelFlow_improvementScale, upgradesToApply.fuelFlow));
						if (engineModules != null) {
							engineModules[i].maxFuelFlow = maxFuelFlow;
						} else if (rcsModule) rcsModule.thrusterPower = maxFuelFlow; // There is only one rcs-module
					}
				} else {
					rndModule.fuelFlow_upgrades = 0;
				}

				// ISP Vac & Atm:
				if (engineModules != null || rcsModule) {
					rndModule.ispVac_upgrades = upgradesToApply.ispVac;
					rndModule.ispAtm_upgrades = upgradesToApply.ispAtm;
					var improvementFactorVac = 1 + calculateImprovementFactor(rndModule.ispVac_improvement, rndModule.ispVac_improvementScale, upgradesToApply.ispVac);
					var improvementFactorAtm = 1 + calculateImprovementFactor(rndModule.ispAtm_improvement, rndModule.ispAtm_improvementScale, upgradesToApply.ispAtm);

					for (var i = 0; i < originalStats.atmosphereCurves.Count; i++) {
						var isAirbreather = false;
						if (engineModules != null) isAirbreather = engineModules[i].engineType == EngineType.Turbine || engineModules[i].engineType == EngineType.Piston || engineModules[i].engineType == EngineType.ScramJet;
						var fc = new FloatCurve();
						for (var v = 0; v < originalStats.atmosphereCurves[i].Curve.length; v++) {
							var frame = originalStats.atmosphereCurves[i].Curve[v];

							var pressure = frame.time;
							float factorAtThisPressure = 1;
							if (isAirbreather && originalStats.atmosphereCurves[i].Curve.length == 1) {
								factorAtThisPressure = improvementFactorAtm; // Airbreathing engines have a preassure curve starting at 0, but they should use Atm. as improvement factor.
							} else if (pressure == 0) {
								factorAtThisPressure = improvementFactorVac; // In complete vacuum
							} else if (pressure >= 1) {
								factorAtThisPressure = improvementFactorAtm; // At lowest kerbal atmosphere
							} else {
								factorAtThisPressure = (1 - pressure) * improvementFactorVac + pressure * improvementFactorAtm; // Mix both
							}

							var newValue = frame.value * factorAtThisPressure;
							fc.Add(pressure, newValue);
						}

						if (engineModules != null) {
							engineModules[i].atmosphereCurve = fc;
						} else if (rcsModule) rcsModule.atmosphereCurve = fc; // There is only one rcs-module
					}
				} else {
					rndModule.ispVac_upgrades = 0;
					rndModule.ispAtm_upgrades = 0;
				}

				// Torque:
				var reactionWheel = getReactionWheelModule(part);
				if (reactionWheel) {
					rndModule.torque_upgrades = upgradesToApply.torque;
					var torque = originalStats.torque * (1 + calculateImprovementFactor(rndModule.torque_improvement, rndModule.torque_improvementScale, upgradesToApply.torque));
					reactionWheel.PitchTorque = torque;
					reactionWheel.YawTorque = torque;
					reactionWheel.RollTorque = torque;
				} else {
					rndModule.torque_upgrades = 0;
				}

				// Charge Rate:
				var solarPanel = getSolarPanelModule(part);
				if (solarPanel) {
					rndModule.chargeRate_upgrades = upgradesToApply.chargeRate;
					var chargeEfficiency = 1 + calculateImprovementFactor(rndModule.chargeRate_improvement, rndModule.chargeRate_improvementScale, upgradesToApply.chargeRate);
					// Somehow changing the charge-rate stopped working in KSP 1.1, so we use the efficiency instead. This however does not
					// show up in the module-info (probably a bug in KSP), which is why we have another workaround to update the info-texts.
					// float chargeRate = originalStats.chargeRate * chargeEfficiency;
					// solarPanel.chargeRate = chargeRate;
					solarPanel.efficiencyMult = chargeEfficiency;
				} else {
					rndModule.chargeRate_upgrades = 0;
				}

				// Crash Tolerance (only for landing legs):
				var landingLeg = getLandingLegModule(part);
				if (landingLeg) {
					rndModule.crashTolerance_upgrades = upgradesToApply.crashTolerance;
					var crashTolerance = originalStats.crashTolerance * (1 + calculateImprovementFactor(rndModule.crashTolerance_improvement, rndModule.crashTolerance_improvementScale, upgradesToApply.crashTolerance));
					part.crashTolerance = crashTolerance;
				} else {
					rndModule.crashTolerance_upgrades = 0;
				}

				// Battery Charge:
				var electricCharge = getChargeResource(part);
				if (electricCharge != null) {
					rndModule.batteryCharge_upgrades = upgradesToApply.batteryCharge;
					var batteryCharge = originalStats.batteryCharge * (1 + calculateImprovementFactor(rndModule.batteryCharge_improvement, rndModule.batteryCharge_improvementScale, upgradesToApply.batteryCharge));
					batteryCharge = Math.Round(batteryCharge); // We don't want half units of electric charge

					var batteryIsFull = false;
					if (electricCharge.amount == electricCharge.maxAmount) batteryIsFull = true;

					electricCharge.maxAmount = batteryCharge;
					if (batteryIsFull) electricCharge.amount = electricCharge.maxAmount;
				} else {
					rndModule.batteryCharge_upgrades = 0;
				}

				// Generator & Fission-Generator Efficiency:
				var generator = getGeneratorModule(part);
				var fissionGenerator = getFissionGeneratorModule(part);
				if (generator || fissionGenerator) {
					rndModule.generatorEfficiency_upgrades = upgradesToApply.generatorEfficiency;

					if (generator) {
						foreach (var outputResource in generator.resHandler.outputResources) {
							double originalRate;
							if (!originalStats.generatorEfficiency.TryGetValue(outputResource.name, out originalRate)) continue;
							outputResource.rate = (float) (originalRate * (1 + calculateImprovementFactor(rndModule.generatorEfficiency_improvement, rndModule.generatorEfficiency_improvementScale, upgradesToApply.generatorEfficiency)));
						}
					}

					if (fissionGenerator) {
						var powerGeneration = originalStats.fissionPowerGeneration * (1 + calculateImprovementFactor(rndModule.generatorEfficiency_improvement, rndModule.generatorEfficiency_improvementScale, upgradesToApply.generatorEfficiency));
						SetGenericModuleValue(fissionGenerator, "PowerGeneration", powerGeneration);
					}
				} else {
					rndModule.generatorEfficiency_upgrades = 0;
				}

				// Converter Efficiency:
				var converterList = getConverterModules(part);
				if (converterList != null) {
					foreach (var converter in converterList) {
						Dictionary<string, double> origiginalOutputResources;
						if (!originalStats.converterEfficiency.TryGetValue(converter.ConverterName, out origiginalOutputResources)) continue;

						rndModule.converterEfficiency_upgrades = upgradesToApply.converterEfficiency;
						// Since KSP 1.2 this can't be done in a foreach anymore, we have to read and write back the entire ResourceRatio-Object:
						for (var i = 0; i < converter.outputList.Count; i++) {
							var resourceRatio = converter.outputList[i];
							double originalRatio;
							if (!origiginalOutputResources.TryGetValue(resourceRatio.ResourceName, out originalRatio)) continue;
							resourceRatio.Ratio = (float) (originalRatio * (1 + calculateImprovementFactor(rndModule.converterEfficiency_improvement, rndModule.converterEfficiency_improvementScale, upgradesToApply.converterEfficiency)));
							converter.outputList[i] = resourceRatio;
						}
					}
				} else {
					rndModule.converterEfficiency_upgrades = 0;
				}

				// Parachute Strength:
				var parachute = getParachuteModule(part);
				if (parachute) {
					rndModule.parachuteStrength_upgrades = upgradesToApply.parachuteStrength;
					var chuteMaxTemp = originalStats.chuteMaxTemp * (1 + calculateImprovementFactor(rndModule.parachuteStrength_improvement, rndModule.parachuteStrength_improvementScale, upgradesToApply.parachuteStrength));
					parachute.chuteMaxTemp = chuteMaxTemp; // The safe deployment-speed is derived from the temperature
				} else {
					rndModule.parachuteStrength_upgrades = 0;
				}

				// Fuel Capacity:
				var fuelResources = getFuelResources(part);
				if (fuelResources != null && originalStats.fuelCapacities != null) {
					rndModule.fuelCapacity_upgrades = upgradesToApply.fuelCapacity;
					double improvementFactor = 1 + calculateImprovementFactor(rndModule.fuelCapacity_improvement, rndModule.fuelCapacity_improvementScale, upgradesToApply.fuelCapacity);

					foreach (var fuelResource in fuelResources) {
						if (!originalStats.fuelCapacities.ContainsKey(fuelResource.resourceName)) continue;
						var originalCapacity = originalStats.fuelCapacities[fuelResource.resourceName];
						var newCapacity = originalCapacity * improvementFactor;
						newCapacity = Math.Round(newCapacity); // We don't want half units of fuel

						var tankIsFull = false;
						if (fuelResource.amount == fuelResource.maxAmount) tankIsFull = true;

						fuelResource.maxAmount = newCapacity;
						if (tankIsFull) fuelResource.amount = fuelResource.maxAmount;
					}
				} else {
					rndModule.fuelCapacity_upgrades = 0;
				}
			} catch (Exception e) {
				Debug.LogError("[KRnD] updatePart(" + part.name + "): " + e);
			}
		}

		// Updates all parts of the given vessel according to their RnD-Module settings (should be executed
		// when the vessel is loaded to make sure, that the vessel uses its own, historic upgrades and not
		// the global part-upgrades).
		public static void updateVessel(Vessel vessel)
		{
			try {
				if (!vessel.isActiveVessel) return; // Only the currently active vessel matters, the others are not simulated anyway.
				if (upgrades == null) throw new Exception("upgrades-dictionary missing");
				//Debug.Log("[KRnD] updating vessel '" + vessel.vesselName.ToString() + "'");

				// Iterate through all parts:
				foreach (var part in vessel.parts) {
					// We only have to update parts which have the RnD-Module:
					var rndModule = GetKRnDModule(part);
					if (rndModule == null) continue;

					if (vessel.situation == Vessel.Situations.PRELAUNCH) {
						// Update the part with the latest model while on the launchpad:
						updatePart(part, true);
					} else if (rndModule.upgradeToLatest > 0) {
						// Flagged by another mod (eg KSTS) to get updated to the latest model (once):
						//Debug.Log("[KRnD] part '"+ KRnD.sanatizePartName(part.name) + "' of '"+ vessel.vesselName + "' was flagged to be updated to the latest model");
						rndModule.upgradeToLatest = 0;
						updatePart(part, true);
					} else {
						// Update this part with its own stats:
						updatePart(part, false);
					}
				}
			} catch (Exception e) {
				Debug.LogError("[KRnD] updateVesselActive(): " + e);
			}
		}

		// Is called every time the active vessel changes (on entering a scene, switching the vessel or on docking).
		private void OnVesselChange(Vessel vessel)
		{
			try {
				updateVessel(vessel);
			} catch (Exception e) {
				Debug.LogError("[KRnD] OnVesselChange(): " + e);
			}
		}

		// Is called when we interact with a part in the editor.
		private void EditorPartEvent(ConstructionEventType ev, Part part)
		{
			try {
				if (ev != ConstructionEventType.PartCreated && ev != ConstructionEventType.PartDetached && ev != ConstructionEventType.PartAttached && ev != ConstructionEventType.PartDragging) return;
				KRnDGUI.selectedPart = part;
			} catch (Exception e) {
				Debug.LogError("[KRnD] EditorPartEvent(): " + e);
			}
		}

		public List<string> getBlacklistedModules()
		{
			var blacklistedModules = new List<string>();
			try {
				var node = ConfigNode.Load(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/blacklist.cfg");

				foreach (var blacklistedModule in node.GetValues("BLACKLISTED_MODULE")) {
					if (!blacklistedModules.Contains(blacklistedModule)) {
						blacklistedModules.Add(blacklistedModule);
					}
				}
			} catch (Exception e) {
				Debug.LogError("[KRnD] getBlacklistedModules(): " + e);
			}

			return blacklistedModules;
		}

		public List<string> getBlacklistedParts()
		{
			var blacklistedParts = new List<string>();
			try {
				var node = ConfigNode.Load(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/blacklist.cfg");

				foreach (var blacklistedPart in node.GetValues("BLACKLISTED_PART")) {
					if (!blacklistedParts.Contains(blacklistedPart)) {
						blacklistedParts.Add(blacklistedPart);
					}
				}
			} catch (Exception e) {
				Debug.LogError("[KRnD] getBlacklistedParts(): " + e);
			}

			return blacklistedParts;
		}

		// Is called when this Addon is first loaded to initializes all values (eg registration of event-handlers and creation
		// of original-stats library).
		public void Awake()
		{
			try {
				// Create a list of all valid fuel resources:
				if (fuelResources == null) {
					fuelResources = new List<string>();
					fuelResources.Add("MonoPropellant"); // Always use MonoPropellant as fuel (RCS-Thrusters don't have engine modules and are not found with the code below)

					foreach (var aPart in PartLoader.LoadedPartsList) {
						var part = aPart.partPrefab;
						var engineModules = getEngineModules(part);
						if (engineModules == null) continue;
						foreach (var engineModule in engineModules) {
							if (engineModule.propellants == null) continue;
							foreach (var propellant in engineModule.propellants) {
								if (propellant.name == "ElectricCharge") continue; // Electric Charge is improved by batteries.
								if (propellant.name == "IntakeAir") continue; // This is no real fuel-type.
								if (propellant.name == "IntakeAtm") continue; // This is no real fuel-type.
								if (!fuelResources.Contains(propellant.name)) fuelResources.Add(propellant.name);
							}
						}
					}

					var listString = "";
					foreach (var fuelName in fuelResources) {
						if (listString != "") listString += ", ";
						listString += fuelName;
					}

					//Debug.Log("[KRnD] found " + KRnD.fuelResources.Count.ToString() + " propellants: " + listString);
				}

				// Create a list of blacklisted parts (parts with known incompatible modules of other mods):
				if (blacklistedParts == null) {
					blacklistedParts = getBlacklistedParts();
					var blacklistedModules = getBlacklistedModules();

					foreach (var aPart in PartLoader.LoadedPartsList) {
						var part = aPart.partPrefab;
						var skip = false;
						var blacklistedModule = "N/A";

						foreach (var partModule in part.Modules) {
							if (blacklistedModules.Contains(partModule.moduleName)) {
								blacklistedModule = partModule.moduleName;
								skip = true;
								break;
							}
						}

						if (skip) {
							if (!blacklistedParts.Contains(part.name)) {
								blacklistedParts.Add(part.name);
							}
						}
					}

					//Debug.Log("[KRnD] blacklisted " + KRnD.blacklistedParts.Count.ToString() + " parts, which contained one of " + blacklistedModules.Count.ToString() + " blacklisted modules");
				}

				// Create a backup of all unmodified parts before we update them. We will later use these backup-parts
				// for all calculations of upgraded stats.
				if (originalStats == null) {
					originalStats = new Dictionary<string, PartStats>();
					foreach (var aPart in PartLoader.LoadedPartsList) {
						var part = aPart.partPrefab;

						// Backup this part, if it has the RnD-Module:
						if (GetKRnDModule(part) != null) {
							PartStats duplicate;
							if (originalStats.TryGetValue(part.name, out duplicate)) {
								//Debug.LogError("[KRnD] Awake(): duplicate part-name: " + part.name.ToString());
							} else {
								originalStats.Add(part.name, new PartStats(part));
							}
						}
					}
				}

				// Execute the following code only once:
				if (initialized) return;
				DontDestroyOnLoad(this);

				// Register event-handlers:
				GameEvents.onVesselChange.Add(OnVesselChange);
				GameEvents.onEditorPartEvent.Add(EditorPartEvent);

				initialized = true;
			} catch (Exception e) {
				Debug.LogError("[KRnD] Awake(): " + e);
			}
		}
	}

	// This class handles load- and save-operations.
	[KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.EDITOR, GameScenes.FLIGHT, GameScenes.TRACKSTATION, GameScenes.SPACECENTER)]
	internal class KRnDScenarioModule : ScenarioModule
	{
		public override void OnSave(ConfigNode node)
		{
			try {
				if (KRnDSettings.Instance != null) KRnDSettings.Instance.OnSave(node);

				double time = DateTime.Now.Ticks;
				var upgradeNodes = new ConfigNode("upgrades");
				foreach (var upgradeName in KRnD.upgrades.Keys) {
					KRnDUpgrade upgrade;
					if (!KRnD.upgrades.TryGetValue(upgradeName, out upgrade)) continue;
					upgradeNodes.AddNode(upgrade.CreateConfigNode(upgradeName));
					//Debug.Log("[KRnD] saved: " + upgradeName + " " + upgrade.ToString());
				}

				node.AddNode(upgradeNodes);

				time = (DateTime.Now.Ticks - time) / TimeSpan.TicksPerSecond;
				//Debug.Log("[KRnD] saved " + upgradeNodes.CountNodes.ToString() + " upgrades in " + time.ToString("0.000s"));

				var guiSettings = new ConfigNode("gui");
				guiSettings.AddValue("left", KRnDGUI.windowPosition.xMin);
				guiSettings.AddValue("top", KRnDGUI.windowPosition.yMin);
				node.AddNode(guiSettings);
			} catch (Exception e) {
				Debug.LogError("[KRnD] OnSave(): " + e);
			}
		}

		public override void OnLoad(ConfigNode node)
		{
			try {
				if (KRnDSettings.Instance != null) KRnDSettings.Instance.OnLoad(node);

				double time = DateTime.Now.Ticks;
				var upgradesApplied = 0;

				KRnD.upgrades.Clear();

				var upgradeNodes = node.GetNode("upgrades");
				if (upgradeNodes != null) {
					foreach (var upgradeNode in upgradeNodes.GetNodes()) {
						var upgrade = KRnDUpgrade.CreateFromConfigNode(upgradeNode);
						KRnD.upgrades.Add(upgradeNode.name, upgrade);
					}

					// Update global part-list with new upgrades from the savegame:
					upgradesApplied = KRnD.updateGlobalParts();

					// If we started with an active vessel, update that vessel:
					var vessel = FlightGlobals.ActiveVessel;
					if (vessel) KRnD.updateVessel(vessel);

					time = (DateTime.Now.Ticks - time) / TimeSpan.TicksPerSecond;
					//Debug.Log("[KRnD] retrieved and applied " + upgradesApplied.ToString() + " upgrades in " + time.ToString("0.000s"));
				}

				var guiSettings = node.GetNode("gui");
				if (guiSettings != null) {
					if (guiSettings.HasValue("left")) KRnDGUI.windowPosition.xMin = (float) double.Parse(guiSettings.GetValue("left"));
					if (guiSettings.HasValue("top")) KRnDGUI.windowPosition.yMin = (float) double.Parse(guiSettings.GetValue("top"));
				}
			} catch (Exception e) {
				Debug.LogError("[KRnD] OnLoad(): " + e);
			}
		}
	}
}
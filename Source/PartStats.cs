using System;
using System.Collections.Generic;
using System.Linq;

namespace KRnD.Source
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary> This class is used to store all relevant base-stats of a part used to calculate all other stats with
	/// 		  incremental upgrades as well as a backup for restoring the original stats (eg after loading a
	/// 		  saved-game).</summary>
	public class PartStats
	{
		public List<FloatCurve> atmosphereCurves;
		public double batteryCharge;
		public float chargeRate;
		public double chuteMaxTemp;
		public Dictionary<string, Dictionary<string, double>> converterEfficiency; // Converter Name, (Resource-Name, Ratio)
		public float crashTolerance;
		public float fairingAreaMass;
		public double fissionPowerGeneration; // From FissionGenerator
		public Dictionary<string, double> fuelCapacities; // Resource-Name, capacity
		public double fuelCapacitiesSum; // Sum of all fuel capacities
		public Dictionary<string, double> generatorEfficiency; // Resource-Name, Rate
		public double intMaxTemp;
		public float dryMass;
		public List<float> maxFuelFlows;
		public double skinMaxTemp;
		public float torqueStrength;
		public float packetSize;
		public double antennaPower;
		public float dataStorage;
		public float resourceHarvester;


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Constructor that makes a copy of all relevant data from the part.</summary>
		///
		/// <param name="part"> The part.</param>
		public PartStats(Part part)
		{
			dryMass = part.mass;
			skinMaxTemp = part.skinMaxTemp;
			intMaxTemp = part.maxTemp;

			// There should only be one or the other, engines or RCS:
			var engine_modules = GetModuleEnginesList(part);
			var rcs_module = GetModuleRCS(part);
			if (engine_modules != null) {
				maxFuelFlows = new List<float>();
				atmosphereCurves = new List<FloatCurve>();

				foreach (var engine_module in engine_modules) {
					maxFuelFlows.Add(engine_module.maxFuelFlow);

					var atmosphere_curve = new FloatCurve();
					for (var i = 0; i < engine_module.atmosphereCurve.Curve.length; i++) {
						var frame = engine_module.atmosphereCurve.Curve[i];
						atmosphere_curve.Add(frame.time, frame.value);
					}

					atmosphereCurves.Add(atmosphere_curve);
				}
			} else if (rcs_module) {
				maxFuelFlows = new List<float>();
				atmosphereCurves = new List<FloatCurve>();

				maxFuelFlows.Add(rcs_module.thrusterPower);
				var atmosphere_curve = new FloatCurve();
				for (var i = 0; i < rcs_module.atmosphereCurve.Curve.length; i++) {
					var frame = rcs_module.atmosphereCurve.Curve[i];
					atmosphere_curve.Add(frame.time, frame.value);
				}

				atmosphereCurves.Add(atmosphere_curve);
			}

			var reaction_wheel = GetModuleReactionWheel(part);
			if (reaction_wheel) torqueStrength = reaction_wheel.RollTorque; // There is also pitch- and yaw-torque, but they should all be the same

			var solar_panel = GetModuleDeployableSolarPanel(part);
			if (solar_panel) chargeRate = solar_panel.chargeRate;

			var landing_leg = GetModuleWheelBase(part);
			if (landing_leg) crashTolerance = part.crashTolerance; // Every part has a crash tolerance, but we only want to improve landing legs.

			var electric_charge = GetElectricCharge(part);
			if (electric_charge != null) batteryCharge = electric_charge.maxAmount;

			var generator = GetModuleGenerator(part);
			if (generator != null) {
				generatorEfficiency = new Dictionary<string, double>();
				foreach (var output_resource in generator.resHandler.outputResources) generatorEfficiency.Add(output_resource.name, output_resource.rate);
			}

			var fission_generator = GetFissionGenerator(part);
			if (fission_generator != null) fissionPowerGeneration = GetGenericModuleValue(fission_generator, "PowerGeneration");

			// There might be different converter-modules in the same part with different names (eg for Fuel, Mono-propellant, etc):
			var converter_list = GetModuleResourceConverterList(part);
			if (converter_list != null) {
				converterEfficiency = new Dictionary<string, Dictionary<string, double>>();
				foreach (var converter in converter_list) {
					var this_converter_efficiency = new Dictionary<string, double>();
					foreach (var resource_ratio in converter.outputList) this_converter_efficiency.Add(resource_ratio.ResourceName, resource_ratio.Ratio);
					converterEfficiency.Add(converter.ConverterName, this_converter_efficiency);
				}
			}

			var harvester = GetModuleResourceHarvester(part);
			if (harvester) resourceHarvester = harvester.Efficiency;

			var parachute = GetModluleParachute(part);
			if (parachute) chuteMaxTemp = parachute.chuteMaxTemp;

			var fairing = GetModuleProceduralFairing(part);
			if (fairing) fairingAreaMass = fairing.UnitAreaMass;

			var fuel_resources = GetFuelResources(part);
			if (fuel_resources != null) {
				fuelCapacities = new Dictionary<string, double>();
				fuelCapacitiesSum = 0;
				foreach (var fuel_resource in fuel_resources) {
					fuelCapacities.Add(fuel_resource.resourceName, fuel_resource.maxAmount);
					fuelCapacitiesSum += fuel_resource.maxAmount;
				}
			}

			// Fetch antenna stats
			var transmitter = GetModuleDataTransmitter(part);
			if (transmitter != null) {
				packetSize = transmitter.packetSize;
				antennaPower = transmitter.antennaPower;
			}

			// Fetch science lab stats
			var lab = GetModluleScienceLab(part);
			if (lab != null) {
				dataStorage = lab.dataStorage;
			}
			
		}

		public static KRnDModule GetKRnDModule(Part part)
		{
			// If this is a blacklisted part, don't touch it, even if it should have an RnD-Module. We do it like
			// this because using module-manager-magic to prevent RnD from getting installed with other, incompatible
			// modules from other mods depends on the order in which module-manager applies the patches; this way
			// we can avoid these problems. It means though that parts might have the RnD-Module, which isn't used though.
			if (KRnD.blacklistedParts.Contains(KRnD.SanatizePartName(part.name))) return null;

			// Check if this part has the RnD-Module and return it:
			foreach (var part_module in part.Modules) {
				if (part_module.moduleName == "KRnDModule") {
					return (KRnDModule)part_module;
				}
			}

			return null;
		}



		// Multi-Mode engines have multiple Engine-Modules which we return as a list.
		public static List<ModuleEngines> GetModuleEnginesList(Part part)
		{
			var engines = new List<ModuleEngines>();
			foreach (var part_module in part.Modules) {
				if (part_module.moduleName == "ModuleEngines" || part_module.moduleName == "ModuleEnginesFX") {
					engines.Add((ModuleEngines)part_module);
				}
			}

			if (engines.Count > 0) return engines;
			return null;
		}

		public static ModuleWheelBase GetModuleWheelBase(Part part)
		{
			foreach (var part_module in part.Modules) {
				if (part_module.moduleName != "ModuleWheelBase") continue;
				var wheel_base = (ModuleWheelBase)part_module;
				if (wheel_base.wheelType == WheelType.LEG) return wheel_base;
			}

			return null;
		}


		public static ModuleReactionWheel GetModuleReactionWheel(Part part)
		{
			foreach (var part_module in part.Modules) {
				if (part_module.moduleName == "ModuleReactionWheel") {
					return (ModuleReactionWheel)part_module;
				}
			}

			return null;
		}

		public static ModuleDeployableSolarPanel GetModuleDeployableSolarPanel(Part part)
		{
			foreach (var part_module in part.Modules) {
				if (part_module.moduleName == "ModuleDeployableSolarPanel") {
					return (ModuleDeployableSolarPanel)part_module;
				}
			}

			return null;
		}


		public static PartResource GetElectricCharge(Part part)
		{
			if (part.Resources == null) return null;
			foreach (var part_resource in part.Resources)
				// Engines with an alternator might have a max-amount of 0, skip those:
			{
				if (part_resource.resourceName == "ElectricCharge" && part_resource.maxAmount > 0) {
					return part_resource;
				}
			}

			return null;
		}



		public static ModuleRCS GetModuleRCS(Part part)
		{
			foreach (var part_module in part.Modules) {
				if (part_module.moduleName == "ModuleRCS" || part_module.moduleName == "ModuleRCSFX") {
					return (ModuleRCS)part_module;
				}
			}

			return null;
		}


		public static List<PartResource> GetFuelResources(Part part)
		{
			if (part.Resources == null) return null;
			var part_fuels = new List<PartResource>();
			foreach (var part_resource in part.Resources) {
				if (KRnD.fuelResources != null && KRnD.fuelResources.Contains(part_resource.resourceName)) {
					part_fuels.Add(part_resource);
				}
			}

			if (part_fuels.Count == 0) return null;
			return part_fuels;
		}

		public static ModuleDataTransmitter GetModuleDataTransmitter(Part part)
		{
			foreach (var part_module in part.Modules) {
				if (part_module.moduleName == "ModuleDataTransmitter") {
					return (ModuleDataTransmitter)part_module;
				}
			}

			return null;
		}


		public static ModuleScienceLab GetModluleScienceLab(Part part)
		{
			foreach (var part_module in part.Modules) {
				if (part_module.moduleName == "ModuleScienceLab") {
					return (ModuleScienceLab)part_module;
				}
			}

			return null;
		}


		public static ModuleGenerator GetModuleGenerator(Part part)
		{
			foreach (var part_module in part.Modules) {
				if (part_module.moduleName == "ModuleGenerator") {
					return (ModuleGenerator)part_module;
				}
			}

			return null;
		}

		public static PartModule GetFissionGenerator(Part part)
		{
			foreach (var part_module in part.Modules)
			// We are only interested in "FissionGenerator" with the tunable attribute "PowerGeneration":
			{
				if (part_module.moduleName == "FissionGenerator" && HasGenericModuleField(part_module, "PowerGeneration")) {
					return part_module;
				}
			}

			return null;
		}

		public static List<ModuleResourceConverter> GetModuleResourceConverterList(Part part)
		{
			var converters = new List<ModuleResourceConverter>();
			foreach (var part_module in part.Modules) {
				if (part_module.moduleName == "ModuleResourceConverter") {
					converters.Add((ModuleResourceConverter)part_module);
				}
			}

			if (converters.Count == 0) return null;
			return converters;
		}


		public static ModuleResourceHarvester GetModuleResourceHarvester(Part part)
		{
			foreach (var part_module in part.Modules) {
				if (part_module.moduleName == "ModuleResourceHarvester") {
					return (ModuleResourceHarvester)part_module;
				}
			}

			return null;
		}

		public static ModuleParachute GetModluleParachute(Part part)
		{
			foreach (var part_module in part.Modules) {
				if (part_module.moduleName == "ModuleParachute") {
					return (ModuleParachute)part_module;
				}
			}

			return null;
		}

		public static ModuleProceduralFairing GetModuleProceduralFairing(Part part)
		{
			foreach (var part_module in part.Modules) {
				if (part_module.moduleName == "ModuleProceduralFairing") {
					return (ModuleProceduralFairing)part_module;
				}
			}

			return null;
		}


		public static double GetGenericPartValue(Part module, string field_name)
		{
			var type = module.GetType();
			foreach (var info in type.GetFields()) {
				if (info.Name == field_name) {
					return Convert.ToDouble(info.GetValue(module));
				}
			}

			throw new Exception("field " + field_name + " not found in part " + module.name);
		}



		public static void SetGenericPartValue(Part module, string field_name, double value)
		{
			var type = module.GetType();
			foreach (var info in type.GetFields()) {
				if (info.Name != field_name) continue;
				info.SetValue(module, Convert.ChangeType(value, info.FieldType));
				return;
			}

			throw new Exception("field " + field_name + " not found in part " + module.name);
		}


		public static bool HasGenericPartField(Part module, string field_name)
		{
			var type = module.GetType();
			return type.GetFields().Any(info => info.Name == field_name);
		}


		// Helper for accessing values in third party modules:
		public static double GetGenericModuleValue(PartModule module, string field_name)
		{
			var type = module.GetType();
			foreach (var info in type.GetFields()) {
				if (info.Name == field_name) {
					return Convert.ToDouble(info.GetValue(module));
				}
			}

			throw new Exception("field " + field_name + " not found in module " + module.moduleName);
		}

		// Helper for setting values in third party modules:
		public static void SetGenericModuleValue(PartModule module, string field_name, double value)
		{
			var type = module.GetType();
			foreach (var info in type.GetFields()) {
				if (info.Name != field_name) continue;
				info.SetValue(module, Convert.ChangeType(value, info.FieldType));
				return;
			}

			throw new Exception("field " + field_name + " not found in module " + module.moduleName);
		}

		// Checks if the given, generic part-module has a field with the given name:
		public static bool HasGenericModuleField(PartModule module, string field_name)
		{
			var type = module.GetType();
			foreach (var info in type.GetFields()) {
				if (info.Name == field_name) {
					return true;
				}
			}

			return false;
		}



	}
}

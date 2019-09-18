using System.Collections.Generic;

namespace KRnD.Source
{
	// This class is used to store all relevant base-stats of a part used to calculate all other stats with
	// incremental upgrades as well as a backup for restoring the original stats (eg after loading a saved-game).
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

		public PartStats(Part part)
		{
			dryMass = part.mass;
			skinMaxTemp = part.skinMaxTemp;
			intMaxTemp = part.maxTemp;

			// There should only be one or the other, engines or RCS:
			var engine_modules = KRnD.GetEngineModules(part);
			var rcs_module = KRnD.GetRcsModule(part);
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

			var reaction_wheel = KRnD.GetReactionWheelModule(part);
			if (reaction_wheel) torqueStrength = reaction_wheel.RollTorque; // There is also pitch- and yaw-torque, but they should all be the same

			var solar_panel = KRnD.GetSolarPanelModule(part);
			if (solar_panel) chargeRate = solar_panel.chargeRate;

			var landing_leg = KRnD.GetLandingLegModule(part);
			if (landing_leg) crashTolerance = part.crashTolerance; // Every part has a crash tolerance, but we only want to improve landing legs.

			var electric_charge = KRnD.GetChargeResource(part);
			if (electric_charge != null) batteryCharge = electric_charge.maxAmount;

			var generator = KRnD.GetGeneratorModule(part);
			if (generator != null) {
				generatorEfficiency = new Dictionary<string, double>();
				foreach (var output_resource in generator.resHandler.outputResources) generatorEfficiency.Add(output_resource.name, output_resource.rate);
			}

			var fission_generator = KRnD.GetFissionGeneratorModule(part);
			if (fission_generator != null) fissionPowerGeneration = KRnD.GetGenericModuleValue(fission_generator, "PowerGeneration");

			// There might be different converter-modules in the same part with different names (eg for Fuel, Mono-propellant, etc):
			var converter_list = KRnD.GetConverterModules(part);
			if (converter_list != null) {
				converterEfficiency = new Dictionary<string, Dictionary<string, double>>();
				foreach (var converter in converter_list) {
					var this_converter_efficiency = new Dictionary<string, double>();
					foreach (var resource_ratio in converter.outputList) this_converter_efficiency.Add(resource_ratio.ResourceName, resource_ratio.Ratio);
					converterEfficiency.Add(converter.ConverterName, this_converter_efficiency);
				}
			}

			var parachute = KRnD.GetParachuteModule(part);
			if (parachute) chuteMaxTemp = parachute.chuteMaxTemp;

			var fairing = KRnD.GetFairingModule(part);
			if (fairing) fairingAreaMass = fairing.UnitAreaMass;

			var fuel_resources = KRnD.GetFuelResources(part);
			if (fuel_resources != null) {
				fuelCapacities = new Dictionary<string, double>();
				fuelCapacitiesSum = 0;
				foreach (var fuel_resource in fuel_resources) {
					fuelCapacities.Add(fuel_resource.resourceName, fuel_resource.maxAmount);
					fuelCapacitiesSum += fuel_resource.maxAmount;
				}
			}
		}
	}
}

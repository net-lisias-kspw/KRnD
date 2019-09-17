using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KRnD.Source
{
	// This class is used to store all relevant base-stats of a part used to calculate all other stats with
	// incremental upgrades as well as a backup for restoring the original stats (eg after loading a savegame).
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
		public float mass;
		public List<float> maxFuelFlows;
		public double skinMaxTemp;
		public float torque;

		public PartStats(Part part)
		{
			mass = part.mass;
			skinMaxTemp = part.skinMaxTemp;
			intMaxTemp = part.maxTemp;

			// There should only be one or the other, engines or RCS:
			var engineModules = KRnD.getEngineModules(part);
			var rcsModule = KRnD.getRcsModule(part);
			if (engineModules != null) {
				maxFuelFlows = new List<float>();
				atmosphereCurves = new List<FloatCurve>();

				foreach (var engineModule in engineModules) {
					maxFuelFlows.Add(engineModule.maxFuelFlow);

					var atmosphereCurve = new FloatCurve();
					for (var i = 0; i < engineModule.atmosphereCurve.Curve.length; i++) {
						var frame = engineModule.atmosphereCurve.Curve[i];
						atmosphereCurve.Add(frame.time, frame.value);
					}

					atmosphereCurves.Add(atmosphereCurve);
				}
			} else if (rcsModule) {
				maxFuelFlows = new List<float>();
				atmosphereCurves = new List<FloatCurve>();

				maxFuelFlows.Add(rcsModule.thrusterPower);
				var atmosphereCurve = new FloatCurve();
				for (var i = 0; i < rcsModule.atmosphereCurve.Curve.length; i++) {
					var frame = rcsModule.atmosphereCurve.Curve[i];
					atmosphereCurve.Add(frame.time, frame.value);
				}

				atmosphereCurves.Add(atmosphereCurve);
			}

			var reactionWheel = KRnD.getReactionWheelModule(part);
			if (reactionWheel) torque = reactionWheel.RollTorque; // There is also pitch- and yaw-torque, but they should all be the same

			var solarPanel = KRnD.getSolarPanelModule(part);
			if (solarPanel) chargeRate = solarPanel.chargeRate;

			var landingLeg = KRnD.getLandingLegModule(part);
			if (landingLeg) crashTolerance = part.crashTolerance; // Every part has a crash tolerance, but we only want to improve landing legs.

			var electricCharge = KRnD.getChargeResource(part);
			if (electricCharge != null) batteryCharge = electricCharge.maxAmount;

			var generator = KRnD.getGeneratorModule(part);
			if (generator != null) {
				generatorEfficiency = new Dictionary<string, double>();
				foreach (var outputResource in generator.resHandler.outputResources) generatorEfficiency.Add(outputResource.name, outputResource.rate);
			}

			var fissionGenerator = KRnD.getFissionGeneratorModule(part);
			if (fissionGenerator != null) fissionPowerGeneration = KRnD.GetGenericModuleValue(fissionGenerator, "PowerGeneration");

			// There might be different converter-modules in the same part with different names (eg for Fuel, Monopropellant, etc):
			var converterList = KRnD.getConverterModules(part);
			if (converterList != null) {
				converterEfficiency = new Dictionary<string, Dictionary<string, double>>();
				foreach (var converter in converterList) {
					var thisConverterEfficiency = new Dictionary<string, double>();
					foreach (var resourceRatio in converter.outputList) thisConverterEfficiency.Add(resourceRatio.ResourceName, resourceRatio.Ratio);
					converterEfficiency.Add(converter.ConverterName, thisConverterEfficiency);
				}
			}

			var parachute = KRnD.getParachuteModule(part);
			if (parachute) chuteMaxTemp = parachute.chuteMaxTemp;

			var fairing = KRnD.getFairingModule(part);
			if (fairing) fairingAreaMass = fairing.UnitAreaMass;

			var fuelResources = KRnD.getFuelResources(part);
			if (fuelResources != null) {
				fuelCapacities = new Dictionary<string, double>();
				fuelCapacitiesSum = 0;
				foreach (var fuelResource in fuelResources) {
					fuelCapacities.Add(fuelResource.resourceName, fuelResource.maxAmount);
					fuelCapacitiesSum += fuelResource.maxAmount;
				}
			}
		}
	}
}

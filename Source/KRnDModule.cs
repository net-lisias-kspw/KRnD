using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP.IO;
using KSP;

namespace KRnD
{
    [KSPModule("R&D")]
    public class KRnDModule : PartModule
    {
        // Version of this part (just for show):
        [KSPField(guiActive = true, guiName = "R&D", guiUnits = "", guiFormat = "", isPersistant = false)]
        public String moduleVersion;

        // Flag, which can be set by other mods to apply latest upgrades on load:
        [KSPField(isPersistant = true)]
        public int upgradeToLatest = 0;

        // ISP Vac
        [KSPField(isPersistant = true)]
        public int ispVac_upgrades = 0;
        [KSPField(isPersistant = false)]
        public int ispVac_scienceCost = 15;
        [KSPField(isPersistant = false)]
        public float ispVac_costScale = 2f;
        [KSPField(isPersistant = false)]
        public float ispVac_improvement = 0.05f;
        [KSPField(isPersistant = false)]
        public float ispVac_improvementScale = 1f;

        // ISP Atm
        [KSPField(isPersistant = true)]
        public int ispAtm_upgrades = 0;
        [KSPField(isPersistant = false)]
        public int ispAtm_scienceCost = 15;
        [KSPField(isPersistant = false)]
        public float ispAtm_costScale = 2f;
        [KSPField(isPersistant = false)]
        public float ispAtm_improvement = 0.05f;
        [KSPField(isPersistant = false)]
        public float ispAtm_improvementScale = 1f;

        // Dry Mass
        [KSPField(isPersistant = true)]
        public int dryMass_upgrades = 0;
        [KSPField(isPersistant = false)]
        public int dryMass_scienceCost = 10;
        [KSPField(isPersistant = false)]
        public float dryMass_costScaleReference = 1f;
        [KSPField(isPersistant = false)]
        public float dryMass_costScale = 2f;
        [KSPField(isPersistant = false)]
        public float dryMass_improvement = -0.1f;
        [KSPField(isPersistant = false)]
        public float dryMass_improvementScale = 1f;

        // Fuel Flow
        [KSPField(isPersistant = true)]
        public int fuelFlow_upgrades = 0;
        [KSPField(isPersistant = false)]
        public int fuelFlow_scienceCost = 10;
        [KSPField(isPersistant = false)]
        public float fuelFlow_costScale = 2f;
        [KSPField(isPersistant = false)]
        public float fuelFlow_improvement = 0.1f;
        [KSPField(isPersistant = false)]
        public float fuelFlow_improvementScale = 1f;

        // Torque
        [KSPField(isPersistant = true)]
        public int torque_upgrades = 0;
        [KSPField(isPersistant = false)]
        public int torque_scienceCost = 5;
        [KSPField(isPersistant = false)]
        public float torque_costScale = 2f;
        [KSPField(isPersistant = false)]
        public float torque_improvement = 0.25f;
        [KSPField(isPersistant = false)]
        public float torque_improvementScale = 1f;

        // Charge Rate
        [KSPField(isPersistant = true)]
        public int chargeRate_upgrades = 0;
        [KSPField(isPersistant = false)]
        public int chargeRate_scienceCost = 10;
        [KSPField(isPersistant = false)]
        public float chargeRate_costScale = 2f;
        [KSPField(isPersistant = false)]
        public float chargeRate_improvement = 0.05f;
        [KSPField(isPersistant = false)]
        public float chargeRate_improvementScale = 1f;

        // Crash Tolerance
        [KSPField(isPersistant = true)]
        public int crashTolerance_upgrades = 0;
        [KSPField(isPersistant = false)]
        public int crashTolerance_scienceCost = 10;
        [KSPField(isPersistant = false)]
        public float crashTolerance_costScale = 2f;
        [KSPField(isPersistant = false)]
        public float crashTolerance_improvement = 0.15f;
        [KSPField(isPersistant = false)]
        public float crashTolerance_improvementScale = 1f;

        // Battery Charge
        [KSPField(isPersistant = true)]
        public int batteryCharge_upgrades = 0;
        [KSPField(isPersistant = false)]
        public int batteryCharge_scienceCost = 10;
        [KSPField(isPersistant = false)]
        public float batteryCharge_costScaleReference = 500f;
        [KSPField(isPersistant = false)]
        public float batteryCharge_costScale = 2f;
        [KSPField(isPersistant = false)]
        public float batteryCharge_improvement = 0.2f;
        [KSPField(isPersistant = false)]
        public float batteryCharge_improvementScale = 1f;

        // Generator Efficiency
        [KSPField(isPersistant = true)]
        public int generatorEfficiency_upgrades = 0;
        [KSPField(isPersistant = false)]
        public int generatorEfficiency_scienceCost = 15;
        [KSPField(isPersistant = false)]
        public float generatorEfficiency_costScale = 2f;
        [KSPField(isPersistant = false)]
        public float generatorEfficiency_improvement = 0.1f;
        [KSPField(isPersistant = false)]
        public float generatorEfficiency_improvementScale = 1f;

        // Converter Efficiency
        [KSPField(isPersistant = true)]
        public int converterEfficiency_upgrades = 0;
        [KSPField(isPersistant = false)]
        public int converterEfficiency_scienceCost = 15;
        [KSPField(isPersistant = false)]
        public float converterEfficiency_costScale = 2f;
        [KSPField(isPersistant = false)]
        public float converterEfficiency_improvement = 0.1f;
        [KSPField(isPersistant = false)]
        public float converterEfficiency_improvementScale = 1f;

        // Parachute Strength
        [KSPField(isPersistant = true)]
        public int parachuteStrength_upgrades = 0;
        [KSPField(isPersistant = false)]
        public int parachuteStrength_scienceCost = 10;
        [KSPField(isPersistant = false)]
        public float parachuteStrength_costScale = 2f;
        [KSPField(isPersistant = false)]
        public float parachuteStrength_improvement = 0.3f;
        [KSPField(isPersistant = false)]
        public float parachuteStrength_improvementScale = 1f;

        // Max Skin Temp
        [KSPField(isPersistant = true)]
        public int maxTemperature_upgrades = 0;
        [KSPField(isPersistant = false)]
        public int maxTemperature_scienceCost = 5;
        [KSPField(isPersistant = false)]
        public float maxTemperature_costScale = 2f;
        [KSPField(isPersistant = false)]
        public float maxTemperature_improvement = 0.2f;
        [KSPField(isPersistant = false)]
        public float maxTemperature_improvementScale = 1f;

        // Fuel Capacity
        [KSPField(isPersistant = true)]
        public int fuelCapacity_upgrades = 0;
        [KSPField(isPersistant = false)]
        public int fuelCapacity_scienceCost = 5;
        [KSPField(isPersistant = false)]
        public float fuelCapacity_costScaleReference = 1000f;
        [KSPField(isPersistant = false)]
        public float fuelCapacity_costScale = 2f;
        [KSPField(isPersistant = false)]
        public float fuelCapacity_improvement = 0.05f;
        [KSPField(isPersistant = false)]
        public float fuelCapacity_improvementScale = 1f;

        public static String ToRoman(int number)
        {
            if (number == 0) return "";
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900);
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            return number.ToString();
        }

        public String getVersion()
        {
            int upgrades =
                this.dryMass_upgrades +
                this.fuelFlow_upgrades +
                this.ispVac_upgrades +
                this.ispAtm_upgrades +
                this.torque_upgrades +
                this.chargeRate_upgrades +
                this.crashTolerance_upgrades +
                this.batteryCharge_upgrades +
                this.generatorEfficiency_upgrades +
                this.converterEfficiency_upgrades +
                this.parachuteStrength_upgrades +
                this.maxTemperature_upgrades +
                this.fuelCapacity_upgrades;
            if (upgrades == 0) return "";
            return "Mk " + ToRoman(upgrades + 1); // Mk I is the part without upgrades, Mk II the first upgraded version.
        }

        public override void OnStart(PartModule.StartState state)
        {
            this.moduleVersion = getVersion();
            if (this.moduleVersion == "")
            {
                this.Fields[0].guiActive = false;
            }
            else
            {
                this.Fields[0].guiActive = true;
            }
        }

        public void Start()
        {
            GameEvents.onVariantApplied.Add(this.OnVariantApplied);
        }
        // Returns the upgrade-stats which this module represents.
        public KRnDUpgrade getCurrentUpgrades()
        {
            KRnDUpgrade upgrades = new KRnDUpgrade();
            upgrades.dryMass = this.dryMass_upgrades;
            upgrades.fuelFlow = this.fuelFlow_upgrades;
            upgrades.ispVac = this.ispVac_upgrades;
            upgrades.ispAtm = this.ispAtm_upgrades;
            upgrades.torque = this.torque_upgrades;
            upgrades.chargeRate = this.chargeRate_upgrades;
            upgrades.crashTolerance = this.crashTolerance_upgrades;
            upgrades.batteryCharge = this.batteryCharge_upgrades;
            upgrades.generatorEfficiency = this.generatorEfficiency_upgrades;
            upgrades.converterEfficiency = this.converterEfficiency_upgrades;
            upgrades.parachuteStrength = this.parachuteStrength_upgrades;
            upgrades.maxTemperature = this.maxTemperature_upgrades;
            upgrades.fuelCapacity = this.fuelCapacity_upgrades;
            return upgrades;
        }


        public override string GetModuleDisplayName()
        {
            return "R & D  Upgrades";
        }
#if true
        public static string GetInfo(Part part)
        {
            float currentImprovement;

            KRnDUpgrade upgrade;
            if (!KRnD.upgrades.TryGetValue(part.name, out upgrade))
                return "none";

            var rndModule = KRnD.getKRnDModule(part);
            KRnDUpgrade currentUpgrade;
            if (!KRnD.upgrades.TryGetValue(part.name, out currentUpgrade)) currentUpgrade = new KRnDUpgrade();

            List<string> u = new List<string>();
            if (upgrade.ispVac > 0)
            {
                currentImprovement = KRnD.calculateImprovementFactor(rndModule.ispVac_improvement, rndModule.ispVac_improvementScale, currentUpgrade.ispVac);
                u.Add("Vacuum ISP: Level: " + upgrade.ispVac.ToString() + ", " + currentImprovement.ToString("+0.##%;-0.##%;-"));
            }
            if (upgrade.ispAtm > 0)
            {
                currentImprovement = KRnD.calculateImprovementFactor(rndModule.ispAtm_improvement, rndModule.ispAtm_improvementScale, currentUpgrade.ispAtm);
                u.Add("ASL ISP: Level: " + upgrade.ispAtm.ToString() + ", " + currentImprovement.ToString("+0.##%;-0.##%;-"));
            }
            if (upgrade.dryMass > 0)
            {
                currentImprovement = KRnD.calculateImprovementFactor(rndModule.dryMass_improvement, rndModule.dryMass_improvementScale, currentUpgrade.dryMass);
                u.Add("Dry Mass: Level: " + upgrade.dryMass.ToString() + ", " + currentImprovement.ToString("+0.##%;-0.##%;-"));
            }
            if (upgrade.fuelFlow > 0)
            {
                currentImprovement = KRnD.calculateImprovementFactor(rndModule.fuelFlow_improvement, rndModule.fuelFlow_improvementScale, currentUpgrade.fuelFlow);
                u.Add("Fuel Flow: Level: " + upgrade.fuelFlow.ToString() + ", " + currentImprovement.ToString("+0.##%;-0.##%;-"));
            }
            if (upgrade.torque > 0)
            {
                currentImprovement = KRnD.calculateImprovementFactor(rndModule.torque_improvement, rndModule.torque_improvementScale, currentUpgrade.torque);
                u.Add("Torque: Level: " + upgrade.torque.ToString() + ", " + currentImprovement.ToString("+0.##%;-0.##%;-"));
            }
            if (upgrade.chargeRate > 0)
            {
                currentImprovement = KRnD.calculateImprovementFactor(rndModule.chargeRate_improvement, rndModule.chargeRate_improvementScale, currentUpgrade.chargeRate);
                u.Add("Charge Rate: Level: " + upgrade.chargeRate.ToString() + ", " + currentImprovement.ToString("+0.##%;-0.##%;-"));
            }
            if (upgrade.crashTolerance > 0)
            {
                currentImprovement = KRnD.calculateImprovementFactor(rndModule.crashTolerance_improvement, rndModule.crashTolerance_improvementScale, currentUpgrade.crashTolerance);
                u.Add("Crash Tolerance: Level: " + upgrade.crashTolerance.ToString() + ", " + currentImprovement.ToString("+0.##%;-0.##%;-"));
            }
            if (upgrade.batteryCharge > 0)
            {
                currentImprovement = KRnD.calculateImprovementFactor(rndModule.batteryCharge_improvement, rndModule.batteryCharge_improvementScale, currentUpgrade.batteryCharge);
                u.Add("Battery Chrg: Level: " + upgrade.batteryCharge.ToString() + ", " + currentImprovement.ToString("+0.##%;-0.##%;-"));
            }
            if (upgrade.generatorEfficiency > 0)
            {
                currentImprovement = KRnD.calculateImprovementFactor(rndModule.generatorEfficiency_improvement, rndModule.generatorEfficiency_improvementScale, currentUpgrade.generatorEfficiency);
                u.Add("Generator Eff.: Level: " + upgrade.generatorEfficiency.ToString() + ", " + currentImprovement.ToString("+0.##%;-0.##%;-"));
            }
            if (upgrade.converterEfficiency > 0)
            {
                currentImprovement = KRnD.calculateImprovementFactor(rndModule.converterEfficiency_improvement, rndModule.converterEfficiency_improvementScale, currentUpgrade.converterEfficiency);
                u.Add("Converter Efficiency: Level: " + upgrade.converterEfficiency.ToString() + ", " + currentImprovement.ToString("+0.##%;-0.##%;-"));
            }
            if (upgrade.parachuteStrength > 0)
            {
                currentImprovement = KRnD.calculateImprovementFactor(rndModule.parachuteStrength_improvement, rndModule.parachuteStrength_improvementScale, currentUpgrade.parachuteStrength);
                u.Add("Chute Strh: Level: " + upgrade.parachuteStrength.ToString() + ", " + currentImprovement.ToString("+0.##%;-0.##%;-"));
            }
            if (upgrade.maxTemperature > 0)
            {
                currentImprovement = KRnD.calculateImprovementFactor(rndModule.maxTemperature_improvement, rndModule.maxTemperature_improvementScale, currentUpgrade.maxTemperature);
                u.Add("Max Temp: Level: " + upgrade.maxTemperature.ToString() + ", " + currentImprovement.ToString("+0.##%;-0.##%;-"));
            }
            if (upgrade.ispVac > 0)
            {
                currentImprovement = KRnD.calculateImprovementFactor(rndModule.fuelCapacity_improvement, rndModule.fuelCapacity_improvementScale, currentUpgrade.fuelCapacity);
                u.Add("Fuel Capacity: Level: " + upgrade.fuelCapacity.ToString() + ", " + currentImprovement.ToString("+0.##%;-0.##%;-"));
            }
            if (u.Count == 0)
                return "No upgrades applied";


            return String.Join("\n", u);
        }
        public override string GetInfo()
        {
            return GetInfo(this.part);
        }

        private void OnVariantApplied(Part p, PartVariant pv)
        {
            if (p == null || p.partInfo.partPrefab == null || p != this.part) return;

            String partName = KRnD.sanatizePartName(p.name);

            if (KRnD.originalStats.TryGetValue(partName, out PartStats partStats))
            {
                if (partStats.kRnDVariants != null && partStats.kRnDVariants.TryGetValue(pv.Name, out KRnDVariant kv))
                {
                    partStats.currentVariant = pv.Name;
                    partStats.currentVariantMass = kv.mass;
                    this.part.partInfo.variant = pv; // ??? why doesn't KSP do this
                }
            }
            else throw new Exception("no original-stats for part '" + partName + "'");
        }

#endif
    }
}

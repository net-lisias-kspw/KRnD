using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;

using KSP.UI.Screens; // For "ApplicationLauncherButton"

using GUILayout = KSPe.UI.GUILayout;
using ToolbarControl_NS;

namespace KRnD
{
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class KRnDGUI : UnityEngine.MonoBehaviour
    {
        public static Rect windowPosition = new Rect(300, 60, 450, 400);
        private static GUIStyle windowStyle;
        private static GUIStyle labelStyle;
        private static GUIStyle labelStyleSmall;
        private static GUIStyle buttonStyle;
        private static GUIStyle scrollStyle;
        private static Vector2 scrollPos = Vector2.zero;
        // private static Texture2D texture = null;
        private static bool showGui = false;

        // The part that was last selected in the editor:
        public static Part selectedPart = null;

        private int selectedUpgradeOption = 0;

        static ToolbarControl toolbarControl = null;
        internal const string MODID = "KRnD_NS";
        internal const string MODNAME = "Kerbal  R & D";

        void Awake()
        {
            windowPosition = new Rect(300, 60, 450, 400);
            windowStyle = new GUIStyle(HighLogic.Skin.window) { fixedWidth = 500f, fixedHeight = 370 };
            labelStyle = new GUIStyle(HighLogic.Skin.label);
            labelStyleSmall = new GUIStyle(HighLogic.Skin.label) { fontSize = 10 };
            buttonStyle = new GUIStyle(HighLogic.Skin.button);
            scrollStyle = new GUIStyle(HighLogic.Skin.scrollView);
            scrollPos = Vector2.zero;

            ReadyEvent();

        }

        public void ReadyEvent()
        {
            if (toolbarControl != null)
                return;
            toolbarControl = gameObject.AddComponent<ToolbarControl>();
            toolbarControl.AddToAllToolbars(GuiOn, GuiOff,
                 ApplicationLauncher.AppScenes.SPH | ApplicationLauncher.AppScenes.VAB,
                 MODID,
                "KRnD",
                "KRnD/PluginData/R&D_icon-38",
                "KRnD/PluginData/R&D_icon-24",
                MODNAME
            );

        }


        // Fires when a scene is unloaded and we should destroy our button:
        //public void DestroyEvent()
        public void OnDestroy()
        {
            if (toolbarControl != null)
            {
                toolbarControl.OnDestroy();
                Destroy(toolbarControl);
            }
            toolbarControl = null;

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
            if (showGui)
            {
                windowPosition = GUILayout.Window(100, windowPosition, OnWindow, "", windowStyle);
            }
        }

        public static int UpgradeIspVac(Part part)
        {
            try
            {
                KRnDUpgrade store = null;
                if (!KRnD.upgrades.TryGetValue(part.name, out store))
                {
                    store = new KRnDUpgrade();
                    KRnD.upgrades.Add(part.name, store);
                }
                store.ispVac++;
                KRnD.updateGlobalParts();
                KRnD.updateEditorVessel();
            }
            catch (Exception e)
            {
                Log.error(e, "UpgradeIspVac(): {0}", e);
            }
            return 0;
        }

        public static int UpgradeIspAtm(Part part)
        {
            try
            {
                KRnDUpgrade store = null;
                if (!KRnD.upgrades.TryGetValue(part.name, out store))
                {
                    store = new KRnDUpgrade();
                    KRnD.upgrades.Add(part.name, store);
                }
                store.ispAtm++;
                KRnD.updateGlobalParts();
                KRnD.updateEditorVessel();
            }
            catch (Exception e)
            {
                Log.error(e, "UpgradeIspAtm(): {0}", e);
            }
            return 0;
        }

        public static int UpgradeDryMass(Part part)
        {
            try
            {
                KRnDUpgrade store = null;
                if (!KRnD.upgrades.TryGetValue(part.name, out store))
                {
                    store = new KRnDUpgrade();
                    KRnD.upgrades.Add(part.name, store);
                }
                store.dryMass++;
                KRnD.updateGlobalParts();
                KRnD.updateEditorVessel();
            }
            catch (Exception e)
            {
                Log.error(e, "UpgradeDryMass(): {0}", e);
            }
            return 0;
        }

        public static int UpgradeFuelFlow(Part part)
        {
            try
            {
                KRnDUpgrade store = null;
                if (!KRnD.upgrades.TryGetValue(part.name, out store))
                {
                    store = new KRnDUpgrade();
                    KRnD.upgrades.Add(part.name, store);
                }
                store.fuelFlow++;
                KRnD.updateGlobalParts();
                KRnD.updateEditorVessel();
            }
            catch (Exception e)
            {
                Log.error(e, "UpgradeFuelFlow(): {0}", e);
            }
            return 0;
        }

        public static int UpgradeTorque(Part part)
        {
            try
            {
                KRnDUpgrade store = null;
                if (!KRnD.upgrades.TryGetValue(part.name, out store))
                {
                    store = new KRnDUpgrade();
                    KRnD.upgrades.Add(part.name, store);
                }
                store.torque++;
                KRnD.updateGlobalParts();
                KRnD.updateEditorVessel();
            }
            catch (Exception e)
            {
                Log.error(e, "UpgradeTorque(): {0}", e);
            }
            return 0;
        }

        // WIP
        //public static int UpgradeAntennaPower(Part part)
        //{
        //    try
        //    {
        //        KRnDUpgrade store = null;
        //        if (!KRnD.upgrades.TryGetValue(part.name, out store))
        //        {
        //            store = new KRnDUpgrade();
        //            KRnD.upgrades.Add(part.name, store);
        //        }
        //        store.antPower++;
        //        KRnD.updateGlobalParts();
        //        KRnD.updateEditorVessel();
        //    }
        //    catch (Exception e)
        //    {
        //        Log.error(e, "UpgradeAntennaPower(): {0}", e);
        //    }
        //    return 0;
        //}

        public static int UpgradeDrillPower(Part part)
        {
            try
            {
                KRnDUpgrade store = null;
                if (!KRnD.upgrades.TryGetValue(part.name, out store))
                {
                    store = new KRnDUpgrade();
                    KRnD.upgrades.Add(part.name, store);
                }
                store.harvester++;
                KRnD.updateGlobalParts();
                KRnD.updateEditorVessel();
            }
            catch (Exception e)
            {
                Log.error(e, "UpgradeDrillPower(): {0}", e);
            }
            return 0;
        }

        public static int UpgradeRadiatorEfficiency(Part part)
        {
            try
            {
                KRnDUpgrade store = null;
                if (!KRnD.upgrades.TryGetValue(part.name, out store))
                {
                    store = new KRnDUpgrade();
                    KRnD.upgrades.Add(part.name, store);
                }
                store.radiatorEfficiency++;
                KRnD.updateGlobalParts();
                KRnD.updateEditorVessel();
            }
            catch (Exception e)
            {
                Log.error(e, "UpgradeRadiatorEfficiency(): {0}", e);
            }
            return 0;
        }

        public static int UpgradeChargeRate(Part part)
        {
            try
            {
                KRnDUpgrade store = null;
                if (!KRnD.upgrades.TryGetValue(part.name, out store))
                {
                    store = new KRnDUpgrade();
                    KRnD.upgrades.Add(part.name, store);
                }
                store.chargeRate++;
                KRnD.updateGlobalParts();
                KRnD.updateEditorVessel();
            }
            catch (Exception e)
            {
                Log.error(e, "UpgradeChargeRate(): {0}", e);
            }
            return 0;
        }

        public static int UpgradeCrashTolerance(Part part)
        {
            try
            {
                KRnDUpgrade store = null;
                if (!KRnD.upgrades.TryGetValue(part.name, out store))
                {
                    store = new KRnDUpgrade();
                    KRnD.upgrades.Add(part.name, store);
                }
                store.crashTolerance++;
                KRnD.updateGlobalParts();
                KRnD.updateEditorVessel();
            }
            catch (Exception e)
            {
                Log.error(e, "UpgradeCrashTolerance(): {0}", e);
            }
            return 0;
        }

        public static int UpgradeBatteryCharge(Part part)
        {
            try
            {
                KRnDUpgrade store = null;
                if (!KRnD.upgrades.TryGetValue(part.name, out store))
                {
                    store = new KRnDUpgrade();
                    KRnD.upgrades.Add(part.name, store);
                }
                store.batteryCharge++;
                KRnD.updateGlobalParts();
                KRnD.updateEditorVessel();
            }
            catch (Exception e)
            {
                Log.error(e, "UpgradeBatteryCharge(): {0}", e);
            }
            return 0;
        }

        public static int UpgradeGeneratorEfficiency(Part part)
        {
            try
            {
                KRnDUpgrade store = null;
                if (!KRnD.upgrades.TryGetValue(part.name, out store))
                {
                    store = new KRnDUpgrade();
                    KRnD.upgrades.Add(part.name, store);
                }
                store.generatorEfficiency++;
                KRnD.updateGlobalParts();
                KRnD.updateEditorVessel();
            }
            catch (Exception e)
            {
                Log.error(e, "UpgradeGeneratorEfficiency(): {0}", e);
            }
            return 0;
        }

        public static int UpgradeConverterEfficiency(Part part)
        {
            try
            {
                KRnDUpgrade store = null;
                if (!KRnD.upgrades.TryGetValue(part.name, out store))
                {
                    store = new KRnDUpgrade();
                    KRnD.upgrades.Add(part.name, store);
                }
                store.converterEfficiency++;
                KRnD.updateGlobalParts();
                KRnD.updateEditorVessel();
            }
            catch (Exception e)
            {
                Log.error(e, "UpgradeConverterEfficiency(): {0}", e);
            }
            return 0;
        }

        public static int UpgradeParachuteStrength(Part part)
        {
            try
            {
                KRnDUpgrade store = null;
                if (!KRnD.upgrades.TryGetValue(part.name, out store))
                {
                    store = new KRnDUpgrade();
                    KRnD.upgrades.Add(part.name, store);
                }
                store.parachuteStrength++;
                KRnD.updateGlobalParts();
                KRnD.updateEditorVessel();
            }
            catch (Exception e)
            {
                Log.error(e, "UpgradeParachuteStrength(): {0}", e);
            }
            return 0;
        }

        public static int UpgradeMaxTemperature(Part part)
        {
            try
            {
                KRnDUpgrade store = null;
                if (!KRnD.upgrades.TryGetValue(part.name, out store))
                {
                    store = new KRnDUpgrade();
                    KRnD.upgrades.Add(part.name, store);
                }
                store.maxTemperature++;
                KRnD.updateGlobalParts();
                KRnD.updateEditorVessel();
            }
            catch (Exception e)
            {
                Log.error(e, "UpgradeMaxTemperature(): {0}", e);
            }
            return 0;
        }

        public static int UpgradeFuelCapacity(Part part)
        {
            try
            {
                KRnDUpgrade store = null;
                if (!KRnD.upgrades.TryGetValue(part.name, out store))
                {
                    store = new KRnDUpgrade();
                    KRnD.upgrades.Add(part.name, store);
                }
                store.fuelCapacity++;
                KRnD.updateGlobalParts();
                KRnD.updateEditorVessel();
            }
            catch (Exception e)
            {
                Log.error(e, "UpgradeFuelCapacity(): {0}", e);
            }
            return 0;
        }

        // Returns the info-text of the given part with the given upgrades to be displayed in the GUI-comparison.
        private String getPartInfo(Part part, KRnDUpgrade upgradesToApply = null)
        {
            String info = "";
            KRnDUpgrade originalUpgrades = null;
            try
            {
                KRnDModule rndModule = KRnD.getKRnDModule(part);
                if (rndModule == null || (originalUpgrades = rndModule.getCurrentUpgrades()) == null) return info;

                // Upgrade the part to get the correct info, we revert it back to its previous values in the finally block below:
                KRnD.updatePart(part, upgradesToApply);
                List<ModuleEngines> engineModules = KRnD.getEngineModules(part);
                ModuleRCS rcsModule = KRnD.getRcsModule(part);
                ModuleReactionWheel reactionWheelModule = KRnD.getReactionWheelModule(part);
                ModuleDeployableSolarPanel solarPanelModule = KRnD.getSolarPanelModule(part);
                ModuleWheelBase landingLegModule = KRnD.getLandingLegModule(part);
                PartResource electricChargeResource = KRnD.getChargeResource(part);
                ModuleGenerator generatorModule = KRnD.getGeneratorModule(part);
                PartModule fissionGenerator = KRnD.getFissionGeneratorModule(part);
                List<ModuleResourceConverter> converterModules = KRnD.getConverterModules(part);
                ModuleParachute parachuteModule = KRnD.getParachuteModule(part);
                ModuleProceduralFairing fairingModule = KRnD.getFairingModule(part);
                List<PartResource> fuelResources = KRnD.getFuelResources(part);
                //ModuleDataTransmitter dataTransmitter = KRnD.getDataTransmitterModule(part); //WIP
                ModuleResourceHarvester resourceHarvester = KRnD.getResourceHarvesterModule(part);
                ModuleActiveRadiator activeRadiator = KRnD.getActiveRadiatorModule(part);

                float partMass = part.mass;
                //if (part.partInfo.partPrefab != null && part.partInfo.Variants != null && part.partInfo.Variants.Count > 0)
                if (part.partInfo.variant != null)
                    partMass += part.partInfo.variant.Mass;

                // Basic stats:
                //info = "<color=#FFFFFF><b>Dry Mass:</b> " + part.mass.ToString("0.#### t") + "\n";
                info = "<color=#FFFFFF><b>Dry Mass:</b> " + partMass.ToString("0.#### t") + "\n";
                info += "<b>Max Temp.:</b> " + part.maxTemp.ToString("0.#") + "/" + part.skinMaxTemp.ToString("0.#") + " °K\n";
                if (landingLegModule != null) info += "<b>Crash Tolerance:</b> " + part.crashTolerance.ToString("0.#### m/s") + "\n";
                if (electricChargeResource != null) info += "<b>Electric Charge:</b> " + electricChargeResource.maxAmount.ToString() + "\n";

                // Fuels:
                if (fuelResources != null)
                {
                    for (int i = 0; i < fuelResources.Count; i++)
                    {
                        PartResource fuelResource = fuelResources[i];

                        // Reformat resource-names like "ElectricCharge" to "Electric Charge":
                        String fuelName = fuelResource.resourceName.ToString();
                        fuelName = Regex.Replace(fuelName, @"([a-z])([A-Z])", "$1 $2");
                        info += "<b>" + fuelName + ":</b> " + fuelResource.maxAmount.ToString() + "\n";
                    }
                }

                // Module stats:
                info += "\n";
                if (engineModules != null)
                {
                    for (int i = 0; i < engineModules.Count; i++)
                    {
                        ModuleEngines engineModule = engineModules[i];

                        info += "<color=#99FF00><b>Engine";
                        if (engineModules.Count > 1) info += " (" + engineModule.engineID.ToString() + ")";
                        info += ":</b></color>\n" + engineModule.GetInfo();
                        if (engineModules.Count > 1) info += "\n";
                    }
                }
                if (rcsModule) info += "<color=#99FF00><b>RCS:</b></color>\n" + rcsModule.GetInfo();
                if (reactionWheelModule) info += "<color=#99FF00><b>Reaction Wheel:</b></color>\n" + reactionWheelModule.GetInfo();
                if (solarPanelModule) info += "<color=#99FF00><b>Solar Panel:</b></color>\n" + KRnD.getSolarPanelInfo(solarPanelModule);
                if (generatorModule) info += "<color=#99FF00><b>Generator:</b></color>\n" + generatorModule.GetInfo();
                if (fissionGenerator) info += "<color=#99FF00><b>Fission-Generator:</b></color>\n" + fissionGenerator.GetInfo();
                if (converterModules != null)
                {
                    for (int i = 0; i < converterModules.Count; i++)
                    {
                        ModuleResourceConverter converterModule = converterModules[i];

                        info += "<color=#99FF00><b>Converter " + converterModule.ConverterName + ":</b></color>\n" + converterModule.GetInfo() + "\n";
                    }
                }

                if (parachuteModule) info += "<color=#99FF00><b>Parachute:</b></color>\n" + parachuteModule.GetInfo();
                if (fairingModule) info += "<color=#99FF00><b>Fairing:</b></color>\n" + fairingModule.GetInfo();
                //if (dataTransmitter) info += "<color=#99FF00><b>Antenna:</b></color>\n" + dataTransmitter.GetInfo(); // WIP
                if (resourceHarvester) info += "<color=#99FF00><b>Drill:</b></color>\n" + resourceHarvester.GetInfo();
                if (activeRadiator) info += "<color=#99FF00><b>Radiator:</b></color>\n" + activeRadiator.GetInfo();
                info += "</color>";
            }
            catch (Exception e)
            {
                Log.error(e, "getPartInfo(): {0}", e);
            }
            finally
            {
                try
                {
                    if (originalUpgrades != null)
                    {
                        KRnD.updatePart(part, originalUpgrades);
                    }
                }
                catch (Exception e)
                {
                    Log.error(e, "getPartInfo() restore of part failed: {0}", e);
                }
            }
            return info;
        }

        // Highlights differences between the two given texts, assuming they contain the same number of words.
        private String highlightChanges(String originalText, String newText, String color = "00FF00")
        {
            String highlightedText = "";
            try
            {
                // Split as whitespaces and tags, we only need normal words and numbers:
                String[] set1 = Regex.Split(originalText, @"([\s<>])");
                String[] set2 = Regex.Split(newText, @"([\s<>])");
                for (int i = 0; i < set2.Length; i++)
                {
                    String oldWord = "";
                    if (i < set1.Length) oldWord = set1[i];
                    String newWord = set2[i];

                    if (oldWord != newWord) newWord = "<color=#" + color + "><b>" + newWord + "</b></color>";
                    highlightedText += newWord;
                }
            }
            catch (Exception e)
            {
                Log.error(e, "highlightChanges(): {0}", e);
            }
            if (highlightedText == "") return newText;
            return highlightedText;
        }

        private void OnWindow(int windowId)
        {
            try
            {
                GUILayout.BeginVertical();

                // Get all modules of the selected part:
                String partTitle = "";
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
                //ModuleDataTransmitter dataTransmitter = null; // WIP
                ModuleResourceHarvester resourceHarvester = null;
                ModuleActiveRadiator activeRadiator = null;
                if (selectedPart != null)
                {
                    for (int i = 0; i < PartLoader.LoadedPartsList.Count; i++)
                    {
                        AvailablePart aPart = PartLoader.LoadedPartsList[i];

                        if (aPart.partPrefab.name == selectedPart.name)
                        {
                            part = aPart.partPrefab;
                            partTitle = aPart.title;
                            break;
                        }
                    }
                    if (part)
                    {
                        rndModule = KRnD.getKRnDModule(part);
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
                        //dataTransmitter = KRnD.getDataTransmitterModule(part); // WIP
                        resourceHarvester = KRnD.getResourceHarvesterModule(part);
                        activeRadiator = KRnD.getActiveRadiatorModule(part);
                    }
                }
                if (!part)
                {
                    // No part selected:
                    GUILayout.BeginArea(new Rect(10, 5, windowStyle.fixedWidth, 20));
                    GUILayout.Label("<b>Kerbal R&D: Select a part to improve</b>", labelStyle);
                    GUILayout.EndArea();
                    GUILayout.EndVertical();
                    GUI.DragWindow();
                    return;
                }
                else if (!rndModule)
                {
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
                String currentInfo = getPartInfo(part, currentUpgrade);

                // Create a copy of the part-stats which we can use to mock an upgrade further below:
                KRnDUpgrade nextUpgrade = currentUpgrade.clone();

                // Title:
                GUILayout.BeginArea(new Rect(10, 5, windowStyle.fixedWidth, 20));
                String version = rndModule.getVersion();
                if (version != "") version = " - " + version;
                GUILayout.Label("<b>" + partTitle + version + "</b>", labelStyle);
                GUILayout.EndArea();

                // List with upgrade-options:
                float optionsWidth = 100;
                float optionsHeight = windowStyle.fixedHeight - 30 - 30 - 20;
                GUILayout.BeginArea(new Rect(10, 30 + 20, optionsWidth, optionsHeight));

                List<String> options = new List<String>();
                options.Add("Dry Mass");
                options.Add("Max Temp");
                if (engineModules != null || rcsModule)
                {
                    options.Add("ISP Vac");
                    options.Add("ISP Atm");
                    options.Add("Fuel Flow");
                }
                if (reactionWheelModule != null)
                {
                    options.Add("Torque");
                }
                if (solarPanelModule != null)
                {
                    options.Add("Charge Rate");
                }
                if (landingLegModule != null)
                {
                    options.Add("Crash Tolerance");
                }
                if (electricChargeResource != null)
                {
                    options.Add("Battery");
                }
                if (fuelResources != null)
                {
                    options.Add("Fuel Pressure");
                }
                if (generatorModule || fissionGenerator)
                {
                    options.Add("Generator");
                }
                if (converterModules != null)
                {
                    options.Add("Converter");
                }
                if (parachuteModule)
                {
                    options.Add("Parachute");
                }
                // WIP                
                //if (dataTransmitter)
                //{
                //    options.Add("Antenna");
                //}
                if (resourceHarvester)
                {
                    options.Add("Drill");
                }
                if (activeRadiator)
                {
                    options.Add("Radiator");
                }
                if (this.selectedUpgradeOption >= options.Count) this.selectedUpgradeOption = 0;
                this.selectedUpgradeOption = GUILayout.SelectionGrid(this.selectedUpgradeOption, options.ToArray(), 1, buttonStyle);
                GUILayout.EndArea();

                String selectedUpgradeOption = options.ToArray()[this.selectedUpgradeOption];
                int currentUpgradeCount = 0;
                int nextUpgradeCount = 0;
                int scienceCost = 0;
                float currentImprovement = 0;
                float nextImprovement = 0;
                Func<Part, int> upgradeFunction = null;
                switch (selectedUpgradeOption)
                {
                    case "ISP Vac":
                        {
                            upgradeFunction = KRnDGUI.UpgradeIspVac;
                            currentUpgradeCount = currentUpgrade.ispVac;
                            nextUpgradeCount = ++nextUpgrade.ispVac;
                            currentImprovement = KRnD.calculateImprovementFactor(rndModule.ispVac_improvement, rndModule.ispVac_improvementScale, currentUpgrade.ispVac);
                            nextImprovement = KRnD.calculateImprovementFactor(rndModule.ispVac_improvement, rndModule.ispVac_improvementScale, nextUpgrade.ispVac);
                            scienceCost = KRnD.calculateScienceCost(rndModule.ispVac_scienceCost, rndModule.ispVac_costScale, nextUpgrade.ispVac);
                        }
                        break;
                    case "ISP Atm":
                        {
                            upgradeFunction = KRnDGUI.UpgradeIspAtm;
                            currentUpgradeCount = currentUpgrade.ispAtm;
                            nextUpgradeCount = ++nextUpgrade.ispAtm;
                            currentImprovement = KRnD.calculateImprovementFactor(rndModule.ispAtm_improvement, rndModule.ispAtm_improvementScale, currentUpgrade.ispAtm);
                            nextImprovement = KRnD.calculateImprovementFactor(rndModule.ispAtm_improvement, rndModule.ispAtm_improvementScale, nextUpgrade.ispAtm);
                            scienceCost = KRnD.calculateScienceCost(rndModule.ispAtm_scienceCost, rndModule.ispAtm_costScale, nextUpgrade.ispAtm);
                        }
                        break;
                    case "Fuel Flow":
                        {
                            upgradeFunction = KRnDGUI.UpgradeFuelFlow;
                            currentUpgradeCount = currentUpgrade.fuelFlow;
                            nextUpgradeCount = ++nextUpgrade.fuelFlow;
                            currentImprovement = KRnD.calculateImprovementFactor(rndModule.fuelFlow_improvement, rndModule.fuelFlow_improvementScale, currentUpgrade.fuelFlow);
                            nextImprovement = KRnD.calculateImprovementFactor(rndModule.fuelFlow_improvement, rndModule.fuelFlow_improvementScale, nextUpgrade.fuelFlow);
                            scienceCost = KRnD.calculateScienceCost(rndModule.fuelFlow_scienceCost, rndModule.fuelFlow_costScale, nextUpgrade.fuelFlow);
                        }
                        break;
                    case "Dry Mass":
                        {
                            upgradeFunction = KRnDGUI.UpgradeDryMass;
                            currentUpgradeCount = currentUpgrade.dryMass;
                            nextUpgradeCount = ++nextUpgrade.dryMass;
                            currentImprovement = KRnD.calculateImprovementFactor(rndModule.dryMass_improvement, rndModule.dryMass_improvementScale, currentUpgrade.dryMass);
                            nextImprovement = KRnD.calculateImprovementFactor(rndModule.dryMass_improvement, rndModule.dryMass_improvementScale, nextUpgrade.dryMass);

                            // Scale science cost with original mass:
                            PartStats originalStats;
                            if (!KRnD.originalStats.TryGetValue(part.name, out originalStats)) throw new Exception("no original-stats for part '" + part.name + "'");
                            float scaleReferenceFactor = 1;
                            if (rndModule.dryMass_costScaleReference > 0) scaleReferenceFactor = originalStats.mass / rndModule.dryMass_costScaleReference;
                            int scaledCost = (int)Math.Round(rndModule.dryMass_scienceCost * scaleReferenceFactor);
                            if (scaledCost < 1) scaledCost = 1;
                            scienceCost = KRnD.calculateScienceCost(scaledCost, rndModule.dryMass_costScale, nextUpgrade.dryMass);
                        }
                        break;
                    case "Torque":
                        {
                            upgradeFunction = KRnDGUI.UpgradeTorque;
                            currentUpgradeCount = currentUpgrade.torque;
                            nextUpgradeCount = ++nextUpgrade.torque;
                            currentImprovement = KRnD.calculateImprovementFactor(rndModule.torque_improvement, rndModule.torque_improvementScale, currentUpgrade.torque);
                            nextImprovement = KRnD.calculateImprovementFactor(rndModule.torque_improvement, rndModule.torque_improvementScale, nextUpgrade.torque);
                            scienceCost = KRnD.calculateScienceCost(rndModule.torque_scienceCost, rndModule.torque_costScale, nextUpgrade.torque);
                        }
                        break;
                    // WIP
                    //case "Antenna":
                    //    {
                    //        upgradeFunction = KRnDGUI.UpgradeAntennaPower;
                    //        currentUpgradeCount = currentUpgrade.antPower;
                    //        nextUpgradeCount = ++nextUpgrade.antPower;
                    //        currentImprovement = KRnD.calculateImprovementFactor(rndModule.antPower_improvement, rndModule.antPower_improvementScale, currentUpgrade.antPower);
                    //        nextImprovement = KRnD.calculateImprovementFactor(rndModule.antPower_improvement, rndModule.antPower_improvementScale, nextUpgrade.antPower);
                    //        scienceCost = KRnD.calculateScienceCost(rndModule.antPower_scienceCost, rndModule.antPower_costScale, nextUpgrade.antPower);
                    //    }
                    //    break;
                    case "Drill":
                        {
                            upgradeFunction = KRnDGUI.UpgradeDrillPower;
                            currentUpgradeCount = currentUpgrade.harvester;
                            nextUpgradeCount = ++nextUpgrade.harvester;
                            currentImprovement = KRnD.calculateImprovementFactor(rndModule.harvester_improvement, rndModule.harvester_improvementScale, currentUpgrade.harvester);
                            nextImprovement = KRnD.calculateImprovementFactor(rndModule.harvester_improvement, rndModule.harvester_improvementScale, nextUpgrade.harvester);
                            scienceCost = KRnD.calculateScienceCost(rndModule.harvester_scienceCost, rndModule.harvester_costScale, nextUpgrade.harvester);
                        }
                        break;
                    case "Radiator":
                        {
                            upgradeFunction = KRnDGUI.UpgradeRadiatorEfficiency;
                            currentUpgradeCount = currentUpgrade.radiatorEfficiency;
                            nextUpgradeCount = ++nextUpgrade.radiatorEfficiency;
                            currentImprovement = KRnD.calculateImprovementFactor(rndModule.radiatorEfficiency_improvement, rndModule.radiatorEfficiency_improvementScale, currentUpgrade.radiatorEfficiency);
                            nextImprovement = KRnD.calculateImprovementFactor(rndModule.radiatorEfficiency_improvement, rndModule.radiatorEfficiency_improvementScale, nextUpgrade.radiatorEfficiency);
                            scienceCost = KRnD.calculateScienceCost(rndModule.radiatorEfficiency_scienceCost, rndModule.radiatorEfficiency_costScale, nextUpgrade.radiatorEfficiency);
                        }
                        break;
                    case "Charge Rate":
                        {
                            upgradeFunction = KRnDGUI.UpgradeChargeRate;
                            currentUpgradeCount = currentUpgrade.chargeRate;
                            nextUpgradeCount = ++nextUpgrade.chargeRate;
                            currentImprovement = KRnD.calculateImprovementFactor(rndModule.chargeRate_improvement, rndModule.chargeRate_improvementScale, currentUpgrade.chargeRate);
                            nextImprovement = KRnD.calculateImprovementFactor(rndModule.chargeRate_improvement, rndModule.chargeRate_improvementScale, nextUpgrade.chargeRate);
                            scienceCost = KRnD.calculateScienceCost(rndModule.chargeRate_scienceCost, rndModule.chargeRate_costScale, nextUpgrade.chargeRate);
                        }
                        break;
                    case "Crash Tolerance":
                        {
                            upgradeFunction = KRnDGUI.UpgradeCrashTolerance;
                            currentUpgradeCount = currentUpgrade.crashTolerance;
                            nextUpgradeCount = ++nextUpgrade.crashTolerance;
                            currentImprovement = KRnD.calculateImprovementFactor(rndModule.crashTolerance_improvement, rndModule.crashTolerance_improvementScale, currentUpgrade.crashTolerance);
                            nextImprovement = KRnD.calculateImprovementFactor(rndModule.crashTolerance_improvement, rndModule.crashTolerance_improvementScale, nextUpgrade.crashTolerance);
                            scienceCost = KRnD.calculateScienceCost(rndModule.crashTolerance_scienceCost, rndModule.crashTolerance_costScale, nextUpgrade.crashTolerance);
                        }
                        break;
                    case "Battery":
                        {
                            upgradeFunction = KRnDGUI.UpgradeBatteryCharge;
                            currentUpgradeCount = currentUpgrade.batteryCharge;
                            nextUpgradeCount = ++nextUpgrade.batteryCharge;
                            currentImprovement = KRnD.calculateImprovementFactor(rndModule.batteryCharge_improvement, rndModule.batteryCharge_improvementScale, currentUpgrade.batteryCharge);
                            nextImprovement = KRnD.calculateImprovementFactor(rndModule.batteryCharge_improvement, rndModule.batteryCharge_improvementScale, nextUpgrade.batteryCharge);

                            // Scale science cost with original battery charge:
                            PartStats originalStats;
                            if (!KRnD.originalStats.TryGetValue(part.name, out originalStats)) throw new Exception("no origional-stats for part '" + part.name + "'");
                            double scaleReferenceFactor = 1;
                            if (rndModule.batteryCharge_costScaleReference > 0) scaleReferenceFactor = originalStats.batteryCharge / rndModule.batteryCharge_costScaleReference;
                            int scaledCost = (int)Math.Round(rndModule.batteryCharge_scienceCost * scaleReferenceFactor);
                            if (scaledCost < 1) scaledCost = 1;
                            scienceCost = KRnD.calculateScienceCost(scaledCost, rndModule.batteryCharge_costScale, nextUpgrade.batteryCharge);
                        }
                        break;
                    case "Fuel Pressure":
                        {
                            upgradeFunction = KRnDGUI.UpgradeFuelCapacity;
                            currentUpgradeCount = currentUpgrade.fuelCapacity;
                            nextUpgradeCount = ++nextUpgrade.fuelCapacity;
                            currentImprovement = KRnD.calculateImprovementFactor(rndModule.fuelCapacity_improvement, rndModule.fuelCapacity_improvementScale, currentUpgrade.fuelCapacity);
                            nextImprovement = KRnD.calculateImprovementFactor(rndModule.fuelCapacity_improvement, rndModule.fuelCapacity_improvementScale, nextUpgrade.fuelCapacity);

                            // Scale science cost with original fuel capacity:
                            PartStats originalStats;
                            if (!KRnD.originalStats.TryGetValue(part.name, out originalStats)) throw new Exception("no origional-stats for part '" + part.name + "'");
                            double scaleReferenceFactor = 1;
                            if (rndModule.fuelCapacity_costScaleReference > 0) scaleReferenceFactor = originalStats.fuelCapacitiesSum / rndModule.fuelCapacity_costScaleReference;
                            int scaledCost = (int)Math.Round(rndModule.fuelCapacity_scienceCost * scaleReferenceFactor);
                            if (scaledCost < 1) scaledCost = 1;
                            scienceCost = KRnD.calculateScienceCost(scaledCost, rndModule.fuelCapacity_costScale, nextUpgrade.fuelCapacity);
                        }
                        break;
                    case "Generator":
                        {
                            upgradeFunction = KRnDGUI.UpgradeGeneratorEfficiency;
                            currentUpgradeCount = currentUpgrade.generatorEfficiency;
                            nextUpgradeCount = ++nextUpgrade.generatorEfficiency;
                            currentImprovement = KRnD.calculateImprovementFactor(rndModule.generatorEfficiency_improvement, rndModule.generatorEfficiency_improvementScale, currentUpgrade.generatorEfficiency);
                            nextImprovement = KRnD.calculateImprovementFactor(rndModule.generatorEfficiency_improvement, rndModule.generatorEfficiency_improvementScale, nextUpgrade.generatorEfficiency);
                            scienceCost = KRnD.calculateScienceCost(rndModule.generatorEfficiency_scienceCost, rndModule.generatorEfficiency_costScale, nextUpgrade.generatorEfficiency);
                        }
                        break;
                    case "Converter":
                        {
                            upgradeFunction = KRnDGUI.UpgradeConverterEfficiency;
                            currentUpgradeCount = currentUpgrade.converterEfficiency;
                            nextUpgradeCount = ++nextUpgrade.converterEfficiency;
                            currentImprovement = KRnD.calculateImprovementFactor(rndModule.converterEfficiency_improvement, rndModule.converterEfficiency_improvementScale, currentUpgrade.converterEfficiency);
                            nextImprovement = KRnD.calculateImprovementFactor(rndModule.converterEfficiency_improvement, rndModule.converterEfficiency_improvementScale, nextUpgrade.converterEfficiency);
                            scienceCost = KRnD.calculateScienceCost(rndModule.converterEfficiency_scienceCost, rndModule.converterEfficiency_costScale, nextUpgrade.converterEfficiency);
                        }
                        break;
                    case "Parachute":
                        {
                            upgradeFunction = KRnDGUI.UpgradeParachuteStrength;
                            currentUpgradeCount = currentUpgrade.parachuteStrength;
                            nextUpgradeCount = ++nextUpgrade.parachuteStrength;
                            currentImprovement = KRnD.calculateImprovementFactor(rndModule.parachuteStrength_improvement, rndModule.parachuteStrength_improvementScale, currentUpgrade.parachuteStrength);
                            nextImprovement = KRnD.calculateImprovementFactor(rndModule.parachuteStrength_improvement, rndModule.parachuteStrength_improvementScale, nextUpgrade.parachuteStrength);
                            scienceCost = KRnD.calculateScienceCost(rndModule.parachuteStrength_scienceCost, rndModule.parachuteStrength_costScale, nextUpgrade.parachuteStrength);
                        }
                        break;
                    case "Max Temp":
                        {
                            upgradeFunction = KRnDGUI.UpgradeMaxTemperature;
                            currentUpgradeCount = currentUpgrade.maxTemperature;
                            nextUpgradeCount = ++nextUpgrade.maxTemperature;
                            currentImprovement = KRnD.calculateImprovementFactor(rndModule.maxTemperature_improvement, rndModule.maxTemperature_improvementScale, currentUpgrade.maxTemperature);
                            nextImprovement = KRnD.calculateImprovementFactor(rndModule.maxTemperature_improvement, rndModule.maxTemperature_improvementScale, nextUpgrade.maxTemperature);
                            scienceCost = KRnD.calculateScienceCost(rndModule.maxTemperature_scienceCost, rndModule.maxTemperature_costScale, nextUpgrade.maxTemperature);
                        }
                        break;
                    default:
                        new Exception("unexpected option '" + selectedUpgradeOption + "'");
                        break;
                }

                String newInfo = getPartInfo(part, nextUpgrade); // Calculate part-info if the selected stat was upgraded.
                newInfo = highlightChanges(currentInfo, newInfo);

                // Current stats:
                GUILayout.BeginArea(new Rect(10 + optionsWidth + 10, 30, windowStyle.fixedWidth, 20));
                GUILayout.Label("<color=#FFFFFF><b>Current:</b> " + currentUpgradeCount.ToString() + " (" + currentImprovement.ToString("+0.##%;-0.##%;-") + ")</color>", labelStyle);
                GUILayout.EndArea();
                float areaWidth = (windowStyle.fixedWidth - 20 - optionsWidth) / 2;
                float areaHeight = optionsHeight;
                GUILayout.BeginArea(new Rect(10 + optionsWidth, 30 + 20, areaWidth, areaHeight));
                scrollPos = GUILayout.BeginScrollView(scrollPos, scrollStyle, GUILayout.Width(areaWidth), GUILayout.Height(areaHeight));

                GUILayout.Label(currentInfo, labelStyleSmall);
                GUILayout.EndScrollView();
                GUILayout.EndArea();

                // Next stats:
                GUILayout.BeginArea(new Rect(10 + optionsWidth + areaWidth + 10, 30, windowStyle.fixedWidth, 20));
                GUILayout.Label("<color=#FFFFFF><b>Next upgrade:</b> " + nextUpgradeCount.ToString() + " (" + nextImprovement.ToString("+0.##%;-0.##%;-") + ")</color>", labelStyle);
                GUILayout.EndArea();

                GUILayout.BeginArea(new Rect(10 + optionsWidth + areaWidth, 30 + 20, areaWidth, areaHeight));
                scrollPos = GUILayout.BeginScrollView(scrollPos, scrollStyle, GUILayout.Width(areaWidth), GUILayout.Height(areaHeight));
                GUILayout.Label(newInfo, labelStyleSmall);
                GUILayout.EndScrollView();
                GUILayout.EndArea();

                // Bottom-line (display only if the upgrade would have an effect):
                if (currentImprovement != nextImprovement)
                {
                    GUILayout.BeginArea(new Rect(10, windowStyle.fixedHeight - 25, windowStyle.fixedWidth, 30));
                    float currentScience = 0;
                    if (ResearchAndDevelopment.Instance != null) currentScience = ResearchAndDevelopment.Instance.Science;
                    String color = "FF0000";
                    if (currentScience >= scienceCost) color = "00FF00";
                    GUILayout.Label("<b>Science: <color=#" + color + ">" + scienceCost.ToString() + " / " + Math.Floor(currentScience).ToString() + "</color></b>", labelStyle);
                    GUILayout.EndArea();
                    if (currentScience >= scienceCost && ResearchAndDevelopment.Instance != null && upgradeFunction != null)
                    {
                        GUILayout.BeginArea(new Rect(windowStyle.fixedWidth - 110, windowStyle.fixedHeight - 30, 100, 30));
                        if (GUILayout.Button("Research", buttonStyle))
                        {
                            upgradeFunction(part);
                            ResearchAndDevelopment.Instance.AddScience(-scienceCost, TransactionReasons.RnDTechResearch);
                            for (int i = 0; i < PartLoader.LoadedPartsList.Count; i++)
                            {
                                AvailablePart ap = PartLoader.LoadedPartsList[i];
                                if (ap.name == part.partInfo.name)
                                {
                                    foreach (var m in ap.moduleInfos)
                                    {
                                        if (m.moduleName == "R&D")
                                        {
                                            AvailablePart.ModuleInfo info = m;
                                            m.info = KRnDModule.GetInfo(part);
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        GUILayout.EndArea();
                    }
                }

                GUILayout.EndVertical();
                GUI.DragWindow();
            }
            catch (Exception e)
            {
                Log.error(e, "GenerateWindow(): {0}", e);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace KRnD.Source
{
	// This class handles load- and save-operations.
	[KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.EDITOR, GameScenes.FLIGHT, GameScenes.TRACKSTATION, GameScenes.SPACECENTER)]
	[UsedImplicitly]
	internal class ScenarioModule : global::ScenarioModule
	{
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Awakes this object.</summary>
		[UsedImplicitly]
		public void Awake()
		{
		}

		public override void OnSave(ConfigNode node)
		{
			try {
				KRnDSettings.OnSave(node);

				double time = DateTime.Now.Ticks;
				var upgrade_nodes = new ConfigNode("upgrades");
				foreach (var upgrade_name in KRnD.upgrades.Keys) {
					PartUpgrades upgrade;
					if (!KRnD.upgrades.TryGetValue(upgrade_name, out upgrade)) continue;
					upgrade_nodes.AddNode(upgrade.CreateConfigNode(upgrade_name));
					//Debug.Log("[KRnD] saved: " + upgradeName + " " + upgrade.ToString());
				}

				node.AddNode(upgrade_nodes);

				time = (DateTime.Now.Ticks - time) / TimeSpan.TicksPerSecond;
				//Debug.Log("[KRnD] saved " + upgradeNodes.CountNodes.ToString() + " upgrades in " + time.ToString("0.000s"));

				var gui_settings = new ConfigNode("gui");
				gui_settings.AddValue("left", UpgradeUI.windowPosition.xMin);
				gui_settings.AddValue("top", UpgradeUI.windowPosition.yMin);
				node.AddNode(gui_settings);
			} catch (Exception e) {
				Debug.LogError("[KRnD] OnSave(): " + e);
			}
		}

		public override void OnLoad(ConfigNode node)
		{
			try {
				KRnDSettings.OnLoad(node);

				double time = DateTime.Now.Ticks;
				var upgrades_applied = 0;

				KRnD.upgrades.Clear();

				var upgrade_nodes = node.GetNode("upgrades");
				if (upgrade_nodes != null) {
					foreach (var upgrade_node in upgrade_nodes.GetNodes()) {
						var upgrade = PartUpgrades.CreateFromConfigNode(upgrade_node);
						KRnD.upgrades.Add(upgrade_node.name, upgrade);
					}

					// Update global part-list with new upgrades from the saved-game:
					upgrades_applied = KRnD.UpdateGlobalParts();

					// If we started with an active vessel, update that vessel:
					var vessel = FlightGlobals.ActiveVessel;
					if (vessel) KRnD.UpdateVessel(vessel);

					time = (DateTime.Now.Ticks - time) / TimeSpan.TicksPerSecond;
					//Debug.Log("[KRnD] retrieved and applied " + upgradesApplied.ToString() + " upgrades in " + time.ToString("0.000s"));
				}

				var gui_settings = node.GetNode("gui");
				if (gui_settings != null) {
					if (gui_settings.HasValue("left")) UpgradeUI.windowPosition.xMin = (float)double.Parse(gui_settings.GetValue("left"));
					if (gui_settings.HasValue("top")) UpgradeUI.windowPosition.yMin = (float)double.Parse(gui_settings.GetValue("top"));
				}
			} catch (Exception e) {
				Debug.LogError("[KRnD] OnLoad(): " + e);
			}
		}
	}

}

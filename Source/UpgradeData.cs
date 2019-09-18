using System;
using UnityEngine;

namespace KRnD.Source
{
	public class UpgradeData
	{
		public string name;

		//public string description;
		//public float costScale;
		public float costDivisor;
		public float improvementValue;
		//public float improvementScale;
		public int scienceCost;

		public UpgradeData(string name_str, float cost_divisor, float improve_value, int science_cost)
		{
			name = name_str;
			//description = desc_str;
			//costScale = cost_scale;
			costDivisor = cost_divisor;
			improvementValue = improve_value;
			//improvementScale = improvement_scale;
			scienceCost = science_cost;
		}



		public float CalculateImprovementFactor(int upgrades)
		{
			float factor = 1;
			if (upgrades < 0) upgrades = 0;
			for (var i = 0; i < upgrades; i++) {
				factor += improvementValue * (float)Math.Pow(KRnDSettings.improvementRate, i);
			}

			/*
			 * Improvement is clamped at a limit of 10% to 400% of original value.
			 */
			if (factor < 0.1) factor = 0.1f;
			if (factor > 4) factor = 4.0f;
			return factor;
		}

		public int CalculateScienceCost(float original_stat, int upgrades)
		{
			float cost_total = 0;
			float cost_base = scienceCost * costDivisor > 0 ? (original_stat / costDivisor) : 1;
			if (upgrades < 0) upgrades = 0;
			for (var i = 0; i < upgrades; i++) {
				cost_total += cost_base * (float)Math.Pow(KRnDSettings.costRate, i);
			}

			/*
			 * Cost is clamped between 1 and max signed int. Science point cost is always a whole number.
			 */
			if (cost_total < 1) cost_total = 1;
			if (cost_total > Int32.MaxValue) return Int32.MaxValue; // Cap at signed 32 bit int
			return (int)Math.Round(cost_total);
		}



		public void SaveToNode(ConfigNode node)
		{
			var data_node = new ConfigNode(name);
			data_node.SetValue(Constants.SCIENCE_COST, scienceCost, true);
			data_node.SetValue(Constants.COST_DIVISOR, costDivisor, true);
			data_node.SetValue(Constants.IMPROVEMENT, improvementValue, true);
			node.AddNode(data_node);
		}

		public void LoadFromNode(ConfigNode node)
		{
			try {
				var data_node = node.GetNode(name);
				data_node.TryGetValue(Constants.COST_DIVISOR, ref costDivisor);
				data_node.TryGetValue(Constants.IMPROVEMENT, ref improvementValue);
				data_node.TryGetValue(Constants.SCIENCE_COST, ref scienceCost);
			} catch (Exception e) {
				Debug.LogError("[KRnD] LoadFromNode(): " + e);
			}
		}
	}
}
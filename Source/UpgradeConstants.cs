using System;
using UnityEngine;

namespace KRnD.Source
{
	public class UpgradeConstants
	{
		public string name;

		public float costDivisor;
		public float improvementValue;
		public int scienceCost;

		public UpgradeConstants(string name_str, float cost_divisor, float improve_value, int science_cost)
		{
			name = name_str;
			costDivisor = cost_divisor;
			improvementValue = improve_value;
			scienceCost = science_cost;
		}

		public float CalculateImprovementValue(float value, int upgrades)
		{
			value *= CalculateImprovementFactor(upgrades);
			return value;
		}


		public float CalculateImprovementFactor(int upgrades)
		{
			float factor = 1;
			if (upgrades < 0) upgrades = 0;
			for (var i = 0; i < upgrades; i++) {
				factor += improvementValue * (float)Math.Pow(InitConstants.improvementRate, i);
			}

			/*
			 * Improvement is clamped at a limit of 10% to 400% of original value, typically.
			 */
			return Mathf.Clamp(factor, InitConstants.minFactor, InitConstants.maxFactor);
		}

		public int CalculateScienceCost(float original_stat, int upgrades)
		{
			float cost_total = 0;
			float cost_base = scienceCost * costDivisor > 0 ? (original_stat / costDivisor) : 1;
			if (upgrades < 0) upgrades = 0;
			for (var i = 0; i < upgrades; i++) {
				cost_total += cost_base * (float)Math.Pow(InitConstants.costRate, i);
			}

			/*
			 * Cost is clamped between 1 and max signed int. Science point cost is always a whole number.
			 */
			if (cost_total < 1) cost_total = 1;
			if (cost_total > int.MaxValue) return int.MaxValue; // Cap at signed 32 bit int
			return (int)Math.Round(cost_total);
		}



		public void SaveUpgradeData(ConfigNode node)
		{
			var data_node = new ConfigNode(name);
			data_node.SetValue(StringConstants.SCIENCE_COST, scienceCost, true);
			data_node.SetValue(StringConstants.COST_DIVISOR, costDivisor, true);
			data_node.SetValue(StringConstants.IMPROVEMENT, improvementValue, true);
			node.AddNode(data_node);
		}

		public void LoadUpgradeData(ConfigNode node)
		{
			/*
			 * There is a possibility that the data does not exist in the saved-game file. This is true
			 * when this mod is added to an already existing game. In such a case, it will fall back to
			 * using the defaults.
			 */
			try {
				var data_node = node.GetNode(name);
				data_node.TryGetValue(StringConstants.COST_DIVISOR, ref costDivisor);
				data_node.TryGetValue(StringConstants.IMPROVEMENT, ref improvementValue);
				data_node.TryGetValue(StringConstants.SCIENCE_COST, ref scienceCost);
			} catch (Exception e) {
				Debug.LogError("[KRnD] LoadFromNode(): " + e);
			}
		}
	}
}
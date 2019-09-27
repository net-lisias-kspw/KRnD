using System;
using UnityEngine;

namespace KRnD.Source
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary> A class that manages the algorithm of updating a stat through the upgrade process. This class
	/// 		  contains coefficients that control the upgrade calculation. There is one of these for every stat
	/// 		  that can be modified.</summary>
	public class UpgradeConstants
	{
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> The name of the stat that this class is associated with. This name is used as the node name in
		/// 		  the mod config file.</summary>
		public string name;

		public float costDivisor;
		public float improvementValue;
		public int scienceCost;
		public bool rounded;

		public Func<PartUpgrades, int> upgradeFunction;

		public Func<UpgradeConstants, Part, PartStats, int, int> applyUpgradeFunction;


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> The initializing constructor for the UpgradeConstants class.</summary>
		///
		/// <param name="name_str">		    The name string as unique identifier for this upgrade.</param>
		/// <param name="cost_divisor">	    The cost divisor for upgrades that scale by initial stat value.</param>
		/// <param name="improve_value">    The improve value scaling factor.</param>
		/// <param name="science_cost">	    The science cost base factor.</param>
		/// <param name="round">		    True to round the upgrade stat value to nearest whole number.</param>
		/// <param name="upgrade_function"> (Optional) The upgrade function.</param>
		/// <param name="apply_upgrade">    (Optional) The apply upgrade.</param>
		public UpgradeConstants(string name_str, float cost_divisor, float improve_value, int science_cost, bool round, Func<PartUpgrades, int> upgrade_function = null, Func<UpgradeConstants, Part, PartStats, int, int> apply_upgrade = null)
		{
			name = name_str;
			costDivisor = cost_divisor;
			improvementValue = improve_value;
			scienceCost = science_cost;
			rounded = round;
			upgradeFunction = upgrade_function;
			applyUpgradeFunction = apply_upgrade;
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Calculates the improvement value for floats</summary>
		///
		/// <param name="value">    The value.</param>
		/// <param name="upgrades"> The upgrade level to calculate the coefficient for.</param>
		///
		/// <returns> The calculated improvement value.</returns>
		public float CalculateImprovementValue(float value, int upgrades)
		{
			value *= CalculateImprovementFactor(upgrades);
			if (rounded) value = (float)Math.Round(value);
			return value;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Calculates the improvement value for doubles</summary>
		///
		/// <param name="value">    The value.</param>
		/// <param name="upgrades"> The upgrade level to calculate the coefficient for.</param>
		///
		/// <returns> The calculated improvement value.</returns>
		public double CalculateImprovementValue(double value, int upgrades)
		{
			value *= CalculateImprovementFactor(upgrades);
			if (rounded) value = Math.Round(value);
			return value;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Calculates the improvement value for ints</summary>
		///
		/// <param name="value">    The value.</param>
		/// <param name="upgrades"> The upgrade level to calculate the coefficient for.</param>
		///
		/// <returns> The calculated improvement value.</returns>
		public int CalculateImprovementValue(int value, int upgrades)
		{
			value = (int)(value * CalculateImprovementFactor(upgrades));
			return value;
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Calculates the improvement coefficient to multiply by the original stat value for the specified
		/// 		  upgrade level.</summary>
		///
		/// <param name="upgrades"> The upgrade level to calculate the coefficient for.</param>
		///
		/// <returns> The calculated improvement coefficient that should be multiplied by the original part stat to
		/// 		  result in the upgraded stat value.</returns>
		public float CalculateImprovementFactor(int upgrades)
		{
			float factor = 1;
			if (upgrades < 0) upgrades = 0;
			for (var i = 0; i < upgrades; i++) {
				factor += improvementValue * (float)Math.Pow(ValueConstants.improvementRate, i);
			}

			/*
			 * Improvement is clamped at a limit of 10% to 400% of original value, typically.
			 */
			return Mathf.Clamp(factor, ValueConstants.minFactor, ValueConstants.maxFactor);
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Calculates the science cost to increase the upgrade level to the level specified presuming it has
		/// 		  already been upgraded to the prior level.</summary>
		///
		/// <param name="original_stat"> The original value of the stat being upgraded. Some upgrades scale according
		/// 							 to this stat.</param>
		/// <param name="upgrades">		 The upgrade level to calculate for.</param>
		///
		/// <returns> The calculated science cost to upgrade the stat to the specified upgrade level from the previous
		/// 		  level.</returns>
		public int CalculateScienceCost(float original_stat, int upgrades)
		{
			float cost_total = 0;
			float cost_base = scienceCost * (costDivisor > 0 ? (original_stat / costDivisor) : 1);
			cost_base = Math.Max(cost_base, 1);
			if (upgrades < 0) upgrades = 0;
			for (var i = 0; i < upgrades; i++) {
				cost_total += cost_base * (float)Math.Pow(ValueConstants.costRate, i);
			}

			/*
			 * Cost is clamped between 1 and max signed int. Science point cost is always a whole number.
			 */
			if (cost_total < 1) cost_total = 1;
			if (cost_total > int.MaxValue) return int.MaxValue; // Cap at signed 32 bit int
			return (int)Math.Round(cost_total);
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Saves this class to the ConfigNode specified.</summary>
		///
		/// <param name="node"> The node.</param>
		public void SaveUpgradeData(ConfigNode node)
		{
			var data_node = new ConfigNode(name);
			data_node.SetValue(StringConstants.SCIENCE_COST, scienceCost, true);
			data_node.SetValue(StringConstants.COST_DIVISOR, costDivisor, true);
			data_node.SetValue(StringConstants.IMPROVEMENT, improvementValue, true);
			data_node.SetValue(StringConstants.ROUNDED, rounded, true);
			node.AddNode(data_node);
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Loads this class from the ConfigNode specified.</summary>
		///
		/// <param name="node"> The node.</param>
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
				data_node.TryGetValue(StringConstants.ROUNDED, ref rounded);
			} catch (Exception e) {
				Debug.LogError("[KRnD] LoadUpgradeData(" + name + "): " + e);
			}
		}
	}
}
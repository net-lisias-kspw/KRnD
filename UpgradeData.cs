namespace KRnD
{
	public class UpgradeData
	{
		public string name;
		public float costScale;
		public float costScaleReference;
		public float improvement;
		public float improvementScale;
		public int scienceCost;

		public UpgradeData(string name_str, float cost_scale, float cost_reference, float improve_value, float scale_value, int science_cost)
		{
			name = name_str;
			costScale = cost_scale;
			costScaleReference = cost_reference;
			improvement = improve_value;
			improvementScale = scale_value;
			scienceCost = science_cost;
		}


		public void SaveToNode(ConfigNode node)
		{
			var data_node = new ConfigNode(name);
			data_node.SetValue(Constants.COST_SCALE, costScale, true);
			data_node.SetValue(Constants.COST_SCALE_REFERENCE, costScaleReference, true);
			data_node.SetValue(Constants.IMPROVEMENT, improvement, true);
			data_node.SetValue(Constants.IMPROVEMENT_SCALE, improvementScale, true);
			data_node.SetValue(Constants.SCIENCE_COST, scienceCost, true);
			node.AddNode(data_node);
		}

		public void LoadFromNode(ConfigNode node)
		{
			var data_node = node.GetNode(name);
			node.TryGetValue(Constants.COST_SCALE, ref costScale);
			node.TryGetValue(Constants.COST_SCALE_REFERENCE, ref costScaleReference);
			node.TryGetValue(Constants.IMPROVEMENT, ref improvement);
			node.TryGetValue(Constants.IMPROVEMENT_SCALE, ref improvementScale);
			node.TryGetValue(Constants.SCIENCE_COST, ref scienceCost);
		}
	}
}
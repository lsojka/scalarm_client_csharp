using System;
using System.Collections.Generic;

namespace Scalarm
{
	public class SimulationParams
	{
		public ValuesMap Input { get; set; }
		public ValuesMap Output { get; set; }

		public static implicit operator SimulationParams(KeyValuePair<ValuesMap, ValuesMap> pair)
		{
			return new SimulationParams(pair.Key, pair.Value);
		}

		public SimulationParams(ValuesMap input, ValuesMap output)
		{
			Input = input;
			Output = output;
		}
	}
}


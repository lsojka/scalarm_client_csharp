using NUnit.Framework;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace Scalarm
{
	[TestFixture()]
	public class SimulationParamsTest
	{
		[TestCase()]
		public void TestCastFromPair() {
			var v1 = new ValuesMap () {
				{"a", 1}
			};
			var v2 = new ValuesMap () {
				{"b", 2}
			};
			var pair = new KeyValuePair<ValuesMap, ValuesMap>(v1, v2);

			SimulationParams converted = (SimulationParams)pair;

			Assert.AreEqual(1, converted.Input["a"]);
			Assert.AreEqual(2, converted.Output["b"]);

			var list = new List<KeyValuePair<ValuesMap, ValuesMap>> () {
				new KeyValuePair<ValuesMap, ValuesMap>(v1, v2)
			};

			Assert.AreEqual(1, list.Select(p => (SimulationParams)p).ToList()[0].Input["a"]);
		}
	}
}


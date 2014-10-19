using NUnit.Framework;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace Scalarm
{
	[TestFixture()]
	public class ExperimentTest
	{
//		[Test()]
//		public void TestSplitParametersAndResults()
//		{
//			var results = new List<ValuesMap> () {
//				new ValuesMap() {
//					{"a", "1"},
//					{"b", "2"},
//					{"c", "3"},
//					{"d", "4"},
//				}
//			};
//
//			var parameters = new List<string> () { "a", "b" };
//
//			var splitted = Experiment.SplitParametersAndResults(results, parameters);
//
////			Console.WriteLine(JsonConvert.SerializeObject(x));
//
////			foreach (var k in splitted[0].Keys) {
////				Console.WriteLine(JsonConvert.SerializeObject(k));
////			}
//
////			Assert.AreEqual("3", splitted [new Dictionary<string, string> () {
////				{"a", "1"},
////				{"b", "2"}
////			}] ["c"]);
//
//			Console.WriteLine(splitted.ToList()[0].Key);
//			Console.WriteLine(splitted.ToList()[0].Value);
//
//			Assert.AreEqual("3", splitted [new ValuesMap () { {"a", "1"}, {"b", "2"} }]["c"]);
//
////			Assert.AreEqual(new Dictionary<ValuesMap, ValuesMap> () {
////				{
////					new ValuesMap() {
////						{"a", "1"},
////						{"b", "2"}
////					},
////					new ValuesMap() {
////						{"c", "3"},
////						{"d", "4"}
////					}
////				}
////				}, splitted);
//		}

		[TestCase()]
		public void TestConvertTypes() {
			var results = new List<ValuesMap>() {
				new ValuesMap() { {"a", "1"}, {"b", "2.0"} }
			};

			var convertedResults = Experiment.ConvertTypes(results);

			Assert.AreEqual(1, convertedResults [0] ["a"]);
			Assert.AreEqual(2.0, convertedResults [0] ["b"]);
		}

		[TestCase()]
		public void TestCastToSimulationParams() {

		}
	}
}


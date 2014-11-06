using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace Scalarm
{
	[TestFixture]
	public class ClientTest
	{
		public ClientTest()
		{
		}

		[Test]
		public void ParseExperimentsConfigurationsCsv()
		{
			var csv = @"param1,param2,moe1,moe2
1,2,3,4
5,6,7,8
9,10,11,12
";

			var results = Client.ParseExperimentsConfigurationsCsv(csv);

			Assert.AreEqual(3, results.Count);

            Console.WriteLine(results[0]);

			Assert.AreEqual(new ValuesMap() {
				{"param1", "1"},
				{"param2", "2"},
				{"moe1", "3"},
				{"moe2", "4"}
			},
			                results [0]
			);

			Assert.AreEqual(new ValuesMap() {
				{"param1", "5"},
				{"param2", "6"},
				{"moe1", "7"},
				{"moe2", "8"}
			},
			                results [1]
			);

			Assert.AreEqual(new Dictionary<string, string>() {
				{"param1", "9"},
				{"param2", "10"},
				{"moe1", "11"},
				{"moe2", "12"}
			},
			results [2]
			);
		}
	}
}


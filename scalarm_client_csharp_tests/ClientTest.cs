using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using Scalarm;

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

		[Test]
		public void PrepareStringForHeader()
		{
			var originalString = @"one
two
three
";
			var shouldBeString = "one\\r\\ntwo\\r\\nthree\\r\\n";

			Assert.AreEqual(shouldBeString, Client.PrepareStringForHeader(originalString));
		}

		[Test]
		public void ToRubyFormat()
		{
			Assert.AreEqual("1", Client.ToRubyFormat(1.0));
			Assert.AreEqual("1", Client.ToRubyFormat(1));
			Assert.AreEqual("hello", Client.ToRubyFormat("hello"));

			Assert.AreEqual("1.2", Client.ToRubyFormat(1.2));
			Assert.AreEqual("262", Client.ToRubyFormat(262.0f));
			Assert.AreEqual("262.4", Client.ToRubyFormat(262.4f));
		}

		[Test]
		public void CreateCsvFromPoints()
		{
			var keys = new List<string> {"a", "b"};
			var values = new List<ValuesMap> {
				new ValuesMap() {
					{"parameter1", 1.5},
					{"parameter2", 3}
				},
				new ValuesMap() {
					{"parameter1", 2.0},
					{"parameter2", 4.1}
				},
			};

			string expectedCsv = @"a,b
1.5,3
2,4.1
";

			string csv = Client.CreateCsvFromPoints(keys, values);

			Assert.AreEqual(
				expectedCsv,
				csv
			);
		}
	}
}


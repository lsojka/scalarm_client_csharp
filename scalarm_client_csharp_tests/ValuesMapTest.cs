using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Scalarm
{
	[TestFixture()]
	public class ValuesMapTest
	{
		[Test()]
		public void JsonOrderTest()
		{
			var vmp = new ValuesMap () {
				{"b", "2"},
				{"a", "1"}
			};

			Assert.AreEqual(@"{""a"":""1"",""b"":""2""}", vmp.ToJson());
		}

		[Test()]
		public void HashNotEqualsTest() {

		}

		[Test()]
		public void HashEqualsTest()
		{
			var vmp1 = new ValuesMap () {
				{"b", "2"},
				{"a", "1"}
			};

			var vmp2 = new ValuesMap () {
				{"a", "1"},
				{"b", "2"}
			};

			Assert.AreEqual(vmp1.GetHashCode(), vmp2.GetHashCode());
			Assert.AreEqual(vmp1, vmp2);
			Assert.AreEqual(vmp1, vmp1.ShallowCopy());
		}

		[Test()]
		public void AsKeyTest()
		{
			var vmp1 = new ValuesMap () {
				{"b", "2"},
				{"a", "1"}
			};

			var vmp2 = new ValuesMap () {
				{"c", "3"},
				{"d", "4"}
			};

			var dict = new Dictionary<ValuesMap, int> () {
				{vmp1, 1},
				{vmp2, 2}
			};

			Assert.AreEqual(1, dict[vmp1.ShallowCopy()]);
			Assert.AreEqual(2, dict[vmp2.ShallowCopy()]);
			Assert.AreNotEqual(1, dict[vmp2.ShallowCopy()]);
		}
	}
}


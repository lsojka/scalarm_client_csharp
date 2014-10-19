using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace Scalarm
{
	public class ValuesMap : SortedDictionary<string, object>
	{
		public ValuesMap()
		{
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}

		public override string ToString()
		{
			return ToJson();
		}

		public override int GetHashCode()
		{
			return string.Join(",", Values.Select(x => x.ToString())).GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return obj.GetType() == this.GetType() && obj.GetHashCode() == this.GetHashCode();
		}

		public ValuesMap ShallowCopy()
		{
			var vmp = new ValuesMap();
			foreach (var obj in this) {
				vmp.Add(obj.Key, obj.Value);
			}

			return vmp;
		}
	}
}


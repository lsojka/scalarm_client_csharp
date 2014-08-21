using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Scalarm
{
	namespace ExperimentInput
	{
		public sealed class ParametrizationType {
		    public static readonly string VALUE = "value";
			public static readonly string RANGE = "range";
			public static readonly string GAUSS = "gauss";
			public static readonly string UNIFORM = "uniform";
		}
		
		// TODO: change sealed class to enum
	//	public enum ParametrizationType
	//	{
	//		[JsonProperty(PropertyName = "value")]
	//		VALUE,
	//		
	//		[JsonProperty(PropertyName = "range")]
	//		RANGE,
	//		
	//		[JsonProperty(PropertyName = "gauss")]
	//		GAUSS,
	//		
	//		[JsonProperty(PropertyName = "uniform")]
	//		UNIFORM
	//	}
		
        public sealed class Type {
            public static readonly string INTEGER = "integer";
            public static readonly string FLOAT = "float";
            public static readonly string STRING = "string";
        }

		public class Parameter
		{
			[JsonProperty(PropertyName = "id")]
			public string Id {get; set;}
			
			[JsonProperty(PropertyName = "label", NullValueHandling = NullValueHandling.Ignore)]
			public string Label {get; set;}
			
			[JsonProperty(PropertyName = "parametrization_type", NullValueHandling = NullValueHandling.Ignore)]
			public string ParametrizationType {get; set;}
			
            [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
            public string Type {get; set;}

            //  TODO: float?

            [JsonProperty(PropertyName = "min", NullValueHandling = NullValueHandling.Ignore)]
            public float Min {get; set;}

            [JsonProperty(PropertyName = "max", NullValueHandling = NullValueHandling.Ignore)]
            public float Max {get; set;}

			public Parameter(string id, string label=null)
			{
				Id = id;
				Label = label;
			}
		}
		
		public class Entity
		{
			[JsonProperty(PropertyName = "id")]
			public string Id {get; set;}
			
			[JsonProperty(PropertyName = "label", NullValueHandling = NullValueHandling.Ignore)]
			public string Label {get; set;}
			
			[JsonProperty(PropertyName = "parameters")]
			public List<Parameter> Parameters = new List<Parameter>();
			
			public Entity(string id, string label=null)
			{
				Id = id;
				Label = label;
				Parameters = new List<Parameter>();
			}
		}
		
		public class Category
		{
			[JsonProperty(PropertyName = "id")]
			public string Id {get; set;}
				
			[JsonProperty(PropertyName = "label", NullValueHandling = NullValueHandling.Ignore)]
			public string Label {get; set;}
			
			[JsonProperty(PropertyName = "entities")]
			public List<Entity> Entities = new List<Entity>();
		
            public Category()
            {
                Entities = new List<Entity>();
            }

			public Category(string id, string label=null) : this()
			{
				Id = id;
				Label = label;
			}
		}
		
		public class InputDefinition
		{		
			private List<Category> _groups = new List<Category>();
			
			[JsonProperty(PropertyName = "categories")]
			public List<Category> Categories { get {return _groups;} }
			
			public string ToJSON()
			{
				return JsonConvert.SerializeObject(Categories);
			}
			
	//		public Category GetCategory(string id)
	//		{
	//			return Categories.FirstOrDefault(g => g.Id == id);
	//		}
			
	//		public static InputDefinition CreateSimpleExperimentInput()
	//		{
	//			Input ei = new tInput();
	//			var g = new Category("_");
	//			var e = new Entity("_");
	//			ei.Categories.Add(g);
	//			ei.Categories.
	//		}
			
			// TODO: parse from file
			// TODO: parse from string
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Datastructures;

namespace temporalsmithing.util;

public static class Extensions {

	#nullable enable
	public static T? AccessSafe<T>(this object[] array, int index) where T : notnull =>
		index >= 0 && index < array.Length && array[index] is T ? (T)array[index] : default;
	#nullable disable

	public static JsonObject Append(this JsonObject obj, string key, object value) {
		obj.Token[key] = JToken.FromObject(value);
		return obj;
	}

	public static JsonObject Append(this JsonObject obj, string key, JsonObject value) {
		obj.Token[key] = value.Token;
		return obj;
	}

	public static JsonObject Append(this JsonObject obj, string key, object[] value) {
		obj.Token[key] = new JArray(value);
		return obj;
	}

	public static Dictionary<T1, T2> Append<T1, T2>(this Dictionary<T1, T2> dict, T1 key, T2 val) {
		dict[key] = val;
		return dict;
	}

	public static T2 GetOrDefault<T1, T2>(this Dictionary<T1, T2> dict, T1 key, T2 defValue = default) {
		return dict.TryGetValue(key, out var value) ? value : defValue;
	}

	public static IEnumerable<T2> GetOrDefault<T1, T2>(this Dictionary<T1, T2> dict,
													   Func<KeyValuePair<T1, T2>, bool> key, T2 defValue = default) {
		var val = dict.Where(key).Select(x => x.Value).ToArray();
		return val.Length > 0 ? val : new[] { defValue };
	}

}
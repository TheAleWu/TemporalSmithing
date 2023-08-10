using System;
using System.Text;
using Newtonsoft.Json.Linq;
using temporalsmithing.content.modifier;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace temporalsmithing.item.modifier;

public class ModifierItem : Item {

	protected ModifierItem() { }

	public string Key { get; private set; }
	public string Group { get; private set; }
	public JsonObject Data { get; private set; }

	public override void OnLoaded(ICoreAPI api) {
		if (Attributes is null) throw new Exception("ModifierItems require attributes but " + Code + " does not!");
		var keyObj = Attributes["key"];
		if (!keyObj.Exists) throw new Exception("ModifierItems require a 'key' attribute but " + Code + " does not!");
		Key = keyObj.AsString();
		var groupObj = Attributes["group"];
		if (groupObj.Exists) Group = groupObj.AsString();
		var dataObj = Attributes["data"];
		if (dataObj.Exists) Data = dataObj;
		else Data = new JsonObject(new JObject());

		base.OnLoaded(api);
	}

	public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo) {
		base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);

		var mod = Modifiers.Instance.GetModifier(Key);
		mod?.WriteDescription(inSlot.Itemstack, dsc);
	}

	public T GetData<T>(string key, T defaultValue = default) where T : notnull {
		if (Data is null || !Data.KeyExists(key)) return defaultValue;
		return Data[key].AsObject<T>();
	}

	public JsonObject GetData(string key, JsonObject defaultValue = default) {
		if (Data is null || !Data.KeyExists(key)) return defaultValue;
		return Data[key];
	}

	public virtual object[] GetHandbookDescriptionArguments() => Array.Empty<object>();

}
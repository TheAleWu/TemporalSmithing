using System;
using System.Collections.Generic;
using System.Linq;
using temporalsmithing.content.modifier.impl;
using temporalsmithing.item.modifier;
using temporalsmithing;
using temporalsmithing.content.modifier;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace temporalsmithing.content.modifier;

public class Modifiers {

	public static readonly Modifiers Instance = new();
	private readonly Dictionary<string, Modifier> registry = new();
	private readonly Dictionary<string, KeyValuePair<int, List<ModifierEntry>>> cache = new();

	private Modifiers() {
		Register(ModifierUnknown.Instance);

		Register(new ModifierRipping());
		Register(new ModifierYield());
		Register(new ModifierHardening());

		Register("temporal-infusion", new ModifierTemporalInfusion());
	}

	private void Register(Modifier mod) {
		registry.Add(mod.GetKey(), mod);
	}

	private void Register(string key, Modifier mod) {
		registry.Add(key, mod);
	}

	public Modifier GetModifier(string key) {
		return key != null && registry.TryGetValue(key, out var value) ? value : ModifierUnknown.Instance;
	}

	public Modifier GetModifier(ItemStack stack) {
		Modifier result = ModifierUnknown.Instance;
		if (stack?.Item is RuneItem mi && registry.TryGetValue(mi.Key, out var value))
			result = value;
		return result;
	}

	public string GetModifierKey(ItemStack stack) {
		return stack?.Item is RuneItem mi ? mi.Key : "unknown";
	}

	public bool IsValidModifier(ItemStack stack) {
		return GetModifier(stack) is not ModifierUnknown;
	}

	public int GetUnlockedSlots(ItemStack stack) {
		return stack?.Attributes?.GetInt(UnlockingModifier.UnlockedSlotsKey) ?? 0;
	}

	public Dictionary<string, List<ModifierEntry>> GroupAppliedModifiers(IEnumerable<ModifierEntry> entries) {
		var filtered = entries
		   .Where(x => x.SourceItem?.Item is RuneItem { Group: not null });
		var result = new Dictionary<string, List<ModifierEntry>>();
		foreach (var entry in filtered) {
			var group = (entry.SourceItem.Item as RuneItem)?.Group;
			if (group is null) continue;

			List<ModifierEntry> list = null;
			if (result.TryGetValue(group, out var value)) list = value;
			if (list is null) {
				list = new List<ModifierEntry>();
				result.Add(group, list);
			}

			list.Add(entry);
		}

		return result;
	}

	public List<ModifierEntry> ReadAppliedModifiers(ItemStack stack) {
		var uid = stack?.Attributes?.GetString("uid");
		if (uid is null) return ReadAppliedModifiersInternal(stack);

		var lastUpdate = stack.Attributes.GetInt("lastModUpdate", -1);
		var update = !cache.ContainsKey(uid) || lastUpdate == -1 || cache[uid].Key < lastUpdate ||
					 cache[uid].Value.Any(x => x.SourceItem?.Item is null);

		if (update)
			cache[uid] = new KeyValuePair<int, List<ModifierEntry>>(lastUpdate, ReadAppliedModifiersInternal(stack));

		return cache[uid].Value;
	}

	public void PerformOnSlots(IEnumerable<ItemSlot> slots, Action<ModifierEntry> action) {
		foreach (var slot in slots) {
			var modifiers = ReadAppliedModifiers(slot?.Itemstack);
			foreach (var entry in modifiers) {
				action.Invoke(entry);
			}
		}
	}

	public static IEnumerable<ItemSlot> GetModifiableSlots(Entity entity) {
		var slots = new List<ItemSlot>();
		if (entity is not EntityPlayer ep) return slots;

		var invManager = ep.Player?.InventoryManager;
		if (invManager is null) return slots;

		slots.Add(invManager.ActiveHotbarSlot);
		slots.AddRange(ep.GearInventory);
		return slots;
	}

	public T PerformOnSlots<T>(IEnumerable<ItemSlot> slots, T initial, Vintagestory.API.Common.Func<ModifierEntry, T, T> action) {
		var val = initial;
		foreach (var slot in slots) {
			var modifiers = ReadAppliedModifiers(slot?.Itemstack);
			foreach (var entry in modifiers) {
				val = action.Invoke(entry, val);
			}
		}

		return val;
	}

	private List<ModifierEntry> ReadAppliedModifiersInternal(IItemStack stack) {
		List<ModifierEntry> mods = new();
		var modifiers = stack?.Attributes?.GetTreeAttribute("modifiers");
		if (modifiers == null) return mods;

		foreach (var pair in modifiers) {
			if (pair.Value is not ITreeAttribute data) continue;
			var mod = GetModifier(data.GetString("key"));
			if (mod is ModifierUnknown) continue;

			var modId = pair.Key;
			var attr = data.GetTreeAttribute("data");
			var sourceItem = data.GetItemstack("source");
			if (sourceItem is null || TemporalSmithing.Instance?.ClientApi?.World is null) continue;
			sourceItem.ResolveBlockOrItem(TemporalSmithing.Instance.ClientApi.World);
			mods.Add(new ModifierEntry(modId, mod, sourceItem, attr));
		}

		return mods;
	}

}

internal class ModifierUnknown : Modifier {

	public static readonly ModifierUnknown Instance = new();

	private ModifierUnknown() { }

	public override string GetKey() {
		return "unknown";
	}

	public override string GetIconKey() {
		return "question";
	}

}

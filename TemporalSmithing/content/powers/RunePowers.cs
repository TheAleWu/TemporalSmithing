using System;
using System.Collections.Generic;
using System.Linq;
using temporalsmithing.content.modifier.impl;
using temporalsmithing.item.modifier;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace temporalsmithing.content.modifier;

public class RunePowers {

	public static readonly RunePowers Instance = new();
	private readonly Dictionary<string, RunePower> registry = new();
	private readonly Dictionary<string, KeyValuePair<int, List<RunePowerEntry>>> cache = new();

	private RunePowers() {
		Register(RunePowerUnknown.Instance);

		Register(new RunePowerRipping());
		Register(new RunePowerYield());
		Register(new RunePowerHardening());

		Register("temporal-infusion", new RunePowerTemporalInfusion());
	}

	private void Register(RunePower mod) {
		registry.Add(mod.GetKey(), mod);
	}

	private void Register(string key, RunePower mod) {
		registry.Add(key, mod);
	}

	public RunePower GetModifier(string key) {
		return key != null && registry.TryGetValue(key, out var value) ? value : RunePowerUnknown.Instance;
	}

	public RunePower GetModifier(ItemStack stack) {
		RunePower result = RunePowerUnknown.Instance;
		if (stack?.Item is RuneItem mi && registry.TryGetValue(mi.Key, out var value))
			result = value;
		return result;
	}

	public string GetModifierKey(ItemStack stack) {
		return stack?.Item is RuneItem mi ? mi.Key : "unknown";
	}

	public bool IsValidModifier(ItemStack stack) {
		return GetModifier(stack) is not RunePowerUnknown;
	}

	public int GetUnlockedSlots(ItemStack stack) {
		return stack?.Attributes?.GetInt(UnlockingRunePower.UnlockedSlotsKey) ?? 0;
	}

	public Dictionary<string, List<RunePowerEntry>> GroupAppliedModifiers(IEnumerable<RunePowerEntry> entries) {
		var filtered = entries
		   .Where(x => x.SourceItem?.Item is RuneItem { Group: not null });
		var result = new Dictionary<string, List<RunePowerEntry>>();
		foreach (var entry in filtered) {
			var group = (entry.SourceItem.Item as RuneItem)?.Group;
			if (group is null) continue;

			List<RunePowerEntry> list = null;
			if (result.TryGetValue(group, out var value)) list = value;
			if (list is null) {
				list = new List<RunePowerEntry>();
				result.Add(group, list);
			}

			list.Add(entry);
		}

		return result;
	}

	public List<RunePowerEntry> ReadAppliedModifiers(ItemStack stack) {
		var uid = stack?.Attributes?.GetString("uid");
		if (uid is null) return ReadAppliedModifiersInternal(stack);

		var lastUpdate = stack.Attributes.GetInt("lastModUpdate", -1);
		var update = !cache.ContainsKey(uid) || lastUpdate == -1 || cache[uid].Key < lastUpdate ||
					 cache[uid].Value.Any(x => x.SourceItem?.Item is null);

		if (update)
			cache[uid] = new KeyValuePair<int, List<RunePowerEntry>>(lastUpdate, ReadAppliedModifiersInternal(stack));

		return cache[uid].Value;
	}

	public void PerformOnSlots(IEnumerable<ItemSlot> slots, Action<RunePowerEntry> action) {
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

	public T PerformOnSlot<T>(ItemSlot slot, T initial, Vintagestory.API.Common.Func<RunePowerEntry, T, T> action,
							  bool duplicate = true) {
		var val = initial;
		var modifiers = ReadAppliedModifiers(slot?.Itemstack);
		var duplicates = new List<string>();
		foreach (var entry in modifiers.Where(entry => !duplicate || !duplicates.Contains(entry.RunePower.GetKey()))) {
			val = action.Invoke(entry, val);
			duplicates.Add(entry.RunePower.GetKey());
		}

		return val;
	}

	public T PerformOnSlots<T>(IEnumerable<ItemSlot> slots, T initial,
							   Vintagestory.API.Common.Func<RunePowerEntry, T, T> action, bool duplicate = true) {
		return slots.Aggregate(initial, (current, slot) => PerformOnSlot(slot, current, action, duplicate));
	}

	private List<RunePowerEntry> ReadAppliedModifiersInternal(IItemStack stack) {
		List<RunePowerEntry> mods = new();
		var modifiers = stack?.Attributes?.GetTreeAttribute("modifiers");
		if (modifiers == null) return mods;

		foreach (var pair in modifiers) {
			if (pair.Value is not ITreeAttribute data) continue;
			var mod = GetModifier(data.GetString("key"));
			if (mod is RunePowerUnknown) continue;

			var modId = pair.Key;
			var attr = data.GetTreeAttribute("data");
			var sourceItem = data.GetItemstack("source");
			if (sourceItem is null || TemporalSmithing.Instance?.ClientApi?.World is null) continue;
			sourceItem.ResolveBlockOrItem(TemporalSmithing.Instance.ClientApi.World);
			mods.Add(new RunePowerEntry(modId, mod, sourceItem, attr));
		}

		return mods;
	}

}

internal class RunePowerUnknown : RunePower {

	public static readonly RunePowerUnknown Instance = new();

	private RunePowerUnknown() { }

	public override string GetKey() {
		return "unknown";
	}

	public override string GetIconKey() {
		return "question";
	}

}

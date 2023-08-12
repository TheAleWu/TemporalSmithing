using System;
using System.Collections.Generic;
using System.Linq;
using temporalsmithing.content.modifier.impl;
using temporalsmithing.item.modifier;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace temporalsmithing.content.modifier;

public class RuneService {

	public static readonly RuneService Instance = new();
	private readonly Dictionary<string, Rune> registry = new();
	private readonly Dictionary<string, KeyValuePair<int, List<AppliedRune>>> cache = new();

	private RuneService() {
		Register(RuneUnknown.Instance);

		Register(new RuneRipping());
		Register(new RuneYield());
		Register(new RuneHardening());

		Register("temporal-infusion", new RuneTemporalInfusion());
	}

	private void Register(Rune mod) {
		registry.Add(mod.GetKey(), mod);
	}

	private void Register(string key, Rune mod) {
		registry.Add(key, mod);
	}

	public Rune GetRune(string key) {
		return key != null && registry.TryGetValue(key, out var value) ? value : RuneUnknown.Instance;
	}

	public Rune GetRune(ItemStack stack) {
		Rune result = RuneUnknown.Instance;
		if (stack?.Item is RuneItem mi && registry.TryGetValue(mi.Key, out var value))
			result = value;
		return result;
	}

	public string GetRuneKey(ItemStack stack) {
		return stack?.Item is RuneItem mi ? mi.Key : "unknown";
	}

	public bool IsValidRune(ItemStack stack) {
		return GetRune(stack) is not RuneUnknown;
	}

	public int GetUnlockedSlots(ItemStack stack) {
		return stack?.Attributes?.GetInt(TemporalInfusion.UnlockedSlotsKey) ?? 0;
	}

	public Dictionary<ItemSlot, Dictionary<Rune, List<AppliedRune>>> ReadRunesGrouped(Entity entity) {
		return ReadRunesGrouped(GetModifiableSlots(entity));
	}

	public Dictionary<ItemSlot, Dictionary<Rune, List<AppliedRune>>> ReadRunesGrouped(IEnumerable<ItemSlot> slots) {
		return slots.ToDictionary(slot => slot, ReadRunesGrouped).Where(x => x.Value.Count > 0)
		   .ToDictionary(e => e.Key, e => e.Value);
	}

	public Dictionary<Rune, List<AppliedRune>> ReadRunesGrouped(ItemSlot slot) {
		return ReadRunesGrouped(slot.Itemstack);
	}

	public Dictionary<Rune, List<AppliedRune>> ReadRunesGrouped(ItemStack stack) {
		var entries = ReadRunes(stack);
		var filtered = entries
		   .Where(x => x.SourceItem?.Item is RuneItem { Group: not null });
		var result = new Dictionary<Rune, List<AppliedRune>>();
		foreach (var entry in filtered) {
			List<AppliedRune> list = null;
			if (result.TryGetValue(entry.Rune, out var value)) list = value;
			if (list is null) {
				list = new List<AppliedRune>();
				result.Add(entry.Rune, list);
			}

			list.Add(entry);
		}

		return result;
	}

	public List<AppliedRune> ReadRunes(ItemStack stack) {
		var uid = stack?.Attributes?.GetString("uid");
		if (uid is null) return ReadAppliedModifiersInternal(stack);

		var lastUpdate = stack.Attributes.GetInt("lastModUpdate", -1);
		var update = !cache.ContainsKey(uid) || lastUpdate == -1 || cache[uid].Key < lastUpdate ||
					 cache[uid].Value.Any(x => x.SourceItem?.Item is null);

		if (update)
			cache[uid] = new KeyValuePair<int, List<AppliedRune>>(lastUpdate, ReadAppliedModifiersInternal(stack));

		return cache[uid].Value;
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

	private List<AppliedRune> ReadAppliedModifiersInternal(IItemStack stack) {
		List<AppliedRune> mods = new();
		var modifiers = stack?.Attributes?.GetTreeAttribute("modifiers");
		if (modifiers == null) return mods;

		foreach (var (modId, value) in modifiers) {
			if (value is not ITreeAttribute data) continue;
			var mod = GetRune(data.GetString("key"));
			if (mod is RuneUnknown) continue;

			var attr = data.GetTreeAttribute("data");
			var sourceItem = data.GetItemstack("source");
			if (sourceItem is null || TemporalSmithing.Instance?.ClientApi?.World is null) continue;
			sourceItem.ResolveBlockOrItem(TemporalSmithing.Instance.ClientApi.World);
			mods.Add(new AppliedRune(modId, mod, sourceItem, attr));
		}

		return mods;
	}

}

internal class RuneUnknown : Rune {

	public static readonly RuneUnknown Instance = new();

	private RuneUnknown() { }

	public override string GetKey() {
		return "unknown";
	}

	public override string GetIconKey() {
		return "question";
	}

}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using temporalsmithing.item.modifier;
using temporalsmithing.util;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;

namespace temporalsmithing.content.modifier;

public abstract class Modifier {

	private const string HandbookInfoColor = "#DAA520";
	private static string WhiteText => ColorToHex.Transform(Color.White);
	public Action<ICoreAPI, ItemSlot, ItemSlot> OnModificationFinish { get; private protected set; } = (_, _, _) => { };

	public Action<ICoreAPI, ItemSlot, ModifierEntry> OnModifierRemoved { get; private protected set; } =
		(_, _, _) => { };

	/// Returns the key of the modification
	public abstract string GetKey();

	/// Returns the key of the icon to use
	public abstract string GetIconKey();

	public string GetFullLangKey() {
		return "temporalsmithing:modifier-" + GetKey();
	}

	public virtual Color GetIconColor() {
		return Color.FromArgb(255, 255, 255);
	}

	#region Events

	public virtual float OnAttackedWith(Entity entity, DamageSource damageSource, float damage,
										ModifierEntry currentEntry) {
		return damage;
	}

	public virtual bool OnAttackingWith(bool continueCode, IWorldAccessor world, Entity byEntity, Entity attackedEntity,
										ItemSlot itemslot, ModifierEntry currentEntry) {
		return continueCode;
	}

	public virtual bool OnDamageItem(bool continueCode, IWorldAccessor world, Entity byEntity, ItemSlot itemslot, int amount = 1) {
		return true;
	}

	public virtual Pair<EnumHandHandling, EnumHandling> OnInteractedWithStart(
		ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent,
		EnumHandHandling handHandling, EnumHandling handling, ModifierEntry currentEntry) {
		return new Pair<EnumHandHandling, EnumHandling>(handHandling, handling);
	}

	public virtual Pair<bool, EnumHandling> OnInteractedWithStep(bool wasCancelledBefore, float secondsUsed,
																 ItemSlot slot, EntityAgent byEntity,
																 BlockSelection blockSel, EntitySelection entitySel,
																 EnumHandling handling,
																 ModifierEntry currentEntry) {
		return new Pair<bool, EnumHandling>(wasCancelledBefore, handling);
	}

	public virtual EnumHandling OnInteractedWithStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity,
													 BlockSelection blockSel, EntitySelection entitySel,
													 EnumHandling handling,
													 ModifierEntry currentEntry) {
		return handling;
	}

	public virtual void OnKillEntityWith(Entity entity, DamageSource damagesource,
										 ModifierEntry currentEntry) { }

	public virtual Pair<float, EnumHandling> OnBreakBlockWith(IServerPlayer byplayer, BlockSelection blocksel,
															  float dropquantitymultiplier, EnumHandling handling,
															  ModifierEntry currentEntry) {
		return new Pair<float, EnumHandling>(dropquantitymultiplier, handling);
	}

	public virtual float OnEntityReceiveDamage(DamageSource damageSource, float damage,
											   ModifierEntry currentEntry) {
		return damage;
	}

	#endregion

	/// Returns the max amount of applicable modifiers of this type
	public virtual int GetMaxOfModifier() {
		return -1;
	}

	public virtual int GetRequiredHitsToApply() {
		return 20;
	}

	public virtual int GetRequiredHitsToRemove() {
		return 10;
	}

	public virtual float GetItemRetrievalChanceOnRemoval() {
		return 0.5f;
	}

	public float GetItemRetrievalPercentageOnRemoval() {
		return GetItemRetrievalChanceOnRemoval() * 100;
	}

	public virtual object[] GetDescriptionArguments(ItemStack stack) {
		return Array.Empty<object>();
	}

	public virtual void WriteDescription(ItemStack stack, StringBuilder builder) {
		var iconColor = "#" + ColorToHex.Transform(GetIconColor());
		var iconName = GetIconKey();
		var stackName = Lang.Get(GetFullLangKey());
		builder.AppendLine($"<font color={iconColor}><icon name={iconName}/></font> {stackName}");

		var descColor = "#" + ColorToHex.Transform(Color.Cornsilk);
		var descriptionArguments = GetDescriptionArguments(stack);
		var descText = Lang.Get(GetFullLangKey() + ".explanation", descriptionArguments);
		builder.AppendLine($"<font color={descColor}><i>{descText}</i></font>");

		var whenAppliedText = Lang.Get("temporalsmithing:modifier.when-applied");
		builder.AppendLine().AppendLine(
			$"<font lineheight=\"1.2\" color=\"#{ColorToHex.Transform(Color.Goldenrod)}\"> {whenAppliedText}</font>"
		);
		var requiredApplyText = Lang.Get("temporalsmithing:modifier.required-apply-hits");
		var requiredHitsToApply = GetRequiredHitsToApply();
		builder.AppendLine(
			$"<font color=\"#{WhiteText}\"><icon name=hammer/> {requiredApplyText} {requiredHitsToApply}</font>"
		);

		if (this is not UnlockingModifier) {
			var whenRemovedText = Lang.Get("temporalsmithing:modifier.when-removed");
			builder.AppendLine().AppendLine(
				$"<font lineheight=\"1.2\" color=\"#{ColorToHex.Transform(Color.Goldenrod)}\"> {whenRemovedText}</font>"
			);
			var requiredRemovalText = Lang.Get("temporalsmithing:modifier.required-removal-hits");
			var requiredHitsToRemove = GetRequiredHitsToRemove();
			builder.AppendLine(
				$"<font color=\"#{WhiteText}\"><icon name=edgecrack/> {requiredRemovalText} {requiredHitsToRemove}</font>"
			);
			var removalChanceText = Lang.Get("temporalsmithing:modifier.refund-chance");
			var removalChancePercentage = Math.Round(GetItemRetrievalPercentageOnRemoval(), 2);
			builder.AppendLine(
				$"<font color=\"#{WhiteText}\"><icon name=shamrock/> {removalChanceText} {removalChancePercentage}%</font>"
			);
		}

		if (stack.Item is not ModifierItem mi) return;
		var descInfoObj = mi.GetData("descInfo");
		if (descInfoObj is null || !descInfoObj["entries"].Exists) return;
		var furtherInfosText = Lang.Get("temporalsmithing:modifier.further-infos");
		builder.AppendLine().AppendLine(
			$"<font lineheight=\"1.2\" color=\"#{ColorToHex.Transform(Color.Goldenrod)}\"> {furtherInfosText}</font>"
		);
		foreach (var obj in descInfoObj["entries"].AsArray()) {
			var icon = obj["icon"].AsString();
			var text = obj["text"].AsString();
			var value = obj["value"].AsString();
			if (icon is null || text is null || value is null) continue;
			builder.AppendLine($"<font color=\"#{WhiteText}\"><icon name={icon}/> {Lang.Get(text)} {value}</font>");
		}
	}

	internal static void ApplyOnItem(ICoreAPI api, ItemSlot modified, ItemSlot modifier,
									 Action<ITreeAttribute> additionalData = null) {
		var modId = Guid.NewGuid().ToString().Replace("-", "");

		if (modified.Empty || modifier.Empty) return;
		var attr = modified.Itemstack.Attributes;
		var modifiers = attr.GetOrAddTreeAttribute("modifiers");
		var entry = modifiers.GetOrAddTreeAttribute(modId);

		var modKey = Modifiers.Instance.GetModifierKey(modifier.Itemstack);
		entry.SetString("key", modKey);
		entry.SetItemstack("source", modifier.Itemstack);
		if (additionalData is not null) {
			var data = entry.GetOrAddTreeAttribute("data");
			additionalData.Invoke(data);
		}

		modifiers[modId] = entry;
		attr["modifiers"] = modifiers;
		if (!attr.HasAttribute("uid")) attr.SetString("uid", Guid.NewGuid().ToString().Replace("-", ""));
		attr.SetLong("lastModUpdate", api.World.Calendar.ElapsedSeconds);
		modified.MarkDirty();
	}

	internal static void RemoveFromItem(ICoreAPI api, ItemSlot modified, ModifierEntry entry) {
		if (modified.Empty || modified.Itemstack.Attributes is null || entry is null) return;

		var modId = entry.EntryId;
		var modifiers = modified.Itemstack.Attributes.GetOrAddTreeAttribute("modifiers");

		modifiers.RemoveAttribute(modId);
		if (modifiers.Count > 0) modified.Itemstack.Attributes["modifiers"] = modifiers;
		else modified.Itemstack.Attributes.RemoveAttribute("modifiers");
		modified.Itemstack.Attributes.SetLong("lastModUpdate", api.World.Calendar.ElapsedSeconds);
		modified.MarkDirty();
	}

	public virtual void WriteHandbookDescription(ICoreAPI api, in StringBuilder builder) {
		var modifierItems = api.World.Items
		   .Where(x => x is ModifierItem mi && mi.Key.Equals(GetKey()))
		   .Select(x => x as ModifierItem)
		   .Where(x => x is not null)
		   .ToArray();
		var argumentCount = modifierItems.Max(x => x?.GetHandbookDescriptionArguments().Length);
		var descArgs = new List<object>();
		for (var i = 0; i < argumentCount; i++) {
			descArgs.Add(BuildArgumentFromMultipleItems(i, modifierItems));
		}

		var source = new StringBuilder();
		foreach (var mod in modifierItems) {
			source.AppendLine(
				$"\t{Lang.GetMatching(mod.Code?.Domain + ":" + mod.ItemClass.Name() + "-" + mod.Code?.Path)}");
		}

		builder.AppendLine(
				$"<font color=\"#{ColorToHex.Transform(GetIconColor())}\">" +
				$"<icon name={GetIconKey()}/> {Lang.Get(GetFullLangKey())}" +
				"</font>"
			).AppendLine()
		   .AppendLine(
				"<font color=\"#fff8dc\"><i>" +
				$"{Lang.Get(GetFullLangKey() + ".explanation", descArgs.ToArray())}" +
				"</i></font>"
			).AppendLine()
		   .AppendLine(
				$"<icon name=hammer/> <font color=\"{HandbookInfoColor}\">" +
				$"{Lang.Get("temporalsmithing:modifier.required-apply-hits")}</font> {GetRequiredHitsToApply()}"
			)
		   .AppendLine(
				$"<icon name=edgecrack/> <font color=\"{HandbookInfoColor}\">" +
				$"{Lang.Get("temporalsmithing:modifier.required-removal-hits")}</font> {GetRequiredHitsToRemove()}"
			)
		   .AppendLine(
				$"<icon name=shamrock/> <font color=\"{HandbookInfoColor}\">" +
				$"{Lang.Get("temporalsmithing:modifier.refund-chance")}</font> {GetItemRetrievalPercentageOnRemoval()}%"
			)
		   .AppendLine(
				$"<icon name=openchest/> <font color=\"{HandbookInfoColor}\">" +
				$"{Lang.Get("temporalsmithing:modifier.source")}</font>"
			).Append(source);

		var infoValues = new Dictionary<string, string>();
		var infoBuilder = new Dictionary<string, System.Func<string, string>>();
		foreach (var mi in modifierItems) {
			var descInfo = mi.Data["descInfo"];
			var entries = descInfo?["entries"].AsArray();
			if (entries is null) continue;
			foreach (var entry in entries) {
				var id = entry["id"].AsString();
				var icon = entry["icon"].AsString();
				var text = entry["text"].AsString();
				var value = entry["value"].AsString();
				if (id is null || icon is null || text is null || value is null) continue;

				if (!infoBuilder.ContainsKey(id))
					infoBuilder.Add(id, val =>
						$"<icon name={icon}/> <font color=\"{HandbookInfoColor}\">" +
						$"{Lang.Get(text)}</font> {val}");
				if (infoValues.ContainsKey(id)) {
					var existing = infoValues[id];
					var existingSplit = existing.Split(new[] { " / " }, StringSplitOptions.None);
					if (existingSplit.Any(x => x.Equals(value))) continue;
					existing += " / " + value;
					infoValues[id] = existing;
				} else {
					infoValues.Add(id, value);
				}
			}
		}

		foreach (var text in infoBuilder.Select(pair => pair.Value.Invoke(infoValues[pair.Key]))) {
			builder.AppendLine(text);
		}

		builder.AppendLine().AppendLine();
	}

	private static string BuildArgumentFromMultipleItems(int index, IEnumerable<ModifierItem> items) {
		Type type = null;
		var arguments = new List<object>();
		foreach (var item in items) {
			var args = item.GetHandbookDescriptionArguments();
			if (index >= args.Length)
				throw new IndexOutOfRangeException(
					$"Expected {index} elements in handbook arguments array of ModifierItem {item.Code}");

			var value = args[index];
			if (value is null)
				throw new IndexOutOfRangeException(
					$"Element {index} in handbook arguments array of ModifierItem {item.Code} is null while it shouldn't!");
			if (type is not null && type != value.GetType())
				throw new IndexOutOfRangeException(
					$"Element {index} in handbook arguments array of ModifierItem {item.Code} is not of type {type}!");
			type = value.GetType();
			arguments.Add(value);
		}

		if (type == typeof(int) && arguments.Count > 1) {
			return arguments.Min(x => (int)x) + " - " + arguments.Max(x => (int)x);
		}

		arguments = arguments.Distinct().ToList();

		return string.Join("/", arguments);
	}

}
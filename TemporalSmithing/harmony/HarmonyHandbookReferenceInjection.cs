using System;
using System.Linq;
using System.Text;
using temporalsmithing.behavior.collectible;
using temporalsmithing.content.modifier;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.GameContent;

namespace temporalsmithing.harmony;

internal class HarmonyHandbookReferenceInjection {

	public static void Patch(GuiHandbookTextPage __instance, ICoreClientAPI capi,
							 ref RichTextComponentBase[] ___comps) {
		var text = __instance.Text;
		var changed = false;
		var start = 0;
		while ((start = text.IndexOf("<ref=", start, StringComparison.Ordinal)) != -1) {
			var end = text.IndexOf(">", start, StringComparison.Ordinal);
			var separatorIndex = text.IndexOf('=', start);
			var reference = text.Substring(separatorIndex + 1, end - separatorIndex - 1);

			if (reference.Length < 2) {
				capi.Logger.Warning(
					$"Illegal length for refCode {reference} (Expected min length of two characters). " +
					"Skipping reference injection for this candidate.");
				continue;
			}

			var refType = reference[0];
			var refVal = reference.Substring(1, reference.Length - 1);
			switch (refType) {
				case '@': {
					var refComponents = refVal.Split('|');
					var langKey = refComponents[0];
					var args = Array.Empty<string>();
					if (refComponents.Length == 2) args = refComponents[1].Split(',');

					text = text.Remove(start, end - start + 1);
					// ReSharper disable once CoVariantArrayConversion
					var translatedText = Lang.Get(langKey, args);
					text = text.Insert(start, translatedText);
					start += translatedText.Length;
					changed = true;
					break;
				}
				case '~': {
					string value = null;
					switch (refVal.Substring(0, refVal.IndexOf('('))) {
						case "mod":
							value = HandleModifierCodeReference(capi, refVal);
							break;
						case "mods":
							value = HandleModifiersCodeReference(capi, refVal);
							break;
						default:
							capi.Logger.Warning($"Unsupported code reference: {refVal}. Skipping replacement");
							break;
					}

					if (value is null) {
						start += refVal.Length;
						break;
					}

					text = text.Remove(start, end - start + 1);
					text = text.Insert(start, value);
					start += value.Length;
					changed = true;
					break;
				}
				default:
					capi.Logger.Warning(
						$"Illegal refType {refType} for refCode {reference}. " +
						"Skipping reference injection for this candidate.");
					break;
			}
		}

		if (!changed) return;
		__instance.Text = text;
		___comps = VtmlUtil.Richtextify(capi, __instance.Text,
			CairoFont.WhiteSmallText().WithLineHeightMultiplier(1.2));
	}

	private static string HandleModifierCodeReference(ICoreAPI api, string refVal) {
		var leftParenthesisIndex = refVal.IndexOf('(');
		var rightParenthesisIndex = refVal.IndexOf(')');
		var leftBracketIndex = refVal.IndexOf('[');
		var rightBracketIndex = refVal.IndexOf(']');
		if (leftParenthesisIndex == -1 || leftBracketIndex == -1 ||
			rightParenthesisIndex == -1 || rightBracketIndex == -1 ||
			leftParenthesisIndex >= rightParenthesisIndex ||
			leftBracketIndex >= rightBracketIndex ||
			leftParenthesisIndex >= leftBracketIndex) {
			api.Logger.Warning($"Invalid format for argument {refVal}. Syntax: classKey(elementKey)[Method]");
			return null;
		}

		var modKey = refVal.Substring(leftParenthesisIndex + 1, rightParenthesisIndex - leftParenthesisIndex - 1);
		var mod = RuneService.Instance.GetRune(modKey);
		if (mod is null) {
			api.Logger.Warning($"Modifier {modKey} could not be resolved.");
			return null;
		}

		var modMethod = refVal.Substring(leftBracketIndex + 1, rightBracketIndex - leftBracketIndex - 1);
		var methodInfo = mod.GetType().GetMethod(modMethod);
		if (methodInfo is not null) return methodInfo.Invoke(mod, Array.Empty<object>())?.ToString();
		api.Logger.Warning($"Method {modMethod} of modifier {modKey} could not be resolved.");
		return null;
	}

	private static string HandleModifiersCodeReference(ICoreAPI api, string refVal) {
		var leftParenthesisIndex = refVal.IndexOf('(');
		var rightParenthesisIndex = refVal.IndexOf(')');
		if (leftParenthesisIndex == -1 || rightParenthesisIndex == -1 ||
			leftParenthesisIndex >= rightParenthesisIndex) {
			api.Logger.Warning($"Invalid format for argument {refVal}. Syntax: classKey(elementKey)[Method]");
			return null;
		}

		var builder = new StringBuilder();
		var itemKeys = refVal.Substring(leftParenthesisIndex + 1, rightParenthesisIndex - leftParenthesisIndex - 1)
		   .Split(',');
		var modifiables = itemKeys
		   .Select(x => api.World.Items.FirstOrDefault(y => y.GetType().Name.Equals(x)))
		   .Where(x => x is not null);

		foreach (var modifiable in modifiables) {
			if (modifiable.CollectibleBehaviors.FirstOrDefault(x => x is RuneApplicableBehavior) is not RuneApplicableBehavior
				behavior) {
				api.Logger.Warning(
					$"Item {modifiable.Code} could be resolved but does not contain behavior ModifiableBehavior.");
				return null;
			}

			var modifiers = behavior.PossibleModifiers.Select(x => RuneService.Instance.GetRune(x));
			foreach (var mod in modifiers) {
				mod?.WriteHandbookDescription(api, builder);
			}
		}

		return builder.ToString();
	}

}

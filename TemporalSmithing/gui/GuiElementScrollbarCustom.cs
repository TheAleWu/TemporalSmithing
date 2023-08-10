using System;
using Vintagestory.API.Client;

namespace temporalsmithing.gui;

public class GuiElementScrollbarCustom : GuiElementScrollbar {

	private float scrollSpeedMultiplier = 1;

	public GuiElementScrollbarCustom(ICoreClientAPI capi, Action<float> onNewScrollbarValue, ElementBounds bounds) :
		base(capi, onNewScrollbarValue, bounds) { }

	public override void OnMouseWheel(ICoreClientAPI capi, MouseWheelEventArgs args) {
		args.deltaPrecise *= scrollSpeedMultiplier;
		base.OnMouseWheel(capi, args);
	}

	public GuiElementScrollbarCustom WithScrollSpeedMultiplier(float multiplier) {
		scrollSpeedMultiplier = multiplier;
		return this;
	}

}
using System.Linq;
using Cairo;
using temporalsmithing.blockentity;
using temporalsmithing.content.modifier;
using Vintagestory.API.Client;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace temporalsmithing.gui;

public class GuiDialogSmithingTable : GuiDialogBlockEntity {

	private const string InputSlotGridKey = "inputSlot";
	private const string ModSlotGridKey = "modSlot";
	private readonly BlockEntitySmithingTable blockEntity;
	private readonly InventorySmithingTable inv;
	private ElementBounds contentBounds;
	private float currentScroll;
	private bool dirty;
	private EnumPosFlag screenPos;

	public GuiDialogSmithingTable(InventorySmithingTable inv, BlockPos blockEntityPos, ICoreClientAPI capi,
								  BlockEntitySmithingTable blockEntity)
		: base(Lang.Get("temporalsmithing:block-smithing-table"), inv, blockEntityPos, capi) {
		this.blockEntity = blockEntity;
		this.inv = inv;

		dirty = true;
	}

	private double SlotSize => GuiElementPassiveItemSlot.unscaledSlotSize;

	public void RefreshDialog() {
		const int guiWidth = 500;
		const int guiHeight = 200;

		var rootBounds = ElementStdBounds.AutosizedMainDialog.WithAlignment(
			IsRight(screenPos)
				? EnumDialogArea.RightMiddle
				: EnumDialogArea.LeftMiddle);
		rootBounds.fixedOffsetY = guiHeight / 2f * (double)YOffsetMul(screenPos);

		contentBounds = ElementBounds.Fixed(0, 20, guiWidth, guiHeight);

		var bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
		bgBounds.BothSizing = ElementSizing.FitToChildren;
		bgBounds.WithChildren(contentBounds);

		var modSlotBounds = ElementStdBounds.Slot(guiWidth * 0.3f, SlotSize);
		var inputSlotBounds = ElementStdBounds.Slot(guiWidth * 0.7f, SlotSize);
		var errorMessageTextBounds = ElementBounds.FixedOffseted(EnumDialogArea.CenterMiddle,
			0, 10, 400, 60);
		bool suppressErrorText;

		#region Modification Element Bounds

		var modifierBounds = ElementBounds.FixedOffseted(EnumDialogArea.CenterBottom,
			0, -10, guiWidth, guiHeight / 1.5f - 25);
		var insetBounds = modifierBounds.ForkBoundingParent(15, 10, -10, -10);
		var clipBounds = modifierBounds.FlatCopy()
		   .WithParent(insetBounds);
		var scrollbarBounds = modifierBounds.FlatCopy()
		   .FixedRightOf(modifierBounds, -20)
		   .WithFixedWidth(10)
		   .WithParent(modifierBounds);
		var containerBounds = ElementBounds.FixedOffseted(EnumDialogArea.LeftTop, 0, 10,
			clipBounds.fixedWidth, clipBounds.fixedHeight);

		#endregion

		var color = ColorUtil.Hex2Doubles("#f00000");
		SingleComposer = capi.Gui.CreateCompo("smithingTableDialog", rootBounds)
		   .AddShadedDialogBG(bgBounds)
		   .AddDialogTitleBar(Lang.Get("temporalsmithing:block-smithing-table"), () => TryClose())
		   .AddItemSlotGrid(Inventory, OnModifySlot, 1, Array(0), inputSlotBounds, InputSlotGridKey)
		   .AddItemSlotGrid(Inventory, OnModifySlot, 1, Array(1), modSlotBounds, ModSlotGridKey)
		   .AddDynamicCustomDraw(ElementBounds.FixedOffseted(EnumDialogArea.CenterTop, 0.0,
					20.0, 200.0, 90.0), OnDrawArrow,
				"symbolDrawer")

			#region Adds Error Messages

		   .AddIf(suppressErrorText = inv.Validation.IsInputSlotInvalid())
		   .AddStaticText(
				Lang.Get("temporalsmithing:gui-smithing-table.input-slot-invalid"),
				CairoFont.WhiteDetailText().WithColor(color),
				EnumTextOrientation.Center,
				errorMessageTextBounds
			)
		   .EndIf()
		   .AddIf(!suppressErrorText && (suppressErrorText = inv.Validation.IsModifierSlotInvalid()))
		   .AddStaticText(
				Lang.Get("temporalsmithing:gui-smithing-table.modifier-slot-invalid"),
				CairoFont.WhiteDetailText().WithColor(color),
				EnumTextOrientation.Center,
				errorMessageTextBounds
			)
		   .AddIf(!suppressErrorText && (suppressErrorText = inv.Validation.AreAllSlotsOccupied()))
		   .AddStaticText(
				Lang.Get("temporalsmithing:gui-smithing-table.no-modifier-slots-remaining"),
				CairoFont.WhiteDetailText().WithColor(color),
				EnumTextOrientation.Center,
				errorMessageTextBounds
			)
		   .AddIf(!suppressErrorText && (suppressErrorText = inv.Validation.MaximumOfRunesApplied()))
		   .AddStaticText(
				Lang.Get("temporalsmithing:gui-smithing-table.maximum-of-runes-reached"),
				CairoFont.WhiteDetailText().WithColor(color),
				EnumTextOrientation.Center,
				errorMessageTextBounds
			)
		   .EndIf()

			#endregion

		   .AddIf(inv.IsItemModifiable())
		   .AddInset(insetBounds, 3, 0.8f)
		   .BeginClip(clipBounds);

		#region Modifications Container

		var maxHeight = 0.0;
		if (inv.IsItemModifiable() && inv.GetUnlockedSlots() > 0) {
			var container = new GuiElementContainer(capi, containerBounds);
			SingleComposer.AddInteractiveElement(container, "modificationContainer");
			var remainingSlots = inv.GetUnlockedSlots();
			var row = 0;
			while (remainingSlots > 0) {
				var addedSlots = GameMath.Min(9, remainingSlots);
				var bounds = ElementStdBounds.SlotGrid(EnumDialogArea.LeftTop, 0, row * 58, addedSlots, 1);
				var slotGrid = new GuiElementItemSlotGrid(capi, Inventory, OnModifySlot, addedSlots,
					Array(row * 9 + InventorySmithingTable.MinSlots,
						row * 9 + addedSlots + InventorySmithingTable.MinSlots),
					bounds);
				container.Add(slotGrid);
				remainingSlots -= addedSlots;
				row++;
			}

			if (containerBounds.ChildBounds.Count > 0)
				maxHeight = containerBounds.ChildBounds.Max(x => x.fixedY + x.fixedHeight) + 20;
		}

		#endregion

		SingleComposer
		   .EndClip()
		   .AddInteractiveElement(new GuiElementScrollbarCustom(capi, OnNewScrollbarValue, scrollbarBounds)
			   .WithScrollSpeedMultiplier(0.2f), "modifierScrollbar")
		   .EndIf()
		   .Compose();

		#region Scroll Bar

		var scrollBar = SingleComposer.GetScrollbar("modifierScrollbar");
		if (scrollBar != null) {
			var wasReloaded = false;
			var prevScroll = currentScroll;
			if (currentScroll > 0) wasReloaded = true;

			scrollBar.SetHeights(
				(float)insetBounds.fixedHeight, (float)maxHeight
			);
			if (wasReloaded) {
				currentScroll = prevScroll;
				scrollBar.CurrentYPosition = currentScroll;
				UpdateScrolling();
			}
		}

		#endregion

		dirty = false;
	}

	private void OnNewScrollbarValue(float value) {
		currentScroll = value;
		UpdateScrolling();
	}

	private void UpdateScrolling() {
		var container = SingleComposer.GetContainer("modificationContainer");
		if (container == null) return;

		var bounds = container.Bounds;
		bounds.fixedY = 0 - currentScroll;
		bounds.CalcWorldBounds();
		bounds.ChildBounds.ForEach(x => x.CalcWorldBounds());
	}

	private void OnModifySlot(object obj) {
		capi.Network.SendBlockEntityPacket(BlockEntityPosition.X,
			BlockEntityPosition.Y, BlockEntityPosition.Z, obj);
	}

	public override void OnRenderGUI(float deltaTime) {
		if (dirty) {
			SingleComposer?.Dispose();
			RefreshDialog();
		}

		base.OnRenderGUI(deltaTime);
	}

	public override void OnGuiOpened() {
		base.OnGuiOpened();

		screenPos = GetFreePos("smallblockgui");
		OccupyPos("smallblockgui", screenPos);
		inv.SlotModified += OnInventorySlotModified;
	}

	public override void OnGuiClosed() {
		FreePos("smallblockgui", screenPos);

		base.OnGuiClosed();
		inv.SlotModified -= OnInventorySlotModified;
	}

	private void OnDrawArrow(Context ctx, ImageSurface surface, ElementBounds currentBounds) {
		var invert = inv.GetSelectedModifierSlot() is not null;

		const double num1 = 30.0;
		ctx.Save();
		var matrix = ctx.Matrix;
		matrix.Translate(GuiElement.scaled(63.0) * (invert ? 2.25 : 1), GuiElement.scaled(num1 + 2.0));
		matrix.Scale((invert ? -1 : 1) * GuiElement.scaled(0.6), GuiElement.scaled(0.6));
		ctx.Matrix = matrix;
		capi.Gui.Icons.DrawArrowRight(ctx, 2.0);
		var num2 = (double)blockEntity.PerformedHits / blockEntity.RequiredHits;
		ctx.Rectangle(GuiElement.scaled(5.0), 0.0, GuiElement.scaled(125.0 * num2), GuiElement.scaled(100.0));
		ctx.Clip();
		var source = new LinearGradient(0.0, 0.0, GuiElement.scaled(200.0), 0.0);
		_ = (int)source.AddColorStop(0.0, new Color(0.0, 0.4, 0.0, 1.0));
		_ = (int)source.AddColorStop(1.0, new Color(0.2, 0.6, 0.2, 1.0));
		ctx.SetSource(source);
		capi.Gui.Icons.DrawArrowRight(ctx, 0.0, false, false);
		source.Dispose();
		ctx.Restore();
	}

	private void OnInventorySlotModified(int slot) {
		var pos = blockEntity.Pos;
		if (slot < 2) blockEntity.ResetPerformedHits();
		if (slot == inv.GetSlotId(inv.GetModifierSlot())) {
			var modKey = RuneService.Instance.GetRuneKey(inv.GetModifierSlot()?.Itemstack);
			blockEntity.SetCurrentModifierKey(modKey);
			blockEntity.CurrentModifierKey = modKey;
		}

		capi.Event.EnqueueMainThreadTask(RefreshDialog, "refreshsmithingtabledialog");
	}

	private static int[] Array(int from, int to = 0) {
		if (from < 0) return System.Array.Empty<int>();

		var size = to - from;
		if (size <= 0) return new[] { from };

		var arr = new int[size];
		for (var i = 0; i < size; i++) arr[i] = i + from;

		return arr;
	}

	public void MarkDirty() {
		dirty = true;
	}

}

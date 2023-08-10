using temporalsmithing.blockentity;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace temporalsmithing.block;

public class BlockSmithingTable : Block {

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel) {
		if (blockSel?.Position == null) return true;

		var flag = base.OnBlockInteractStart(world, byPlayer, blockSel);
		var pos = blockSel.Position;
		if (flag || byPlayer.WorldData.EntityControls.ShiftKey) return flag;
		var blockEntity = world.BlockAccessor.GetBlockEntity(pos);
		if (blockEntity is BlockEntitySmithingTable be) be.OnPlayerRightClick(byPlayer);

		return true;
	}

	public override float OnGettingBroken(IPlayer player, BlockSelection blockSel, ItemSlot itemslot,
										  float remainingResistance, float dt,
										  int counter) {
		var world = player.Entity.World;
		var blockEntity = world.BlockAccessor.GetBlockEntity(blockSel.Position);
		if (blockEntity is not BlockEntitySmithingTable be)
			return base.OnGettingBroken(player, blockSel, itemslot, remainingResistance, dt, counter);
		return be.OnPlayerLeftClick(player, itemslot)
			? float.MaxValue
			: base.OnGettingBroken(player, blockSel, itemslot, remainingResistance, dt, counter);
	}

	public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer,
									   float dropQuantityMultiplier = 1) {
		var entity = world.BlockAccessor.GetBlockEntity(pos);
		if (entity is BlockEntitySmithingTable st) st.ForceCloseGuiForAll();

		base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier);
	}

}
using temporalsmithing.content.modifier;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace temporalsmithing.behavior.entity;

public class PlayerDamagedEntityListener : EntityBehavior {

	public PlayerDamagedEntityListener(Entity entity) : base(entity) { }

	public override string PropertyName() => "playerdamagedentitylistener";

	public override void OnEntityReceiveDamage(DamageSource damageSource, ref float damage) {
		if (damageSource.SourceEntity is not EntityPlayer || damageSource.Source is EnumDamageSource.Internal) return;
		var slots = RunePowers.GetModifiableSlots(damageSource.SourceEntity);
		var dmg = damage;

		float OnEntityReceiveDamageInternal(RunePowerEntry x, float val) =>
			x.RunePower.OnAttackedWith(entity, damageSource, val, x);

		damage = RunePowers.Instance.PerformOnSlots(slots, dmg, OnEntityReceiveDamageInternal);
	}

}
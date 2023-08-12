using temporalsmithing.content.modifier;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace temporalsmithing.behavior.entity;

public class EntityHurtListener : EntityBehavior {

	public EntityHurtListener(Entity entity) : base(entity) { }

	public override string PropertyName() => "entityhurtlistener";

	public override void OnEntityReceiveDamage(DamageSource damageSource, ref float damage) {
		var slots = RunePowers.GetModifiableSlots(entity);
		var dmg = damage;

		float OnEntityReceiveDamageInternal(RunePowerEntry x, float val) =>
			x.RunePower.OnEntityReceiveDamage(damageSource, val, x);

		damage = RunePowers.Instance.PerformOnSlots(slots, dmg, OnEntityReceiveDamageInternal);
	}

}
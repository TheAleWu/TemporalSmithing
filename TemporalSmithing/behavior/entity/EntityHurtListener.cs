using temporalsmithing.content.modifier;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace temporalsmithing.behavior.entity;

public class EntityHurtListener : EntityBehavior {

	public EntityHurtListener(Entity entity) : base(entity) { }

	public override string PropertyName() => "entityhurtlistener";

	public override void OnEntityReceiveDamage(DamageSource damageSource, ref float damage) {
		var slots = Modifiers.GetModifiableSlots(entity);
		var dmg = damage;

		float OnEntityReceiveDamageInternal(ModifierEntry x, float val) =>
			x.Modifier.OnEntityReceiveDamage(damageSource, val, x);

		damage = Modifiers.Instance.PerformOnSlots(slots, dmg, OnEntityReceiveDamageInternal);
	}

}
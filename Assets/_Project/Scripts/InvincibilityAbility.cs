using UnityEngine;

[CreateAssetMenu(menuName = "Overrun/Abilities/Invincibility", fileName = "InvincibilityAbility")]
public class InvincibilityAbility : AbilityData
{
    public override void Activate(AbilityContext ctx) => ctx.Health.SetInvulnerable(true);
    public override void Deactivate(AbilityContext ctx) => ctx.Health.SetInvulnerable(false);
}
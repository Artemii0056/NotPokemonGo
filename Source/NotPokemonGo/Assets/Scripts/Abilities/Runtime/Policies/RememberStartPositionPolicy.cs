namespace Abilities.Runtime.Policies
{
    public sealed class RememberStartPositionPolicy : AbilityPolicyBase
    {
        public override void OnAbilityStart(AbilityContext context)
            => context.Source.SetStartPosition(context.Source.transform.position);
    }
}

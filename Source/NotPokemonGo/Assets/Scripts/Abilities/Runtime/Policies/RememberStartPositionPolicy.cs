using UnityEngine;

namespace Abilities.Runtime.Policies
{
    /// <summary>
    /// Запоминает стартовую позицию Source, чтобы MoveCommand.ToStartPosition мог работать.
    /// </summary>
    public sealed class RememberStartPositionPolicy : AbilityPolicyBase
    {
        public override void OnAbilityStart(AbilityContext ctx)
            => ctx.Source.SetStartPosition(ctx.Source.transform.position);
    }
}

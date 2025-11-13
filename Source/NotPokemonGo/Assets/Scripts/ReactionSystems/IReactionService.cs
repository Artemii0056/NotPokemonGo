using Infrastructure.ReactionSystem;

namespace ReactionSystems
{
    public interface IReactionService
    {
        void Register(IReaction reaction);
        bool TryReact(ReactionContext context);
    }
}
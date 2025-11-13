namespace Infrastructure.ReactionSystem
{
    public interface IReaction
    {
        bool CanReact(ReactionContext context);
        void React(ReactionContext context);
    }
}
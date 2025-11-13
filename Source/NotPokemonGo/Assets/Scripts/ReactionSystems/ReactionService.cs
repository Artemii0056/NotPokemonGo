using System.Collections.Generic;
using Infrastructure.ReactionSystem;

namespace ReactionSystems
{
    public class ReactionService : IReactionService
    {
        private readonly List<IReaction> _reactions = new();

        public void Register(IReaction reaction) => 
            _reactions.Add(reaction);

        public bool TryReact(ReactionContext context)
        {
            foreach (var reaction in _reactions)
            {
                if (reaction.CanReact(context))
                {
                    reaction.React(context);
                    return true; 
                }
            }

            return false;
        }
    }
}
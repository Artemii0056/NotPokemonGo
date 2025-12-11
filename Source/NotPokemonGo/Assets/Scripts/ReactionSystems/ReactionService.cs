using System.Collections.Generic;
using UnityEngine;

namespace ReactionSystems
{
    public class ReactionService : IReactionService
    {
        private readonly List<IReaction> _reactions = new();

        public void Register(IReaction reaction) => 
            _reactions.Add(reaction);

        public bool TryReact(ReactionContext context)
        {
            Debug.Log(_reactions.Count);
            
            foreach (var reaction in _reactions)
            {
                Debug.Log(reaction.GetType().Name + " In TryReact");
                
                if (reaction.CanReact(context))
                {
                    Debug.Log("CanReact...");

                    reaction.React(context);
                    return true;
                }
            }

            return false;
        }
    }
}
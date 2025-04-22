namespace Code.Extensions
{
    public static class TargetCollectionEntityExtension
    {
        public static GameEntity RemoveTargetCollectionComponents(this GameEntity entity)
        {
            if (entity.hasTargetsBuffer)
                entity.RemoveTargetsBuffer();

            if (entity.hasCollectTargetsInterval) 
                entity.RemoveCollectTargetsInterval();

            if (entity.hasCollectTargetsTimer) 
                entity.RemoveCollectTargetsTimer();
        
            entity.isReadyToCollectTargets = false;
        
            return entity;
        }
    }
}
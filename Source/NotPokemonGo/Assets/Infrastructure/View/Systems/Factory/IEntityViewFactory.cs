namespace Infrastructure.View.Systems.Factory
{
    public interface IEntityViewFactory
    {
        EntityBehavior CreateViewForEntity(GameEntity entity);
        EntityBehavior CreateViewForEntityFromPrefab(GameEntity entity);
    }
}
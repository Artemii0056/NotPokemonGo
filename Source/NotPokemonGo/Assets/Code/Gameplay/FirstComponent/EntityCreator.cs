namespace Code.Gameplay.FirstComponent
{
    public static class EntityCreator
    {
        public static GameEntity Create() => 
            Contexts.sharedInstance.game.CreateEntity();
    }
}
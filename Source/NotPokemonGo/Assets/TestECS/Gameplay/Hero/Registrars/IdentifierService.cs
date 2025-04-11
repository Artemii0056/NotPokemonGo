namespace TestECS.Gameplay.Hero.Registrars
{
    public class IdentifierService : IIdService
    {
        private int _lastId = 1;

        public int Next() =>
            ++_lastId;
    }
}
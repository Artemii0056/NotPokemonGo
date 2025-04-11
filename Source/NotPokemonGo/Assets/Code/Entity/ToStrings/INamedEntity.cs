using Entitas;

namespace Code.Entity.ToStrings
{
    public interface INamedEntity : IEntity
    {
        string EntityName(IComponent[] components);
        string BaseToString();
    }
}
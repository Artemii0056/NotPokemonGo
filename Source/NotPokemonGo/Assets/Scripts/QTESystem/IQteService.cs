using Units;

namespace QTESystem
{
    public interface IQteService
    {
        IQteSession StartSession(QteType qteType, Unit target, float duration);
    }
}
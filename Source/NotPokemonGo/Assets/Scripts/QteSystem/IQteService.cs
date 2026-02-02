using Units;

namespace QteSystem
{
    public interface IQteService
    {
        IQteSession StartSession(QteType qteType, Unit target, float duration);
    }
}
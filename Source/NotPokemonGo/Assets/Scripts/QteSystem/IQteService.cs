using Units;

namespace QteSystem
{
    public interface IQteService
    {
        IQteSession StartSession(QteRequest request);
    }
}
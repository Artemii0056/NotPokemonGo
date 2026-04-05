namespace QteSystem.Core
{
    public interface IQteService
    {
        IQteSession StartSession(QteRequest request);
    }
}
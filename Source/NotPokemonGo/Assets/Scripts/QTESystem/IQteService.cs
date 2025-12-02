using System;

namespace QTESystem
{
    public interface IQteService
    {
        void Start(QteType qteType);
        event Action <bool> Completed;
    }
}
using System;

namespace Services.QTEServices
{
    public interface IQTEService
    {
        void Start();
        event Action <bool> Completed;
    }
}
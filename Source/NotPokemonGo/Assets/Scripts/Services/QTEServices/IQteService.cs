using System;
using Abilities;
using QTESystem;

namespace Services.QTEServices
{
    public interface IQteService
    {
        void Start(QteType qteType);
        event Action <bool> Completed;
    }
}
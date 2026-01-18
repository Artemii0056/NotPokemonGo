using TimeServices;
using UnityEngine;
using VContainer;

public sealed class TimeServiceRunner : MonoBehaviour
{
    private ITimeService _time;

    [Inject]
    public void Construct(ITimeService time)
    {
        _time = time;
    }

    private void Update()
    {
        if (_time is TimeService service)
            service.Tick();
    }
}
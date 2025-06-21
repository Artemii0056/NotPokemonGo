using System.Collections.Generic;
using System.Linq;
using Services.StaticDataServices;
using Statuses;
using UnityEngine;

public class StatusViewPanel : MonoBehaviour
{
    [SerializeField] private List<StatusView> _statusViews;

    private IStaticDataService _staticDataLoadService;

    public void Add(Status status)
    {
        StatusView view;

        if (TrySearch(status.Setup.Type, out StatusView statusView) == false)
            view = GetFreeView();
        else
            view = statusView;

        view.Initialize(status, _staticDataLoadService.GetStatusIcon(status.Setup.Type));
    }

    public void Remove(Status status)
    {
        if (TrySearch(status.Setup.Type, out StatusView statusView))
        {
            statusView.Initialize(status, _staticDataLoadService.GetStatusIcon(status.Setup.Type));
            statusView.Dispose();
        }
    }

    private StatusView GetFreeView()
    {
        return _statusViews.FirstOrDefault(view => !view.HasStatus);
    }

    private bool TrySearch(StatusType searchType, out StatusView status)
    {
        status = null;

        foreach (var searchedStatus in _statusViews)
        {
            if (searchedStatus.HasStatus && searchedStatus.StatusType == searchType)
            {
                status = searchedStatus;
                return true;
            }
        }

        return false;
    }

    public void Init(IStaticDataService staticDataLoadService)
    {
        _staticDataLoadService = staticDataLoadService;
    }
}
using System.Collections.Generic;
using System.Linq;
using PersistentProgresses;
using UnityEngine;

namespace SaveLoadService
{
  public class PlayerPrefsSaveLoad : ISaveLoadService
  {
    private readonly PersistentProgressService _progressService;

    public PlayerPrefsSaveLoad(PersistentProgressService progressService)
    {
      _progressService = progressService;
    }

    public List<IProgressReader> ProgressReaders { get; } = new();
    public bool HasSavedProgress => PlayerPrefs.HasKey(ProgressKey());

    public void SaveProgress()
    {
      UpdateProgressWriters();
      WritePlayerPrefs();
    }

    public void LoadProgress()
    {
      ReadPlayerPrefs();
      UpdateProgressReaders();
    }

    public void DeleteSaves()
    {
      PlayerPrefs.DeleteKey(ProgressKey());
    }

    private string ProgressKey() =>
      "_progress";

    private void UpdateProgressReaders()
    {
      foreach (IProgressReader progressReader in ProgressReaders)
        progressReader.ReadProgress(_progressService.ProjectProgress);
    }

    private void UpdateProgressWriters()
    {
      foreach (IProgressWriter progressWriter in ProgressReaders
                 .OfType<IProgressWriter>()
                 .ToList())

        progressWriter.WriteProgress(_progressService.ProjectProgress);
    }

    private void WritePlayerPrefs()
    {
      string json = _progressService.ToEnvelopeJson();
      PlayerPrefs.SetString(ProgressKey(), json);
      PlayerPrefs.Save();
    }

    private void ReadPlayerPrefs()
    {
      if (PlayerPrefs.HasKey(ProgressKey()))
      {
        string json = PlayerPrefs.GetString(ProgressKey());
        _progressService.LoadProgress(json); 
      }
      else
      {
        _progressService.SetDefault();
      }
    }
  }
}
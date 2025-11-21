using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace PersistentProgresses
{
  public class PersistentProgressService
  {
    public ProjectProgress ProjectProgress { get; private set; }

    public static readonly JsonSerializerSettings JsonSettings = new()
    {
      Formatting = Formatting.None,
      NullValueHandling = NullValueHandling.Ignore,
      DefaultValueHandling = DefaultValueHandling.Include,
      ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
      Converters = { new StringEnumConverter() } 
    };

    private static readonly JsonSerializer Serializer = JsonSerializer.Create(JsonSettings);

    public void LoadProgress(string data)
    {
      if (string.IsNullOrWhiteSpace(data))
      {
        SetDefault();
        return;
      }

      try
      {
        JToken token = JToken.Parse(data);

        JToken dataNode = token["data"];

        if (dataNode == null)
        {
          // Битый или старый формат — откат на дефолт
          SetDefault();
          return;
        }

        ProjectProgress = dataNode.ToObject<ProjectProgress>(Serializer) 
                          ?? new ProjectProgress(new PlayerProgress());

        EnsureNotNullCollections();
      }
      catch
      {
        SetDefault();
        throw new Exception("Error parsing data Пидор");
      }
    }

    public void SetDefault()
    {
      ProjectProgress = new ProjectProgress(new PlayerProgress());
    }

    public string ToEnvelopeJson(int version = 1, string format = "ProjectProgress") =>
      JsonConvert.SerializeObject(new
      {
        version,
        format,
        data = ProjectProgress
      }, JsonSettings);

    private void EnsureNotNullCollections()
    {
      ProjectProgress.PlayerProgress ??= new PlayerProgress();
      ProjectProgress.PlayerProgress.LevelProgress ??= new LevelProgress();
      ProjectProgress.PlayerProgress.LevelProgress.StartMapsCompleted ??= new();
    }
  }
}
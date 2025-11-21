using UnityEditor;
using UnityEngine;

namespace Tools.Editor
{
  public class PrefsUtility
  {
    [MenuItem("PrefsUtility/ClearPrefs")]
    public static void ClearPrefs()
    {
      PlayerPrefs.DeleteAll();
      PlayerPrefs.Save();
    }
  }
}
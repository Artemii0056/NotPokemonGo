using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(fileName = nameof(SpawnPositionConfig), menuName = "StaticData/"+nameof(SpawnPositionConfig))]
    public class SpawnPositionConfig : ScriptableObject
    {
        public SpawnPositionType SpawnPositionType;
        public GameObject PositionContainer;
    }
}
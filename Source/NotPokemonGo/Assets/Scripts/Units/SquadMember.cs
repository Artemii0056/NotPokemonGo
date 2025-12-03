using UnityEngine;

namespace Units
{
	public class SquadMember : MonoBehaviour
	{
		[field: SerializeField] public SquadMemberType SquadMemberType { get; private set; }
	}
}
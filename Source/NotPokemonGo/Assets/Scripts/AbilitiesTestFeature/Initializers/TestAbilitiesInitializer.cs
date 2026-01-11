using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;

public class TestAbilitiesInitializer : MonoBehaviour, IInitializable, ICoroutineRunner
{
	private const string TestAbilitiesSceneName = "TestAbilities";
	
	public void Initialize()
	{
		if (SceneManager.GetActiveScene().name != TestAbilitiesSceneName)	
			SceneManager.LoadScene(TestAbilitiesSceneName);
	}
}
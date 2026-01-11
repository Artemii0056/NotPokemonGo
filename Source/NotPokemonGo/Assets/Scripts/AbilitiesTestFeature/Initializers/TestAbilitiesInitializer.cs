using Services;
using Services.SceneServices;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class TestAbilitiesInitializer : MonoBehaviour, IInitializable, ICoroutineRunner
{
	private const string TestAbilitiesSceneName = "TestAbilities";

	private ISceneLoader _sceneLoader;

	[Inject]
	public void Consctruct(ISceneLoader sceneLoader)
	{
		_sceneLoader = sceneLoader;
	}

	public void Initialize()
	{
		_sceneLoader.Load(TestAbilitiesSceneName);
	}
}
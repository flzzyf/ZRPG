using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFader : Singleton<SceneFader>
{
    public Animator animator;

    string sceneToLoad;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		animator.SetTrigger("FadeIn");
	}

	//淡出并加载场景
	public void FadeToScene(string sceneName)
    {
        animator.SetTrigger("FadeOut");

        sceneToLoad = sceneName;
    }

    //场景淡出完成
    void OnSceneFadeOut()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

	public void ReloadScene()
	{
		FadeToScene(SceneManager.GetActiveScene().name);
	}
}

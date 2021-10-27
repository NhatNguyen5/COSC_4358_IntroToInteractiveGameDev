using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChangeScene : MonoBehaviour
{

	public void loadNextScene(string sceneName)
	{

		OptionSettings.GameisPaused = false;
		Time.timeScale = 1f;
		SceneManager.LoadScene(sceneName);
		
	}


	public void tryagain(string sceneName)
	{

		OptionSettings.GameisPaused = false;
		GlobalPlayerVariables.GameOver = false;
		Time.timeScale = 1f;
		SceneManager.LoadScene(sceneName);
	}
}

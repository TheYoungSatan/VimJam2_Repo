using Sound;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainmenuHandler : MonoBehaviour
{
    private void Start()
    {
        GameInfo.ResetInfo();
        GUI.SetActive(false);
    }

    public void PlaySound()
    {
        AudioHub.PlaySound(AudioHub.ButtonClick);
    }

    public void PlayGame(string SceneName)
    {
        GameInfo.ResetInfo();
        StartCoroutine(LoadGame(SceneName));
    }

    public void QuitGame() => Application.Quit();

    IEnumerator LoadGame(string sceneToLoad)
    {
        SceneTransition transition = FindObjectOfType<SceneTransition>();
        transition.FadeOut();
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(sceneToLoad);
    }
}
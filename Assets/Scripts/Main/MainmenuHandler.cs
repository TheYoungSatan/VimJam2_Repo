using Sound;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainmenuHandler : MonoBehaviour
{
    [SerializeField] private Slider _master;
    [SerializeField] private Slider _music;
    [SerializeField] private Slider _sfx;

    private void Start()
    {
        //_master.value = .75f;
        //_music.value = 1f;
        //_sfx.value = 1f;
        //ChangeVolume();
        GameInfo.ResetInfo();
    }

    private void Update()
    {
        if(GUI.IsActive)
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

    public void ChangeVolume()
    {
        AkSoundEngine.SetRTPCValue("MasterVolume", _master.value);
        AkSoundEngine.SetRTPCValue("MusicVolume", _music.value);
        AkSoundEngine.SetRTPCValue("SFXVolume", _sfx.value);
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
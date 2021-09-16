using Sound;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interacting 
{
    public class LoadSceneInteraction : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _audioName = "_Computer";
        [SerializeField] private string _sceneToLoad = "Yoni_MinigameTesting";
        [SerializeField] private bool _loadMainScene = true;
        [SerializeField] private bool _showInfoPanel = true;
        [SerializeField] private string _text = "";

        public bool HasInfoPanel()
        {
            return _showInfoPanel;
        }

        public string InfoText()
        {
            return _text;
        }

        public void OnInteract()
        {
            if (_loadMainScene)
                GameInfo.AddTime(GameInfo.TravelTime);

            AudioHub.PlaySound(AudioHub.Interact + _audioName);
            StartCoroutine("LoadScene");
        }

        public bool Interactable()
        {
            return true;
        }

        public Transform Position()
        {
            return transform;
        }

        IEnumerator LoadScene()
        {
            yield return new WaitForEndOfFrame();
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneToLoad);

            while (!asyncLoad.isDone)
                yield return null;
        }
    }
}
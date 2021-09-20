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
        [SerializeField] private int _timePassed = 0;

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
            _timePassed = _timePassed > GameInfo.TravelTime ? _timePassed : GameInfo.TravelTime;

            if (_loadMainScene)
                GameInfo.AddTime(_timePassed);
            
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
            SceneTransition transition = FindObjectOfType<SceneTransition>();
            transition.FadeOut();
            yield return new WaitForSeconds(.5f);

            SceneManager.LoadScene(_sceneToLoad);
        }
    }
}
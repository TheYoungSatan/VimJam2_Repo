using Sound;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interacting 
{
    public class LoadSceneInteraction : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _audioName = "_Computer";
        [SerializeField] private string _sceneToLoad = "Yoni_MinigameTesting";

        public bool Interactable()
        {
            return true;
        }

        public void OnInteract()
        {
            AudioHub.PlaySound(AudioHub.Interact + _audioName);
            SceneManager.LoadScene(_sceneToLoad);
        }
    }
}
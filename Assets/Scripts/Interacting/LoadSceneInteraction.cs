using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interacting 
{
    public class LoadSceneInteraction : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _sceneToLoad = "Yoni_MinigameTesting";
        public void OnInteract()
        {
            SceneManager.LoadScene(_sceneToLoad);
        }
    }
}
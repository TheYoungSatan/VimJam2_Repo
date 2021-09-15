using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interacting 
{
    [RequireComponent(typeof(SwitchEventCaller), typeof(AkState))]
    public class LoadSceneInteraction : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _sceneToLoad = "Yoni_MinigameTesting";
        private SwitchEventCaller _caller;

        private void Awake()
        {
            if (TryGetComponent(out SwitchEventCaller caller))
                _caller = caller;
            else
                _caller = gameObject.AddComponent<SwitchEventCaller>();
        }

        public void OnInteract()
        {
            _caller.CallSwitch();
            SceneManager.LoadScene(_sceneToLoad);
        }
    }
}
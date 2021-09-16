using UnityEngine;

namespace Interacting
{
    public interface IInteractable
    {
        bool Interactable();
        void OnInteract();
        string InfoText();
        bool HasInfoPanel();
        Transform Position();
    }
}

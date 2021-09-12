using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MiniGame
{
    public class Minigame : MonoBehaviour
    {
        [HideInInspector] public MinigameHub Hub;
        public virtual void RunGame() { }
        public virtual void CheckInput() { }
        public virtual void UpdateGame() { }

    }
}
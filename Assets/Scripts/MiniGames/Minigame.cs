using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static MinigameHub;

namespace MiniGame
{
    public class Minigame : MonoBehaviour
    {
        [HideInInspector] public MinigameHub Hub;

        [HideInInspector] public Difficulty Difficulty;

        public virtual void RunGame() { }
        public virtual void CheckInput() { }
        public virtual void UpdateGame() { }
    }
}
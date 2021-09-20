using MiniGame.ConnectPipe;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MiniGame
{
    public class ConnectPipes : Minigame
    {
        [SerializeField] private Text _timer;
        [SerializeField] private int _time = 30;

        public static List<PipePoint> Points = new List<PipePoint>();
        private Pipe[] _pipes;

        private Pipe _startPipe;
        private Pipe _endPipe;

        private Coroutine routine;

        public override void RunGame()
        {
            Points.Clear();
            _pipes = FindObjectsOfType<Pipe>();
            _timer.text = _time.ToString();
            routine = StartCoroutine(CheckTime());

            foreach (var p in _pipes)
            {
                p.OnInitialize();
                Points.AddRange(p.Points);
                p.OnPipeRotated += CheckPath;
                p.OnEndReached += EndReached;
            }

            _startPipe = _pipes.Aggregate((p, n) => p.HasStartPoint ? p : n);
            _startPipe.name = "StartPipe: " + _startPipe.name;
            _endPipe = _pipes.Aggregate((p, n) => p.HasEndPoint ? p : n);
            _endPipe.name = "EndPipe: " + _endPipe.name;
        }

        private void EndReached()
        {
            StopCoroutine(routine);
            Hub.OnGameSucces(Difficulty);
        }

        private void CheckPath()
        {
            foreach (var p in Points)
            {
                p.ResetValues();
            }
            _startPipe.CheckConnection();
        }

        private IEnumerator CheckTime()
        {
            for (int i = _time; i > 0; i--)
            {
                yield return new WaitForSeconds(1f);
                _timer.text = i.ToString();
            }
            Hub.OnGameOver(Difficulty);
        }
    }
}

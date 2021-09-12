using MiniGame.ConnectPipe;
using System.Collections.Generic;
using System.Linq;

namespace MiniGame
{
    public class ConnectPipes : Minigame
    {
        public static List<PipePoint> Points = new List<PipePoint>();
        private Pipe[] _pipes;

        private Pipe _startPipe;
        private Pipe _endPipe;

        public override void RunGame()
        {
            _pipes = FindObjectsOfType<Pipe>();

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
            Hub.OnGameSucces();
        }

        private void CheckPath()
        {
            foreach (var p in Points)
            {
                p.ResetValues();
            }
            _startPipe.CheckConnection();
        }
    }
}

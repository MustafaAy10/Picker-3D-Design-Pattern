using DevelopmentKit.Base.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Picker.Services
{
    public class InGameMessageBroadcaster : IDestructible
    {
        public delegate void ScoreMessageDelegate(int score);
        public delegate void GateOpenMessageDelegate();
        public delegate void GameOverDelegate();

        private event ScoreMessageDelegate onScoreChange;
        private event GateOpenMessageDelegate onGateOpen;

        public event GameOverDelegate OnGameOver;

        public event ScoreMessageDelegate OnScoreChange
        {
            add
            {
                onScoreChange += value;
                scoreDelegates.Add(value);
            }
            remove 
            {
                onScoreChange -= value;    
                scoreDelegates.Remove(value); 
            }
        }

        public event GateOpenMessageDelegate OnGateOpen
        {
            add
            {
                onGateOpen += value;
                gateOpenDelegates.Add(value);
            }
            remove
            {
                onGateOpen -= value;
                gateOpenDelegates.Remove(value);
            }
        }

        private List<ScoreMessageDelegate> scoreDelegates;
        private List<GateOpenMessageDelegate> gateOpenDelegates;

        public InGameMessageBroadcaster()
        {
            scoreDelegates = new List<ScoreMessageDelegate>();
            gateOpenDelegates = new List<GateOpenMessageDelegate>();
        }

        private void RemoveAllEvents()
        {
            foreach (var scoreChange in scoreDelegates)
            {
                onScoreChange -= scoreChange;
            }

            foreach (var gateOpen in gateOpenDelegates)
            {
                onGateOpen -= gateOpen;
            }

            scoreDelegates.Clear();
            gateOpenDelegates.Clear();
        }

        public void TriggerGameOver()
        {
            if(OnGameOver != null)
                OnGameOver();
        }

        public void TriggerScoreChange(int score)
        {
            if(onScoreChange != null)
                onScoreChange(score);
        }

        public void TriggerGateOpen()
        {
            if(onGateOpen != null)  
                onGateOpen();
        }

        public void OnDestruct()
        {
            RemoveAllEvents();
        }
    }
}
using System;
using System.Collections;
using UnityEngine;

namespace BlockBirds.Platform.Interface
{
    public interface ILeaderboard
    {
        public void SubmitScore(int score, Action callback);
        public IEnumerator ShowLeaderboard();
    }

}

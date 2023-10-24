using UnityEngine;

namespace App.Mixed
{
    public static class DataRepository
    {
        public static int BestScore
        {
            get => PlayerPrefs.GetInt("BestScore", 0);
            set => PlayerPrefs.SetInt("BestScore", value);
        }
    }
}
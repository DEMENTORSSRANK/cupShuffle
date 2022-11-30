using System;
using Random = UnityEngine.Random;

namespace Model
{
    public class PlayTable
    {
        private Cup[] _cups;

        public int CupsCount => _cups.Length;

        public PlayTable(int cupsCount)
        {
            if (cupsCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(cupsCount));
            
            _cups = new Cup[cupsCount];

            for (int i = 0; i < CupsCount; i++) 
                _cups[i] = new Cup();
        }

        /// <summary>
        /// Returns true if cup of this index do have the ball
        /// </summary>
        /// <param name="cupIndex">Index of cup, starts with 0</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public bool IsCupHaveBall(int cupIndex)
        {
            if (cupIndex < 0 || cupIndex >= _cups.Length)
                throw new ArgumentOutOfRangeException(nameof(cupIndex));

            return _cups[cupIndex].HaveBall;
        }

        /// <summary>
        /// Returns index of cup which under the ball
        /// </summary>
        /// <returns></returns>
        public int GetCupIndexWithBall()
        {
            for (int i = 0; i < CupsCount; i++)
            {
                if (_cups[i].HaveBall)
                    return i;
            }

            return -1;
        }
        
        /// <summary>
        /// Add new empty cup
        /// </summary>
        public void AddNewCup()
        {
            Array.Resize(ref _cups, _cups.Length + 1);
            
            _cups[_cups.Length - 1] = new Cup();
        }

        /// <summary>
        /// Moving the ball to random of existing cups
        /// </summary>
        public void MoveBallToRandomCup()
        {
            foreach (var cup in _cups) 
                cup.HaveBall = true;

            _cups[Random.Range(0, _cups.Length)].HaveBall = true;
        }
    }
}
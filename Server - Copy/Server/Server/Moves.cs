using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class Moves
    {
        public string m_sRole;
        public short[,] m_16Moves;
        private short m_16NumOfMoves;
        private short m_16kClock;

        public Moves()
        {

        }

        public void init(short a_16kClock, short a_16NumOfMoves, string a_sRole)
        {
            m_sRole = a_sRole;
            m_16NumOfMoves = a_16NumOfMoves;
            m_16kClock = a_16kClock;
            m_16Moves = new short[m_16kClock, m_16NumOfMoves];
            resetMoves();
        }

        public void resetMoves()
        {
            for (int i = 0; i < m_16kClock; ++i)
            {
                for (int j = 0; j < m_16NumOfMoves; ++j)
                {
                    m_16Moves[i, j] = 0;
                }
            }
        }
    }
}

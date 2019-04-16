using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    class Thief
    {
        public short[] m_16ThiefPosition { get => m_16ThiefPosition; private set => m_16ThiefPosition = value; }

        public Thief()
        {
            m_16ThiefPosition = new short[2];
            Random generator = new Random();
            m_16ThiefPosition[0] = Convert.ToInt16(generator.Next(1, 22));
            m_16ThiefPosition[1] = Convert.ToInt16(generator.Next(1, 22));

        }

        public void Move(short a_16Direction)
        {

        }
    }
}

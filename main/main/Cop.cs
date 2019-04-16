using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    class Cop
    {
        public short[] m_16CopPosition { get => m_16CopPosition; private set => m_16CopPosition = value; }
        
        public Cop()
        {
            m_16CopPosition = new short[2];
            Random generator = new Random();
            m_16CopPosition[0] = Convert.ToInt16(generator.Next(1, 22));
            m_16CopPosition[1] = Convert.ToInt16(generator.Next(1, 22));
        }

        public void Move(short a_16Direction)
        {

        }
    }
}

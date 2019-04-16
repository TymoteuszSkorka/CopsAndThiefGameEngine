using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    class Wall
    {
        public short[,] m_16WallPosition {get => m_16WallPosition; private set => m_16WallPosition = value; }
        private float m_fProbalityOfMovement;
        private float m_fProbalityOfChangingDirection;

        public Wall(short a_16SizeofWall = 4, float a_fProbalityOfMovement = 0.5f, float a_fProbalityOfChangingDirection = 0.01f)
        {
            m_16WallPosition = new short[a_16SizeofWall, 2];
            m_fProbalityOfMovement = a_fProbalityOfMovement;
            m_fProbalityOfChangingDirection = a_fProbalityOfChangingDirection;
            Random generator = new Random();
            m_16WallPosition[0, 0] = Convert.ToInt16(generator.Next(1, 22));
            m_16WallPosition[0, 1] = Convert.ToInt16(generator.Next(1, 22));
            for (int i = 0; i < a_16SizeofWall; ++i)
            {
                
            }
        }

        public void Move()
        {

        }
    }
}

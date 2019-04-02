using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    abstract class Field
    {
        private short m_16PositionX;
        private short m_16PositionY;
        private char m_cType;
        public abstract void Move();
    }
}
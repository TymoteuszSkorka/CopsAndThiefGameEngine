using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace main
{
    class Board
    {
        private short m_16NumOfRows;
        private short m_16NumOfColumns;
        private List<Field> m_listOfFields;

        public Board(short a_16NumOfRows, short a_16NumOfColumns)
        {
            m_16NumOfColumns = a_16NumOfColumns;
            m_16NumOfRows = a_16NumOfRows;
        }
    }
}

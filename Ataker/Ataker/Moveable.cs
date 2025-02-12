using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ataker
{
    public interface Moveable
    {
        bool Move(int deltaX, int deltaY, GameObject[,] grid);
        bool CheckCollision(int x, int y, GameObject[,] grid);
    }
}

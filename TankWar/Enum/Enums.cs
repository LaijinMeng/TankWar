using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.Enum
{
    public class Enums
    {
        /// <summary>
        /// 方向枚举
        /// </summary>
        public enum Direction
        {
            Up,     // 上
            Down,   // 下
            Left,   // 左
            Right   // 右
        }

        public enum MapType
        {
            Walls,
            Steels,
            Grass,
            Water
        }

    }
}

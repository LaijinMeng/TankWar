using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankWar.Enum;
using TankWar.Properties;

namespace TankWar.Entity
{
    public class MapElement : GameObject
    {
        //导入图片资源
        private static Image[] imgs = {
                                Resources.walls,
                                Resources.steels,
                                Resources.grass,
                                Resources.water
                               };

        //地图元素类型 
        public Enums.MapType mapType
        {
            get;
            set;
        }
        //是否可穿越
        public bool isPass;

        private static Image SetImg(Enums.MapType type)
        {
            Image mg;
            if (type == Enums.MapType.Walls)
            {
                mg = imgs[0];
            }
            else if (type == Enums.MapType.Steels)
            {
                mg = imgs[1];
            }
            else if (type == Enums.MapType.Grass)
            {
                mg = imgs[2];
            }
            else if (type == Enums.MapType.Water)
            {
                mg = imgs[3];
            }
            else
            {
                mg = imgs[0];
            }
                    
            return mg;
        }

        private void SetIsPass(Enums.MapType type)
        {
            switch(type)
            {
                case Enums.MapType.Walls:
                case Enums.MapType.Steels:
                    this.isPass = false;
                    break;
                case Enums.MapType.Grass:
                case Enums.MapType.Water:
                    this.isPass = true;
                    break;
            }           
        }

        public MapElement(int x,int y, Enums.MapType type)
            :base(x,y, SetImg(type))
        {
            this.mapType = type;
            SetIsPass(type);
        }
        public override void Draw(Graphics g)
        {
            g.DrawImage(this.Img, this.X, this.Y);
        }
    }
}

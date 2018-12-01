using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.Entity
{
    /// <summary>
    /// 游戏对象基类
    /// </summary>
    public abstract class GameObject
    {
        #region 公共属性
        public int X
        {
            get;
            set;
        }
        public int Y
        {
            get;
            set;
        }
        public Image Img
        {
            get;
            set;
        }
        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        } 
        #endregion


        /// <summary>
        /// 初始化游戏对象
        /// </summary>
        public GameObject(int x, int y, Image img,int width, int height)
            
        {
            this.X = x;
            this.Y = y;
            this.Img = img;
            this.Width = width;
            this.Height = height;
        }

        public GameObject(int x, int y, Image img)
            :this(x,y,img,img.Width,img.Height)
        {
        }

        #region 公有方法
        /// <summary>
        /// 抽象方法：绘制
        /// </summary>
        /// <param name="g"></param>
        public abstract void Draw(Graphics g);

        /// <summary>
        /// 获取所在区域，用于碰撞检测
        /// </summary>
        /// <returns>矩形区域</returns>
        public virtual Rectangle GetRectangle()
        {
            return new Rectangle(this.X, this.Y, this.Width, this.Height);
        }
        #endregion
    }
}

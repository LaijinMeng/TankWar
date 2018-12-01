using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankWar.Enum;

namespace TankWar.Entity
{
    public class BulletFather : GameObject
    {

        //子弹的方向
        public Enums.Direction Dir
        {
            get;
            set;
        }
        //子弹的速度
        protected int Speed
        {
            get;
            set;
        }
        //子弹等级
        public int Grade
        {
            get;
            set;
        }

        /// <summary>
        /// 根据坦克位坐标，图片大小调整子弹坐标
        /// </summary>
        /// <param name="tank"></param>
        /// <param name="img"></param>
        private void SetBullteXY(TankFather tank,Image img)
        {
            switch(tank.Dir)
            {
                case Enums.Direction.Up:
                    this.X += (int)(Math.Abs(tank.Width - img.Width) / 2);
                    break;
                case Enums.Direction.Down:
                    this.X += (int)(Math.Abs(tank.Width - img.Width) / 2);
                    this.Y += tank.Img.Height;
                    break;
                case Enums.Direction.Left:
                    this.X -= img.Width;
                    this.Y += (int)(Math.Abs(tank.Height - img.Height) / 2);
                    break;
                case Enums.Direction.Right:
                    this.X += tank.Img.Width;
                    this.Y += (int)(Math.Abs(tank.Height - img.Height) / 2);
                    break;

            }
        }

        /// <summary>
        /// 初始化子弹基类
        /// </summary>
        /// <param name="tank"></param>
        /// <param name="img"></param>
        public BulletFather(TankFather tank,Image img,int speed)
            :base(tank.X,tank.Y,img,img.Width,img.Height)   
        {
            SetBullteXY(tank, img);
            this.Dir = tank.Dir;
            this.Speed = speed;
            this.Grade = 1;
        }

        public override void Draw(Graphics g)
        {
            g.DrawImage(Img, this.X, this.Y);
        }


        /// <summary>
        /// 子弹移动
        /// </summary>
        public void Move()
        {
            switch (this.Dir)
            {
                case Enums.Direction.Up:
                    this.Y -= this.Speed;
                    break;
                case Enums.Direction.Down:
                    this.Y += this.Speed;
                    break;
                case Enums.Direction.Left:
                    this.X -= this.Speed;
                    break;
                case Enums.Direction.Right:
                    this.X += this.Speed;
                    break;
            }
        }
    }
}

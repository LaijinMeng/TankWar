using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using TankWar.Enum;
using TankWar.Properties;
using System.Collections.Generic;

namespace TankWar.Entity
{
    /// <summary>
    /// 坦克基类
    /// </summary>
    public abstract class TankFather : GameObject
    {
        // 绘制坦克需要的图片数组
        protected Image[] imgs
        {
            get;
            set;
        }

        //坦克的方向
        public Enums.Direction Dir
        {
            get;
            set;
        }
        //坦克的速度
        protected int Speed
        {
            get;
            set;
        }
        //坦克生命值
        protected int Life
        {
            get;
            set;
        }

        protected int bornTime = 0;
        protected bool canMove = false;

        /// <summary>
        /// 初始化坦克父类对象
        /// </summary>
        public TankFather(int x, int y, Image[] imgs,Enums.Direction dir,int speed,int life)
            :base(x,y,imgs[0], imgs[0].Width, imgs[0].Height)
        {
            this.imgs = imgs;
            this.Dir = dir;
            this.Speed = speed;
            this.Life = life;
        }

        // 抽象方法：具体的坦克子类去实现
        public abstract void Fire();

        /// <summary>
        /// 虚方法：移动
        /// </summary>
        public virtual void Move()
        {
            bornTime++;
            if (bornTime % 20 == 0)
            {
                canMove = true;
            }
            if (canMove)
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
                // 在游戏对象移动完成后判断一下:当前游戏对象是否超出当前的窗体 
                if (this.X <= 0)
                {
                    this.X = 0;
                }
                if (this.Y <= 0)
                {
                    this.Y = 0;
                }
                if (this.X >= 720)
                {
                    this.X = 720;
                }
                if (this.Y >= 540)
                {
                    this.Y = 540;
                }
            }

        }

        /// <summary>
        /// 坦克被击中
        /// </summary>
        public void Hited(BulletFather bullet)
        {           
            this.Life -= bullet.Grade;//扣除生命值
        }

        /// <summary>
        /// 绘制坦克
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g)
        {
            bornTime++;
            if (bornTime % 20 == 0)
            {
                canMove = true;
            }
            if (canMove)
            {
                switch (this.Dir)
                {
                    case Enums.Direction.Up:
                        g.DrawImage(imgs[0], this.X, this.Y);
                        break;
                    case Enums.Direction.Down:
                        g.DrawImage(imgs[1], this.X, this.Y);
                        break;
                    case Enums.Direction.Left:
                        g.DrawImage(imgs[2], this.X, this.Y);
                        break;
                    case Enums.Direction.Right:
                        g.DrawImage(imgs[3], this.X, this.Y);
                        break;
                }
            }
            
        }

        /// <summary>
        /// 获取坦克的生命值
        /// </summary>
        /// <returns></returns>
        public int GetLife()
        {
            return this.Life;
        }

    }
}

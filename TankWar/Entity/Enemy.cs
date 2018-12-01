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
    public class Enemy : TankFather
    {

        private static Image[] imgs1 = {
                                       Resources.enemy1U,
                                       Resources.enemy1D,
                                       Resources.enemy1L,
                                       Resources.enemy1R
                                       };
        private static Image[] imgs2 = {
                                       Resources.enemy2U,
                                       Resources.enemy2D,
                                       Resources.enemy2L,
                                       Resources.enemy2R
                                       };
        private static Image[] imgs3 = {
                                       Resources.enemy3U,
                                       Resources.enemy3D,
                                       Resources.enemy3L,
                                       Resources.enemy3R
                                       };

        // 电脑坦克类型
        private int EnemyType;
        //电脑子弹速度
        private int BulletSpeed = 10;

        // 随机数
        private static Random ro = new Random();

        private static Image[] SetImgs(int type)
        {
            Image[] _img = new Image[] { };
            switch (type)
            {
                case 0:
                    _img= imgs1;
                    break;
                case 1:
                    _img= imgs2;
                    break;
                case 2:
                    _img= imgs3;
                    break;
            }
            return _img;
        }

        private static int SetLife(int type)
        {
            int _life = 0;
            switch (type)
            {
                case 0:
                    _life = 1;
                    break;
                case 1:
                    _life = 2;
                    break;
                case 2:
                    _life = 3;
                    break;
            }
            return _life;
        }

        private static int SetSpeed(int type)
        {
            int _speed = 0;
            switch (type)
            {
                case 0:
                    _speed = 5;
                    break;
                case 1:
                    _speed = 10;
                    break;
                case 2:
                    _speed = 5;
                    break;
            }
            return _speed;
        }

        /// <summary>
        /// 初始化敌方坦克
        /// </summary>
        public Enemy(int x, int y, Enums.Direction dir, int type)
            : base(x, y, SetImgs(type), dir, SetSpeed(type), SetLife(type))
        {
            this.EnemyType = type;
        }

        /// <summary>
        /// 敌方坦克移动
        /// </summary>
        public void Move(List<EnemyBullet> eBulletList,List<MapElement> mapList)
        {           
            if (canMove)
            {
                int x = 0;
                int y = 0;
                switch (this.Dir)
                {
                    case Enums.Direction.Up:
                        x = this.X;
                        y = this.Y - this.Speed;
                        if (CanMoveTo(mapList, x,y))
                        {
                            this.Y -= this.Speed;
                        }                        
                        break;
                    case Enums.Direction.Down:
                        x = this.X;
                        y = this.Y + this.Speed;
                        if (CanMoveTo(mapList, x, y))
                        {
                            this.Y += this.Speed;
                        }                        
                        break;
                    case Enums.Direction.Left:
                        x = this.X- this.Speed;
                        y = this.Y;
                        if (CanMoveTo(mapList, x, y))
                        {
                            this.X -= this.Speed;
                        }                        
                        break;
                    case Enums.Direction.Right:
                        x = this.X + this.Speed;
                        y = this.Y;
                        if (CanMoveTo(mapList, x, y))
                        {
                            this.X += this.Speed;
                        }                       
                        break;
                }
                // 在游戏对象移动完成后判断一下:当前游戏对象是否超出当前的窗体 
                if (this.X <= 0)
                {
                    this.X = 0;
                    ReSetDirection();//超出当前窗体就重置方向
                }
                if (this.Y <= 0)
                {
                    this.Y = 0;
                    ReSetDirection();//超出当前窗体就重置方向
                }
                if (this.X >= 720)
                {
                    this.X = 720;
                    ReSetDirection();//超出当前窗体就重置方向
                }
                if (this.Y >= 540)
                {
                    this.Y = 540;
                    ReSetDirection();//超出当前窗体就重置方向
                }

                // 给一个很小的概率 使改变移动方向
                if (ro.Next(0, 100) < 5)
                {
                    ReSetDirection();
                }
                // 给一个很小的概率 开火
                if (ro.Next(0, 500) < 5)
                {
                    Fire(eBulletList);
                }
            }
            

        }

        public override void Fire()
        {
            //throw new NotImplementedException();
        }

        public void Fire(List<EnemyBullet> eBulletList)
        {
            //to do添加子弹
            EnemyBullet bullet = new EnemyBullet(this,this.BulletSpeed);
            eBulletList.Add(bullet);
        }

        /// <summary>
        /// 重置方向
        /// </summary>
        public void ReSetDirection()
        {
            List<Enums.Direction> dirs = new List<Enums.Direction> { Enums.Direction.Up, Enums.Direction.Down, Enums.Direction.Left, Enums.Direction.Right };
            dirs.Remove(this.Dir);
            this.Dir = dirs[ro.Next(0, 3)];
            switch (this.Dir)
            {
                case Enums.Direction.Up:
                    this.Dir = Enums.Direction.Right;
                    break;
                case Enums.Direction.Down:
                    this.Dir = Enums.Direction.Left;
                    break;
                case Enums.Direction.Left:
                    this.Dir = Enums.Direction.Up;
                    break;
                case Enums.Direction.Right:
                    this.Dir = Enums.Direction.Down;
                    break;
            }
        }

        public void Born(List<TankBorn> bornList)
        {
            TankBorn borm = new TankBorn(this.X, this.Y);
            bornList.Add(borm);
        }

        private bool CanMoveTo(List<MapElement> mapList,int x,int y)
        {
            bool can = true;
            Rectangle tk = new Rectangle(x, y, this.Width, this.Height);
            foreach (var item in mapList)
            {
                if (tk.IntersectsWith(item.GetRectangle()))
                {
                    if (item.mapType == Enum.Enums.MapType.Walls || item.mapType == Enum.Enums.MapType.Steels)
                    {
                        can = false;
                        break;
                    }                   
                }   
            }
            return can;
        }
    }
}

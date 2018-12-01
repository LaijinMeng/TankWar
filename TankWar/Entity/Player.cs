using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using TankWar.Enum;
using TankWar.Properties;

namespace TankWar.Entity
{
    public class Player : TankFather
    {
        private static Image[] imgs = {
                              Resources.p1tankU,
                              Resources.p1tankD,
                              Resources.p1tankL,
                              Resources.p1tankR
                               };
        private int BulletSpeed = 10;
        /// <summary>
        /// 初始化玩家 初始坐标(240,540),初始方向Up,初始速度5,生命值1
        /// </summary>
        public Player()
            :base( 240, 540, imgs, Enums.Direction.Up, 5, 1)
        {

        }
        //public override void Create()
        //{
        //    throw new NotImplementedException();
        //}

        public override void Fire()
        {
            //SoundPlayer sp = new SoundPlayer(Resources.hit);
            //sp.Play();
            //to do添加子弹
        }

        public void Fire(List<PlayerBullet> pBulletList)
        {
            SoundPlayer sp = new SoundPlayer(Resources.hit);
            sp.Play();
            //to do添加子弹
            PlayerBullet bullet = new PlayerBullet(this, this.BulletSpeed);
            pBulletList.Add(bullet);
        }
        /// <summary>
        /// 玩家通过键盘移动
        /// </summary>
        /// <param name="e"></param>
        public void MoveByKey(Keys key, List<MapElement> mapList)
        {
            if (canMove)
            {
                int x = 0;
                int y = 0;
                switch (key)
                {
                    case Keys.W:
                        x = this.X;
                        y = this.Y - this.Speed;
                        this.Dir = Enums.Direction.Up;
                        if (CanMoveTo(mapList, x, y))
                        {
                            base.Move();
                        }
                        break;
                    case Keys.S:
                        x = this.X;
                        y = this.Y + this.Speed;
                        this.Dir = Enums.Direction.Down;
                        if (CanMoveTo(mapList, x, y))
                        {
                            base.Move();
                        }
                        break;
                    case Keys.A:
                        x = this.X - this.Speed;
                        y = this.Y;
                        this.Dir = Enums.Direction.Left;
                        if (CanMoveTo(mapList, x, y))
                        {
                            base.Move();
                        }
                        break;
                    case Keys.D:
                        x = this.X + this.Speed;
                        y = this.Y;
                        this.Dir = Enums.Direction.Right;
                        if (CanMoveTo(mapList, x, y))
                        {
                            base.Move();
                        }
                        break;
                    case Keys.K:
                        Fire();
                        break;
                }
            }           
        }

        public void Born(List<TankBorn> bornList)
        {
            TankBorn borm = new TankBorn(this.X, this.Y);
            bornList.Add(borm);
        }

        private bool CanMoveTo(List<MapElement> mapList, int x, int y)
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

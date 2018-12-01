using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankWar.Properties;

namespace TankWar.Entity
{
    /// <summary>
    /// 产生坦克时的闪烁效果类
    /// </summary>
    public class TankBorn : GameObject
    {
        // 导入闪烁的图片数组
        private static Image[] imgs = {
                                   Resources.born1,
                                   Resources.born2,
                                   Resources.born3,
                                   Resources.born4
                               };

        public TankBorn(int x, int y)
            : base(x, y,imgs[0], imgs[0].Width, imgs[0].Height)
        {
        }

        private int time = 0;

        public void Draw(Graphics g, List<TankBorn> bornList)
        {
            time++;
            for (int i = 0; i < imgs.Length; i++)
            {
                switch (time % 10)
                {
                    case 1:
                        g.DrawImage(imgs[0], this.X, this.Y);
                        break;
                    case 3:
                        g.DrawImage(imgs[1], this.X, this.Y);
                        break;
                    case 5:
                        g.DrawImage(imgs[2], this.X, this.Y);
                        break;
                    case 7:
                        g.DrawImage(imgs[3], this.X, this.Y);
                        break;
                }

            }
            // 将闪烁图片从集合中移除
            if (time % 15 == 0)
            {
                bornList.Remove(this);
            }
        }

        public override void Draw(Graphics g)
        {
            //throw new NotImplementedException();
        }
    }
}

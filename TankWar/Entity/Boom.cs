using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using TankWar.Properties;

namespace TankWar.Entity
{
    public class Boom : GameObject
    {
        //导入图片资源
        private static Image[] imgs = {
                                Resources.blast1,
                                Resources.blast2,
                                Resources.blast3,
                                Resources.blast4,
                                Resources.blast5,
                                Resources.blast6,
                                Resources.blast7,
                                Resources.blast8,
                               };

        public Boom(int x, int y)
            : base(x,y,imgs[0], imgs[0].Width, imgs[0].Height)
        {

        }

        public override void Draw(System.Drawing.Graphics g)
        {
            //播放爆炸音效
            SoundPlayer sp = new SoundPlayer(Resources.fire);
            sp.Play();

            for (int i = 0; i < imgs.Length; i++)
            {
                g.DrawImage(imgs[i], this.X, this.Y);
            }
        }
    }
}

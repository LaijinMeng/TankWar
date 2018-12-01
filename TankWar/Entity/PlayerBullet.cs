using System.Drawing;
using TankWar.Properties;

namespace TankWar.Entity
{
    public class PlayerBullet:BulletFather
    {
        // 导入玩家子弹图片
        private new static Image Img = Resources.tankmissile;

        public PlayerBullet(Player player,int speed)
            :base(player as TankFather, Img, speed)
        {

        }

        public void SetBulletGrade(int grade)
        {
            this.Grade += grade;
        }
    }
}

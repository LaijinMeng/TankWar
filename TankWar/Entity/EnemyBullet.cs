using System.Drawing;
using TankWar.Properties;

namespace TankWar.Entity
{
    public class EnemyBullet: BulletFather
    {
        // 导入玩家子弹图片
        private new static Image Img = Resources.enemymissile;

        public EnemyBullet(Enemy enemy, int speed)
            : base(enemy as TankFather, Img, speed)
        {

        }
    }
}

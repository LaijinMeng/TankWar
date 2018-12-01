using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TankWar.Entity;
using TankWar.Enum;
using TankWar.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TankWar
{
    public partial class F_Main : Form
    {
        public static Graphics g = null;
        // 随机数
        private static Random ro = new Random();
        //玩家
        private Player player;
        //存储玩家子弹
        private List<PlayerBullet> pBulletList = new List<PlayerBullet>();
        //存储敌方坦克
        private List<Enemy> enemyTankList = new List<Enemy>();
        //敌方坦克最大数量
        private int maxEnemyCount = 20;
        private int createdEnemyCount = 0;
        //存储电脑子弹
        private List<EnemyBullet> eBulletList = new List<EnemyBullet>();
        //存储爆炸效果
        private List<Boom> boomList = new List<Boom>();
        //坦克产生闪烁动画
        private List<TankBorn> bornList = new List<TankBorn>();
        //存储地图元素数据
        private List<MapElement> mapList = new List<MapElement>();
        //记录下按下的按键，优化按键操作
        private List<Keys> KeyDownList = new List<Keys>();

        public F_Main()
        {
            InitializeComponent();
           
            InitGame();
        }

        /// <summary>
        /// 初始化游戏
        /// </summary>
        private void InitGame()
        {
            //初始化地图数据
            InitMap();
            //初始化玩家
            player = new Player();
            player.Born(bornList);

            //初始化敌方坦克
            CreateEnemy(true);

            //初始化timer控件
            this.tm_main.Interval = 50;
            this.tm_main.Enabled = true;

        }

        private void F_Main_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            if (mapList != null)
            {
                foreach (MapElement me in mapList)
                {
                    me.Draw(g);
                }
            }
            //绘制玩家
            if (player != null)
            {
                player.Draw(g);
            }
            //绘制玩家子弹
            if (pBulletList != null)
            {
                foreach (PlayerBullet pBullet in pBulletList)
                {
                    pBullet.Draw(g);
                }
            }
            //绘制敌方坦克
            if (enemyTankList != null)
            {
                foreach (Enemy enemy in enemyTankList)
                {
                    enemy.Draw(g);
                }
            }
            //绘制敌方子弹
            if (eBulletList != null)
            {
                foreach (EnemyBullet eBullet in eBulletList)
                {
                    eBullet.Draw(g);
                }
            }
            //绘制爆炸效果
            if (boomList != null)
            {
                for (int i = 0; i < boomList.Count; i++)
                {
                    boomList[i].Draw(g);
                    boomList.Remove(boomList[i]);
                }
            }
            //绘制出生效果
            if (bornList != null)
            {
                for (int i = 0; i < bornList.Count; i++)
                {
                    bornList[i].Draw(g, bornList);
                }
            }
        }

        /// <summary>
        /// 按下键盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void F_Main_KeyDown(object sender, KeyEventArgs e)
        {
            #region MyRegion
            //switch (e.KeyCode)
            //{
            //    case Keys.W:
            //        player.Dir = Enum.Enums.Direction.Up;
            //        player.Move();
            //        break;
            //    case Keys.S:
            //        player.Dir = Enum.Enums.Direction.Down;
            //        player.Move();
            //        break;
            //    case Keys.A:
            //        player.Dir = Enum.Enums.Direction.Left;
            //        player.Move();
            //        break;
            //    case Keys.D:
            //        player.Dir = Enum.Enums.Direction.Right;
            //        player.Move();
            //        break;
            //    case Keys.K:
            //        player.Fire(pBulletList);
            //        break;
            //} 
            #endregion

            switch (e.KeyCode)
            {
                case Keys.W:
                case Keys.S:
                case Keys.A:
                case Keys.D:
                    if (!KeyDownList.Contains(e.KeyCode))
                    {
                        KeyDownList.Add(e.KeyCode);
                    }
                    break;
                case Keys.K:
                    player.Fire(pBulletList);
                    break;
            }
        }
        /// <summary>
        /// 松开按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void F_Main_KeyUp(object sender, KeyEventArgs e)
        {
            if (KeyDownList.Contains(e.KeyCode))
            {
                KeyDownList.Remove(e.KeyCode);
            }
        }

        private void tm_main_Tick(object sender, EventArgs e)
        {
            //碰撞
            Collision();

            //优化玩家移动
            PlayMove();

            //玩家子弹移动
            if (pBulletList != null)
            {
                foreach (PlayerBullet pBullet in pBulletList)
                {
                    pBullet.Move();
                }
            }
            //敌方坦克移动
            if (enemyTankList != null)
            {
                foreach (Enemy enemy in enemyTankList)
                {
                    enemy.Move(eBulletList,mapList);
                }
            }
            // 敌方子弹移动
            if (eBulletList != null)
            {
                foreach (EnemyBullet eBullet in eBulletList)
                {
                    eBullet.Move();
                }
            }
            //坦克被击中
            TankHited();

            MapElementHited();

            // 对窗体进行重新绘制
            this.Invalidate();
            
        }

        /// <summary>
        /// 生产敌方坦克
        /// </summary>
        private void CreateEnemy(bool isInit=false)
        {
            int count = 0;
            if (isInit)
            {
                count = 6;//初始化的时候生成6个
            }
            else
            {
                if (createdEnemyCount >= maxEnemyCount)
                {
                    return;
                }
                count = 1;
            }
            for (int i = 0; i < count; i++)
            {
                int x = 0;
                int y = 0;
                Enums.Direction dir = Enums.Direction.Up;
                int type = ro.Next(0, 3);//随机坦克类型
                switch (ro.Next(0, 3))//敌方坦克三个随机出生点
                {
                    case 0:
                        x = 0;
                        break;
                    case 1:
                        x = 360;
                        break;
                    case 2:
                        x = 720;
                        break;
                }
                switch ( ro.Next(0,4))//随机方向
                {
                    case 0:
                        dir = Enums.Direction.Up;
                        break;
                    case 1:
                        dir = Enums.Direction.Down;
                        break;
                    case 2:
                        dir = Enums.Direction.Left;
                        break;
                    case 3:
                        dir = Enums.Direction.Right;
                        break;
                }

                Enemy etank = new Enemy(x, y, dir, type);
                enemyTankList.Add(etank);
                createdEnemyCount++;
                etank.Born(bornList);
            }
        }


        /// <summary>
        /// 坦克被击中
        /// </summary>
        private void TankHited()
        {
            //敌方坦克被击中
            for (int i = 0; i < pBulletList.Count; i++)
            {
                for (int j = 0; j < enemyTankList.Count; j++)
                {
                    if(enemyTankList[j].GetRectangle().IntersectsWith(pBulletList[i].GetRectangle()))
                    {
                        enemyTankList[j].Hited(pBulletList[i] as BulletFather);
                        if (enemyTankList[j].GetLife() <= 0)
                        {
                            Boom boom = new Boom(enemyTankList[j].X, enemyTankList[j].Y);
                            boomList.Add(boom);//存入爆炸列表
                            enemyTankList.Remove(enemyTankList[j]);//移除敌方坦克
                            CreateEnemy();
                        }                        
                        pBulletList.Remove(pBulletList[i]);//移除子弹
                        break;
                    }
                }
            }
            //我方坦克被击中
            for (int i = 0; i < eBulletList.Count; i++)
            {
                if (eBulletList[i].GetRectangle().IntersectsWith(this.player.GetRectangle()))
                {
                    player.Hited(eBulletList[i] as BulletFather);
                    Boom boom = new Boom(player.X, player.Y);
                    boomList.Add(boom);//存入爆炸列表
                    eBulletList.Remove(eBulletList[i]);
                    player = new Player();
                    break;
                }
            }
        }

        /// <summary>
        /// 碰撞
        /// </summary>
        private void Collision()
        {
            //电脑坦克碰撞
            for (int i = 0; i < enemyTankList.Count; i++)
            {
                //电脑和玩家相撞
                if(enemyTankList[i].GetRectangle().IntersectsWith(player.GetRectangle()))
                {
                    enemyTankList[i].ReSetDirection();//重新设置方向
                }

                //电脑与电脑相撞
                for (int j = i+1; j < enemyTankList.Count; j++)
                {
                    if (enemyTankList[i].GetRectangle().IntersectsWith(enemyTankList[j].GetRectangle()))
                    {
                        enemyTankList[i].ReSetDirection();//重新设置方向
                    }
                }
            }
            //电脑与元素相撞
            for (int i = 0; i < enemyTankList.Count; i++)
            {
                //电脑与电脑相撞
                for (int j = 0; j < mapList.Count; j++)
                {
                    if (enemyTankList[i].GetRectangle().IntersectsWith(mapList[j].GetRectangle()) && 
                        (mapList[j].mapType == Enum.Enums.MapType.Walls || mapList[j].mapType == Enum.Enums.MapType.Steels))
                    {
                        enemyTankList[i].ReSetDirection();//重新设置方向
                    }
                }
            }

        }

        private void F_Main_Load(object sender, EventArgs e)
        {
            // 播放开始音乐
            SoundPlayer sp = new SoundPlayer(Resources.start);
            sp.Play();
            // 控制控件使其不闪烁
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.AllPaintingInWmPaint, true);
        }

        /// <summary>
        /// 初始化地图元素
        /// </summary>
        private void InitMap()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"MapData\map.txt"))
            {
                string str = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"MapData\map.txt");
                JArray ja = (JArray)JsonConvert.DeserializeObject(str);
                foreach (var item in ja)
                {
                    Enums.MapType mt = Enums.MapType.Walls;
                    switch (item["type"].ToString())
                    {
                        case "Walls":
                            mt = Enums.MapType.Walls;
                            break;
                        case "Steels":
                            mt = Enums.MapType.Steels;
                            break;
                        case "Grass":
                            mt = Enums.MapType.Grass;
                            break;
                        case "Water":
                            mt = Enums.MapType.Water;
                            break;
                    }
                    int x = Convert.ToInt32(item["X"]) * 60;
                    int y = Convert.ToInt32(item["Y"]) * 60;
                    MapElement me = new MapElement(x, y, mt);
                    mapList.Add(me);
                }
                
            }
        }

        private void MapElementHited()
        {
            for (int i = eBulletList.Count-1; i >= 0; i--)
            {
                for (int j = mapList.Count-1; j >= 0 ; j--)
                {
                    if (eBulletList[i].GetRectangle().IntersectsWith(mapList[j].GetRectangle()))
                    {
                        if (mapList[j].mapType == Enum.Enums.MapType.Walls)
                        {
                            Boom boom = new Boom(mapList[j].X, mapList[j].Y);
                            boomList.Add(boom);//存入爆炸列表
                            mapList.Remove(mapList[j]);
                            eBulletList.Remove(eBulletList[i]);//移除子弹
                            break;
                        }
                        else if (mapList[j].mapType == Enum.Enums.MapType.Steels)
                        {
                            eBulletList.Remove(eBulletList[i]);//移除子弹
                            break;
                        }
                    }
                }
            }

            for (int i = pBulletList.Count-1; i >= 0; i--)
            {
                for (int j = mapList.Count-1; j >= 0; j--)
                {
                    if (pBulletList[i].GetRectangle().IntersectsWith(mapList[j].GetRectangle()))
                    {
                        if (mapList[j].mapType == Enum.Enums.MapType.Walls || (mapList[j].mapType == Enum.Enums.MapType.Steels && pBulletList[i].Grade >= 2))
                        {
                            Boom boom = new Boom(mapList[j].X, mapList[j].Y);
                            boomList.Add(boom);//存入爆炸列表
                            mapList.Remove(mapList[j]);
                            pBulletList.Remove(pBulletList[i]);//移除子弹
                            break;
                        }
                        else if(mapList[j].mapType == Enum.Enums.MapType.Steels && pBulletList[i].Grade <2)
                        {
                            pBulletList.Remove(pBulletList[i]);//移除子弹
                            break;
                        }
                            
                    }
                }
            }

        }

        /// <summary>
        /// 优化玩家移动
        /// </summary>
        private void PlayMove()
        {
            if (KeyDownList.Contains(Keys.W))
            {               
                player.MoveByKey(Keys.W,mapList);
            }
            if (KeyDownList.Contains(Keys.S))
            {               
                player.MoveByKey(Keys.S, mapList);
            }
            if (KeyDownList.Contains(Keys.A))
            {
                player.MoveByKey(Keys.A, mapList);
            }
            if (KeyDownList.Contains(Keys.D))
            {
                player.MoveByKey(Keys.D, mapList);
            }
            if (KeyDownList.Contains(Keys.K))
            {
                player.Fire(pBulletList);
            }           
        }


    }
}

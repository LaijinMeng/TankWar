using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TankWar.TestDemo.Properties;

namespace TankWar.TestDemo
{
    public partial class Form1 : Form
    {
        //玩家坦克图片
        private static Image[] imgs_play = {
            Resources.p1tankU,
            Resources.p1tankD,
            Resources.p1tankL,
            Resources.p1tankR,
                               };
        private int p_width = 60;//玩家图片宽度
        private int p_height = 60;//玩家图片高度
        private static Image imgs_pbullet = Resources.tankmissile;//子弹图片
        private int b_width = 17;//子弹图片宽度
        private int b_height = 17;//子弹图片高度
        private int p_speed = 10;//玩家速度
        private int p_x = 0;//玩家x坐标
        private int p_y = 0;//玩家y坐标
        private int p_direction = 0;//玩家方向  0向上，1向下，2向左，3向右

        //private int b_x = 6;//子弹x坐标
        //private int b_y = 6;//子弹y坐标
        //private int b_direction = 0;//子弹方向  0向上，1向下，2向左，3向右
        private int b_speed = 15;//子弹速度（最好比paly大）  


        private List<int[]> listBullet = new List<int[]>();//存放画布中的子弹
        private int e_speed = 10;//敌方速度


        

        //敌方坦克图片
        private static Image[] imgs_enemy = {
            Resources.enemy2U,
            Resources.enemy2D,
            Resources.enemy2L,
            Resources.enemy2R,
                               };
        private int e_width = 60;//敌方图片宽度
        private int e_height = 60;//敌方图片高度
        private List<int[]> listEnemyTank = new List<int[]>();//存放画布中地方坦克

        //爆炸效果图片
        private Image[] imgs_boom = {
                                Resources.blast1,
                                Resources.blast2,
                                Resources.blast3,
                                Resources.blast4,
                                Resources.blast5,
                                Resources.blast6,
                                Resources.blast7,
                                Resources.blast8,
                               };

        private List<int[]> listBoom = new List<int[]>();//存放爆炸效果坐标

        private static Random ro = new Random();

        public Form1()
        {
            InitializeComponent();

            //初始化玩家数据
            p_x = 300;
            p_y = 300;
            p_direction = 0;

            CreateEnemyTank();//生成敌方坦克数据

            this.timer1.Interval = 100;
            this.timer1.Enabled = true;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //g.DrawImage(imgs[0], 0, 0);//绘制图片到(0,0)位置，(0,0)对应的是图片imgs[0]的左上角

            //绘制玩家坦克
            g.DrawImage(imgs_play[p_direction], p_x, p_y);

            //绘制地方坦克
            DrawEnemyTank(g);

            //绘制爆炸效果
            DrawBoom(g);

            //绘制子弹
            DrawPlayBullet(g);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //击中敌方坦克检测
            HitEnemyDetection();
            //每次重绘就移动子弹
            PlayBulletMove();
            //敌方坦克移动
            EnemyMove();
            // 对窗体进行重新绘制
            this.Invalidate();
        }

        //键盘按下事件
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    p_direction = 0;
                    p_y = p_y - p_speed;
                    break;
                case Keys.S:
                    p_direction = 1;
                    p_y = p_y + p_speed;
                    break;
                case Keys.A:
                    p_direction = 2;
                    p_x = p_x - p_speed;
                    break;
                case Keys.D:
                    p_direction = 3;
                    p_x = p_x + p_speed;
                    break;
                case Keys.K:
                    AddPlayBullet(p_direction, p_x, p_y);
                    break;
            }
        }

        /// <summary>
        /// 添加玩家子弹
        /// </summary>
        /// <param name="b_direction">子弹方向</param>
        /// <param name="p_x">玩家的x坐标</param>
        /// <param name="p_y">玩家的y坐标</param>
        private void AddPlayBullet(int b_direction,int p_x, int p_y)
        {
            int b_x = p_x;
            int b_y = p_y;
            //因为图片的大小，玩家方向，需要调整下子弹的位置，让其再玩家前正中方
            switch (b_direction)
            {
                case 0:
                    b_x += (int)(Math.Abs(b_width- p_width)/2);
                    b_y -= imgs_pbullet.Height;
                    break;
                case 1:
                    b_x += (int)(Math.Abs(b_width - p_width) / 2);
                    b_y += imgs_play[b_direction].Height;
                    break;
                case 2:
                    b_x -= imgs_pbullet.Width;
                    b_y += (int)(Math.Abs(b_height - p_height) / 2);
                    break;
                case 3:
                    b_x += imgs_play[b_direction].Width;
                    b_y += (int)(Math.Abs(b_height - p_height) / 2);
                    break;               
            }
            int[] _b = new int[4] { b_direction, b_x, b_y, b_speed };
            listBullet.Add(_b);
        }

        /// <summary>
        /// 移动子弹
        /// </summary>
        private void PlayBulletMove()
        {
            foreach (var item in listBullet)
            {
                switch (item[0])
                {
                    case 0:
                        item[2] -= item[3];
                        break;
                    case 1:
                        item[2] += item[3];
                        break;
                    case 2:
                        item[1] -= item[3];
                        break;
                    case 3:
                        item[1] += item[3];
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// 绘制玩家子弹
        /// </summary>
        /// <param name="g"></param>
        private void DrawPlayBullet(Graphics g)
        {
            foreach (var item in listBullet)
            {
                g.DrawImage(imgs_pbullet, item[1], item[2]);     
            }
        }

        /// <summary>
        /// 绘制敌方坦克
        /// </summary>
        /// <param name="g"></param>
        private void DrawEnemyTank(Graphics g)
        {
            if (listEnemyTank != null)
            {
                foreach (var enemy in listEnemyTank)
                {
                    g.DrawImage(imgs_enemy[enemy[0]], enemy[1], enemy[2]);
                }
            }
            
        }

        /// <summary>
        /// 随机坐标生成3个坦克
        /// </summary>
        /// <param name="g"></param>
        private void CreateEnemyTank()
        {           
            for (int i = 0; i < 3; i++)
            {
                int _direction = ro.Next(0, 4);
                int _x = ro.Next(0, 600);
                int _y = ro.Next(0, 600);
                int[] enemy = new int[4] { _direction, _x, _y, e_speed };
                listEnemyTank.Add(enemy);
            }

        }

        /// <summary>
        /// 敌方坦克移动
        /// </summary>
        private void EnemyMove()
        {
            foreach (var item in listEnemyTank)
            {
                
                switch (item[0])
                {
                    case 0:
                        if (item[2] <= 0)//超过上边界
                        {
                            item[2] = 0;
                            item[0] = ro.Next(0, 4);//重新随机方向
                        }
                        else
                        {
                            item[2] -= item[3];
                        }                        
                        break;
                    case 1:
                        if (item[2] >= this.Height- e_height - 60)//超过下边界
                        {
                            item[2] = this.Height - e_height - 60;
                            item[0] = ro.Next(0, 4);//重新随机方向
                        }
                        else
                        {
                            item[2] += item[3];
                        }                        
                        break;
                    case 2:
                        if (item[1] <= 0)//超过左边界
                        {
                            item[1] = 0;
                            item[0] = ro.Next(0, 4);//重新随机方向
                        }
                        else
                        {
                            item[1] -= item[3];
                        }                       
                        break;
                    case 3:
                        if (item[1] >= this.Width - e_width - 60)//超过右边界
                        {
                            item[1] = this.Width - e_width - 60;
                            item[0] = ro.Next(0, 4);//重新随机方向
                        }
                        else
                        {
                            item[1] += item[3];
                        }                        
                        break;
                }

                // 给一个很小的概率 使改变移动方向
                if (ro.Next(0, 100) < 5)
                {
                    item[0] = ro.Next(0, 4);
                }

            }
        }

        /// <summary>
        /// 击中敌方坦克检测
        /// </summary>
        private void HitEnemyDetection()
        {
            for (int i = 0; i < listBullet.Count; i++)
            {
                for (int j = 0; j < listEnemyTank.Count; j++)
                {
                    Rectangle rec_bullet = new Rectangle(listBullet[i][1], listBullet[i][2], b_width, b_height);
                    Rectangle rec_enemy = new Rectangle(listEnemyTank[j][1], listEnemyTank[j][2], e_width, e_height);
                    if (rec_bullet.IntersectsWith(rec_enemy))
                    {
                        listBoom.Add(new[] { rec_enemy.X, rec_enemy.Y });//加入爆炸效果集合
                        listBullet.Remove(listBullet[i]);// 移除子弹
                        listEnemyTank.Remove(listEnemyTank[j]);// 移除坦克
                        
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 绘制爆炸效果
        /// </summary>
        /// <param name="g"></param>
        public void DrawBoom(Graphics g)
        {
            foreach (var item in listBoom)
            {
                for (int i = 0; i < imgs_boom.Length; i++)
                {
                    g.DrawImage(imgs_boom[i], item[0], item[1]);
                }
            }
            listBoom.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 控制控件使其不闪烁
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.AllPaintingInWmPaint, true);
        }
    }
}

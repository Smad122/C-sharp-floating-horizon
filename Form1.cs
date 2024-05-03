using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace KG4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }
        int XC = 0; int YC = 0;
        int bt = 1;
        int XKG = 0, YKG = 0;
        public class ImageRenderer
        {
            // «адание переменных дл€ хранени€ ширины и высоты изображени€
            public int imgWidth = 0;
            public int imgHeight = 0;

            // —оздание массива высот (интенсивностей)
            public int[,] heights;

            // «адание размера окна и добавление событи€ "Paint"
            public ImageRenderer(int[,] heights)
            {
                this.heights = heights;
                imgWidth = heights.GetLength(0);
                imgHeight = heights.GetLength(1);


            }


         




        }
        public class Point3d
        {
            public double x = 0;
            public double y = 0;
            public double z = 0;
            public Point3d()
            {
                x = 0; y = 0; z = 0;
            }
            public Point3d(double x, double y, double z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
        };
        Point3d offset(double X1, double Y1, double Z1, int XC, int YC, int XKG, int YKG)
        {
            double X2 = 0, Y2 = 0, Z2 = 0, X3 = 0, Y3 = 0, Z3 = 0;
            double LX = XC;
            double LY = YC;
            double LZ = 0;
            

            double COS = 0;
            double SIN = 0;
            COS = Math.Cos(LX * Math.PI / 180);
            SIN = Math.Sin(LX * Math.PI / 180);
            Y2 = Math.Round(Y1 * COS - Z1 * SIN, 5);
            Z2 = Math.Round(Y1 * SIN + Z1 * COS, 5);
            COS = Math.Cos(LY * Math.PI / 180);
            SIN = Math.Sin(LY * Math.PI / 180);
            X2 = Math.Round(X1 * COS - Z2 * SIN, 5);
            Z3 = Math.Round(X1 * SIN + Z2 * COS, 5);
            COS = Math.Cos(LZ * Math.PI / 180);
            SIN = Math.Sin(LZ * Math.PI / 180);
            X3 = Math.Round(X2 * COS - Y2 * SIN, 5);
            Y3 = Math.Round(X2 * SIN + Y2 * COS, 5);
            X3 += XKG;
            Y3 += YKG;
            Point3d p = new Point3d(0, 0, 0);
            p.x = X3;
            p.y = Y3;


            return p;

        }
        class Point2d
        {
            public int x = 0;
            public int y = 0;
            public Point2d()
            {
                x = 0; y = 0;
            }
            public Point2d(int x, int y)
            {
                this.x = x;
                this.y = y;

            }

        }
        class Horizon
        {
            public static int width;
            public Point2d[] pt = new Point2d[width];
            public Horizon(int w)
            {
                width = w;
                pt = new Point2d[width];
            }
            public int w()
            {
                return width;
            }
        }







        void DrawLineL(int x, int y, int x2, int y2, Graphics g, Pen p, Horizon up, Horizon down, double kx, double ky)
        {
            int x1 = x;
            int y1 = y;

            int xc = x, yc = y;
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);

            // ќпредел€ем направление рисовани€ линии по оси x
            int stepX = x1 < x2 ? 1 : -1;

            // ќпредел€ем направление рисовани€ линии по оси y
            int stepY = y1 < y2 ? 1 : -1;

            // ќпредел€ем значение ошибки
            int error = dx - dy;

            // Ќачинаем цикл рисовани€ линии пиксель за пикселем
            while (x1 != x2 || y1 != y2)
            {
                // –исуем текущий пиксель
                putpixel(x1, y1, xc, yc, g, p, up, down, kx, ky);
                xc = x1; yc = y1;
                // ”величиваем значение координат x и y в зависимости от направлени€ рисовани€
                int error2 = error * 2;
                if (error2 > -dy)
                {
                    error -= dy;
                    x1 += stepX;
                }
                if (error2 < dx)
                {
                    error += dx;
                    y1 += stepY;
                }
            }

            // –исуем последний пиксель
            putpixel(x1, y1, xc, yc, g, p, up, down, kx, ky);
        }
        void putpixel(int x, int y, int xc, int yc, Graphics g, Pen p, Horizon up, Horizon down, double kx, double ky)
        {
            if ((x >= 0) && (x < up.w()))
            {

                int ax = Convert.ToInt32(x * kx);
                int ay = Convert.ToInt32(y * ky);
                int xc1 = Convert.ToInt32(xc * kx);
                int yc1 = Convert.ToInt32(yc * ky);
                if (up.pt[x] == null)
                {
                    Point2d temp = new Point2d(ax, ay);
                    up.pt[x] = temp;
                    g.DrawLine(p, xc1, yc1, ax, ay);
                }
                if (down.pt[x] == null)
                {
                    Point2d temp = new Point2d(ax, ay);
                    down.pt[x] = temp;
                    g.DrawLine(p, xc1, yc1, ax, ay);
                }
                if (up.pt[x].y < ay)
                {
                    up.pt[x].y = ay;
                    g.DrawLine(p, xc1, yc1, ax, ay);
                }
                if (down.pt[x].y > ay)
                {
                    down.pt[x].y = ay;
                    g.DrawLine(p, xc1, yc1, ax, ay);
                }
            }

            //g.DrawLine(p, Convert.ToInt32(x), Convert.ToInt32(y), Convert.ToInt32(x) - 1, Convert.ToInt32(y) - 1);

        }






        public void RenderImage(ImageRenderer imag, int XC, int YC, int bt, int XKG, int YKG)
        {
            Graphics g = pictureBox1.CreateGraphics();
            SolidBrush b = new SolidBrush(Color.Black);
            g.FillRectangle(b, 0, 0, pictureBox1.Width, pictureBox1.Height);
            double kx = pictureBox1.Width / (1.414 * imag.imgWidth);
            double ky = pictureBox1.Height / (1.6 * imag.imgHeight);
            int[,] heights = new int[Convert.ToInt32(1.414 * imag.imgWidth), Convert.ToInt32(imag.imgHeight * 1.414)];

            for (int i = Convert.ToInt32(0.207 * imag.imgWidth); i < Convert.ToInt32(1.207 * (imag.imgWidth-1)); i++)
            {
                for (int j = Convert.ToInt32(imag.imgHeight * 0.207); j < Convert.ToInt32((imag.imgHeight-1) * 1.207); j++)
                {
                    if (bt==1)
                    {
                        heights[i, j] = Convert.ToInt32(imag.heights[(i+1- Convert.ToInt32(0.207 * imag.imgWidth)), j+1- Convert.ToInt32(imag.imgHeight * 0.207)] * 0.1);
                    }
                    else heights[i, j] = Convert.ToInt32(imag.heights[(i+1 - Convert.ToInt32(0.207 * imag.imgWidth)), j - (Convert.ToInt32(imag.imgHeight * 0.207)-1)] * 0.5);
                }
            }


            Point3d tempa;
            tempa = offset(0, Convert.ToInt32(1.414 * imag.imgHeight) - 1, heights[0, Convert.ToInt32(1.414 * imag.imgHeight) - 1], XC, YC, XKG, YKG);
            Point3d tempb = new Point3d(0, 0, 0);


            Color color = Color.FromArgb(100, 255, 120);
            Pen p = new Pen(color);
            Horizon down = new Horizon(Convert.ToInt32(1.414 * imag.imgWidth));
            Horizon up = new Horizon(Convert.ToInt32(1.414 * imag.imgWidth));



            for (int y = Convert.ToInt32(1.414 * imag.imgHeight) - 1; y > Convert.ToInt32(1.414 * imag.imgHeight) - 2; y--)

            {
                for (int x = 1; x < Convert.ToInt32(1.414 * imag.imgWidth); x += bt)
                {
                    tempb = offset(x, y, heights[x, y], XC, YC, XKG, YKG);
                    int txa = Convert.ToInt32(tempa.x);
                    int tya = Convert.ToInt32(tempa.y);
                    int txb = Convert.ToInt32((tempb.x));
                    int tyb = Convert.ToInt32((tempb.y));
                    DrawLineL(txa, tya, txb, tyb, g, p, up, down, kx, ky);
                    tempa = tempb;









                }
            }


            for (int y = Convert.ToInt32(1.414 * imag.imgHeight) - 2; y > 0; y -= bt)

            {
                for (int x = 1; x < Convert.ToInt32(1.414 * imag.imgWidth); x += bt)
                {


                    tempb = offset(x, y, heights[x, y], XC, YC, XKG, YKG);
                    int txa = Convert.ToInt32(tempa.x);
                    int tya = Convert.ToInt32(tempa.y);
                    int txb = Convert.ToInt32((tempb.x));
                    int tyb = Convert.ToInt32((tempb.y));
                    DrawLineL(txa, tya, txb, tyb, g, p, up, down, kx, ky);
                    tempa = tempb;



                }

            }





        }

        public static int[,] ReadImage(string path)
        {
            Bitmap bmp = new Bitmap(path);
            int[,] heights = new int[bmp.Width, bmp.Height];
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {

                    Color color = bmp.GetPixel(x, y);
                    int intensity = (color.R + color.G + color.B) / 3;



                    heights[x, y] = intensity;
                }
            }bmp.Dispose();
            return heights;
        }
        private void button1_Click(object sender, EventArgs e)
        {

            int[,] heights = ReadImage("Path to image");



            ImageRenderer renderer = new ImageRenderer(heights);
            XC = Convert.ToInt32(textBox1.Text);
            YC = Convert.ToInt32(textBox2.Text);
            bt = 1;
            XKG = Convert.ToInt32(50 * Math.Sin(Math.PI * YC / 180));
            YKG = Convert.ToInt32(50 * Math.Sin(Math.PI * XC / 180));
            RenderImage(renderer, XC, YC, bt, XKG, YKG);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int[,] heights = ReadImage("Path to image");



            ImageRenderer renderer = new ImageRenderer(heights);
            XC = Convert.ToInt32(textBox1.Text);
            YC = Convert.ToInt32(textBox2.Text);
            bt = 15;
            XKG = Convert.ToInt32(500 * Math.Sin(Math.PI * YC / 180));
            YKG = Convert.ToInt32(500 * Math.Sin(Math.PI * XC / 180));
            RenderImage(renderer, XC, YC, bt, XKG, YKG);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int[,] heights = ReadImage("Path to image");



            ImageRenderer renderer = new ImageRenderer(heights);
            XC = Convert.ToInt32(textBox1.Text);
            YC = Convert.ToInt32(textBox2.Text);
            bt = 15;
            XKG = Convert.ToInt32(500 * Math.Sin(Math.PI * YC / 180));
            YKG = Convert.ToInt32(500 * Math.Sin(Math.PI * XC / 180));
            RenderImage(renderer, XC, YC, bt, XKG, YKG);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Interop;

namespace balls
{
    /// <summary>
    /// Interaction logic for BallForm.xaml
    /// </summary>
    public partial class BallForm : Window
    {
        Thread FirstThread = null;
        Thread SecondThread = null;

        Color[,] colMat = new Color[MainWindow.BoxHeigth, MainWindow.BoxWidth];

        int colorCount = 4;

        object locker;

        public BallForm()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            for (var row = 0; row < MainWindow.BoxHeigth; row++)
            {
                for (var col = 0; col < MainWindow.BoxWidth; col++)
                {
                    colMat[row, col] = Color.White;
                }
            }

            locker = new object();

            FirstThread = new Thread(BallFill);
            FirstThread.IsBackground = true;

            FirstThread.Start(new WindowInteropHelper(this).Handle);

        }

        private Color GetLayBallColor(int r, int c)
        {
            lock(locker){
                return colMat[r, c];
            }
        }

        private void BallFill(object param)
        {
            var handle = (IntPtr)param;

            Random random = new Random();
            var gCntx = Graphics.FromHwnd(handle);
            var frame = new Bitmap(
                MainWindow.BallSize * MainWindow.BoxWidth,
                MainWindow.BallSize * MainWindow.BoxHeigth
                );
            var fCntx = Graphics.FromImage(frame);

            var cleanBrush = new SolidBrush(Color.White);

            int fallRow = 1;
            while (fallRow != MainWindow.BoxHeigth - 1)
            {

                var ballColor = Color.Black;
                switch (random.Next(colorCount))
                {
                    case 0:
                        ballColor = Color.DarkBlue;
                        break;
                    case 1:
                        ballColor = Color.Red;
                        break;
                    case 2:
                        ballColor = Color.Green;
                        break;
                    case 3:
                        ballColor = Color.Khaki;
                        break;
                    default:
                        ballColor = Color.White;
                        break;
                }

                var fallCol = random.Next(MainWindow.BoxWidth);
                var ballY = 0;
                fallRow = 1;

                do
                {
                    fCntx.FillRectangle(cleanBrush, 0, 0,
                    MainWindow.BallSize * MainWindow.BoxWidth,
                    MainWindow.BallSize * MainWindow.BoxHeigth);

                    fallRow = MainWindow.BoxHeigth - 1 - ballY / MainWindow.BallSize;



                    for (var row = 0; row < MainWindow.BoxHeigth; row++)
                    {
                        for (var col = 0; col < MainWindow.BoxWidth; col++)
                        {
                            var layBallColor = GetLayBallColor(row, col);
                            if (layBallColor != Color.White)
                            {
                                var brush = new SolidBrush(layBallColor);
                                fCntx.FillEllipse(brush, col * MainWindow.BallSize,
                                    row * MainWindow.BallSize,
                                    MainWindow.BallSize,
                                    MainWindow.BallSize);
                                brush.Dispose();
                            }
                        }
                    }

                    var ballBrush = new SolidBrush(ballColor);
                    fCntx.FillEllipse(ballBrush, fallCol * MainWindow.BallSize,
                                      ballY, MainWindow.BallSize, MainWindow.BallSize);

                    gCntx.DrawImage(frame, 0, 0);

                    ballY += 8;
                } while (fallRow > 0 && GetLayBallColor(fallRow, fallCol) == Color.White);
                lock (locker)
                {
                    colMat[fallRow, fallCol] = ballColor;
                }
            }
            fCntx.Dispose();
            gCntx.Dispose();

        }

       
    }
}

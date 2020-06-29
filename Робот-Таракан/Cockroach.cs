using Робот_Таракан;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Робот_Таракан
{
    class Cockroach
    {
        public Bitmap image; //изображение
        direction trend = direction.Left;
        //direction trend = direction.Right; //направление
        const int step = 30; //шаг
        private int x, y;

        //Конструктор с параметром – изображение
        public Cockroach(Bitmap _Image)
        {
            this.image = _Image;
        }

        //хранение и вычисление координат
        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }

        public void Step()
        {
            switch (trend)
            {
                case direction.Right:
                    x += step;
                    break;
                case direction.Down:
                    y += step;
                    break;
                case direction.Left:
                    x -= step;
                    break;
                case direction.Up:
                    y -= step;
                    break;
            }
        }

        //Изменение направления, параметр – первая буква направления
        public void ChangeTrend(char c)
        {
            direction newtrend = trend;
            for (direction y = direction.Up; y <= direction.Left; y++)
                if (char.ToLower(c) == char.ToLower(y.ToString()[0]))
                {
                    newtrend = y;
                    break;
                }
            switch (trend)
            {
                case direction.Up:
                    switch (newtrend)
                    {
                        case direction.Right:
                            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case direction.Down:
                            image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                        case direction.Left:
                            image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                    }
                    break;
                case direction.Right:
                    switch (newtrend)
                    {
                        case direction.Down:
                            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case direction.Left:
                            image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                        case direction.Up:
                            image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                    }

                    break;
                case direction.Down:
                    switch (newtrend)
                    {
                        case direction.Left:
                            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case direction.Up:
                            image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                        case direction.Right:
                            image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                    }
                    break;
                case direction.Left:
                    switch (newtrend)
                    {
                        case direction.Up:
                            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case direction.Right:
                            image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                        case direction.Down:
                            image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                    }
                    break;
            }
            trend = newtrend;
        }
    }
}


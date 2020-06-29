using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Робот_Таракан
{
    enum direction : byte { Up, Right, Down, Left }; //переменная с перечислением значений направления

    public partial class Form1 : Form
    {
        List <Cockroach> workCockroach;//рабочий Таракан - активный Таракан, который будет выполнять алгоритм
        List <PictureBox> workpb;//рабочее поле PictureBox - поле на котрой будет рабочий Таракан
        List<Cockroach> LC;//Список для хранения созданных Тараканов
        List<PictureBox> PB;//Список для хранения созданных объектов PictureBox
        int algStep = 0;
        string skin = "cockroach.png";

        public Form1()
        {
            InitializeComponent();
            workpb = new List<PictureBox>();
            workCockroach = new List<Cockroach>();

            LC = new List<Cockroach>();
            PB = new List<PictureBox>();
        }        

        private void NewBtn_Click(object sender, EventArgs e)
        {
            Cockroach cockroach = new Cockroach(new Bitmap(skin));
            cockroach.image = new Bitmap(cockroach.image, new Size(100, 100));
            PictureBox p = new PictureBox();
            p.MouseMove += new MouseEventHandler(IMouseMove);
            p.MouseDown += new MouseEventHandler(IMouseDown);
            workCockroach.Clear();
            workCockroach.Add(cockroach);
            workpb.Clear();
            workpb.Add(p);
            Show();
            PB.Add(p);
            LC.Add(cockroach);
        }

        private void IMouseDown(object sender, MouseEventArgs e) //выбор таракана - исполнителя алгоритма
        {
            if (e.Button == MouseButtons.Left)
            {
                int k = PB.IndexOf(sender as PictureBox); //запоминаем номер нажатого компонента
                if (!(ModifierKeys == Keys.Control)) //если ctrl не нажат
                {
                    for (int i = 0; i < workpb.Count; i++)
                        workpb[i].BackColor = Color.Transparent; //делаем цвет прозрачный
                    workpb.Clear();
                    workCockroach.Clear();
                }
                ((PictureBox)sender).BackColor = Color.BlueViolet; //выделение активно
                workpb.Add(sender as PictureBox);//объявляет его рабочим
                workCockroach.Add(LC[k]);//по найденному номеру находим Таракана в списке
            }
        }
        private void IMouseMove(object sender, MouseEventArgs e) //перетаскивание таракана по полю
        {
            if (e.Button == MouseButtons.Left)
            {
                PictureBox picture = sender as PictureBox;
                picture.Tag = new Point(e.X, e.Y);//запоминаем координаты мыши на момент начала перетаскивания
                picture.DoDragDrop(sender, DragDropEffects.Move);//начинаем перетаскивание ЧЕГО и с КАКИМ ЭФФЕКТОМ
            }
        }

        private void Field_DragEnter(object sender, DragEventArgs e) //при перетаскивании в пределы границ
        {
            if (e.Data.GetDataPresent(typeof(PictureBox)))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void Field_DragDrop(object sender, DragEventArgs e) //при завершении операции перетаскивания
        {
            //извлекаем PictureBox
            PictureBox picture = (PictureBox)e.Data.GetData(typeof(PictureBox));
            Panel panel = sender as Panel;
            //получаем клиентские координаты в момент отпускания кнопки
            Point pointDrop = panel.PointToClient(new Point(e.X, e.Y));
            //извлекаем клиентские координаты мыши в момент начала перетскивания
            Point pointDrag = (Point)picture.Tag;
            //вычисляем и устанавливаем Location для PictureBox в Panel
            picture.Location = pointDrop;
            //устанавливаем координаты для X и Y для рабочего таракана
            for (int i = 0; i < workCockroach.Count; i++)
            {
                workCockroach[i].X = picture.Location.X;
                workCockroach[i].Y = picture.Location.Y;
            }
            picture.Parent = panel;
        }

        private void UpBtn_Click(object sender, EventArgs e)
        {
            Algorithm.Items.Add((sender as Button).Text);
        }

        private void RightBtn_Click(object sender, EventArgs e)
        {
            Algorithm.Items.Add((sender as Button).Text);
        }

        private void DownBtn_Click(object sender, EventArgs e)
        {
            Algorithm.Items.Add((sender as Button).Text);
        }

        private void LeftBtn_Click(object sender, EventArgs e)
        {
            Algorithm.Items.Add((sender as Button).Text);
        }

        public void RePaint() //Изображение объекта Таракан в PictureBox используется при изменении направления Таракана
        {
            for (int i = 0; i < workCockroach.Count; i++)
            {
                //новые границы
                workpb[i].Bounds = new Rectangle(workCockroach[i].X, workCockroach[i].Y, workCockroach[i].image.Width, workCockroach[i].image.Height);
                workpb[i].Image = workCockroach[i].image;
            }
        }

        public new void Show() //Изображение Таракана на поле
        {
            for (int i = 0; i < workCockroach.Count; i++)
            {
                workCockroach[i].X = (Field.Width - workCockroach[i].image.Width) / 2;
                workCockroach[i].Y = (Field.Height - workCockroach[i].image.Height) / 2;
                RePaint();
                Field.Controls.Add(workpb[i]);
            }
        }

        private void RunBtn_Click(object sender, EventArgs e)
        {
            algStep = 0;
            timerAlgorithm.Start();
        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            Algorithm.Items.Clear();
            timerAlgorithm.Stop();
        }

        private void timerAlgorithm_Tick(object sender, EventArgs e)
        {
            if (algStep == Algorithm.Items.Count) //конец алгоритма
            {
                timerAlgorithm.Stop();
            }
            else //выполнение команды из списка
            {
                string s = (string)Algorithm.Items[algStep];
                Algorithm.SetSelected(algStep, true);
                if (s == "Step") //если нажата кнопка Step
                {
                    for (int i = 0; i < workCockroach.Count; i++) workCockroach[i].Step(); 
                }
                else //если нажата кнопка направления
                {
                    for (int i = 0; i < workCockroach.Count; i++) workCockroach[i].ChangeTrend(s[0]);
                }
                RePaint();
                algStep++;
            }
        }

        private void StepBtn_Click(object sender, EventArgs e)
        {
            if (workCockroach != null)
                foreach (Cockroach active in workCockroach) active.Step();
            Algorithm.Items.Add((sender as Button).Text);
        }

        private void ChangeImageBtn_Click(object sender, EventArgs e) //новый таракан будет создан с новой картинкой
        {
            OpenFileDialog file = new OpenFileDialog();
            file.ShowDialog();
            if (file.FileName != "") skin = file.FileName;
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < workCockroach.Count; i++)
            {
                workpb[i].Dispose();
                LC.Remove(workCockroach[i]);
                PB.Remove(workpb[i]);
            }
            workCockroach.Clear();
            workpb.Clear();
            Field.Refresh();
            RePaint();
        }
    }
}

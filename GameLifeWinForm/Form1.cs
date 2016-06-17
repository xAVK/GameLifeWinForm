using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace GameLifeWinForm
{
    public partial class Form1 : Form
    {
        Thread Play;
        Graphics Painter;
        cCurrentGeneration CurrentGeneration;
        public int CellSize = 20;
        int Time = 500;
        public Form1()
        {
            InitializeComponent();
            Painter = Graphics.FromHwnd(pictureBoxGameField.Handle);
            CurrentGeneration = new cCurrentGeneration();
            CurrentGeneration.Clear();
            PaintField();
        }

        private void checkBoxStart_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxStart.Checked)
            {
                checkBoxStart.Text = "Стоп";
                ButtonEnabled(false);
                Play = new Thread(() => PaintField());
                Play.Start();
            }
            else
            {
                checkBoxStart.Text = "Старт";
                ButtonEnabled(true);
                Play.Abort();
                Play = null;
            }
        }
        private void ButtonEnabled(bool State)
        {
            buttonClear.Enabled = State;
            buttonNext.Enabled = State;
            buttonRND.Enabled = State;
            buttonLoad.Enabled = State;
            buttonSave.Enabled = State;
        }
        private void PaintField()
        {
            do
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Painter.FillRectangle(GetColor(CurrentGeneration.Get(i, j).IsAlive), i * CellSize, j * CellSize, CellSize, CellSize);
                    }
                }
                if (Play != null && Play.IsAlive)
                {
                    NextGeneration();
                }
                Thread.Sleep(4);
            } while (Play != null);
        }

        private Brush GetColor(bool IsAlive)
        {
            if (IsAlive)
            {
                return Brushes.Black;
            }
            else
            {
                return Brushes.White;
            }
        }
        private void buttonClear_Click(object sender, EventArgs e)
        {
            CurrentGeneration.Clear();
            PaintField();
        }

        private void pictureBoxGameField_MouseClick(object sender, MouseEventArgs e)
        {
            if (Play == null)
            {
                int X = GetNumber(e.X);
                int Y = GetNumber(e.Y);

                if (CurrentGeneration.Get(X, Y).IsAlive)
                {
                    CurrentGeneration.Set(X, Y, false);
                }
                else
                {
                    CurrentGeneration.Set(X, Y, true);
                }
                PaintField();
            }
        }
        private int GetNumber(float X)
        {
            X /= (float)CellSize;
            return (int)X;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Play != null)
            {
                Play.Abort();
            }
        }

        private void trackBarSpeed_ValueChanged(object sender, EventArgs e)
        {
            lock (this)
            {
                Time = trackBarSpeed.Value * 100;
            }
        }
        private void NextGeneration()
        {
            cNextGeneration NextGeneration = new cNextGeneration(CurrentGeneration);
            NextGeneration.Next();
            if (!NextGeneration.CompareGeneration())
            {
                CurrentGeneration.Set(NextGeneration.Get(Time));
            }
            else
            {
                MessageBox.Show("Поколение не изменилось. Генерация остановлена");
                if (Play != null)
                {
                    Play.Abort();
                    Play = null;
                }
            }

        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            NextGeneration();
            PaintField();
        }

        private void buttonRND_Click(object sender, EventArgs e)
        {
            CurrentGeneration.RND();
            PaintField();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.SaveFileDialog Save = new SaveFileDialog();
            Save.ShowDialog();
            if (Save.FileName != "")
            {
                try
                {
                    BinaryFormatter binFormat = new BinaryFormatter();
                    using (Stream fStream = new FileStream(Save.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        binFormat.Serialize(fStream, CurrentGeneration);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка при сохранении");
                }
            }
            else
            {
                MessageBox.Show("Файл не сохранен");
            }

        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog Open = new OpenFileDialog();
            Open.ShowDialog();
            if (Open.FileName != "")
            {
                try
                {
                    BinaryFormatter binFormat = new BinaryFormatter();
                    using (Stream fStream = new FileStream(Open.FileName, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        CurrentGeneration = (cCurrentGeneration)binFormat.Deserialize(fStream);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Выбран неправильный файл");
                }
            }

            PaintField();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            CurrentGeneration.Clear();
            PaintField();
            timer1.Enabled = false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                string s1 = "Старт/Стоп - Запуск и остановка процесса генерации новых поколенний\r\n";
                string s2 = "Очистить - Очищает поле\r\n";
                string s3 = "Следующее - Отображает следующее поколение\r\n";
                string s4 = "Случайно - Случайно заполняет поле\r\n";
                string s5 = "Сохранить - Позволяет сохранить текущее поколение в файл\r\n";
                string s6 = "Загрузить - Позволяет загрузить поколение из файла\r\n";
                string s7 = "Скорость - Регулировка генерации новых поколений\r\n";

                MessageBox.Show(s1 + s2 + s3 + s4 + s5 + s6 + s7, "Справка");
            }
        }
    }
}

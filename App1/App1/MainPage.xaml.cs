using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        int[,] Matrix = new int[3, 3];

        public MainPage()
        {
            this.InitializeComponent();
            this.MaxHeight = 600;
            this.MaxWidth = 600;

            // начальное заполнение матрицы
            createMatrix();

            // проверка на решаемость и перезапись в противном случае
            if (checkMatrix() == false)
                while (!checkMatrix())
                    createMatrix();
        }

        // главная функция - клак по кнопке
        private void my_func(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;                                      // принимаем нажатую кнопку
            var number = int.Parse(button.Content.ToString());                  // получаем нажатую кнопку

            if (canWeSwap(number) != 0)                                         // проверяем, можно ли свапать
                swapTwoButtons(number, canWeSwap(number), button);              // свапаем, если можно

            if (victory()) repaintIt();
        }

        // проверка, можем ли менять, если да - то в каком направлении
        public int canWeSwap(int _number)
        {
            int[] index = new int[2];
            int[] index_zero = new int[2];

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (Matrix[i, j] == _number)
                    {
                        index[0] = i;
                        index[1] = j;
                    }

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (Matrix[i, j] == 9)
                    {
                        index_zero[0] = i;
                        index_zero[1] = j;
                    }

            // мы получили индекс нажатой кнопки и индекс нулевых элементов
            // сейчас "изи" проверочка, могут ли они меняться и едем дальше

            if ((index[0] + 1 == index_zero[0] && index[0] != 2 && index[1] == index_zero[1]) ||
                (index[0] - 1 == index_zero[0] && index[0] != 0 && index[1] == index_zero[1]))
                return 1;


            if ((index[1] + 1 == index_zero[1] && index[1] != 2 && index[0] == index_zero[0]) ||
                (index[1] - 1 == index_zero[1] && index[1] != 0 && index[0] == index_zero[0]))
                return 2;

            return 0;
        }

        // смена позиции кнопки + обновление данных в матрице
        public void swapTwoButtons(int _number, int mode, Button _but)
        {
            //int temp = 0;
            int[] index = new int[2];
            int[] index_zero = new int[2];

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (Matrix[i, j] == _number)
                    {
                        index[0] = i;
                        index[1] = j;
                    }

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (Matrix[i, j] == 9)
                    {
                        index_zero[0] = i;
                        index_zero[1] = j;
                    }

            // двигаем по вертикали
            if (mode == 1)
            {
                Grid.SetRow(_but, index_zero[0]);

                Matrix[index_zero[0], index_zero[1]] = Matrix[index[0], index[1]];
                Matrix[index[0], index[1]] = 9;
            }

            // двигаем по горизонтали
            if (mode == 2)
            {
                Grid.SetColumn(_but, index_zero[1]);

                Matrix[index_zero[0], index_zero[1]] = Matrix[index[0], index[1]];
                Matrix[index[0], index[1]] = 9;
            }
        }

        // проверка на победу
        public bool victory()
        {
            bool vict = true;

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (Matrix[i, j] != i * 3 + j + 1)
                        vict = false;

            return vict;
        }

        // то, что мы делаем при победе - лок кнопок
        public void repaintIt()
        {
            but_1.IsEnabled = false;
            but_2.IsEnabled = false;
            but_3.IsEnabled = false;
            but_4.IsEnabled = false;
            but_5.IsEnabled = false;
            but_6.IsEnabled = false;
            but_7.IsEnabled = false;
            but_8.IsEnabled = false;
        }

        // проверка матрицы на решаемость
        public bool checkMatrix()
        {
            int[] a = new int[9];

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    a[i * 3 + j] = Matrix[i, j];

            for (int i = 0; i < 9; i++)
                if (a[i] == 9)
                    a[i] = 0;

            int inv = 0;

            for (int i = 0; i < 9; ++i)
                if (a[i] != 0)
                    for (int j = 0; j < i; ++j)
                        if (a[j] > a[i])
                            ++inv;

            for (int i = 0; i < 9; ++i)
                if (a[i] == 0)
                    inv += 1 + i / 4;

            if (inv % 2 == 0)
                return false;
            else
                return true;
        }

        // заполнение матрицы случайными неповторяющимися числами от 1 до 9
        public void createMatrix()
        {
            Random b = new Random();
            int[] a = new int[9];
            int temp;
            int counter;
            bool chk;

            for (int i = 0; i < 9; i++)
                a[i] = 0;

            a[0] = b.Next(1, 10);
            counter = 1;

            while (counter != 9)
            {
                chk = true;

                temp = b.Next(1, 10);
                for (int i = 0; i < 9; i++)
                    if (a[i] == temp)
                        chk = false;

                if (chk)
                {
                    a[counter] = temp;
                    counter++;
                }
            }

            for (int i = 0; i < 9; i++)
                if (a[i] == 9)
                {
                    a[i] = a[8];
                    a[8] = 9;
                }

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    Matrix[i, j] = a[i * 3 + j];

            but_1.Content = a[0].ToString();
            but_2.Content = a[1].ToString();
            but_3.Content = a[2].ToString();
            but_4.Content = a[3].ToString();
            but_5.Content = a[4].ToString();
            but_6.Content = a[5].ToString();
            but_7.Content = a[6].ToString();
            but_8.Content = a[7].ToString();
        }
    }
}

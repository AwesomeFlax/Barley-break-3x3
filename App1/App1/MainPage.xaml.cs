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

            // матрица для работы с этой херней

            /*for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    Matrix[i, j] = i * 3 + j + 1;*/

            Matrix[0, 0] = 1;
            Matrix[0, 1] = 3;
            Matrix[0, 2] = 7;
            Matrix[1, 0] = 8;
            Matrix[1, 1] = 2;
            Matrix[1, 2] = 5;
            Matrix[2, 0] = 4;
            Matrix[2, 1] = 6;
            Matrix[2, 2] = 9;
        }

        private void my_func(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;                                      // принимаем нажатую кнопку
            var number = int.Parse(button.Content.ToString());                  // получаем нажатую кнопку

            if (canWeSwap(number) != 0)                                         // проверяем, можно ли свапать
                swapTwoButtons(number, canWeSwap(number), button);              // свапаем, если можно

            if (victory())
                repaintIt();
        }

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

        public bool victory()
        {
            bool vict = true;

            for (int i = 0; i < 3; i++)
                for (int j = 0; i < 3; i++)
                    if (Matrix[i, j] != i * 3 + j + 1)
                        vict = false;

            return vict;
        }

        public void repaintIt()
        {
            but_1.Content = "V";
            but_2.Content = "I";
            but_3.Content = "C";
            but_4.Content = "T";
            but_5.Content = "O";
            but_6.Content = "R";
            but_7.Content = "Y";
            but_8.Content = "!";
        }
    }
}

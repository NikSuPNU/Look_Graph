using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace LookSin
{
    class DrawGraph
    {


        /// <summary>
        /// Лист данных хранящий в себе точки для рисовнаия
        /// </summary>
        //private static List<double> point_graph = new List<double>();

        /// <summary>
        /// Локальный экземпляр класса хранящий в себе панель для рисования
        /// </summary>
        private static GraphPane panel;

        /// <summary>
        /// Специфичный список точек для панели ZedGraph
        /// </summary>
        private static PointPairList list = new PointPairList();

        private static ZedGraphControl ZedGraphControl;

        private static LineItem LineItem;
        
        /// <summary>
        /// Храним данные тут 
        /// </summary>
        private static RollingPointPairList rollingPoint;


        private static double num_look_point = 0;
        /// <summary>
        /// Конструктор принимает на вход частоту дискретизации т.е. с каким шагом будет производиться отрисовка графика
        /// и вторым параметром принимает на вход панель zedgraph.panel для возможности отрисовки
        /// </summary>
        /// <param name="panel">Панель для рисования</param>
        /// <param name="num_look_point">Количество отображаемых точек на графике</param>
        /// 
        public DrawGraph(ZedGraphControl ZedGraphControl, uint num_look_point)
        {
            DrawGraph.ZedGraphControl = ZedGraphControl;//Присваиваем экземпляру класса значение класса переданного извне
            DrawGraph.panel = ZedGraphControl.GraphPane;//Присваиваем локальному экземпляру класса ZedGraph панель 
            DrawGraph.panel.CurveList.Clear();//В самом начала очищаем поле от возможных кривых

            rollingPoint = new RollingPointPairList((int)num_look_point);

            //Оищаем весь график перед начало работы
            DrawGraph.LineItem = panel.AddCurve("Wave", rollingPoint, System.Drawing.Color.Red, SymbolType.None);
            //DrawGraph.LineItem = panel.AddCurve("Wave", point_graph, System.Drawing.Color.Red, SymbolType.None);

            // Вызываем метод AxisChange (), чтобы обновить данные об осях.
            // В противном случае на рисунке будет показана только часть графика,
            // которая умещается в интервалы по осям, установленные по умолчанию
            ZedGraphControl.AxisChange();
            // Обновляем график
            ZedGraphControl.Invalidate();

            DrawGraph.num_look_point = num_look_point;


        }



        double xmin;
        double xmax;
        public void Do_Dinamik_DrawGraph(double [] x, double [] y, double _step) 
        {
            if((x.Length == y.Length) && x != null && y != null && _step > 0)
            {

                rollingPoint.Add(x, y);
                // Рассчитаем интервал по оси X, который нужно отобразить на графике
                if ((x[39] - num_look_point * _step) <= 0) xmin = 0;
                else if ((x[39] - num_look_point * _step) > 0) xmin = (x[39] - num_look_point * _step);

                xmax = x[39];

                panel.XAxis.Scale.Min = xmin;
                panel.XAxis.Scale.Max = xmax;
                //panel.YAxis.Scale.Min = y.Min();
                //panel.YAxis.Scale.Max = y.Max();
                //MessageBox.Show($"Минимальное {xmin}\r\n" +
                //    $"Максимальное {xmax} \r\n" +
                //    $"По y min {y.Min()}\r\n" +
                //    $"По y max {y.Max()}\r\n");
                // Обновим оси
                ZedGraphControl.AxisChange();

                // Обновим сам график
                ZedGraphControl.Invalidate();
            }
            else
            {
                //TODO: Вывести сообщение о том, что массивы не соответствуют по длине либо один из массивов равен null
            }
        }
    }
}

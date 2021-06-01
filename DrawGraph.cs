using System;
using System.Collections.Generic;
using System.Text;
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
            
        }


        static double _currentx = 0;
        public void Do_Dinamik_DrawGraph() 
        {
            // Вычислим новое значение
            //double newValue = 10 * Math.Sin(_currentx * 3);

            // !!! Добавим новый отсчет к данным
            for(int i = 0; i < 50;i++) 
            {
                rollingPoint.Add(_currentx, 10 * Math.Sin(_currentx * 3));
                _currentx += 0.1;
            }


            // Рассчитаем интервал по оси X, который нужно отобразить на графике
            double xmin = _currentx - 100 * 0.1;
            double xmax = _currentx;

            panel.XAxis.Scale.Min = xmin;
            panel.XAxis.Scale.Max = xmax;

            // Обновим оси
            ZedGraphControl.AxisChange();

            // Обновим сам график
            ZedGraphControl.Invalidate();
        }
    }
}

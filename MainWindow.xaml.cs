using ScottPlot;
using ScottPlot.Collections;
using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using ScottPlot.DataGenerators;
using ScottPlot.WPF;
using Colors = ScottPlot.Colors;

namespace WpfAppTestDemo
{
    public partial class MainWindow : Window
    {
        // 用于生成示例数据的随机数生成器
        private readonly Random _random = new Random();

        private WpfPlot PlotControl { get; set; } = new WpfPlot();

        public MainWindow()
        {
            InitializeComponent();
            //BuildMultiplot();
            Loaded += MainWindow_Loaded; // 窗口加载完成后初始化图表
        }

        private void BuildMultiplot()
        {
            var multiplot = new Multiplot();
            multiplot.AddPlots(2);

            // 获取第一个子图并添加随机曲线数据
            var plot1 = multiplot.GetPlot(0);
            // 增加数据点数量，进一步增大步长标准差
            plot1.Add.Signal(Generate.RandomWalk(50, 1, 5));
            plot1.Axes.SetLimitsY(-20, 20); // 调整 Y 轴范围以适应更大波动
            plot1.Axes.SetLimitsX(0, 50); // 设置 X 轴范围

            // 获取第二个子图并添加方波数据
            var plot2 = multiplot.GetPlot(1);
            // 生成标准方波数据
            int pointCount = 1000;
            double[] squareWave = new double[pointCount];
            int period = 100; // 方波周期
            for (int i = 0; i < pointCount; i++)
            {
                squareWave[i] = (i % period < period / 2) ? 1 : 0;
            }
            plot2.Add.Signal(squareWave);
            plot2.Axes.SetLimitsY(0, 5);

            // 将 Multiplot 绑定到 WpfPlot 控件
            WpfPlot1.Multiplot = multiplot;

            // 刷新显示
            WpfPlot1.Refresh();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 初始化折线图
            InitializeLinePlot();
        }

        private void InitializeLinePlot()
        {
            // 1. 清除
            WpfPlot1.Plot.Clear();

            WpfPlot1.Plot.HideLegend();
            WpfPlot1.Plot.Legend.Alignment = Alignment.UpperCenter;
            // 2. 用 DateTimeAutomatic（默认就是智能的）
            var bottomAxis = WpfPlot1.Plot.Axes.DateTimeTicksBottom();

            // 3. 仅设置格式，不改生成器
            var dtGen = (ScottPlot.TickGenerators.DateTimeAutomatic)bottomAxis.TickGenerator;
            dtGen.LabelFormatter = dt => dt.ToString("HH:mm:ss");
            //整个控件背景颜色
            WpfPlot1.Plot.FigureBackground.Color = Colors.LightGoldenRodYellow;
            //绘图区域的颜色
            //WpfPlot1.Plot.DataBackground.Color = Colors.Black;
            //网格线颜色
            WpfPlot1.Plot.Grid.LineColor = Colors.White.WithAlpha(.6);

            WpfPlot1.Plot.Grid.XAxisStyle.MajorLineStyle.Color = Colors.Magenta;

            //WpfPlot1.Plot.Grid.YAxisStyle.MajorLineStyle.Color = Colors.Pink;
            //隐藏Y轴的刻度
            WpfPlot1.Plot.Axes.Left.TickLabelStyle.IsVisible = false;
            WpfPlot1.Plot.Axes.Left.MajorTickStyle.Length = 0;
            // Y轴的边框线
            WpfPlot1.Plot.Axes.Left.FrameLineStyle.Width = 0;

            //网格类型 实线-虚线
            WpfPlot1.Plot.Grid.LinePattern = LinePattern.Solid;
            //抗锯齿
            WpfPlot1.Plot.Axes.AntiAlias(true);
            // 4. 旋转
            bottomAxis.TickLabelStyle.Rotation = 90;
            bottomAxis.TickLabelStyle.FontSize = 10;
            // 生成示例数据 - 3条不同的折线
            int pointCount = 50;
            double[] x = Generate.Range(pointCount, 100); // X轴数据：0, 0.1, 0.2, ..., 4.9

            double[] squareWave = new double[pointCount];
            int period = 100; // 方波周期
            for (int i = 0; i < pointCount; i++)
            {
                squareWave[i] = (i % period < period / 2) ? 1 : 0;
            }

            double[] xxxy = new double[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                xxxy[i] = (i % period < period / 2) ? 1 : 2;
            }

            double[] doubles3 = new double[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                doubles3[i] = (i % period < period / 2) ? 2 : 3;
            }

            // 第一条线：正弦曲线
            //double[] y1 = Generate.Sin(pointCount, 1, 0.5);
            //var line1 = WpfPlot1.Plot.Add.Scatter(x, squareWave);
            var line1 = WpfPlot1.Plot.Add.Signal(squareWave);

            line1.LegendText = "Sin";
            line1.Color = Colors.Blue;
            line1.LineWidth = 2;
            line1.LinePattern = ScottPlot.LinePattern.Solid;
            line1.MarkerShape = MarkerShape.None;
            line1.MarkerSize = 5;
            line1.MarkerColor = Colors.Blue;

            // 第二条线：余弦曲线
            double[] y2 = Generate.Cos(pointCount, 1, 0.5);
            //var line2 = WpfPlot1.Plot.Add.Scatter(x, squareWave);
            var line2 = WpfPlot1.Plot.Add.Signal(xxxy);
            line2.LegendText = "cos";
            line2.Color = Colors.Red;
            line2.LineWidth = 2;
            line2.LinePattern = ScottPlot.LinePattern.Dotted;
            line2.MarkerShape = MarkerShape.None;
            // 第三条线：随机数据
            double[] y3 = Generate.RandomWalk(pointCount, 0, 0.2);
            //var line3 = WpfPlot1.Plot.Add.Scatter(x, squareWave);
            var line3 = WpfPlot1.Plot.Add.Signal(doubles3);
            line3.LegendText = "A";
            line3.Color = Colors.Green;
            line3.LineWidth = 2;
            line3.LinePattern = ScottPlot.LinePattern.Solid;
            line3.MarkerShape = MarkerShape.None;
            //设置初始状态：显示图例和网格
            WpfPlot1.Plot.HideGrid();

            //// 自动调整坐标轴范围以适应所有数据
            //WpfPlot1.Plot.Axes.AutoScale();

            // 刷新图表显示
            WpfPlot1.Refresh();
        }

        private void LegendCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            WpfPlot1.Plot.ShowLegend();
            WpfPlot1.Refresh();
        }

        // 实现Unchecked事件处理程序
        private void LegendCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            WpfPlot1.Plot.HideLegend();
            WpfPlot1.Refresh();
        }

        private void ShowGridCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            WpfPlot1.Plot.ShowGrid();
            WpfPlot1.Refresh();
        }

        private void ShowGridCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            WpfPlot1.Plot.HideGrid();
            WpfPlot1.Refresh();
        }
    }
}
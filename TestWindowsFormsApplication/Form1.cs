using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Temp;

using vrp;

namespace TestWindowsFormsApplication
{
    public partial class TestForm : Form
    {
        private Bitmap m_Bitmap;

        private int m_Width = 1200;
        private int m_Height = 900;
        private double m_XScale = 1;
        private double m_XShift = 0;
        private double m_YScale = 1;
        private double m_YShift = 0;

        private Color[] _colors = new[] { Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.CornflowerBlue, Color.GreenYellow };

        private VrpData _vrpData; 
        private VrpResult _vrpResult;

        private readonly IVrpSolver _solver = new VrpClusteringMethod();

        public TestForm()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            InitializeComponent();
            m_Bitmap = new Bitmap(m_Width, m_Height);
            bitmapPanel.Width = m_Width;
            bitmapPanel.Height = m_Height;

            //RefreshPanel();
        }

        private void bitmapPanel_MouseUp(object sender, MouseEventArgs e)
        {
            /*m_Points.Add(new Point2DReal(e.X, e.Y));
            DrawPoint(e.X, e.Y, Graphics.FromImage(m_Bitmap));
            textBox1.Text = m_Points.Count.ToString();
            bitmapPanel.Refresh();*/
        }

        private void DrawPoint(double x, double y, Color color, Graphics g, int r = 4)
        {
            g.FillEllipse(new SolidBrush(color), (float)(x - r), (float) (y - r), 2 * r, 2 * r);
        }

        private void bitmapPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(m_Bitmap, 0, 0);
        }

        private void buttonGetPath_Click(object sender, EventArgs e)
        {
            GetPath();

            RefreshDraw();
        }

        private void GetPath()
        {
            if (_vrpData == null)
            {
                return;
            }

            _vrpResult = _solver.Solve(_vrpData);
        }

        private void RefreshDraw()
        {
            if (_vrpData == null)
            {
                return;
            }

            m_Bitmap = new Bitmap(m_Width, m_Height);
            var g = Graphics.FromImage(m_Bitmap);

            if (_vrpResult != null)
            {
                var count = 0;
                foreach (var route in _vrpResult.Routes)
                {
                    var pen = new Pen(_colors[count], 2);
                    for (int i = 0; i < route.Count - 1; i++)
                    {
                        DrawLine(pen, _vrpData.Customers[route[i]].Point, _vrpData.Customers[route[i + 1]].Point, g);
                    }

                    count = (count + 1) % _colors.Length;
                }
            }

            double maxDemand = Math.Sqrt(_vrpData.Customers.Max(c => c.Demand));
            foreach (var customer in _vrpData.Customers)
            {
                var radius = (int) (10 * Math.Sqrt(customer.Demand) / maxDemand) + 1;
                //DrawPoint(customer.Point.X, customer.Point.Y, Color.Black, g, radius + 2);
                DrawPoint(customer.Point.X, customer.Point.Y, Color.Red, g, radius);
                if (cbShowDemand.Checked && customer.Demand != 0)
                {
                    g.DrawString(customer.Demand.ToString(), new Font("Arial", 16), new SolidBrush(Color.Black), (float)customer.Point.X, (float)customer.Point.Y);
                }
            }

            textBox1.Text = _vrpData.Customers.Length.ToString();
            tbTotalLength.Text = _vrpResult.Routes.Sum(r => GetLength(r.Count - 1, _vrpData.Customers.Select(c => c.Point), r)).ToString();

            bitmapPanel.Refresh();
        }

        private double GetLength(int n, IEnumerable<Point2DReal> points, IList<int> path)
        {
            var p = points.Select(point => new Point2DReal((point.X - m_XShift) * m_XScale, (point.Y - m_YShift) * m_YScale)).ToArray();
            var length = p[path[n - 1]].Dist(p[path[0]]);
            for (int i = 0; i < n - 1; i++)
            {
                length += p[path[i]].Dist(p[path[i + 1]]);
            }
            return length;
        }

        private void DrawLine(Pen pen, Point2DReal a, Point2DReal b, Graphics g)
        {
            g.DrawLine(pen, (float) a.X, (float) a.Y, (float) b.X, (float) b.Y);
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            /*m_Points = new List<Point2DReal>();
            m_Bitmap = new Bitmap(m_Width, m_Height);
            textBox1.Text = m_Points.Count.ToString();
            bitmapPanel.Refresh();*/
        }

        private void buttonAddPoints_Click(object sender, EventArgs e)
        {
            /*var rnd = new Random();
            for (int i = 0; i < numericPoints.Value; i++)
            {
                double x = rnd.NextDouble() * (m_Width - 20) + 10;
                double y = rnd.NextDouble() * (m_Height - 20) + 10;

                m_Points.Add(new Point2DReal(x, y));
            }
            var g = Graphics.FromImage(m_Bitmap);
            foreach(var point in m_Points)
            {
                DrawPoint(point.X, point.Y, g);
            }
            textBox1.Text = m_Points.Count.ToString();
            bitmapPanel.Refresh();*/
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var path = dialog.FileName;
                _vrpData = VrpData.ReadData(path);
                var minx = 1e30;
                var maxx = -1e30;
                var miny = 1e30;
                var maxy = -1e30;
                foreach (var point in _vrpData.Customers.Select(c => c.Point))
                {
                    minx = Math.Min(minx, point.X);
                    maxx = Math.Max(maxx, point.X);
                    miny = Math.Min(miny, point.Y);
                    maxy = Math.Max(maxy, point.Y);
                }

                foreach (var point in _vrpData.Customers.Select(c => c.Point))
                {
                    point.X = (point.X - minx) / (maxx - minx) * (m_Width - 60) + 30;
                    point.Y = (point.Y - miny) / (maxy - miny) * (m_Height - 60) + 30;
                }
                m_XScale = (maxx - minx) / (m_Width - 60);
                m_XShift = 30;
                m_YScale = (maxy - miny) / (m_Height - 60);
                m_YShift = 30;
            }

            GetPath();
            RefreshDraw();
        }

        private void cbShowDemand_CheckedChanged(object sender, EventArgs e)
        {
            RefreshDraw();
        }
    }
}

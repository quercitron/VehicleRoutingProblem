using System.IO;

using Temp;

namespace vrp
{
    public class VrpData
    {
        public int N { get; set; }
        public int V { get; set; }
        public int C { get; set; }

        public Customer[] Customers { get; set; }

        public static VrpData ReadData(string path)
        {
            var data = new VrpData();

            using (var reader = File.OpenText(path))
            {
                var line = reader.ReadLine().Split();

                data.N = int.Parse(line[0]);
                data.V = int.Parse(line[1]);
                data.C = int.Parse(line[2]);

                data.Customers = new Customer[data.N];

                for (int i = 0; i < data.N; i++)
                {
                    line = reader.ReadLine().Split();
                    data.Customers[i] = new Customer
                    {
                        Id = i,
                        Demand = int.Parse(line[0]),
                        Point = new Point2DReal(double.Parse(line[1]), double.Parse(line[2]))
                    };
                }
            }

            return data;
        }
    }

    public class Customer
    {
        public Point2DReal Point { get; set; }

        public int Demand { get; set; }

        public int Id { get; set; }
    }
}

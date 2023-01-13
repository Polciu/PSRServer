using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSRServer
{
    class Metal : Model
    {
        public Metal()
        {
            this.productName = "Metal";
        }

        public override List<int> Generate()
        {
            List<int> values = new List<int>();

            Random rnd = new Random();
            for (int i = 0; i < 100; i++)
            {
                values.Add(rnd.Next(0, 2));
                //Console.WriteLine(values[i]);
            }
            this.data = values;
            //Console.WriteLine(values.Count);
            return values;
        }

        public override double PositivePercentage(int threshold)
        {
            int positiveCount = 0;

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] == threshold)
                {
                    positiveCount++;
                }
            }

            //Console.WriteLine("Positive count: " + positiveCount);
            //Console.WriteLine("Data count: " + data.Count);
            double percentage = (double)((double)positiveCount / (double)data.Count) * 100;
            Console.WriteLine("RESULT: " + percentage);
            return percentage;
        }

        public override double NegativePercentage(int threshold)
        {
            int negativeCount = 0;

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] == threshold)
                {
                    negativeCount++;
                }
            }

            //Console.WriteLine("NEGATIVE COUNT: " + negativeCount);
            //Console.WriteLine("DATA COUNT: " + data.Count);
            double percentage = (double)((double)negativeCount / (double)data.Count) * 100;
            //Console.WriteLine("RESULT: " + percentage);
            return percentage;
        }

        public override string ShowDataSet()
        {
            string result = "";
            for (int i = 0; i < data.Count; i++)
            {
                result = result + "," + data[i];
            }
            return result;
        }

        public override string GetValuesBetween(int x, int y)
        {
            string result = "";
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] >= x && data[i] <= y)
                {
                    result = result + "," + data[i];
                }
            }
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSRServer
{
    class Plastic : Model
    {
        public Plastic()
        {
            this.productName = "Plastic";
        }

        public override List<int> Generate()
        {
            List<int> values = new List<int>();
            Random rnd = new Random();

            for (int i = 0; i < 200; i++)
            {
                values.Add(rnd.Next(-100,100));
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
                if (data[i] > threshold)
                {
                    positiveCount++;
                }
            }

            double percentage = (double)((double)positiveCount / (double)data.Count) * 100;
            Console.WriteLine("RESULT: " + percentage);
            return percentage;
        }

        public override double NegativePercentage(int threshold)
        {
            int negativeCount = 0;

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] < threshold)
                {
                    negativeCount++;
                }
            }

            double percentage = (double)((double)negativeCount / (double)data.Count) * 100;
            return percentage;
        }

        public override string ShowDataSet()
        {
            string result = "";
            for (int i = 0; i < data.Count; i++)
            {
                result = result + ";" + data[i];
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

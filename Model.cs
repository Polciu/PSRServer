using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSRServer
{
    abstract class Model
    {
        public string productName = "";
        public List<int> data = new List<int>();

        public virtual List<int> Generate() { return new List<int>(); }
        public virtual double PositivePercentage(int threshold) { return new double(); }
        public virtual double NegativePercentage(int threshold) { return new double(); }
        public virtual string ShowDataSet() { return ""; }
        public virtual string GetValuesBetween(int x, int y) { return ""; }
       // public virtual void Invoke(string method, List<Tuple<string,string>> parameters) { }
    }
}

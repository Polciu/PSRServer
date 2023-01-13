using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PSRServer
{
    class Program
    {

        // Lista obiektow.
        static void Main(string[] args)
        {
            ProgramController pc = new ProgramController();
            NetworkController nc = new NetworkController(ref pc);    
            nc.initialize();
        }


    }

}

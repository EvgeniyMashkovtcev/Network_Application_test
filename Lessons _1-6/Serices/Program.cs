using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace Patterns
{
    public class Program
    {
        static void Main(string[] args)
        {
            TheServer server = new TheServer();
            server.StartServ();
        }


    }
}
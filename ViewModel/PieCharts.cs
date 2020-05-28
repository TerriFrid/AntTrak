using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AntTrak.ViewModel
{
    public class PieCharts
    {
        //public PieChartData Data { get; set; }
         public List<string> Labels { get; set; }
        public List<string> Colors { get; set; }
        public List <int> Values { get; set; }

        public PieCharts()
        {
            
            Labels = new List<string>();
            Colors = new List<string>();
            Values = new List<int>();

        }
        
    }
    //public class PieChartData
    //{
    //    //Ticket priorities
    //    string label { get; set; }
    //    string color { get; set; }
    //    string value { get; set; }

   //}
}
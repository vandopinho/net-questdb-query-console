using QuestDbQueryConsole.Entity;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.Reflection.PortableExecutable;
using QuestDbQueryConsole.Query;

namespace QuestDbQueryConsole
{
     public class Start
     {
          static  void Main(string[] args)
          {    
               QueryManager qm= new QueryManager();
               Console.WriteLine("Consulta QuestDB");
            //qm.DisplayData_QuestDBWireProtocol();
            //qm.DisplayData();
            qm.QuestDBWireProtocolInsert();
          }
     }
}
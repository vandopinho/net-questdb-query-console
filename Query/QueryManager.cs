using QuestDbQueryConsole.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestDbQueryConsole.Query
{
     public class QueryManager
     {
          private string WriteQuery()
          {
               string tipoDado = string.Empty;
               int limit = 0;
               Console.Write("SELECT: ");
               tipoDado = Console.ReadLine();
               Console.Write("LIMIT: ");
               limit = int.Parse(Console.ReadLine());
               string query = "SELECT " + tipoDado + " FROM OPENQUERY(QUESTDB,'SELECT * FROM [dados2] LIMIT " + limit + "')";
               return query;
          }

          public void DisplayData()
          {
               List<DadosEntity> listDados = new List<DadosEntity>();
               using (SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=master;Integrated Security=True;"))
               {
                    string query = WriteQuery();
                    DateTime inicio = DateTime.Now;
                    Console.WriteLine("Hora inicio: " + inicio.ToString());
                    conn.Open();

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                         SqlDataReader dr = command.ExecuteReader();
                         while (dr.Read())
                         {
                              string datetime = dr.GetString(0);
                              DateTime periodStart = dr.GetDateTime(1);
                              string name = dr.GetString(2);
                              float flow = dr.GetFloat(3);
                              float flowSetPoint = dr.GetFloat(4);
                              float pressure = dr.GetFloat(5);
                              float pressureSetPoint = dr.GetFloat(6);
                              float overloadValue = dr.GetFloat(7);
                              int operationStatus = dr.GetInt32(8);
                              int operationType = dr.GetInt32(9);
                              int operationMode = dr.GetInt32(10);
                              listDados.Add(new DadosEntity() { Datetime = datetime, PeriodStart = periodStart, Name = name, Flow = flow, FlowSetpoint = flowSetPoint, Pressure = pressure, PressureSetpoint = pressureSetPoint, OverloadValue = overloadValue, OperationStatus = operationStatus, OperationType = operationType, OperationMode = operationMode });
                         }
                         DateTime final = DateTime.Now;
                         Console.WriteLine("Hora fim: " + final.ToString());
                         conn.Close();
                    }
               }
          }
     }
}

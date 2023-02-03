using QuestDbQueryConsole.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Data.Common;
using System.Reflection.PortableExecutable;

namespace QuestDbQueryConsole.Query
{
     public class QueryManager
     {
          private string WritePartialQuery()
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

          private string WriteFullQuery()
          {
              string query;
              Console.Write("Query: ");
              return query = Console.ReadLine();
          }
          public void DisplayData()
          {
               List<DadosEntity> listDados = new List<DadosEntity>();
               using (SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=master;Integrated Security=True;"))
               {
                    //string query = WriteFullQuery();
                    string query = WritePartialQuery();
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
        public void DisplayData_QuestDBWireProtocol()
        {

            string username = "admin";
            string password = "quest";
            string database = "questdb";
            int port = 8812;
            var connectionString = $@"host=localhost;port={port};username={username};password={password};database={database};ServerCompatibilityMode=NoTypeLoading;";

            var dataSource = NpgsqlDataSource.Create(connectionString);

            DateTime inicio = DateTime.Now;
            Console.WriteLine("Hora inicio: " + inicio.ToString());

            List<DadosEntityWireProtocol> listmachine = new List<DadosEntityWireProtocol>();
            DadosEntityWireProtocol machine = new DadosEntityWireProtocol();
            //listmachine[0] = new QuestDB_MachineModel();
            string query = "SELECT * FROM 'questdb-query-1675076348034.csv LIMIT 15000000000'";
            Console.WriteLine(query);
            using (var cmd = dataSource.CreateCommand(query))
            using (var reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {

                    machine = new DadosEntityWireProtocol();
                    machine.Datetime = (reader.GetString(0));
                    machine.PeriodStart = (reader.GetDateTime(1));
                    machine.Name = (reader.GetString(2));
                    machine.Flow = (reader.GetDouble(3));
                    machine.FlowSetpoint = (reader.GetDouble(4));
                    machine.Pressure = (reader.GetDouble(5));
                    machine.PressureSetpoint = (reader.GetDouble(6));
                    machine.OverloadValue = (reader.GetDouble(7));
                    machine.OperationStatus = (reader.GetInt32(8));
                    machine.OperationType = (reader.GetInt32(9));
                    machine.OperationMode = (reader.GetInt32(10));

                    listmachine.Add(machine);
                }
            }
            DateTime final = DateTime.Now;
            Console.WriteLine("Hora fim: " + final.ToString());
            
        }

        public void QuestDBWireProtocolInsert()
        {
            Console.WriteLine("Teste QuestDB Wire Protocol c/ Insert");
            string username = "admin";
            string password = "quest";
            string database = "qdb";
            int port = 8812;
            var connectionString = $@"host=localhost;port={port};username={username};password={password};database={database};ServerCompatibilityMode=NoTypeLoading;";
            var dataSource = NpgsqlDataSource.Create(connectionString);
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
            dataSource = dataSourceBuilder.Build();

            using (var cmd = dataSource.CreateCommand("INSERT INTO QuestTeste (ID) VALUES (" + GenerateRandom().ToString() + ")"))
            {
                DateTime inicio = DateTime.Now;
                Console.WriteLine("Hora inicio: " + inicio.ToString());

                cmd.Parameters.AddWithValue(1);
                cmd.ExecuteNonQuery();
            }

            DateTime final = DateTime.Now;
            Console.WriteLine("Hora fim: " + final.ToString());

            using (var cmd = dataSource.CreateCommand("SELECT ID FROM QuestTeste"))
            using (DbConnection conn = new NpgsqlConnection(connectionString))
            {
                
                    conn.Open();
                using (DbCommand command = conn.CreateCommand())
                {
                    //cmd.Parameters.AddWithValue(1);
                    //cmd.ExecuteNonQuery();

                    using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetInt64(0));
                    }
                }            
            }       
        }
        private static float GenerateRandom()
        {
            Random r = new Random();
            int rInt = r.Next(0, 50);

            var rand = new Random();
            double min = 1;
            double max = 100;
            double range = max - min;

            double sample2 = rand.NextDouble();
            double scaled2 = (sample2 * range) + min;
            //return (float)scaled2 + rInt
            return rInt;

        }
    }
}

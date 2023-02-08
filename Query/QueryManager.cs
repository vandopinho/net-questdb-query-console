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
               Console.WriteLine("\n\nSelect usando OpenQuery do SQL Server");
               List<DadosEntity> listDados = new List<DadosEntity>();
               using (SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=master;Integrated Security=True;"))
               {
                //string query = WriteFullQuery();
                string query = "SELECT * FROM OPENQUERY (\r\n    QuestDB, \r\n    'SELECT * FROM QuestTeste'\r\n    );";
                    DateTime inicio = DateTime.Now;
                    Console.WriteLine(query);
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                         SqlDataReader dr = command.ExecuteReader();
                        try
                    {
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

                    }
                        catch (InvalidCastException e) {

                        while (dr.Read())
                        {
                            Console.Write(dr.GetInt32(0) + "; ");
                        }
                    }

                    }
                    Console.WriteLine("\nHora inicio: " + inicio.ToString());
                    DateTime final = DateTime.Now;
                Console.WriteLine("Hora fim: " + final.ToString()+"\n");
                conn.Close();
            }
          }
          public void DisplayData_QuestDBWireProtocol()
        {
            Console.WriteLine("\n\nSelect usando Npgsql Wire Protocol");
            string username = "admin";
            string password = "quest";
            string database = "questdb";
            int port = 8812;
            var connectionString = $@"host=localhost;port={port};username={username};password={password};database={database};ServerCompatibilityMode=NoTypeLoading;";

            var dataSource = NpgsqlDataSource.Create(connectionString);
            string query = "SELECT * FROM 'dados2' LIMIT 150000000";
            Console.WriteLine(query);
            DateTime inicio = DateTime.Now;
            Console.WriteLine("Hora inicio: " + inicio.ToString());

            List<DadosEntityWireProtocol> listmachine = new List<DadosEntityWireProtocol>();
            DadosEntityWireProtocol machine = new DadosEntityWireProtocol();
            //listmachine[0] = new QuestDB_MachineModel();
            
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
                cmd.Dispose();
            }
            

            DateTime final = DateTime.Now;
            Console.WriteLine("Hora fim: " + final.ToString()+"\n");
            
        }
          public void QuestDb_Partition()
          {
               Console.WriteLine("\n\nQuestDb_Partition");
               string username = "admin";
               string password = "quest";
               string database = "questdb";
               int port = 8812;
               var connectionString = $@"host=localhost;port={port};username={username};password={password};database={database};ServerCompatibilityMode=NoTypeLoading;";

               var dataSource = NpgsqlDataSource.Create(connectionString);
               string query = "INSERT INTO my_table\r\nSELECT timestamp_sequence(\r\n    to_timestamp('2021-01-01T00:00:00', 'yyyy-MM-ddTHH:mm:ss'),100000L * 36000), x\r\nFROM long_sequence(120);";
               Console.WriteLine(query);
               DateTime inicio = DateTime.Now;
               Console.WriteLine("Hora inicio: " + inicio.ToString());

               List<DadosEntityWireProtocol> listmachine = new List<DadosEntityWireProtocol>();
               DadosEntityWireProtocol machine = new DadosEntityWireProtocol();
               using (var cmd = dataSource.CreateCommand(query))
               using (var reader = cmd.ExecuteReader())
               {
                    while (reader.Read())
                    {
                         machine = new DadosEntityWireProtocol();
                         machine.Datetime = (reader.GetString(0));
                         machine.Name = (reader.GetString(1));
                         listmachine.Add(machine);
                    }
                    cmd.Dispose();
               }


               DateTime final = DateTime.Now;
               Console.WriteLine("Hora fim: " + final.ToString() + "\n");

               using (var cmd = dataSource.CreateCommand("SELECT x FROM my_table"))
               using (DbConnection conn = new NpgsqlConnection(connectionString))
               {
                    Console.WriteLine(cmd.CommandText);
                    using (DbCommand command = conn.CreateCommand())
                    {
                         //cmd.Parameters.AddWithValue(1);
                         //cmd.ExecuteNonQuery();
                         using (var reader = cmd.ExecuteReader())
                              while (reader.Read())
                              {
                                   Console.Write(reader.GetInt64(0) + "; ");
                              }
                         conn.Close();
                         conn.Dispose();
                    }

                    cmd.Dispose();
               }
               using (var cmd = dataSource.CreateCommand("ALTER TABLE my_table DROP PARTITION\r\nWHERE timestamp < to_timestamp('2021-01-03', 'yyyy-MM-dd');"))
               using (DbConnection conn = new NpgsqlConnection(connectionString))
               {
                    Console.WriteLine("\n"+cmd.CommandText);
                    using (DbCommand command = conn.CreateCommand())
                    {
                         //cmd.Parameters.AddWithValue(1);
                         //cmd.ExecuteNonQuery();
                         using (var reader = cmd.ExecuteReader())
                              while (reader.Read())
                              {
                                  //Console.Write(reader.GetInt64(0) + "; ");
                              }
                         conn.Close();
                         conn.Dispose();
                    }
                   
                    cmd.Dispose();
               }
               using (var cmd = dataSource.CreateCommand("SELECT x FROM my_table"))
               using (DbConnection conn = new NpgsqlConnection(connectionString))
               {
                    Console.WriteLine(cmd.CommandText);
                    using (DbCommand command = conn.CreateCommand())
                    {
                         //cmd.Parameters.AddWithValue(1);
                         //cmd.ExecuteNonQuery();
                         using (var reader = cmd.ExecuteReader())
                              while (reader.Read())
                              {
                                   Console.Write(reader.GetInt64(0) + "; ");
                              }
                         conn.Close();
                         conn.Dispose();
                    }
                    //dataSource.Dispose();
                    cmd.Dispose();
               }

          }
          public void InsertData_QuestDBWireProtocol()
          {
            Console.WriteLine("Teste QuestDB Wire Protocol c/ Insert");
            string username = "admin";
            string password = "quest";
            string database = "questdb";
            int port = 8812;
            var connectionString = $@"host=localhost;port={port};username={username};password={password};database={database};ServerCompatibilityMode=NoTypeLoading;";
            var dataSource = NpgsqlDataSource.Create(connectionString);
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
            dataSource = dataSourceBuilder.Build();
            
            DateTime inicio = DateTime.Now;
           

                using (var cmd = dataSource.CreateCommand("INSERT INTO QuestTeste (ID) VALUES (" + GenerateRandom().ToString() + ")"))
                {
                //dataSource.OpenConnection();
                cmd.Parameters.AddWithValue(1);
                cmd.ExecuteNonQuery();
                Console.WriteLine(cmd.CommandText);
                }
               Console.WriteLine("Hora inicio: " + inicio.ToString());
               DateTime final = DateTime.Now;
            Console.WriteLine("Hora fim: " + final.ToString() + "\n");


            using (var cmd = dataSource.CreateCommand("SELECT ID FROM QuestTeste"))
            using (DbConnection conn = new NpgsqlConnection(connectionString))
            {
                    Console.WriteLine(cmd.CommandText);
                    using (DbCommand command = conn.CreateCommand())
                {
                    //cmd.Parameters.AddWithValue(1);
                    //cmd.ExecuteNonQuery();
                    using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        Console.Write(reader.GetInt64(0) + "; ");
                    }
                    conn.Close();
                    conn.Dispose();
                }
                dataSource.Dispose();
                cmd.Dispose();
                
            }       
        }
          public void InsertData()
        {
            List<DadosEntity> listDados = new List<DadosEntity>();
            using (SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=master;Integrated Security=True;"))
            {
                //string query = WriteFullQuery();
                //string query = WritePartialQuery();
                string query = "INSERT OPENQUERY (QUESTDB,'Select ID from QuestTeste') VALUES('1');";
                Console.WriteLine("Insert usando OpenQuery\n"+query);
                DateTime inicio = DateTime.Now;
                
                conn.Open();

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    //command.Parameters.AddWithValue(1);
                    command.ExecuteNonQuery();

                    //SqlDataReader dr = command.ExecuteReader();
                    //while (dr.Read())
                    //{
                    //    string datetime = dr.GetString(0);
                    //    DateTime periodStart = dr.GetDateTime(1);
                    //    string name = dr.GetString(2);
                    //    float flow = dr.GetFloat(3);
                    //    float flowSetPoint = dr.GetFloat(4);
                    //    float pressure = dr.GetFloat(5);
                    //    float pressureSetPoint = dr.GetFloat(6);
                    //    float overloadValue = dr.GetFloat(7);
                    //    int operationStatus = dr.GetInt32(8);
                    //    int operationType = dr.GetInt32(9);
                    //    int operationMode = dr.GetInt32(10);
                    //    listDados.Add(new DadosEntity() { Datetime = datetime, PeriodStart = periodStart, Name = name, Flow = flow, FlowSetpoint = flowSetPoint, Pressure = pressure, PressureSetpoint = pressureSetPoint, OverloadValue = overloadValue, OperationStatus = operationStatus, OperationType = operationType, OperationMode = operationMode });
                    //}
                    Console.WriteLine("Hora inicio: " + inicio.ToString());
                    DateTime final = DateTime.Now;
                    Console.WriteLine("Hora fim: " + final.ToString() + "\n");
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Método Auxiliar para gerar int ou float aleatório
        /// </summary>
        /// <returns></returns>
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

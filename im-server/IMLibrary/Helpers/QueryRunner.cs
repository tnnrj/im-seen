﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;

namespace IMLibrary.Helpers
{
    public interface IQueryRunner
    {
        JArray ExecuteAsJson(string sql);
    }

    public class QueryRunner : IQueryRunner
    {
        private readonly string _connectionString;

        public QueryRunner(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("IM_API_Context");
        }

        public JArray ExecuteAsJson(string sql)
        {
            var dt = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }

            return JArray.Parse(JsonConvert.SerializeObject(dt));
        }
    }
}

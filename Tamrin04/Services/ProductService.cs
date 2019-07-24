using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Tamrin04.Entities;

namespace Tamrin04.Services
{
    public interface IProductService
    {
        Task AddAsync(Products model);
        Task<int> AddWithSPAsync(Products model);
    }
    public class ProductService : IProductService
    {
        private IConfiguration Configuration { get; }
        public ProductService(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public async Task AddAsync(Products model)
        {
            string connectionString = Configuration.GetConnectionString("DbConnection");
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                string commandText = "Insert Products(ProductName,CategoryId,UnitPrice) Values(@ProductName,@CategoryId,@UnitPrice)";

                //تعریف پارامترها
                var parameters = new DynamicParameters();
                parameters.Add("ProductName", model.ProductName);
                parameters.Add("CategoryId", model.CategoryId);
                parameters.Add("UnitPrice", model.UnitPrice);

                //اجرای دستور
                //await connection.ExecuteAsync(commandText, parameters);

                //ارسال پارامترها با مدل
                await connection.ExecuteAsync(commandText, model);


            }
        }

        public async Task<int> AddWithSPAsync(Products model)
        {
            string connectionString = Configuration.GetConnectionString("DbConnection");
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                string commandText = "SP_Product_Insert";

                //تعریف پارامترها
                var parameters = new DynamicParameters();
                parameters.Add("ProductName", model.ProductName);
                parameters.Add("CategoryId", model.CategoryId);
                parameters.Add("UnitPrice", model.UnitPrice);
                parameters.Add("id", dbType: DbType.Int32, direction: ParameterDirection.Output);



                //ارسال پارامترها با مدل
                await connection.ExecuteAsync(commandText, parameters, commandType: CommandType.StoredProcedure);

                int id = parameters.Get<int>("id");

                return id;
            }
        }
    }
}

using System.Globalization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using WaterLogger.Models;

namespace WaterLogger.Pages;

public class IndexModel : PageModel
{
    private readonly IConfiguration _configuration;

    public List<DrinkingWaterModel> Records { get; set; }

    public IndexModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void OnGet()
    {
        Records = GetAllRecords();
        ViewData["Total"] = Records.AsEnumerable().Sum(x => x.Quantity);
    }

    private List<DrinkingWaterModel> GetAllRecords()
    {
        string connectionString = "Data Source=AVARISLI\\SQLEXPRESS01;Initial Catalog=WaterLogger;Integrated Security=True;";

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = "SELECT * FROM drinking_water";

            var tableData = new List<DrinkingWaterModel>();

            using (SqlDataReader reader = tableCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tableData.Add(new DrinkingWaterModel
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                    });
                }
            }

            return tableData;
        }
    }

}

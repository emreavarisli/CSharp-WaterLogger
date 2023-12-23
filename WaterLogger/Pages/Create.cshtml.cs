using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WaterLogger.Models;

namespace WaterLogger.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public DrinkingWaterModel DrinkingWater { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    "INSERT INTO drinking_water (date, quantity) " +
                    "VALUES (@Date, @Quantity)";

                // Parametreleri ekleyin ve deðerlerini atayýn
                tableCmd.Parameters.AddWithValue("@Date", DrinkingWater.Date);
                tableCmd.Parameters.AddWithValue("@Quantity", DrinkingWater.Quantity);

                tableCmd.ExecuteNonQuery();
            }
            return RedirectToPage("./Index");
        }
    }
}
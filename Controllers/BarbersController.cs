using barberShop.Models;
using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc;


namespace barberShop.Controllers;

[Route("/api[controller]")]
[ApiController]
public class BarbersController:ControllerBase
{
    private readonly string conn_string;
    public BarbersController(IConfiguration config)
    {
        conn_string = config.GetConnectionString("conn_string");
    }
    [HttpPost]
    public IActionResult CreateBarber([FromBody] BarberModel barber)
    {
        using (MySqlConnection conn = new MySqlConnection(conn_string))
        {
            MySqlCommand cmd = conn.CreateCommand();

            int id = barber.Id;
            string? name = barber.Name;
            string? specialization = barber.Specialization;

            cmd.CommandText = "INSERT INTO barbers (Name,Specialization) VALUES (@name,@specialization)";
            cmd.Parameters.AddWithValue("@id",id);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@specialization",specialization);

            try
            {
                conn.Open();
                int rowsafected = cmd.ExecuteNonQuery();

                if (rowsafected > 0)
                {
                    return Ok("Profissional cadastrado!");
                }
                else
                {
                    return StatusCode(500, "Erro");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500,"Erro: "+ex.Message);
            }
        }
    }
    [HttpGet("{id}")]
    public IActionResult GetBarber(int id)
    {
        BarberModel barber=new BarberModel();
        using(MySqlConnection conn=new MySqlConnection(conn_string))
        {
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM barbers WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                conn.Open();
                MySqlDataReader rdr = cmd.ExecuteReader();

                if(rdr.Read())
                {
                    barber.Name = rdr.GetString(1);
                    barber.Specialization = rdr.GetString(2);
                }
                else
                {
                    return NotFound();
                }
                rdr.Close();
                return Ok(barber);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro: " + ex.Message);
            }
        }
    }
}

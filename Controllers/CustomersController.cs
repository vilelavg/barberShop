using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using barberShop.Models;
using MySql.Data.MySqlClient;
using System.Data;
namespace barberShop.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CustomersController:ControllerBase
{
    private readonly string conn_string;
    public CustomersController(IConfiguration configuration)
    {
        conn_string = configuration.GetConnectionString("conn_string");
    }
    [HttpPost]
    public IActionResult CreateCustomer([FromBody] CustomerModel customer)
    {
        using(MySqlConnection conn = new MySqlConnection(conn_string))
        {
            MySqlCommand cmd=conn.CreateCommand();

            int id = customer.Id;
            string? name = customer.Name;
            long telephone = customer.Telephone;
            DateTime desde = customer.Desde;

            cmd.CommandText = "INSERT INTO customers(Id,Name,Telephone,Desde) VALUES (@id,@name,@telephone,@desde)";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@telephone",telephone);
            cmd.Parameters.AddWithValue("@desde", desde);
            cmd.Parameters.AddWithValue("id", id);

            try
            {
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                if ( rowsAffected > 0){
                    return Ok("Usuario cadastrado");
                }
                else
                {
                    return StatusCode(500, "Erro");
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro"+ex.Message);
            }
            
        }
    }
    [HttpGet("{id}")]
    public IActionResult GetCustomer(int id)
    {
        CustomerModel customer=new CustomerModel();
        using (MySqlConnection conn = new MySqlConnection(conn_string))
        {
            MySqlCommand command=conn.CreateCommand();
            command.CommandText = "SELECT * FROM customers WHERE id=@id";
            command.Parameters.AddWithValue("@id", id);

            try
            {
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    customer.Name = reader.GetString(1);
                    customer.Desde = reader.GetDateTime(3);
                }
                else
                {
                    return NotFound();
                }
                reader.Close();
                return Ok(customer);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Erro: " + ex.Message);
            }
            
        }
    }

    
}

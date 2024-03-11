using System;
using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);
var build = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json",optional:true,reloadOnChange:true);
IConfiguration configuration = build.Build();
string connectionString = configuration.GetConnectionString("conn_string");

string conn_string = "server=127.0.0.1;port=3306;database=barberShop;uid=root;password=callofdutty2";
try
{
    using(MySqlConnection conn=new MySqlConnection(conn_string))
    {
        conn.Open();
        Console.WriteLine("Conexão bem sucedida!");
    }
}
catch (Exception ex)
{
    Console.WriteLine("Erro: "+ex.Message);
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

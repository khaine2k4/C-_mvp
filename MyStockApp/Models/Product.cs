namespace MyStockApp.Models;

public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; } = "";
    [System.ComponentModel.DataAnnotations.Schema.Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

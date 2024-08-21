using System.ComponentModel.DataAnnotations.Schema;

namespace DapperWebAPIProject.Entity;
[Table("users")]
public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    [Column("batch_id")]
    public Guid? BatchId { get; set; }
    public override string ToString()
    {
        return $"ID: {Id}, Name: {Name}, Email: {Email}, BatchId: {BatchId}";
    }
}
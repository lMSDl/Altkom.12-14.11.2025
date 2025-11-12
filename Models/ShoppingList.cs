namespace Models
{
    public class ShoppingList : Entity
    {
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}

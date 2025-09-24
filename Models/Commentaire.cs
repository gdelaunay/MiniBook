namespace MiniBookApp.Models;

public class Commentaire : Post
{
    public int PostParentId { get; set; }      // FK explicite
    public Post PostParent { get; set; } = null!;
}
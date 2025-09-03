namespace MiniBookApp.Models;

public class Commentaire : Post
{
    public required Post PostParent { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace MiniBookApp.Models;

public class Post
{
    
    public int Id { get; set; }
    
    public required Utilisateur Auteur { get; set; }
    
    [MaxLength(280)]
    public required string Contenu { get; set; }
    
    public required DateTime DatePublication { get; set; }
    
}
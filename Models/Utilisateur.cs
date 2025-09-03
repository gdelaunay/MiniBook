using System.ComponentModel.DataAnnotations;

namespace MiniBookApp.Models;

public class Utilisateur
{
    
    public int Id { get; set; }
    
    [MaxLength(256)]
    public required string Nom { get; set; }
    
    [MaxLength(256)]
    public required string Prénom { get; set; }
    
    public required DateTime DateNaissance { get; set; }
    
    [MaxLength(256)]
    public required string Email { get; set; }

}
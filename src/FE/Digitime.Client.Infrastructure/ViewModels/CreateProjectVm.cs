using System.ComponentModel.DataAnnotations;

namespace Digitime.Client.Infrastructure.ViewModels;

public class CreateProjectVm
{
    [Required]
    [StringLength(30, ErrorMessage = "Oops... Name is too long. 30 characters max.")]
    public string Title { get; set; }
    
    [Required]
    [StringLength(30, ErrorMessage = "Oops... Code is too long. 30 characters max.")]
    public string Code { get; set; }

    [StringLength(500, ErrorMessage = "Oops... Description is too long. 500 characters max.")]
    public string? Description { get; set; }

    public string WorkspaceId { get; set; } = string.Empty;
}

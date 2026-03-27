using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record CreateGameDto(
    //the required modifier ensures that before the dto object is even accepted into our endpoint,
    //asp.det core is going to validate whether the field has an actual value in it or not in which case
    //it will return a validation error
    [Required][StringLength(50)] string Name, 
    [Range(1, 50)] int GenreId,
    [Range(1, 100)] decimal Price,
    DateOnly ReleaseDate
);

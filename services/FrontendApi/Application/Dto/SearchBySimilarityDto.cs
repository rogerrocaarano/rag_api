using Domain.Model;

namespace Application.Dto;

public class SearchBySimilarityDto
{
    public EmbeddedText Query { get; set; }
}
using Domain.Model;

namespace Application.Dto;

public class GetContextDto
{
    public List<string> QueryTags { get; set; }

    public List<Fragment> Fragments { get; set; }
}
namespace Domain.Model;

public class Collection : BaseEntity
{
    public string Name { get; set; }

    public List<MetaDataTag> MetaData { get; set; }
}
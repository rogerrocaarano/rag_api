using Domain.Model;

namespace Domain.Repository;

public interface IVectorsRepository
{
    Task<Guid> AddCollection(string name, List<MetaDataTag> metaData);

    Task DeleteCollection(Guid collectionId);

    Task UpdateCollection(Collection collection);

    Task<Collection> GetCollection(Guid collectionId);

    Task<Guid> AddDocument(EmbeddedText content, List<MetaDataTag> metaData);

    Task UpdateDocumentTags(Guid documentId, List<MetaDataTag> tags);

    Task DeleteDocument(Guid documentId);

    Task<Guid> GetDocumentIdsByMetaDataTags(List<MetaDataTag> tags);

    Task<List<Guid>> GetSimilarDocumentIds(EmbeddedText content);
}
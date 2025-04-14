using Application.Dto;
using Domain.Repository;
using Domain.Service;

namespace Application.UseCase;

public class SearchBySimilarity {

    public SearchBySimilarity() {
    }

    private IContextRepository contextDb {
        get; set;
    }

    private IVectorsRepository vectorDb {
        get; set;
    }

    private ITextProcessorService TextProcessor {
        get; set;
    }

    /// <summary>
    /// @param query 
    /// @return
    /// </summary>
    public SearchBySimilarityDto Execute(String query) {
        // TODO implement here
        return null;
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository{
    public interface IVectorsRepository {

        void addCollection();

        void deleteCollection();

        void updateCollection();

        void getCollection();

        void addDocument();

        void updateDocument();

        void deleteDocument();

        void getDocument();

        void getSimilarDocuments();

    }
}
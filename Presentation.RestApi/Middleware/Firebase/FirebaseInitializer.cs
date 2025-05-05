using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace Presentation.RestApi.Middleware.Firebase;

public static class FirebaseInitializer
{
    public static void Initialize()
    {
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile("Middleware/Firebase/google-services.json")
            });
        }
    }
}
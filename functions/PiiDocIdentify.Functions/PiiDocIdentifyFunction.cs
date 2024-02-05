using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace PiiDocIdentify.Functions
{
    public class PiiDocIdentifyFunction
    {
        private readonly ILogger<PiiDocIdentifyFunction> _logger;
        private readonly DocumentAnalysisClient _documentAnalysisClient;

        public PiiDocIdentifyFunction(
            ILogger<PiiDocIdentifyFunction> logger,
            DocumentAnalysisClient documentAnalysisClient
            )
        {
            _logger = logger;
            _documentAnalysisClient = documentAnalysisClient;
        }

        [Function(nameof(PiiDocIdentifyFunction))]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req, CancellationToken cancellationToken)
        {
            Uri idDocumentUri = new Uri("https://raw.githubusercontent.com/Azure-Samples/cognitive-services-REST-api-samples/master/curl/form-recognizer/rest-api/identity_documents.png");

            AnalyzeDocumentOperation operation = await _documentAnalysisClient.AnalyzeDocumentFromUriAsync(WaitUntil.Completed, "prebuilt-idDocument", idDocumentUri, cancellationToken: cancellationToken);

            AnalyzeResult identityDocuments = operation.Value;

            AnalyzedDocument identityDocument = identityDocuments.Documents.Single();

            if (identityDocument.Fields.TryGetValue("Address", out DocumentField addressField))
            {
                if (addressField.FieldType == DocumentFieldType.String)
                {
                    string address = addressField.Value.AsString();
                    Console.WriteLine($"Address: '{address}', with confidence {addressField.Confidence}");
                }
            }

            if (identityDocument.Fields.TryGetValue("CountryRegion", out DocumentField countryRegionField))
            {
                if (countryRegionField.FieldType == DocumentFieldType.CountryRegion)
                {
                    string countryRegion = countryRegionField.Value.AsCountryRegion();
                    Console.WriteLine($"CountryRegion: '{countryRegion}', with confidence {countryRegionField.Confidence}");
                }
            }

            if (identityDocument.Fields.TryGetValue("DateOfBirth", out DocumentField dateOfBirthField))
            {
                if (dateOfBirthField.FieldType == DocumentFieldType.Date)
                {
                    DateTimeOffset dateOfBirth = dateOfBirthField.Value.AsDate();
                    Console.WriteLine($"Date Of Birth: '{dateOfBirth}', with confidence {dateOfBirthField.Confidence}");
                }
            }

            if (identityDocument.Fields.TryGetValue("DateOfExpiration", out DocumentField dateOfExpirationField))
            {
                if (dateOfExpirationField.FieldType == DocumentFieldType.Date)
                {
                    DateTimeOffset dateOfExpiration = dateOfExpirationField.Value.AsDate();
                    Console.WriteLine($"Date Of Expiration: '{dateOfExpiration}', with confidence {dateOfExpirationField.Confidence}");
                }
            }

            if (identityDocument.Fields.TryGetValue("DocumentNumber", out DocumentField documentNumberField))
            {
                if (documentNumberField.FieldType == DocumentFieldType.String)
                {
                    string documentNumber = documentNumberField.Value.AsString();
                    Console.WriteLine($"Document Number: '{documentNumber}', with confidence {documentNumberField.Confidence}");
                }
            }

            if (identityDocument.Fields.TryGetValue("FirstName", out DocumentField firstNameField))
            {
                if (firstNameField.FieldType == DocumentFieldType.String)
                {
                    string firstName = firstNameField.Value.AsString();
                    Console.WriteLine($"First Name: '{firstName}', with confidence {firstNameField.Confidence}");
                }
            }

            if (identityDocument.Fields.TryGetValue("LastName", out DocumentField lastNameField))
            {
                if (lastNameField.FieldType == DocumentFieldType.String)
                {
                    string lastName = lastNameField.Value.AsString();
                    Console.WriteLine($"Last Name: '{lastName}', with confidence {lastNameField.Confidence}");
                }
            }

            if (identityDocument.Fields.TryGetValue("Region", out DocumentField regionfield))
            {
                if (regionfield.FieldType == DocumentFieldType.String)
                {
                    string region = regionfield.Value.AsString();
                    Console.WriteLine($"Region: '{region}', with confidence {regionfield.Confidence}");
                }
            }

            if (identityDocument.Fields.TryGetValue("Sex", out DocumentField sexfield))
            {
                if (sexfield.FieldType == DocumentFieldType.String)
                {
                    string sex = sexfield.Value.AsString();
                    Console.WriteLine($"Sex: '{sex}', with confidence {sexfield.Confidence}");
                }
            }

            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}

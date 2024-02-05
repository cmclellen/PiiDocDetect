using System.Windows.Markup;
using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
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
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            CancellationToken cancellationToken)
        {

            using (var streamReader = new StreamReader(req.Body))
            {
                var content = await streamReader.ReadToEndAsync();
                _logger.LogInformation("Content is {Content}", content);
            }

            //var idDocumentUri =
            //    new Uri(
            //        "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-REST-api-samples/master/curl/form-recognizer/rest-api/identity_documents.png");

            //var operation = await _documentAnalysisClient.AnalyzeDocumentFromUriAsync(WaitUntil.Completed,
            //    "prebuilt-idDocument", idDocumentUri, cancellationToken: cancellationToken);

            //var identityDocuments = operation.Value;

            //var identityDocument = identityDocuments.Documents.Single();

            //if (identityDocument.Fields.TryGetValue("Address", out var addressField))
            //{
            //    if (addressField.FieldType == DocumentFieldType.String)
            //    {
            //        var address = addressField.Value.AsString();
            //        Console.WriteLine($"Address: '{address}', with confidence {addressField.Confidence}");
            //    }
            //}

            //if (identityDocument.Fields.TryGetValue("CountryRegion", out var countryRegionField))
            //{
            //    if (countryRegionField.FieldType == DocumentFieldType.CountryRegion)
            //    {
            //        var countryRegion = countryRegionField.Value.AsCountryRegion();
            //        Console.WriteLine(
            //            $"CountryRegion: '{countryRegion}', with confidence {countryRegionField.Confidence}");
            //    }
            //}

            //if (identityDocument.Fields.TryGetValue("DateOfBirth", out var dateOfBirthField))
            //{
            //    if (dateOfBirthField.FieldType == DocumentFieldType.Date)
            //    {
            //        var dateOfBirth = dateOfBirthField.Value.AsDate();
            //        Console.WriteLine($"Date Of Birth: '{dateOfBirth}', with confidence {dateOfBirthField.Confidence}");
            //    }
            //}

            //if (identityDocument.Fields.TryGetValue("DateOfExpiration", out var dateOfExpirationField))
            //{
            //    if (dateOfExpirationField.FieldType == DocumentFieldType.Date)
            //    {
            //        var dateOfExpiration = dateOfExpirationField.Value.AsDate();
            //        Console.WriteLine(
            //            $"Date Of Expiration: '{dateOfExpiration}', with confidence {dateOfExpirationField.Confidence}");
            //    }
            //}

            //if (identityDocument.Fields.TryGetValue("DocumentNumber", out var documentNumberField))
            //{
            //    if (documentNumberField.FieldType == DocumentFieldType.String)
            //    {
            //        var documentNumber = documentNumberField.Value.AsString();
            //        Console.WriteLine(
            //            $"Document Number: '{documentNumber}', with confidence {documentNumberField.Confidence}");
            //    }
            //}

            //if (identityDocument.Fields.TryGetValue("FirstName", out var firstNameField))
            //{
            //    if (firstNameField.FieldType == DocumentFieldType.String)
            //    {
            //        var firstName = firstNameField.Value.AsString();
            //        Console.WriteLine($"First Name: '{firstName}', with confidence {firstNameField.Confidence}");
            //    }
            //}

            //if (identityDocument.Fields.TryGetValue("LastName", out var lastNameField))
            //{
            //    if (lastNameField.FieldType == DocumentFieldType.String)
            //    {
            //        var lastName = lastNameField.Value.AsString();
            //        Console.WriteLine($"Last Name: '{lastName}', with confidence {lastNameField.Confidence}");
            //    }
            //}

            //if (identityDocument.Fields.TryGetValue("Region", out var regionfield))
            //{
            //    if (regionfield.FieldType == DocumentFieldType.String)
            //    {
            //        var region = regionfield.Value.AsString();
            //        Console.WriteLine($"Region: '{region}', with confidence {regionfield.Confidence}");
            //    }
            //}

            //if (identityDocument.Fields.TryGetValue("Sex", out var sexfield))
            //{
            //    if (sexfield.FieldType == DocumentFieldType.String)
            //    {
            //        var sex = sexfield.Value.AsString();
            //        Console.WriteLine($"Sex: '{sex}', with confidence {sexfield.Confidence}");
            //    }
            //}

            var result =
                new
                {
                    Values = new[]
                    {
                        new
                        {
                            RecordId = 0,
                            Data = new
                            {
                                Name = "Craig"
                            }
                        }
                    }
                };

            return new OkObjectResult(result);
        }
    }
}
using System.Net;
using System.Text.Json;
using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using PiiDocIdentify.Functions.Dtos;

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
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            CancellationToken cancellationToken)
        {
            var opt = new JsonSerializerOptions(JsonSerializerDefaults.Web);

            PiiDetectRequestDto piiDetectRequestDto;
            using (var streamReader = new StreamReader(req.Body))
            {
                var content = await streamReader.ReadToEndAsync();
                _logger.LogInformation("Content is {Content}", content);

                piiDetectRequestDto = JsonSerializer.Deserialize<PiiDetectRequestDto>(content, opt)!;
            }

            var imageData = piiDetectRequestDto.Values[0].Data.Image.Data;
            var stream = new MemoryStream(Convert.FromBase64String(imageData));

            var operation = await _documentAnalysisClient.AnalyzeDocumentAsync(WaitUntil.Completed,
                "prebuilt-idDocument", stream, cancellationToken: cancellationToken);

            var piiDocumentInfoDto = new PiiDocumentInfoDto();

            var identityDocuments = operation.Value;

            var identityDocument = identityDocuments.Documents.Single();
            piiDocumentInfoDto.DocumentType = identityDocument.DocumentType;

            _logger.LogInformation("{DocumentType} found.", identityDocument.DocumentType);

            if (identityDocument.Fields.TryGetValue("Address", out var addressField))
            {
                if (addressField.FieldType == DocumentFieldType.String)
                {
                    piiDocumentInfoDto.Address = addressField.Value.AsString();
                }
            }

            if (identityDocument.Fields.TryGetValue("DateOfBirth", out var dateOfBirthField))
            {
                if (dateOfBirthField.FieldType == DocumentFieldType.Date)
                {
                    piiDocumentInfoDto.DateOfBirth = dateOfBirthField.Value.AsDate();
                }
            }

            if (identityDocument.Fields.TryGetValue("DateOfExpiration", out var dateOfExpirationField))
            {
                if (dateOfExpirationField.FieldType == DocumentFieldType.Date)
                {
                    piiDocumentInfoDto.DateOfExpiration = dateOfExpirationField.Value.AsDate();
                }
            }

            if (identityDocument.Fields.TryGetValue("DocumentNumber", out var documentNumberField))
            {
                if (documentNumberField.FieldType == DocumentFieldType.String)
                {
                    piiDocumentInfoDto.DocumentNumber = documentNumberField.Value.AsString();
                }
            }

            if (identityDocument.Fields.TryGetValue("FirstName", out var firstNameField))
            {
                if (firstNameField.FieldType == DocumentFieldType.String)
                {
                    piiDocumentInfoDto.FirstName = firstNameField.Value.AsString();
                }
            }

            if (identityDocument.Fields.TryGetValue("LastName", out var lastNameField))
            {
                if (lastNameField.FieldType == DocumentFieldType.String)
                {
                    piiDocumentInfoDto.LastName = lastNameField.Value.AsString();
                }
            }

            if (identityDocument.Fields.TryGetValue("Sex", out var sexfield))
            {
                if (sexfield.FieldType == DocumentFieldType.String)
                {
                    piiDocumentInfoDto.Sex = sexfield.Value.AsString();
                }
            }

            var result =
                new
                {
                    Values = new[]
                    {
                        new
                        {
                            RecordId = 0,
                            Data = piiDocumentInfoDto
                        }
                    }
                };

            var text = JsonSerializer.Serialize(result, opt);
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(text, cancellationToken);
            return response;
        }
    }
}
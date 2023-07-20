using System.Net;
using FluentValidation.Results;

namespace API.ResponseModels;

public static class ValidationErrorResponseFactory
{
    public static HttpValidationProblemDetails Create(ValidationResult validationResult)
    {
        var errorDictionary = new Dictionary<string, string[]>();

        foreach (var error in validationResult.Errors)
        {
            if (errorDictionary.ContainsKey(error.PropertyName))
            {
                errorDictionary[error.PropertyName].Append(error.ErrorMessage);
            }
            else
            {
                errorDictionary.Add(error.PropertyName, new string[] { error.ErrorMessage });
            }
        }

        var validationProblem = new HttpValidationProblemDetails(errorDictionary);
        validationProblem.Title = "Failed to validate request";
        validationProblem.Detail = "One or more validation errors occurred.";
        validationProblem.Status = (int)HttpStatusCode.BadRequest;
        return validationProblem;
    }
}

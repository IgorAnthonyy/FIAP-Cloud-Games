using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;

namespace FCG.Api.Controllers;

/// <summary>
/// Base Controller
/// </summary>
[ApiController]
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Retorno padrão de sucesso
    /// </summary>
    protected IActionResult Success()
    {
        return Ok(new { result = "success" });
    }

    /// <summary>
    /// Retorno de sucesso com objeto
    /// </summary>
    protected IActionResult Success(object value)
    {
        if (value == null)
            value = new { };

        return Ok(value);
    }

    /// <summary>
    /// Retorna NotFound se o objeto for nulo
    /// </summary>
    protected IActionResult SuccessOrNotFound(object value)
    {
        if (value == null)
            return NotFound();

        return Ok(value);
    }

    /// <summary>
    /// Retorno para criação de recurso
    /// </summary>
    protected IActionResult CreatedResult(object value)
    {
        return StatusCode(StatusCodes.Status201Created, value);
    }

    /// <summary>
    /// Retorno REST padrão com Location
    /// </summary>
    protected IActionResult CreatedResult(string action, object routeValues, object value)
    {
        return CreatedAtAction(action, routeValues, value);
    }

    /// <summary>
    /// Retorno customizado
    /// </summary>
    protected IActionResult CustomResponse(HttpStatusCode statusCode, object value)
    {
        return StatusCode((int)statusCode, value);
    }

    /// <summary>
    /// Converte QueryString em objeto
    /// </summary>
    protected T ParseQueryString<T>()
    {
        if (!Request.QueryString.HasValue)
            return default;

        var values = QueryHelpers.ParseQuery(Request.QueryString.Value);

        var dictionary = values.ToDictionary(
            x => x.Key,
            x => x.Value.Count > 1
                ? (object)x.Value.ToArray()
                : x.Value.ToString()
        );

        var jObject = JObject.FromObject(dictionary);

        return jObject.ToObject<T>();
    }

    /// <summary>
    /// Expor header de download
    /// </summary>
    protected void ExposeFileNameHeader()
    {
        Response.Headers.Append("Access-Control-Expose-Headers", "Content-Disposition");
    }
}
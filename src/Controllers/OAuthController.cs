using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace EXAMPLE.SOURCE.API.Controllers
{
    [Route("api/oauth")]
    [ApiExplorerSettings(GroupName = "0-OAuth", IgnoreApi = false)]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private readonly SymmetricSecurityKey _key = new(Encoding.ASCII.GetBytes("i8Z5SkolOrUOyh69p04kxNkTnovE1Ye6"));

        // POST: api/oauth/token
        /// <summary>
        /// Retrieve an access token using a clientId and clientSecret
        /// </summary>
        /// <remarks>
        /// <h2>Implementation notes</h2>
        /// We prefer OAuth for authentication because it’s secure, easy to use, and works smoothly with modern apps and APIs.
        /// <br/>
        /// Example request:
        /// 
        ///     POST /oauth/token
        ///     {
        ///         "client_id": "example-client-id",
        ///         "client_secret": "example-client-secret"
        ///     }
        /// </remarks>
        /// <response code="200">A valid access token was issued.</response>
        /// <response code="400">The request was malformed or missing required fields.</response>
        /// <response code="401">Authentication failed or was not provided.</response>
        [HttpPost("token")]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        [ProducesResponseType(typeof(OAuthErrorResponse), 400)]
        [ProducesResponseType(typeof(OAuthErrorResponse), 401)]
        public ActionResult<TokenResponse> GenerateToken([FromBody] TokenRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.ClientId) || string.IsNullOrWhiteSpace(request.ClientSecret))
            {
                return BadRequest(new OAuthErrorResponse
                {
                    Error = "invalid_request",
                    ErrorDescription = "ClientId and ClientSecret are required."
                });
            }

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddMinutes(30);

            var token = new JwtSecurityToken(
                claims: new[] { new Claim(ClaimTypes.Name, request.ClientId) },
                expires: expiration,
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new TokenResponse
            {
                AccessToken = tokenString,
                TokenType = "Bearer",
                ExpiresIn = (int)(expiration - DateTime.UtcNow).TotalSeconds
            });
        }

        public record TokenRequest
        {
            /// <summary>The ClientId to authenticate to the example API.</summary>
            /// <example>example-client-id</example>
            [JsonPropertyName("client_id")]
            public string ClientId { get; init; }

            /// <summary>The ClientSecret to authenticate to the example API.</summary>
            /// <example>example-client-secret</example>
            [JsonPropertyName("client_secret")]
            public string ClientSecret { get; init; }
        }

        public record TokenResponse
        {
            [JsonPropertyName("access_token")]
            public string AccessToken { get; set; }

            [JsonPropertyName("token_type")]
            public string TokenType { get; set; } = "Bearer";

            [JsonPropertyName("expires_in")]
            public int ExpiresIn { get; set; }

            [JsonIgnore]
            public DateTime ExpiresAt => DateTime.UtcNow.AddSeconds(ExpiresIn);
        }

        public record OAuthErrorResponse
        {
            [JsonPropertyName("error")]
            public string Error { get; init; }

            [JsonPropertyName("error_description")]
            public string ErrorDescription { get; init; }
        }
    }
}

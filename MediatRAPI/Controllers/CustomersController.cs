using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Asp.Versioning;
using MediatR;
using MediatRHandlers.Application.Customers.Commands;
using MediatRHandlers.Application.Customers.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MediatRAPI.Controllers
{
    [ApiController]
    [Route("api/v{v:apiVersion}/customers")]
    [ApiVersion(1.0)]
    [Authorize]
    public class CustomersController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerCommand command)
        {
            Guid id = await _mediator.Send(command);
            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _mediator.Send(new GetCustomerByIdQuery(id)));
        }
    }

    [ApiController]
    [Route("api/auth")]
    public class AuthController(IConfiguration configuration) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;

        [HttpPost("token")]
        [AllowAnonymous]
        public IActionResult GetToken()
        {
            string issuer = _configuration["Jwt:Issuer"] ?? string.Empty;
            string audience = _configuration["Jwt:Audience"] ?? string.Empty;
            string key = _configuration["Jwt:Key"] ?? string.Empty;

            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            SymmetricSecurityKey securityKey = new(keyBytes);
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims =
            [
                new Claim(JwtRegisteredClaimNames.Sub, "test-user"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];

            JwtSecurityToken token = new(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { access_token = tokenString });
        }
    }
}
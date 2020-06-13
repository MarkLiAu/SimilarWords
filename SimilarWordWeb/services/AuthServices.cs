using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace SimilarWordWeb.auth
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }

    public class AppSettings
    {
        public string Secret { get; set; }
    }

    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<User> _users = new List<User>
        {
            new User { Id = 1, FirstName = "Mark", LastName = "Li", Username = "markli", Password = "test" }
        };

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public User Authenticate(string username, string password)
        {
            var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.GivenName,user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim(ClaimTypes.Email,user.Username),
                    
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }

        ////Authentication successful, Issue Token with user credentials 
        ////Provide the security key which is given in 
        ////Startup.cs ConfigureServices() method 
        //var key = Encoding.ASCII.GetBytes
        //("YourKey-2374-OFFKDI940NG7:56753253-tyuw-5769-0921-kfirox29zoxv");
        ////Generate Token for user 
        //var JWToken = new JwtSecurityToken(
        //    issuer: "http://localhost:45092/",
        //    audience: "http://localhost:45092/",
        //    claims: GetUserClaims(user),
        //    notBefore: new DateTimeOffset(DateTime.Now).DateTime,
        //    expires: new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,
        //    //Using HS256 Algorithm to encrypt Token  
        //    signingCredentials: new SigningCredentials
        //    (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //);
        //var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
        //        return token;


        public IEnumerable<User> GetAll()
        {
            // return users without passwords
            return _users.Select(x => {
                x.Password = null;
                return x;
            });
        }

    }
}

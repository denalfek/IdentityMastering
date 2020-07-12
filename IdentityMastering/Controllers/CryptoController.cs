using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMastering.Controllers
{
    [Route("api/[controller]")]
    public class CryptoController : ControllerBase
    {
        [HttpGet, Route("test")]
        public void Index(string message, string password)
        {
            var cryptoText = Protector.Encrypt(message, password);

            var clearText = Protector.Decrypt(cryptoText, password);
        }
    }
}

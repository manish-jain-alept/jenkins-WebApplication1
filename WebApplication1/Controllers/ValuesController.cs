using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication1.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            // **********
            // PASSWORD TO HASH
            // **********
            string myPassword = "Alept1#";
            string savedPasswordHash = PasswordToHash(myPassword);
            // **********
            // HASH TO PASSWORD
            // **********
            bool Validate = HashToPassword(savedPasswordHash, myPassword + "1");

            return new string[] { myPassword, "value2" };
        }

        public string PasswordToHash(string myPassword)
        {
            // SALT + ORIGINAL_PASSWORD = HASH

            // GENERATE RANDOM SALT
            byte[] salt;
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            // ITERATION OF 10000, MAKE THIS FUNCTION SLOW. SO ATTACKER CAN'T FREQUENTLY TRY NEW PASSWORD
            var pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(myPassword, salt, 10000);

            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }

        public bool HashToPassword(string savedPasswordHash, string myPassword)
        {
            bool validate = false;
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            byte[] salt = new byte[16];

            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new System.Security.Cryptography.Rfc2898DeriveBytes(myPassword, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            //PASSWORD COMPARISION
            int ok = 1;
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                    ok = 0;

                if (ok == 1)
                {
                    validate = true;
                }
            }
            return validate;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}

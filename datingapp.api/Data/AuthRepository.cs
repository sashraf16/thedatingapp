using System;
using System.Threading.Tasks;
using datingapp.api.Models;
using Microsoft.EntityFrameworkCore;

namespace datingapp.api.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;

        }
        public async Task<user> LoginAsync(string username, string password)
        {
           var user = await _context.users.FirstOrDefaultAsync(x => x.username == username);

           if (user == null)
                return null;

            if (!VerifyPassWordHash(password, user.passwordhash, user.passwordsalt))
                return null;

            //auth successsful
            return user;
        }

        private bool VerifyPassWordHash(string password, byte[] passwordhash, byte[] passwordsalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordsalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i<computedHash.Length; i++)
                {
                    if(computedHash[i] != passwordhash[i]) return false;
                }
            }

            return true;
        }

        public async Task<user> Register(user user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password,out passwordHash,out passwordSalt);

            user.passwordhash = passwordHash;
            user.passwordsalt = passwordSalt;

            await _context.users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordhash, out byte[] passwordsalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordsalt = hmac.Key;
                passwordhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.users.AnyAsync(x => x.username == username))
                return true;

            return false;
        }
    }
}
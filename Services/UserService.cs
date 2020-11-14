using Prism.Data;
using Prism.Models;
using System;
using System.Linq;

namespace Prism.Services
{
    public class UserService
    {
        private DatabaseCtx context;

        public UserService(DatabaseCtx context)
        {
            this.context = context;
        }

        public User Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return null;
            }
            var user = context.Users.SingleOrDefault(u => u.Email.Equals(email));
            
            if (user == null)
                return null;
            if (!VerifyPassword(password, user.Password, user.Salt))
                return null;
            return user;
        }

        public User Register(User user, string password)
        {
            var pair = PasswordHash(password);

            user.Password = pair.Item1;
            user.Salt = pair.Item2;

            context.Add(user);
            context.SaveChanges();

            return user;
        }

        public User GetById(int id)
        {
            return context.Users.Find(id);
        }

        public void Update(User newUser, string password)
        {
            var user = context.Users.Find(newUser.UserId);

            if (user == null)
                throw new ApplicationException("User not found");

            context.Users.Update(newUser);
            context.SaveChanges();

        }

        public void Delete(int id)
        {
            var user = context.Users.Find(id);
            if (user != null)
            {
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }

        private (string,string) PasswordHash(string password)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty.");
            var sha256 = new System.Security.Cryptography.HMACSHA256();
            
            string passwordSalt = BitConverter.ToString(sha256.Key);
            string passwordHash = BitConverter.ToString(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
            System.Diagnostics.Debug.WriteLine(passwordHash);
            System.Diagnostics.Debug.WriteLine(passwordSalt);
            sha256.Clear();

            return (passwordHash, passwordSalt);
        }

        private bool VerifyPassword(string password, string passwordHash, string salt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty");
            /*if (passwordHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (password.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");*/

            var sha256 = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(salt));
            System.Diagnostics.Debug.WriteLine(salt);
            string computedHash = BitConverter.ToString(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
            System.Diagnostics.Debug.WriteLine(passwordHash);
            System.Diagnostics.Debug.WriteLine(computedHash);

            if (computedHash.Equals(passwordHash))
                return false;

            return true;
        }
    }
}

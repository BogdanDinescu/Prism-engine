using Prism.Data;
using Prism.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Prism.Services
{
    public interface IUserService
    {
        User Authenticate(string email, string password);
        User Register(User user, string password);
        void UpdatePassword(int id, string oldPassword, string newPassword);
        User Update(int id, UserUpdate newUser);
        void Delete(int id);
        User GetById(int id);
    }

    public class UserService: IUserService
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
            User user = context.Users.SingleOrDefault(u => u.Email.Equals(email));
            
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

        public void UpdatePassword(int id, string oldPassword, string newPassword)
        {
            var user = context.Users.Find(id);

            if (user == null)
                throw new ApplicationException("User not found");

            if (!VerifyPassword(oldPassword, user.Password, user.Salt))
                throw new ApplicationException("Old password is not corect");

            var pair = PasswordHash(newPassword);

            user.Password = pair.Item1;
            user.Salt = pair.Item2;

            context.Users.Update(user);
            context.SaveChanges();
        }

        public User Update(int id, UserUpdate newUser)
        {
            var user = context.Users.Find(id);

            if (user == null)
                throw new ApplicationException("User not found");

            user.Name = newUser.Name;
            context.Users.Update(user);

            context.SaveChanges();
            return user;
        }

        public void Delete(int id)
        {
            var user = context.Users.Find(id);
            var userPreference = context.UserPreferences.Find(id);
            if (user != null)
            {
                context.UserPreferences.Remove(userPreference);
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }

        private (string,byte[]) PasswordHash(string password)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty.");
            byte[] passwordSalt = new byte[128 / 8];
            var rng = RandomNumberGenerator.Create();
            var sha256 = SHA256.Create();
                
            rng.GetBytes(passwordSalt);
            string passwordHash = BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password).Concat(passwordSalt).ToArray()));
            sha256.Clear();
            
            return (passwordHash, passwordSalt);
        }

        private bool VerifyPassword(string password, string passwordHash, byte[] salt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty");
            var sha256 = SHA256.Create();
            
            string computedHash = BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password).Concat(salt).ToArray()));
            sha256.Clear();
            
            if (!computedHash.Equals(passwordHash))
                return false;
            
            return true;
        }
    }
}

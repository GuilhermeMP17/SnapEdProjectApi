using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using SnapEd.Infra.DataContexts;
using SnapEd.Infra.Models;

namespace SnapEd.Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsersController : ApiController
    {
        private SnapEdDataContext db = new SnapEdDataContext();
        private const string PASSWORDFIELD = "Password";

        // GET: api/Users
        //[Authorize]
        public IQueryable<User> GetUsers()
        {
            return db.Users.Where(o => o.Active == true);
        }

        // GET: api/Users/5
        //[Authorize]
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        //[Authorize]
        [ResponseType(typeof(User))]
        public IHttpActionResult GetByName(string name)
        {
            User user = db.Users.Where(p => p.Login.ToLower() == name.ToLower()).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.IdUser)
            {
                return BadRequest();
            }
            //User userBd = db.User.Find(id);

            //if (userBd.password != user.password)
            //{
            //    return BadRequest();
            //}
            updatePasswordWithHash(user);
            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User validation = db.Users.Where(p => p.Login.ToLower() == user.Login.ToLower()).FirstOrDefault();
            if (validation != null)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);
            updatePasswordWithHash(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.IdUser }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.IdUser == id) > 0;
        }

        private string getSavedPassword(int id)
        {
            return new SnapEdDataContext().Users.Find(id).Password;
        }

        private string getCurrentPassword(User tableContext)
        {
            return db.Entry(tableContext).Property(PASSWORDFIELD).CurrentValue.ToString();
        }

        private void updatePasswordWithHash(User tableContext)
        {
            var _currentPassword = getCurrentPassword(tableContext);

            if (tableContext.IdUser == 0 || getSavedPassword(tableContext.IdUser) != _currentPassword)
                db.Entry(tableContext).Property(PASSWORDFIELD).CurrentValue = Cryptography.GetMD5Hash(_currentPassword);
        }
    }

    public static class Cryptography
    {
        public static string GetMD5Hash(string Valor)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(Valor))).Replace("--", string.Empty);
        }
    }
}
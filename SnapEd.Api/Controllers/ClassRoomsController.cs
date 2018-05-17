using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SnapEd.Infra.DataContexts;
using SnapEd.Infra.Models;
using System.Web.Http.Cors;
using System.Security.Claims;

namespace SnapEd.Api.Controllers
{
    #region HEADS
    [RoutePrefix("api")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    #endregion
    public class ClassRoomsController : ApiController
    {
        #region CONTEXT
        private SnapEdDataContext db = new SnapEdDataContext();
        #endregion

        #region GETS
        // GET: api/ClassRooms
        [Authorize]
        [Route("ClassRooms")]
        public IQueryable<ClassRoom> GetClassRoom()
        {
            return db.ClassRoom;
        }

        // GET: api/ClassRooms/5
        [Authorize]
        [Route("ClassRooms/{id}")]
        [ResponseType(typeof(ClassRoom))]
        public async Task<IHttpActionResult> GetClassRoom(int id)
        {
            ClassRoom classRoom = await db.ClassRoom.FindAsync(id);
            if (classRoom == null)
            {
                return NotFound();
            }

            return Ok(classRoom);
        }
        #endregion

        #region PUT
        // PUT: api/ClassRooms/5
        [Authorize(Roles = "Administrator")]
        [Route("ClassRooms/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutClassRoom(int id, ClassRoom classRoom)
        {
            ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;
            var currentUser = db.Users.Where(w => w.Login.ToLower() == claimsIdentity.Name.ToLower()).SingleOrDefault();
            classRoom.IdUserCreated = currentUser.IdUser;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != classRoom.IdClassRom)
            {
                return BadRequest();
            }

            db.Entry(classRoom).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassRoomExists(id))
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
        #endregion

        #region POST
        // POST: api/ClassRooms
        [Authorize(Roles = "Administrator")]
        [Route("ClassRooms/{id}")]
        [ResponseType(typeof(ClassRoom))]
        public async Task<IHttpActionResult> PostClassRoom(ClassRoom classRoom)
        {
            ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;
            var currentUser = db.Users.Where(w => w.Login.ToLower() == claimsIdentity.Name.ToLower()).SingleOrDefault();
            classRoom.IdUserCreated = currentUser.IdUser;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ClassRoom.Add(classRoom);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = classRoom.IdClassRom }, classRoom);
        }
        #endregion

        #region DELETE
        // DELETE: api/ClassRooms/5
        [Authorize(Roles = "Administrator")]
        [Route("ClassRooms/{id}")]
        [ResponseType(typeof(ClassRoom))]
        public async Task<IHttpActionResult> DeleteClassRoom(int id)
        {
            ClassRoom classRoom = await db.ClassRoom.FindAsync(id);
            if (classRoom == null)
            {
                return NotFound();
            }

            db.ClassRoom.Remove(classRoom);
            await db.SaveChangesAsync();

            return Ok(classRoom);
        }
        #endregion

        #region DISPOSE
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Exists
        private bool ClassRoomExists(int id)
        {
            return db.ClassRoom.Count(e => e.IdClassRom == id) > 0;
        }
        #endregion
    }
}
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

namespace SnapEd.Api.Controllers
{
    public class ClassRoomsController : ApiController
    {
        private SnapEdDataContext db = new SnapEdDataContext();

        // GET: api/ClassRooms
        public IQueryable<ClassRoom> GetClassRoom()
        {
            return db.ClassRoom;
        }

        // GET: api/ClassRooms/5
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

        // PUT: api/ClassRooms/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutClassRoom(int id, ClassRoom classRoom)
        {
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

        // POST: api/ClassRooms
        [ResponseType(typeof(ClassRoom))]
        public async Task<IHttpActionResult> PostClassRoom(ClassRoom classRoom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ClassRoom.Add(classRoom);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = classRoom.IdClassRom }, classRoom);
        }

        // DELETE: api/ClassRooms/5
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClassRoomExists(int id)
        {
            return db.ClassRoom.Count(e => e.IdClassRom == id) > 0;
        }
    }
}
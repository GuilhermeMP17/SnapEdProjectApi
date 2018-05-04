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

namespace SnapEd.Api.Controllers
{
    #region HEADS
    [RoutePrefix("api")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    #endregion
    public class StudentClassesController : ApiController
    {
        #region CONTEXT
        private SnapEdDataContext db = new SnapEdDataContext();
        #endregion

        #region GETS
        // GET: api/StudentClasses
        [Authorize]
        [Route("StudentClasses")]
        public IQueryable<StudentClass> GetStudantClass()
        {
            return db.StudantClass;
        }

        // GET: api/StudentClasses/5
        [Authorize]
        [Route("StudentClasses/{id}")]
        [ResponseType(typeof(StudentClass))]
        public async Task<IHttpActionResult> GetStudentClass(int id)
        {
            StudentClass studentClass = await db.StudantClass.FindAsync(id);
            if (studentClass == null)
            {
                return NotFound();
            }

            return Ok(studentClass);
        }
        #endregion

        #region PUT
        // PUT: api/StudentClasses/5
        [Authorize (Roles = "Administrator, Professor, Aluno")]
        [Route("StudentClasses/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStudentClass(int id, StudentClass studentClass)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != studentClass.IdMatriculation)
            {
                return BadRequest();
            }

            db.Entry(studentClass).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentClassExists(id))
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
        // POST: api/StudentClasses
        [Authorize(Roles = "Administrator, Professor, Aluno")]
        [Route("StudentClasses")]
        [ResponseType(typeof(StudentClass))]
        public async Task<IHttpActionResult> PostStudentClass(StudentClass studentClass)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StudantClass.Add(studentClass);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = studentClass.IdMatriculation }, studentClass);
        }
        #endregion

        #region DELETE
        // DELETE: api/StudentClasses/5
        [Authorize(Roles = "Administrator, Professor, Aluno")]
        [Route("StudentClasses/{id}")]
        [ResponseType(typeof(StudentClass))]
        public async Task<IHttpActionResult> DeleteStudentClass(int id)
        {
            StudentClass studentClass = await db.StudantClass.FindAsync(id);
            if (studentClass == null)
            {
                return NotFound();
            }

            db.StudantClass.Remove(studentClass);
            await db.SaveChangesAsync();

            return Ok(studentClass);
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

        #region EXISTS
        private bool StudentClassExists(int id)
        {
            return db.StudantClass.Count(e => e.IdMatriculation == id) > 0;
        }
        #endregion
    }
}
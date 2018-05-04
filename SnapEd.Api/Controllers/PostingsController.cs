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
    public class PostingsController : ApiController
    {
        #region CONTEXT
        private SnapEdDataContext db = new SnapEdDataContext();
        #endregion

        #region GETS
        // GET: api/Postings
        [Authorize]
        [Route("Postings")]
        public IQueryable<Posting> GetPosting()
        {
            return db.Posting;
        }

        // GET: api/Postings/5
        [Authorize]
        [Route("Postings/{id}")]
        [ResponseType(typeof(Posting))]
        public async Task<IHttpActionResult> GetPosting(int id)
        {
            Posting posting = await db.Posting.FindAsync(id);
            if (posting == null)
            {
                return NotFound();
            }

            return Ok(posting);
        }
        #endregion

        #region PUT
        // PUT: api/Postings/5
        [Authorize(Roles = "Administrator, Professor, Aluno")]
        [Route("Postings/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPosting(int id, Posting posting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != posting.IdPosting)
            {
                return BadRequest();
            }

            db.Entry(posting).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostingExists(id))
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
        // POST: api/Postings
        [Authorize (Roles = "Administrator, Professor, Aluno")]
        [Route("Postings")]
        [ResponseType(typeof(Posting))]
        public async Task<IHttpActionResult> PostPosting(Posting posting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Posting.Add(posting);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = posting.IdPosting }, posting);
        }
        #endregion

        #region DELETE
        // DELETE: api/Postings/5
        [Authorize(Roles = "Administrator, Professor, Aluno")]
        [Route("Postings/{id}")]
        [ResponseType(typeof(Posting))]
        public async Task<IHttpActionResult> DeletePosting(int id)
        {
            Posting posting = await db.Posting.FindAsync(id);
            if (posting == null)
            {
                return NotFound();
            }

            db.Posting.Remove(posting);
            await db.SaveChangesAsync();

            return Ok(posting);
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
        private bool PostingExists(int id)
        {
            return db.Posting.Count(e => e.IdPosting == id) > 0;
        }
        #endregion
    }
}
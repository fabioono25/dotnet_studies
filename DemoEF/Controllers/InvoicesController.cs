using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemoEF.Data;

namespace DemoEF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly InvoiceDbContext _context;

        public InvoicesController(InvoiceDbContext context)
        {
            _context = context;
        }

        // GET: api/Invoices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices(int page = 1, int pageSize = 10, InvoiceStatus? status = null)
        {
            return await _context.Invoices
                            .AsNoTracking()
                            .Where(i => !status.HasValue || i.Status == status)
                            .OrderByDescending(i => i.InvoiceDate)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
        }

        // GET: api/Invoices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(Guid id)
        {
            var invoice = await _context.Invoices.FindAsync(id);

            if (invoice == null)
            {
                return NotFound();
            }

            return invoice;
        }

        // PUT: api/Invoices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoice(Guid id, Invoice invoice)
        {
            // if (id != invoice.Id)
            // {
            //     return BadRequest();
            // }

            // _context.Entry(invoice).State = EntityState.Modified;

            // try
            // {
            //     await _context.SaveChangesAsync();
            // }
            // catch (DbUpdateConcurrencyException)
            // {
            //     if (!InvoiceExists(id))
            //     {
            //         return NotFound();
            //     }
            //     else
            //     {
            //         throw;
            //     }
            // }

            // return NoContent();
            if (id != invoice.Id)
            {
                return BadRequest();
            }

            //_context.Entry(invoice).State = EntityState.Modified;

            try
            {
                var invoiceToUpdate = await _context.Invoices.FindAsync(id);
                if (invoiceToUpdate == null)
                {
                    return NotFound();
                }
                // invoiceToUpdate.InvoiceNumber = invoice.InvoiceNumber;
                // invoiceToUpdate.ContactName = invoice.ContactName;
                // invoiceToUpdate.Description = invoice.Description;
                // invoiceToUpdate.Amount = invoice.Amount;
                // invoiceToUpdate.InvoiceDate = invoice.InvoiceDate;
                // invoiceToUpdate.DueDate = invoice.DueDate;
                // invoiceToUpdate.Status = invoice.Status;

                // Update only the properties that have changed - cannot be used for navigation properties
                _context.Entry(invoiceToUpdate).CurrentValues.SetValues(invoice);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Invoices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInvoice", new { id = invoice.Id }, invoice);
        }

        // DELETE: api/Invoices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(Guid id)
        {
            // var invoice = await _context.Invoices.FindAsync(id);
            // if (invoice == null)
            // {
            //     return NotFound();
            // }

            // _context.Invoices.Remove(invoice);
            // await _context.SaveChangesAsync();

            // return NoContent();
            var affectedRows = await _context.Invoices.Where(i => i.Id == id).ExecuteDeleteAsync();
            return affectedRows == 0 ? NotFound() : NoContent();
        }

        private bool InvoiceExists(Guid id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }
    }
}

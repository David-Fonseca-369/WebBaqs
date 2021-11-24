using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebBaqs.Data;
using WebBaqs.Data.BAQS;
using WebBaqs.Entities;

namespace WebBaqs.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class BAQSController : ControllerBase
    {
        private readonly DbContextSystem context;

        public BAQSController(DbContextSystem context)
        {
            this.context = context;
        }

        //GET api/BAQS/getBAQS
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<BAQ>>> getBAQS()
        {
            return await context.BAQs.ToListAsync();
        }

        //GET api/BAQS/Job
        [HttpGet("[action]/{option?}")]
        public async Task<ActionResult> Job(string option)
        {
            var job = await context.BAQs
                 .Where(x => x.status_baq == true)
                 .FirstOrDefaultAsync(x => x.id == 1);
            if (job != null)
            {
                Jobs.GetJobs(option);
            }
            else
            {
                Debug.WriteLine("Desactivado --- JOB");
            }

            return Ok();
        }

        //GET api/BAQS/Labor_time
        [HttpGet("[action]/{option?}")]
        public async Task<ActionResult> Labor_Time(string option)

        {
            var laborTime = await context.BAQs
                .Where(x => x.status_baq == true)
                .FirstOrDefaultAsync(x => x.id == 2);

            if (laborTime != null)
            {
                LaborTime.GetLaborTime(option);
            }
            else
            {
                Debug.WriteLine("Desactivado -- JOB LABOR");
                return NotFound();
            }

            return Ok();
        }

        //GET api/BAQS/Job_Materials
        [HttpGet("[action]/{option?}")]
        public async Task<ActionResult> Job_Materials(string option)
        {
            var jobMaterials = await context.BAQs
                .Where(x => x.status_baq == true)
                .FirstOrDefaultAsync(x => x.id == 3);

            if (jobMaterials != null)
            {
                JobMaterials.GetJobMaterials(option);
            }
            else
            {
                Debug.WriteLine("Desactivado --- JOB MATERIALS");
                return NotFound();
            }
            return Ok();
        }

        //GET api/BAQS/Part_Cost
        [HttpGet("[action]/{option?}")]
        public async Task<ActionResult> Part_Cost(string option)
        {
            var partCost = await context.BAQs
                .Where(x => x.status_baq == true)
                .FirstOrDefaultAsync(x => x.id == 4);
            if (partCost != null)
            {
                PartCost.GetPartCost(option);
            }
            else
            {
                Debug.WriteLine("Desactivado -- PART COST");
                return NotFound();
            }

            return Ok();
        }

        //GET api/BAQS/Booking
        [HttpGet("[action]/{option?}")]
        public async Task<ActionResult>Booking([FromRoute]string option)
        {
            //Checar si el BAQ esta activo o no
            //
             await BookingsReport.GetBookingsReport(option);
            return Ok();
        }

        //GET api/BAQs/WIPReconciliation
        [HttpGet("[action]/{option?}")]
        public async Task<ActionResult> WIP_Reconciliation(string option)
        {
            var wipReconciliation = await context.BAQs
                .Where(x => x.status_baq == true)
                .FirstOrDefaultAsync(x => x.id == 5);

            if (wipReconciliation != null)
            {
                WIPReconciliation.GetWIPReconciliation(option);
            }
            else
            {
                return NotFound();

            }
            return Ok();

        }

    }
}

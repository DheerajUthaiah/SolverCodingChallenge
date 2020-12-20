using DataService.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolverDataController : ControllerBase
    {
        private ISolverDataService SolverDataService;
        private ILogger Logger;

        public SolverDataController(ISolverDataService solverDataService, ILogger logger)
        {
            SolverDataService = solverDataService;
            Logger = logger;
        }

        [HttpGet("Data/{database}/{table}")]
        public async Task<string> GetData(string database, string table)
        {
            var result = await DoGetData(() => SolverDataService.GetData(database, table));
            return result;
        }

        private async Task<string> DoGetData(Func<Task<string>> method)
        {
            try
            {
                return await method.Invoke();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Invalid object"))
                {
                    return Ok("Invalid Table name");
                }
                else if (ex.Message.Contains("Invalid Database"))
                {
                    return BadRequest("Invalid Database");
                }
                else
                {
                    return BadRequest("Unexpected error occured");
                }
            }
        }
    }
}

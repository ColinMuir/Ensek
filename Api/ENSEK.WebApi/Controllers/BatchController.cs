using AutoMapper;
using ENSEK.WebApi.DTOs;
using ENSEK.WebApi.Infrastucture.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ENSEK.WebApi.Controllers
{
    
    [ApiController]
    public class BatchController : ControllerBase
    {
        private readonly ILogger<BatchController> logger;
        private readonly IBatchService batchService;
        private readonly IMapper mapper;

        public BatchController(ILogger<BatchController> logger, IBatchService batchService, IMapper mapper)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.batchService = batchService ?? throw new ArgumentNullException(nameof(batchService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("meter-reading-uploads")]
        public async Task<ActionResult<BatchDto>> PostMeterReadBatch(IFormFile file)
        {
            if (file == null)
                return BadRequest();

            var batch = await batchService.ProcessBatchAsync(file);

            if (batch.IsFailure)
            {
                logger.LogError(batch.Error);
                return Problem("Error processing batch");
            }

            var recordToReturn = mapper.Map<BatchDto>(batch.Value);

            return CreatedAtRoute("GetMeterReadBatch", new { id = recordToReturn.Id }, recordToReturn);
        }

        [HttpGet("api/[controller]")]
        public ActionResult<IEnumerable<BatchDto>> Get()
        {
            var batches = batchService.Get();

            return Ok(mapper.Map<IEnumerable<BatchDto>>(batches));
        }


        [HttpGet("api/[controller]/{id}", Name = "GetMeterReadBatch")]
        public async Task<ActionResult<BatchDto>> GetMeterReadBatch(int id)
        {
            var batch = await batchService.GetAsync(id);

            if (batch == null)
                return NotFound();

            return Ok(mapper.Map<BatchDto>(batch));
        }
    }
}
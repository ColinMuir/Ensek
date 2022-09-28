using AutoMapper;
using ENSEK.WebApi.DTOs;
using ENSEK.WebApi.Infrastucture.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ENSEK.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeterReadsController : ControllerBase
    {
        private readonly ILogger<MeterReadsController> logger;
        private readonly IMeterReadingService meterReadingService;
        private readonly IMapper mapper;

        public MeterReadsController(ILogger<MeterReadsController> logger, IMeterReadingService meterReadingService, IMapper mapper)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.meterReadingService = meterReadingService ?? throw new ArgumentNullException(nameof(meterReadingService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult<IEnumerable<MeterReadDto>> Get()
        {
            var reads = meterReadingService.Get();

            return Ok(mapper.Map<IEnumerable<MeterReadDto>>(reads));
        }
    }
}
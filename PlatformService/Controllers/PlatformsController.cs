using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
	private readonly IPlatformRepo _repository;
	private readonly IMapper _mapper;

	public PlatformsController(IPlatformRepo repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	[HttpGet]
	public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
	{
		Console.WriteLine("Getting Platforms...");
		var platformsAsModels = _repository.GetAllPlatforms();
		var platformsAsDtos = _mapper.Map<IEnumerable<PlatformReadDto>>(platformsAsModels);
		return Ok(platformsAsDtos);
	}

	[HttpGet("{id}", Name = "GetPlatformById")]
	public ActionResult<PlatformReadDto> GetPlatformById(int id)
	{
		var platformModel = _repository.GetPlatformById(id);
		if (platformModel is null)
		{
			return NotFound();
		}
		var platformDto = _mapper.Map<PlatformReadDto>(platformModel);
		return Ok(platformDto);
	}

	[HttpPost]
	public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto platformDto)
	{
		var platformModel = _mapper.Map<Platform>(platformDto);
		_repository.CreatePlatform(platformModel);
		_repository.SaveChanges();

		var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);
		return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);
	}
}

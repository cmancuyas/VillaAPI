using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using VillaAPI.Data;
using VillaAPI.Models;
using VillaAPI.Models.Dto;
using VillaAPI.Repository.IRepository;

namespace VillaAPI.Controllers.v2;

[Route("api/v{version:apiVersion}/VillaNumberAPI")]
[ApiController]
[ApiVersion("2.0")]
public class VillaNumberAPIController : Controller
{
    protected APIResponse _response;
    private readonly IVillaNumberRepository _dbVillaNumber;
    private readonly IVillaRepository _dbVilla;
    private readonly IMapper _mapper;

    public VillaNumberAPIController(IVillaNumberRepository dbVillaNumber, IVillaRepository dbVilla, IMapper mapper)
    {
        _dbVillaNumber = dbVillaNumber;
        _dbVilla = dbVilla;
        _mapper = mapper;
        _response = new();
    }

    //[MapToApiVersion("2.0")]
    [HttpGet("GetString")]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }
}

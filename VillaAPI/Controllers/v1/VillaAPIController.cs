using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using VillaAPI.Data;
using VillaAPI.Models;
using VillaAPI.Models.Dto;
using VillaAPI.Repository.IRepository;

namespace VillaAPI.Controllers.v1;

[Route("api/v{version:apiVersion}/villaAPI")]
[ApiController]
//Deprecated = true
[ApiVersion("1.0")]
public class VillaAPIController : Controller
{
    protected APIResponse _response;
    private readonly IVillaRepository _dbVilla;
    private readonly IMapper _mapper;

    public VillaAPIController(IVillaRepository dbVilla, IMapper mapper)
    {
        _dbVilla = dbVilla;
        _mapper = mapper;
        _response = new();
    }

    [HttpGet]
    //[ResponseCache(Duration = 30)]
    [ResponseCache(CacheProfileName = "Default30")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<APIResponse>> GetVillas(
        [FromQuery(Name = "filterOccupancy")] int? occupancy,
        [FromQuery] string? search,
        int pageSize = 0,
        int pageNumber = 1)
    {
        try
        {
            //IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();
            IEnumerable<Villa> villaList;

            if (occupancy > 0)
            {
                villaList = await _dbVilla.GetAllAsync(u => u.Occupancy == occupancy, pageSize: pageSize, pageNumber: pageNumber);
            }
            else
            {
                villaList = await _dbVilla.GetAllAsync(pageSize: pageSize, pageNumber: pageNumber);
            }

            if (!string.IsNullOrEmpty(search))
            {
                villaList = villaList.Where(x => x.Amenity.ToLower().Contains(search) || x.Name.ToLower().Contains(search));
            }

            Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
            _response.Result = _mapper.Map<List<VillaDTO>>(villaList);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }
        return _response;
    }

    [HttpGet("{id:int}", Name = "GetVilla")]
    //[ResponseCache(Duration = 30, Location = ResponseCacheLocation.None, NoStore = true)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> GetVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var villa = await _dbVilla.GetAsync(x => x.Id == id);
            if (villa == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            _response.Result = _mapper.Map<VillaDTO>(villa);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }
        return _response;
    }


    [HttpPost]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO createDTO)
    {
        try
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            if (await _dbVilla.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already exists");
                return BadRequest(ModelState);
            }

            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }
            //if (villaDTO.Id > 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError);
            //}

            Villa villa = _mapper.Map<Villa>(createDTO);

            //Villa model = new Villa()
            //{
            //    Amenity = createDTO.Amenity,
            //    Details = createDTO.Details,
            //    ImageUrl = createDTO.ImageUrl,
            //    Name = createDTO.Name,
            //    Occupancy = createDTO.Occupancy,
            //    Rate = createDTO.Rate,
            //    Sqft = createDTO.Sqft,
            //};

            await _dbVilla.CreateAsync(villa);
            await _dbVilla.SaveAsync();

            _response.Result = _mapper.Map<VillaDTO>(villa);
            _response.StatusCode = HttpStatusCode.Created;
            return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }
        return _response;
    }

    [HttpDelete("{id:int}", Name = "DeleteVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = await _dbVilla.GetAsync(x => x.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            await _dbVilla.RemoveAsync(villa);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }
        return _response;
    }
    [Authorize(Roles = "admin")]
    [HttpPut("{id:int}", Name = "UpdateVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
    {
        try
        {
            if (updateDTO == null || id != updateDTO.Id)
            {
                return BadRequest();
            }
            //var villa = _db.Villas.FirstOrDefault(x => x.Id == id);
            //villa.Name = villaDTO.Name;
            //villa.Sqft = villaDTO.Sqft;
            //villa.Occupancy = villaDTO.Occupancy;
            Villa model = _mapper.Map<Villa>(updateDTO);
            //Villa model = new Villa()
            //{
            //    Amenity = updateDTO.Amenity,
            //    Details = updateDTO.Details,
            //    Id = updateDTO.Id,
            //    ImageUrl = updateDTO.ImageUrl,
            //    Name = updateDTO.Name,
            //    Occupancy = updateDTO.Occupancy,
            //    Rate = updateDTO.Rate,
            //    Sqft = updateDTO.Sqft,
            //};
            await _dbVilla.UpdateAsync(model);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }
        return _response;
    }

    [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
    {
        if (patchDTO == null || id == 0)
        {
            return BadRequest();
        }
        var villa = await _dbVilla.GetAsync(x => x.Id == id, tracked: false);

        VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);
        //VillaUpdateDTO villaDTO = new()
        //{
        //    Amenity = villa.Amenity,
        //    Details = villa.Details,
        //    Id = villa.Id,
        //    ImageUrl = villa.ImageUrl,
        //    Name = villa.Name,
        //    Occupancy = villa.Occupancy,
        //    Rate = villa.Rate,
        //    Sqft = villa.Sqft,
        //};

        if (villa == null)
        {
            return BadRequest();
        }
        patchDTO.ApplyTo(villaDTO, ModelState);
        Villa model = _mapper.Map<Villa>(villaDTO);
        //Villa model = new Villa()
        //{
        //    Amenity = villaDTO.Amenity,
        //    Details = villaDTO.Details,
        //    Id = villaDTO.Id,
        //    ImageUrl = villaDTO.ImageUrl,
        //    Name = villaDTO.Name,
        //    Occupancy = villaDTO.Occupancy,
        //    Rate = villaDTO.Rate,
        //    Sqft = villaDTO.Sqft,
        //};

        await _dbVilla.UpdateAsync(model);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return NoContent();
    }


}

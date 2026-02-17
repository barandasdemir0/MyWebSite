using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared;

namespace WebApiLayer.Controllers;

[ApiController]
public abstract class CrudController<TListDto,TCreateDto,TUpdateDto> : ControllerBase where TListDto:class,IHasId
{
    protected readonly ICrudService<TListDto, TCreateDto, TUpdateDto> _crudService;

    protected CrudController(ICrudService<TListDto, TCreateDto, TUpdateDto> crudService)
    {
        _crudService = crudService;
    }

    [HttpGet]
    public virtual async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = await _crudService.GetAllAsync(cancellationToken);
        return Ok(query);
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = await _crudService.GetByIdAsync(id, cancellationToken);
        if (query== null)
        {
            return NotFound();
        }
        return Ok(query);
    }

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] TCreateDto createDto , CancellationToken cancellationToken)
    {
        var result = await _crudService.AddAsync(createDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] TUpdateDto updateDto,CancellationToken cancellationToken)
    {
        var result = await _crudService.UpdateAsync(id, updateDto, cancellationToken); 
        if (result==null)
        {
            return NotFound();
        }
        return Ok(result);
    }


    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(Guid id,CancellationToken cancellationToken)
    {
        var result = await _crudService.GetByIdAsync(id, cancellationToken);
        if (result == null)
        {
            return NotFound();
        }
        await _crudService.DeleteAsync(id, cancellationToken);
        return Ok(result);
    }


}

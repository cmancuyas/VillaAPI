using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VillaUI.Models.Dto;

namespace VillaUI.Models.VM;

public class VillaNumberDeleteVM
{
    public VillaNumberDeleteVM()
    {
        VillaNumber = new VillaNumberDTO();
    }
    public VillaNumberDTO VillaNumber { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem> VillaList { get; set; }
}

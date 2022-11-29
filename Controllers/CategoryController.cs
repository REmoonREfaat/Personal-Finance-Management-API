using AutoMapper;
using Common.Models;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Personal_Finance_Management_API.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Personal_Finance_Management_API.Controllers
{
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(IMapper mapper, ILogger<CategoryController> logger, ICategoryService service)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ResponseModel<List<CategoryDTO>>> Categories()
        {
            try
            {
                var result = await _service.GetAllCategories();
                var list = _mapper.Map<List<CategoryDTO>>(result.ToList());
                var responseModel = HelperClass<List<CategoryDTO>>.CreateResponseModel(list, false, "");
                return responseModel;

            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured CategoryController\\GetAll" + " with EX: " + ex.ToString());
                return HelperClass<List<CategoryDTO>>.CreateResponseModel(null, true, ex.Message);
            }
        }      

    }
}
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
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(IMapper mapper, ILogger<TransactionsController> logger, ITransactionService service)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

   



        [HttpGet]
        public async Task<ResponseModel<List<TransactionDTO>>> Transactions(string TransactionKind , DateTime StartDate, DateTime EndDate, string SortBy , int? Page=1, int? PageSize=10
            )
        {
            try
            {
                var result = await _service.GetAllTransactions();
                var list = _mapper.Map<List<TransactionDTO>>(result.ToList());
                var responseModel = HelperClass<List<TransactionDTO>>.CreateResponseModel(list, false, "");
                return responseModel;

            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured CategoryController\\GetAll" + " with EX: " + ex.ToString());
                return HelperClass<List<TransactionDTO>>.CreateResponseModel(null, true, ex.Message);
            }
        }

    }
}

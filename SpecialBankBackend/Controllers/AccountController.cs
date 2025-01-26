using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SpecialBankAPI.Data;
using SpecialBankAPI.Models;
using SpecialBankAPI.Services.Interfaces;
using System.Text;

namespace SpecialBankAPI.Controllers
{
    [ApiController]
    [Route("api/v3/[controller]")]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;
        IMapper _mapper;
        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult CreateNewAccount([FromBody] CreateNewAccountModel newAccountModel)
        {
            if(!ModelState.IsValid) { return BadRequest(newAccountModel); }
            var account = _mapper.Map<Account>(newAccountModel);
            return Ok(_accountService.CreateAccount(account, newAccountModel.Pin, newAccountModel.ConfirmPin));
        }
        [HttpGet]
        [Route("Update")]
        public IActionResult GetAllAccounts() 
        {
            var accounts = _accountService.GetAllAccounts();
            var mappedAccounts = _mapper.Map<IList<GetAccountModel>>(accounts);
            return Ok(mappedAccounts);
        }
        [HttpPost]
        [Route("Authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel authenticateModel)
        {
            if (!ModelState.IsValid) { return BadRequest(authenticateModel); }
            return Ok(_accountService.Authenticate(authenticateModel.AccountNumber, authenticateModel.Pin));
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SpecialBankAPI.Data;
using SpecialBankAPI.Models;
using SpecialBankAPI.Services.Interfaces;
using System.Text;
using System.Text.RegularExpressions;

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
        [Route("GetAll")]
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
        [HttpGet]
        [Route("GetByAccountNumber")]
        public IActionResult GetByAccountNumber(string accountNumber)
        {
            if (!Regex.IsMatch(accountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$")) return BadRequest("Account nust be 10 digits");
            var account = _accountService.GetAccountByAccountNumber(accountNumber);
            var mappedAccount = _mapper.Map<GetAccountModel>(account);
            return Ok(mappedAccount);
        }
        [HttpGet]
        [Route("GetByAccountId")]
        public IActionResult GetByAccountId(int Id)
        {
            var account = _accountService.GetAccountById(Id);
            var mappedAccount = _mapper.Map<GetAccountModel>(account);
            return Ok(mappedAccount);
        }
        [HttpPut]
        [Route("UpdateAccount")]
        public IActionResult UpdateAccount([FromBody] UpdateAccountModel updateAccountModel)
        {
            if (!ModelState.IsValid) { return BadRequest(updateAccountModel); }
            var account = _mapper.Map<Account>(updateAccountModel);
            _accountService.UpdateAccount(account, updateAccountModel.Pin);
            return Ok(account);
        }
    }
}

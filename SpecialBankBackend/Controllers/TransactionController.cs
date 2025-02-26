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
    public class TransactionController : ControllerBase
    {
        private ITransactionService _transactionService;
        IMapper _mapper;
        public TransactionController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult CreateNewTransaction([FromBody] TransactionRequestDTO transactionRequest)
        {
            if (!ModelState.IsValid) { return BadRequest(transactionRequest); }
            var transaction = _mapper.Map<Transaction>(transactionRequest);
            return Ok(_transactionService.CreateNewTransaction(transaction));
        }

        [HttpPost]
        [Route("MakeDeposit")]
        public IActionResult MakeDeposit(string accountNumber, decimal amount, string transactionPin)
        {
            if (!Regex.IsMatch(accountNumber, @"[0][1-9]\d{9}$|^[1-9]\d{9}$")) return BadRequest("Account number must be 10 digit");
            return Ok(_transactionService.MakeDeposit(accountNumber, amount, transactionPin));
        }

        [HttpPost]
        [Route("MakeWithDrawal")]
        public IActionResult MakeWithDrawal(string accountNumber, decimal amount, string transactionPin)
        {
            if (!Regex.IsMatch(accountNumber, @"[0][1-9]\d{9}$|^[1-9]\d{9}$")) return BadRequest("Account number must be 10 digit");
            return Ok(_transactionService.MakeWithdrawal(accountNumber, amount, transactionPin));
        }

        [HttpPost]
        [Route("MakeFundsTransfer")]
        public IActionResult MakeFundsTransfer(string fromAccount, string toAccount, decimal amount, string transactionPin)
        {
            if (!Regex.IsMatch(fromAccount, @"[0][1-9]\d{9}$|^[1-9]\d{9}$") || !Regex.IsMatch(toAccount, @"[0][1-9]\d{9}$|^[1-9]\d{9}$")) return BadRequest("Account number must be 10 digit");
            return Ok(_transactionService.MakeFundsTransfer(fromAccount, toAccount, amount, transactionPin));
        }

    }
}

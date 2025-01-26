using AutoMapper;
using SpecialBankAPI.Models;

namespace SpecialBankAPI.Profiles
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<CreateNewAccountModel, Account>();
            CreateMap<UpdateAccountModel, Account>(); 
            CreateMap<Account, GetAccountModel>();
        }
    }
}

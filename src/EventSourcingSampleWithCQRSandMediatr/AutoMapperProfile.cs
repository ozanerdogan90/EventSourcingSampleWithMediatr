using AutoMapper;
using EventSourcingSampleWithCQRSandMediatr.DataAccess.Entities;
using EventSourcingSampleWithCQRSandMediatr.Models;
namespace EventSourcingSampleWithCQRSandMediatr
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Audit, AuditDTO>();
        }
    }
}

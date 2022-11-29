using Common.Models;
using Core.Entities;
using AutoMapper;
using System;
using System.Linq;
using System.Reflection;

namespace Personal_Finance_Management_API.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            var asm = Assembly.Load("Core");
            var classes =
                asm.GetTypes().Where(p =>
                    p.Namespace != null && p.Namespace.Equals("Core.Entities") &&
                    p.IsClass
                && (p.IsSubclassOf(typeof(BaseEntity)) )

                ).ToList();
            foreach (Type c in classes)
            {
                CreateMap(c, c)
                        .ForMember("CreationDate", act => act.Ignore())
                        .ForMember("LastUpdatedDate", act => act.Ignore());
            }
            // map Entity with Model
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<MccCode, MccCodeDTO>().ReverseMap();
            CreateMap<Transaction, TransactionDTO>().ReverseMap().ForMember(x=>x.Mcc, act => act.Ignore());



        }
    }
}

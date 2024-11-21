using Application.Dto;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class AutoMapperMappings : Profile
{
	public AutoMapperMappings()
	{
		CreateMap<Person, PersonViewModelDto>().ReverseMap();
		CreateMap<Person, PersonInputModelDto>().ReverseMap();
	}
}

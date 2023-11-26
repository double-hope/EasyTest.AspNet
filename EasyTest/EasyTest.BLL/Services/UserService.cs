using AutoMapper;
using EasyTest.BLL.Interfaces;
using EasyTest.DAL.Repository.IRepository;

namespace EasyTest.BLL.Services
{
	public class UserService : Service, IUserService
	{
		public UserService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
		{
			
		}
	}
}

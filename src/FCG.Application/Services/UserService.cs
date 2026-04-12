using AutoMapper;
using FCG.Application.DTOs;
using FCG.Application.Interfaces;
using FCG.Application.ViewModels;
using FCG.Domain.Contants;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces;
using FCG.Domain.Interfaces.IService;
using FCG.Exception.Exceptions;
using FluentValidation;
using System.Threading.Tasks;

namespace FCG.Application.Services;

public class UserService : BaseApplicationService, IUserService
{
    private readonly IValidator<UserDTO> _validator;
    private readonly IEmailService _emailService;
    private readonly IUserDomainService _userDomainService;
    private readonly IMapper _mapper;
    public UserService(IUnitOfWork unitOfWork,
        IValidator<UserDTO> validator,
        IMapper mapper,
        IEmailService emailService,
        IUserDomainService userDomainService) : base(unitOfWork)
    {
        _validator = validator;
        _mapper = mapper;
        _emailService = emailService;
        _userDomainService = userDomainService;
    }

    public async Task<UserViewModel> CreateUser(UserDTO userModel)
    {
        var validation = await _validator.ValidateAsync(userModel);
        if (!validation.IsValid) 
            throw new BusinessException("Erro na validação");

        var user = _mapper.Map<User>(userModel);
        var insertedUser = await _userDomainService.CreateUser(user, FCGConstant.UserDefault);
        await UnitOfWork.CommitAsync();

        var userView = _mapper.Map<UserViewModel>(insertedUser);

        await _emailService.SendAsync(userView);

        return userView;
    }
}
